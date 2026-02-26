using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RevenueRecognitionSystem.Domain.Interfaces;
using RevenueRecognitionSystem.Infrastructure.Data;
using RevenueRecognitionSystem.Infrastructure.Repositories;
using RevenueRecognitionSystem.Services.Implementations;
using RevenueRecognitionSystem.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IClientRepository, ClientRepository>();
builder.Services.AddScoped<IContractRepository, ContractRepository>();
builder.Services.AddScoped<ISubscriptionRepository, SubscriptionRepository>();
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddScoped<ISoftwareRepository, SoftwareRepository>();
builder.Services.AddScoped<IRevenueRepository, RevenueRepository>();

builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IContractService, ContractService>();
builder.Services.AddScoped<ISubscriptionService, SubscriptionService>();
builder.Services.AddHttpClient<IExchangeRateService, ExchangeRateService>();
builder.Services.AddScoped<IRevenueCalculationService, RevenueCalculationService>();

builder.Services.AddControllers();

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
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();