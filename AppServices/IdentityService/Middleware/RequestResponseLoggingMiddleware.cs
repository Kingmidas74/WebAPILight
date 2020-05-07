using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Diagnostics;

namespace IdentityService.Middleware {

    public class RequestResponseLoggingMiddleware {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestResponseLoggingMiddleware> _logger;

        public RequestResponseLoggingMiddleware (RequestDelegate next, ILogger<RequestResponseLoggingMiddleware> logger) {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke (HttpContext context) {            
            if(!context.Request.Path.Value.Contains("api")) {
                await _next(context);
                return;
            }
            var scopeId = Guid.NewGuid();
            using (_logger.BeginScope(new Dictionary<string, object>{
                                                { nameof(scopeId), scopeId }
                                    }
                )
            ) 
            {
                var request = await FormatRequest (context.Request, scopeId);
                var originalBodyStream = context.Response.Body;
                
                using (var responseBody = new MemoryStream ()) {
                    context.Response.Body = responseBody;

                    var timer = new Stopwatch();
                    timer.Start();
                    await _next (context);
                    timer.Stop();
                    request.Elapsed = timer.Elapsed.Milliseconds;
                    request.Response = context.Response.StatusCode switch 
                    {
                        var n when(n>=500) => await FormatServerErrorResponse(context.Response),
                        var n when(n>=400 && n<500) => await FormatClientErrorResponse(context.Response),                        
                        _ => await FormatSuccessResponse(context.Response)                        
                    };               
                    
                    switch (request.Response)
                    {
                        case WebAPIClientErrorResponse r: 
                            _logger.LogError("{@request}", request); 
                            break;
                        case WebAPIServerErrorResponse r: 
                            _logger.LogWarning("{@request}", request); 
                            break;
                        default: 
                            _logger.LogInformation("{@request}", request);
                            break;
                    };
                
                    await responseBody.CopyToAsync (originalBodyStream);
                }
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
                if(!String.IsNullOrEmpty(text)) result.Body = JsonConvert.DeserializeObject<Dictionary<string,object>>(text);
            }
            response.Body.Seek (0, SeekOrigin.Begin);
            return result;
        }

        private async Task<WebAPIResponse> FormatClientErrorResponse (HttpResponse response) {
            var result = new WebAPIClientErrorResponse();
            response.Body.Seek (0, SeekOrigin.Begin);
            var text = await new StreamReader (response.Body).ReadToEndAsync ();        
            if(!String.IsNullOrEmpty(text)) result.Body = JsonConvert.DeserializeObject<Dictionary<string,object>>(text);
            response.Body.Seek (0, SeekOrigin.Begin);
            return result;
        }

        private async Task<WebAPIResponse> FormatServerErrorResponse (HttpResponse response) {
            var result = new WebAPIServerErrorResponse();
            response.Body.Seek (0, SeekOrigin.Begin);
            var text = await new StreamReader (response.Body).ReadToEndAsync ();
            if(!String.IsNullOrEmpty(text)) result.Body = JsonConvert.DeserializeObject<Dictionary<string,object>>(text);
            response.Body.Seek (0, SeekOrigin.Begin);
            return result;
        }
    }
}