using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using Newtonsoft.Json;

namespace WebAPIService.Extensions {
    public static class ClaimsPrincipalExtensions {
        public static Guid ExtractIdentifier (this ClaimsPrincipal user) {
            if (Guid.TryParse (user.Claims.FirstOrDefault (c => c.Type.Equals ("userId"))?.Value ?? String.Empty, out Guid userId)) {
                return userId;
            }
            throw new InvalidDataException (JsonConvert.SerializeObject (user.Claims.Select (c => new { c.Type, c.Value })));
        }

    }
}