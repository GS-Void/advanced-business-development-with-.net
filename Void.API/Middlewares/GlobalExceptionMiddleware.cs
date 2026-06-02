using System.Net;
using System.Text.Json;

namespace Void.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
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
                // Se der erro em qualquer lugar cai aqui
                _logger.LogError(ex, "Ocorreu uma exceção não tratada na API.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Define o status padrão como 500 
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Retorna um JSON padronizado e limpo
            var response = new
            {
                erro = "Ocorreu um erro interno no servidor ao processar sua solicitação.",
                mensagem = exception.Message 
            };

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}