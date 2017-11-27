using System;
using System.IO;
using System.Threading.Tasks;
using HTF2017.Business.Exceptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace HTF2017.WebApi.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (HtfValidationException ex)
            {
                if (context.Response.HasStarted) { throw; }

                context.Response.Clear();
                context.Response.StatusCode = 400;
                context.Response.ContentType = "application/json";

                using (var writer = new StreamWriter(context.Response.Body))
                {
                    new JsonSerializer().Serialize(writer, new ValidationResult { Message = ex.ValidationMessage });
                    await writer.FlushAsync().ConfigureAwait(false);
                }
            }
        }
    }

    public static class ExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionMiddleware>();
        }
    }
}