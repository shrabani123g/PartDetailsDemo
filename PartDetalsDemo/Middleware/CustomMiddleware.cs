using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace PartDetailsDemo.Middleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class CustomMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<CustomMiddleware> seriLogger;

        public CustomMiddleware(RequestDelegate next, ILogger<CustomMiddleware> seriLogger)
        {
            this.next = next;
            this.seriLogger = seriLogger;

        }

        public async Task Invoke(HttpContext httpContext)
        {
            var userLanguage = httpContext.Request.Headers["Accept-Language"].FirstOrDefault();

            //Set culture from header
            CultureInfo culture;
            try
            {
                if (!string.IsNullOrEmpty(userLanguage))
                {
                    culture = new CultureInfo(userLanguage.Split(',')[0]);
                    if (culture.Name == "en-US")
                    {
                        await next(httpContext);
                    }
                    else
                    {
                        httpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                        await httpContext.Response.WriteAsync("Time Stamp: " + DateTime.Now.ToString("G") + " " + culture.Name  + " is not accepted as a language, please verify.");
                    }
                }
                else
                {
                    seriLogger.Log(LogLevel.Error, "Accept-Language key is not set properly.");
                    httpContext.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                    await httpContext.Response.WriteAsync("Time Stamp: " + DateTime.Now.ToString("G") + " An error occured, please contact admin.");
                }
            }
            catch(Exception ex)
            {
                seriLogger.Log(LogLevel.Error, ex.Message, ex);
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync("Time Stamp: " + DateTime.Now.ToString("G") + " An error occured, please contact admin.");
            }

        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class CustomMiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<CustomMiddleware>();
        }
    }
}
