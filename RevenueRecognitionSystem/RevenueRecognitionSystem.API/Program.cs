using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;
using RevenueRecognitionSystem.Infrastructure.Repositories;
using RevenueRecognitionSystem.Services.Implementations;
using RevenueRecognitionSystem.Services.Interfaces;
using System.Text.Json.Serialization;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using RevenueRecognitionSystem.API.Filters;
using RevenueRecognitionSystem.API.Middleware;
using RevenueRecognitionSystem.Services.Validators;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        sqlServerOptions => sqlServerOptions.EnableRetryOnFailure(
            maxRetryCount: 5,           
            maxRetryDelay: TimeSpan.FromSeconds(10), 
            errorNumbersToAdd: null     
        )
    )
);

builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<ISoftwareRepository, SoftwareRepository>();
builder.Services.AddScoped<IRevenueRepository, RevenueRepository>();

builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<IRevenueCalculationService, RevenueCalculationService>();

builder.Services.AddControllers(options =>
    {
        options.Filters.Add<LoggingFilter>();
        options.Filters.Add<ValidateModelFilter>();
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<AddCompanyRequestValidator>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(options =>  
{  
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;  
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  
}).AddJwtBearer(opt =>  
{  
    opt.TokenValidationParameters = new TokenValidationParameters  
    {  
        ValidateIssuer = true,   
        ValidateAudience = true, 
        ValidateLifetime = true,  
        ClockSkew = TimeSpan.FromMinutes(2),  
        ValidIssuer = "https://localhost:5001", 
        ValidAudience = "https://localhost:5001", 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))  
    };  
    opt.Events = new JwtBearerEvents  
    {  
        OnAuthenticationFailed = context =>  
        {  
            if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))  
            {                
                context.Response.Headers.Add("Token-expired", "true");  
            }            
            
            return Task.CompletedTask;  
        }    
    };
}).AddJwtBearer("IgnoreTokenExpirationScheme",opt =>  
{  
    opt.TokenValidationParameters = new TokenValidationParameters  
    {  
        ValidateIssuer = true,   
        ValidateAudience = true,  
        ValidateLifetime = false,  
        ClockSkew = TimeSpan.FromMinutes(2),  
        ValidIssuer = "https://localhost:5001", 
        ValidAudience = "https://localhost:5001", 
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["SecretKey"]))  
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token like this: Bearer {your token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<ExceptionMiddleware>();

app.MapControllers();

app.Urls.Add("http://+:80");

app.Run();