using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MonitoringSystem.Application.Common.Exceptions;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EducationCenterMonitoringSystem.Filters
{

    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
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
            catch (AlreadyExistsException e)
            {
                 HandleException(httpContext,e.Message);

                
            }
            catch(NotFoundException e)
            {

            }
            catch(Exception e)
            {


            }

        }

        public  void HandleException( HttpContext context, string message)
        {
            string encodedMessage = Uri.EscapeDataString(message);
            context.Response.Redirect($"/Home/Error?errorMessage={encodedMessage}");
        }

    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<GlobalExceptionMiddleware>();
        }
    }

    
}
