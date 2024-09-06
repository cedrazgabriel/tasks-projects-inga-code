using System.ComponentModel.DataAnnotations;
using FluentValidation;
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
            catch (FluentValidation.ValidationException ex) // Captura erros de validação do FluentValidation
            {
                _logger.LogError(ex, "Validation error occurred.");
                await HandleValidationExceptionAsync(httpContext, ex);
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

        // Novo método para tratar erros de validação do FluentValidation
        private Task HandleValidationExceptionAsync(HttpContext context, FluentValidation.ValidationException exception)
        {
            var errors = exception.Errors.Select(e => new
            {
                Property = e.PropertyName,
                Error = e.ErrorMessage
            }).ToList();

            var response = new
            {
                message = "Validation failed",
                errors
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status400BadRequest;

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
