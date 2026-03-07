using Microsoft.AspNetCore.Mvc.Filters;

namespace RevenueRecognitionSystem.API.Filters;

public class LoggingFilter : IActionFilter
{
    private readonly ILogger<LoggingFilter> _logger;

    public LoggingFilter(ILogger<LoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("Action started");
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("Action finished");
    }
}