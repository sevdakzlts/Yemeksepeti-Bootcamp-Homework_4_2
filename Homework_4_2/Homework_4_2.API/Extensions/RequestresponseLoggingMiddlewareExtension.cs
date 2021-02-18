using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Homework_4_2.API.Middleware;
using Microsoft.AspNetCore.Builder;

namespace Homework_4_2.API.Extensions
{
    public static class RequestresponseLoggingMiddlewareExtension
    {
        public static IApplicationBuilder UseRequestResponseLogging(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RequestResponseLoggingMiddleware>();
        }
    }
}
