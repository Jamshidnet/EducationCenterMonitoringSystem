using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MonitoringSystem.Application.Common.Exceptions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EducationCenterMonitoringSystem.Filters
{

    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;


        public GlobalExceptionMiddleware(RequestDelegate next)
        {
            _next = next;


        }

    public async Task Invoke(HttpContext httpContext)
        {

            try
            {
                await _next(httpContext) ;

            }
            catch(Exception e)
            {
                 HandleException(httpContext,e.Message);
            }

        }

        public  void HandleException( HttpContext context, string message)
        {
            string encodedMessage = Uri.EscapeDataString(message);
            context.Response.Redirect($"/Home/Error?errorMessage={encodedMessage}");
        }

    }

    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }

    
}
