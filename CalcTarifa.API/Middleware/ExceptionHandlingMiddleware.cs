using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using CalcTarifa.Domain;

namespace CalcTarifa.API.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next   = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (DomainValidationException ex)
            {
                await Responder(context, HttpStatusCode.BadRequest, "Error de validación", ex.Message);
            }
            catch (EntityNotFoundException ex)
            {
                await Responder(context, HttpStatusCode.NotFound, "Recurso no encontrado", ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error no controlado en la API. CorrelationId: {CorrelationId}", context.TraceIdentifier);
                await Responder(context, HttpStatusCode.InternalServerError, "Error interno del servidor", "Ha ocurrido un error inesperado en el servidor. Intenta más tarde.");
            }
        }

        private static Task Responder(HttpContext ctx, HttpStatusCode status, string title, string detail)
        {
            ctx.Response.ContentType = "application/problem+json";
            ctx.Response.StatusCode  = (int)status;

            var problem = new ProblemDetails
            {
                Status = (int)status,
                Title = title,
                Detail = detail,
                Instance = ctx.Request.Path
            };

            return ctx.Response.WriteAsync(JsonSerializer.Serialize(problem));
        }
    }
}
