using System.ComponentModel.DataAnnotations;
using TaskManager.Application.UseCases.Errors;

namespace TaskManager.API.Middlewares
{
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (BaseError ex)
            {
                _logger.LogError(ex, "Application error occurred.");
                await HandleApplicationExceptionAsync(httpContext, ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception has occurred.");
                await HandleGeneralExceptionAsync(httpContext, ex);
            }
        }

        private Task HandleApplicationExceptionAsync(HttpContext context, BaseError exception)
        {
            var response = new
            {
                message = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = exception.StatusCode;

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }

        private Task HandleGeneralExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new
            {
                message = "An unexpected error occurred. Please try again later."
            };

            return context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
        }
    }
}
