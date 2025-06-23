/**
 * @copyright Dorian Thivolle
 * @license MIT
 * @see https://github.com/NoxFly/angular-dotnet-graphql
 */

using Core.Helpers;
using Core.Models;
using Newtonsoft.Json;

namespace Core.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
{
    private readonly RequestDelegate _next = next;
    private readonly ILogger _logger = logger;

    public async Task InvokeAsync(HttpContext context)
    {
        var originalBodyStream = context.Response.Body;

        using var newBodyStream = new MemoryStream();
        context.Response.Body = newBodyStream;

        try
        {
            await _next(context);

            if (context.Response.StatusCode >= 400)
            {
                throw new HttpException(context.Response.StatusCode, "");
            }

            newBodyStream.Seek(0, SeekOrigin.Begin);
            await newBodyStream.CopyToAsync(originalBodyStream);
        }
        // Custom
        catch(HttpException e)
        {
            await HandleErrorResponse(context, originalBodyStream, newBodyStream, e);
        }
        // Unhandled
        catch(Exception e)
        {
            _logger.LogError(e, "WebApi responded to a client with unhandled error on path {0}", new { RequestPath = context.Request.Path });
            var err = new InternalServerErrorException("", null, e);
            await HandleErrorResponse(context, originalBodyStream, newBodyStream, err);
        }
    }

    private async Task HandleErrorResponse(HttpContext context, Stream originalBodyStream, Stream newBodyStream, HttpException e)
    {
        _logger.LogError($"Response Error : [{e.StatusCode}] {e.Message}");

        if(e.InnerException != null)
        {
            _logger.LogError(e.InnerException.Message);
        }

        newBodyStream.Seek(0, SeekOrigin.Begin);
        var body = await new StreamReader(newBodyStream).ReadToEndAsync();
        _logger.LogError(body);

        _logger.LogError("");

        var error = new ResponseError()
        {
            Error = new ResponseErrorDetails()
            {
                StatusCode = e.StatusCode,
                Message = e.Message,
                Details = e.Details,
            }
        };

        context.Response.Body = originalBodyStream;
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = error.Error.StatusCode;
        await context.Response.WriteAsync(JsonHelper.Serialize(error, Formatting.None));
    }
}
