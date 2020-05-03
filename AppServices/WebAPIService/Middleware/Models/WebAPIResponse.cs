using System.Collections.Generic;

namespace WebAPIService.Middleware
{
    public abstract class WebAPIResponse
    {
        public Dictionary<string,object> Body { get; internal set; } = new Dictionary<string, object>();
    }
}