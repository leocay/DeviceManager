using Microsoft.AspNetCore.Mvc;

namespace DeviceManager.Api.Extensions;

public static class ControllerProblemDetailsExtensions
{
    public static ActionResult ToProblem(
        this ControllerBase controller,
        int statusCode,
        string title,
        string detail,
        string errorCode)
    {
        var problemDetails = new ProblemDetails
        {
            Type = $"https://httpstatuses.com/{statusCode}",
            Title = title,
            Detail = detail,
            Status = statusCode,
            Instance = controller.HttpContext.Request.Path
        };

        problemDetails.Extensions["errorCode"] = errorCode;
        problemDetails.Extensions["traceId"] = controller.HttpContext.TraceIdentifier;

        return controller.StatusCode(statusCode, problemDetails);
    }
}