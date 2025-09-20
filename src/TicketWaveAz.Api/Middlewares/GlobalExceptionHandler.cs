using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TicketWaveAz.Shared.Abstractions;
using TicketWaveAz.Shared.Abstractions.Errors;

namespace TicketWaveAz.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private string ErrorMessage = "Erro Interno. Tente novamente mais tarde.";
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            httpContext.Response.ContentType = "application/json";
            
            ErrorMessage = exception.Message;

            var contextFeature = httpContext.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature != null)
            {
                httpContext.Response.StatusCode = HandleUhandledException(exception);

                var result = Result<bool>.Fail(new InternalError(ErrorMessage));

                var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings
                {
                    ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
                    Converters = { new Newtonsoft.Json.Converters.StringEnumConverter() }
                });

                await httpContext.Response.WriteAsync(json);
            }
            return true;
        }

        private int HandleUhandledException(Exception ex)
        {
            ErrorMessage = $"Erro interno. {ex.Message}.";
            return StatusCodes.Status500InternalServerError;
        }
    }
}
