using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Homework_4_2.API.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _request;
        private readonly ILogger _logger;

        private FileStream responseStream;
        private FileStream requestStream;
        private string sLogFormat;
        private string sErrorTime;

        
        
        
        public RequestResponseLoggingMiddleware(RequestDelegate request, ILoggerFactory loggerFactory)
        {
            _request = request;
            _logger = loggerFactory
                .CreateLogger<RequestResponseLoggingMiddleware>();
            
            responseStream = File.OpenWrite($"responseLogs-{DateTime.UtcNow.ToFileTimeUtc()}.txt");
            requestStream = File.OpenWrite($"requestLogs-{DateTime.UtcNow.ToFileTimeUtc()}.txt");
        }

        public async Task Invoke(HttpContext context)
        {
           var requestText =  await FormatRequest(context.Request);
           requestStream.Write(Encoding.UTF8.GetBytes(requestText));
           requestStream.Flush();
           
            var originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                await _request(context);

                var responseText = await FormatResponse(context.Response);
                responseStream.Write(Encoding.UTF8.GetBytes(responseText));

                responseStream.Flush();

                //_logger.LogInformation();
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private static async Task<string> FormatRequest(HttpRequest request)
        {
            request.EnableBuffering();
            var body = request.Body;
           
           var buffer = new byte[Convert.ToInt32(request.ContentLength)];
            await request.Body.ReadAsync(buffer, 0, buffer.Length);
            var bodyAsText = Encoding.UTF8.GetString(buffer);
            request.Body = body;

            return $"\n{request.Scheme}\n \n{request.Host}\n \n{request.Path}\n \n{request.QueryString}\n \n{bodyAsText}\n";

        }

        private static async Task<string> FormatResponse(HttpResponse response)
        {
            response.Body.Seek(0, SeekOrigin.Begin);
            string text = await new StreamReader(response.Body).ReadToEndAsync();
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"{response.StatusCode}: {text}";
        }
    }
}
