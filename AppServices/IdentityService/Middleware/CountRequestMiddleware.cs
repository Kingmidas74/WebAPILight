using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Prometheus;

namespace IdentityService.Middleware
{
    public class CountRequestMiddleware
    {
        private readonly RequestDelegate _next;

        public CountRequestMiddleware(RequestDelegate next)
        {
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public async Task Invoke(HttpContext httpContext, MetricReporter reporter)
        {         
            Metrics.CreateCounter("PathCounter", "Count request", 
                                                new CounterConfiguration{
                                                    LabelNames = new [] {"method", "endpoint"}
                                                }).WithLabels(httpContext.Request.Method, httpContext.Request.Path).Inc();            
            await _next(httpContext);
            return;
        }
    }
}