using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace IdentityService.Middleware
{
    public class WebAPIRequest
    {
        public string Protocol { get; internal set; }
        public string Method { get; internal set; }
        public string Scheme { get; internal set; }
        public HostString Host { get; internal set; }
        public PathString Path { get; internal set; }
        public QueryString QueryString { get; internal set; }
        public string ContentType { get; internal set; }
        public Dictionary<string, object> Body { get; internal set; }
        public WebAPIResponse Response { get; internal set; }
        public Guid Id { get; internal set; }
        public int Elapsed { get; internal set; }
    }
}