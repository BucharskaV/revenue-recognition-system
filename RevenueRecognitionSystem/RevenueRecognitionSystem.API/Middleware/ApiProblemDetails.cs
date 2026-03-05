using Microsoft.AspNetCore.Mvc;

namespace RevenueRecognitionSystem.API.Middleware;

public class ApiProblemDetails : ProblemDetails
{
    public string TraceId { get; set; } = string.Empty;
}