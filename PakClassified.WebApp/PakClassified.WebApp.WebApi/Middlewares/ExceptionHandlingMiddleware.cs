using System.Net;
using System.Text.Json;

namespace PakClassified.WebApp.WebApi.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError("Exception detected...\n{ex}\n", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            HttpStatusCode statusCode = GetStatusCode(ex);
            context.Response.StatusCode = (int)statusCode;
            var response = new
            {
                success = false,
                message = ex.Message,
                statusCode = statusCode
            };

            _logger.LogError("Status Code: {code}", (int)statusCode);
            //_logger.LogError("Exception resolved....{ex}", ex.StackTrace);
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

        private HttpStatusCode GetStatusCode(Exception ex)
        {
            if (ex is ArgumentException) return HttpStatusCode.BadRequest;               // 400
            if (ex is UnauthorizedAccessException) return HttpStatusCode.Unauthorized;   // 401
            if (ex is KeyNotFoundException) return HttpStatusCode.NotFound;              // 404
            if (ex is NotImplementedException) return HttpStatusCode.NotImplemented;     // 501
            if (ex is TimeoutException) return HttpStatusCode.RequestTimeout;            // 408
            if (ex is InvalidOperationException) return HttpStatusCode.Conflict;         // 409

            return HttpStatusCode.InternalServerError;                                   // 500 default
        }
    }
}
