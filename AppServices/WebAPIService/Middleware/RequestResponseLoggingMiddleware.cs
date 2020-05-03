using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebAPIService.Middleware {

    public class RequestResponseLoggingMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger _logger;

        public RequestResponseLoggingMiddleware (RequestDelegate next, ILoggerFactory loggerFactory) {
            _next = next;
            _logger = loggerFactory.CreateLogger<RequestResponseLoggingMiddleware> ();
        }

        public async Task Invoke (HttpContext context) {            

            var request = await FormatRequest (context.Request, Guid.NewGuid());
            var originalBodyStream = context.Response.Body;
            
            using (var responseBody = new MemoryStream ()) {
                context.Response.Body = responseBody;

                await _next (context);

                request.Response = context.Response.StatusCode switch 
                {
                    var n when(n>=500) => await FormatServerErrorResponse(context.Response),
                    var n when(n>=400 && n<500) => await FormatClientErrorResponse(context.Response),                        
                    _ => await FormatSuccessResponse(context.Response)                        
                };               
                
                switch (request.Response)
                {
                    case WebAPIClientErrorResponse r: 
                        _logger.LogError(nameof(WebAPIClientErrorResponse)+"{request}", request); 
                        break;
                    case WebAPIServerErrorResponse r: 
                        _logger.LogWarning(nameof(WebAPIServerErrorResponse)+"{request}", request); 
                        break;
                    default: 
                        _logger.LogInformation(nameof(WebAPISuccessResponse)+"{request}", request);
                        break;
                };

                await responseBody.CopyToAsync (originalBodyStream);
            }
        }

        private async Task<WebAPIRequest> FormatRequest (HttpRequest request, Guid id) {
            var body = request.Body;
            request.EnableBuffering ();

            request.Body.Seek (0, SeekOrigin.Begin);
            var bodyAsText = await new StreamReader (request.Body).ReadToEndAsync ();
            request.Body.Seek (0, SeekOrigin.Begin);

            return new WebAPIRequest {
                Id = id,
                Protocol = request.Protocol,
                Method = request.Method,
                Scheme = request.Scheme,
                Host = request.Host,
                Path = request.Path,
                QueryString = request.QueryString,
                ContentType = request.ContentType,
                Body = !String.IsNullOrEmpty(bodyAsText) 
                            ? JObject.Parse(bodyAsText).ToObject<Dictionary<string, object>>()
                            : new Dictionary<string,object>()
            };
        }

        private async Task<WebAPIResponse> FormatSuccessResponse (HttpResponse response) {                        
            var result = new WebAPISuccessResponse();
            if (response.ContentLength == null) {
                response.Body.Seek (0, SeekOrigin.Begin);
                var text = await new StreamReader (response.Body).ReadToEndAsync ();
                if(!String.IsNullOrEmpty(text)) result.Body = text;
            }
            response.Body.Seek (0, SeekOrigin.Begin);
            return result;
        }

        private async Task<WebAPIResponse> FormatClientErrorResponse (HttpResponse response) {
            var result = new WebAPIClientErrorResponse();
            response.Body.Seek (0, SeekOrigin.Begin);
            var text = await new StreamReader (response.Body).ReadToEndAsync ();            
            if(!String.IsNullOrEmpty(text)) result.Body = text;
            response.Body.Seek (0, SeekOrigin.Begin);
            return result;
        }

        private async Task<WebAPIResponse> FormatServerErrorResponse (HttpResponse response) {
            var result = new WebAPIServerErrorResponse();
            response.Body.Seek (0, SeekOrigin.Begin);
            var text = await new StreamReader (response.Body).ReadToEndAsync ();
            if(!String.IsNullOrEmpty(text)) result.Body = text;
            response.Body.Seek (0, SeekOrigin.Begin);
            return result;
        }
    }
}