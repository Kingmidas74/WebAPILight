using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading.Tasks;

namespace WebAPIService.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware>();
        }

        public async Task Invoke(HttpContext context)
        {
            Guid id = Guid.NewGuid();

            //логируем request
            string message = await FormatRequest(context.Request, id);
            _logger.LogDebug(message);

            Stream originalBodyStream = context.Response.Body;

            using (var responseBody = new MemoryStream())
            {
                context.Response.Body = responseBody;
                try
                {
                    await _next(context);
                }
                catch(Exception e)
                {
                    _logger.LogError(e, "Execution error");
                }
                
                _logger.LogDebug(await FormatResponse(context.Response, id));
                await responseBody.CopyToAsync(originalBodyStream);
            }
        }

        private async Task<string> FormatRequest(HttpRequest request, Guid id)
        {
            Stream body = request.Body;
            request.EnableBuffering();

            request.Body.Seek(0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Seek(0, SeekOrigin.Begin);

            return $"==================================    REQUEST     ================================== {{{id}}} => {Environment.NewLine}" +
                $"{request.Protocol} {request.Method} {request.Scheme}://{request.Host}{request.Path}{request.QueryString} {request.ContentType} {bodyAsText.Replace("{{", "{ {").Replace("}}", "} }")} ";
        }

        private async Task<string> FormatResponse(HttpResponse response, Guid id)
        {
            string text = "File content is not logged.";
            // Содержимое файла не логируется ибо начинает тормозить
            if (response.ContentLength == null)
            {
                response.Body.Seek(0, SeekOrigin.Begin);
                text = await new StreamReader(response.Body).ReadToEndAsync();
            }
            response.Body.Seek(0, SeekOrigin.Begin);

            return $"==================================    RESPONSE    ================================== {{{id}}} => {Environment.NewLine}" +
                $"{text}";
        }
    }
}