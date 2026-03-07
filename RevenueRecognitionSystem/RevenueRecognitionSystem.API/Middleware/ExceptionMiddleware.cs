using System.Net;
using System.Text.Json;
using RevenueRecognitionSystem.Domain.Exceptions;
using RevenueRecognitionSystem.Domain.Exceptions.Contract;
using RevenueRecognitionSystem.Domain.Exceptions.Employee;
using RevenueRecognitionSystem.Domain.Exceptions.Payment;
using RevenueRecognitionSystem.Domain.Exceptions.Revenue;
using RevenueRecognitionSystem.Domain.Exceptions.SoftwareSystem;
using RevenueRecognitionSystem.Domain.Exceptions.Subscription;

namespace RevenueRecognitionSystem.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var response = context.Response;
        var statusCode = GetStatusCode(exception);

        var problem = new ApiProblemDetails
        {
            Type = "https://httpstatuses.com/{(int)statusCode}",
            Title = statusCode.ToString(),
            Status = (int)statusCode,
            Detail = exception.Message,
            Instance = context.Request.Path,
            TraceId = context.TraceIdentifier
        };
        response.ContentType = "application/json";
        response.StatusCode = (int)statusCode;
        
        var result = JsonSerializer.Serialize(problem);
        return response.WriteAsync(result);
    }

    private static HttpStatusCode GetStatusCode(Exception exception)
    {
        return exception switch
        {
            InvalidEmployeeLoginException
                or IndividualAlreadyExistsException
                or CompanyAlreadyExistsException
                or CompanyCantBeRemovedException
                or ClientAlreadyHasContractException
                or InvalidDurationException
                or InvalidSupportYearsNumberException
                or ContractIsCancelledException
                or ContractIsFullyPaidException
                or ContractHasExpiredException
                or InvalidPaymentAmountException
                or TotalPaymentExceedException
                or IncorrectPasswordException
                or ExpiredRefreshTokenException
                or InvalidRenewalPeriodException
                or SubscriptionIsCancelledException
                or PaymentOutsidePeriodException
                or AlreadyPaidPeriodException
                or PreviousPeriodUnpaidException
                or IncorrectPaymentAmountException
                or InvalidCurrencyException
                => HttpStatusCode.BadRequest,

            ClientNotFoundException
                or SoftwareSystemNotFoundException
                or InvalidEmployeeRefreshTokenException
                or SubscriptionNotFoundException
                => HttpStatusCode.NotFound,

            _ => HttpStatusCode.InternalServerError
        };
    }
}