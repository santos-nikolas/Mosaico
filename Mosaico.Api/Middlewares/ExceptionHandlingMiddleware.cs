using System.Net;
using System.Text.Json;
using Mosaico.Api.Common;

namespace Mosaico.Api.Middlewares
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Log do erro para análise
            _logger.LogError(exception, "Erro não tratado na requisição.");

            var traceId = Guid.NewGuid().ToString("N");
            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = HttpStatusCode.InternalServerError;
            var message = "Ocorreu um erro inesperado ao processar sua requisição.";

            // Você pode refinar por tipo de exceção se quiser:
            // ex: if (exception is KeyNotFoundException) => 404
            if (exception is KeyNotFoundException)
            {
                statusCode = HttpStatusCode.NotFound;
                message = "Recurso não encontrado.";
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = HttpStatusCode.Unauthorized;
                message = "Acesso não autorizado.";
            }
            // pode criar custom exceptions (ex.: DomainException) e mapear pra 400 aqui

            response.StatusCode = (int)statusCode;

            var error = new ErrorResponse
            {
                TraceId = traceId,
                StatusCode = (int)statusCode,
                Message = message,
#if DEBUG
                Details = exception.Message // em ambiente real, normalmente omitimos
#endif
            };

            var json = JsonSerializer.Serialize(error);
            await response.WriteAsync(json);
        }
    }
}
