using System;

namespace Domain.Exceptions {
    public class WebApiDomainExceptionBase : Exception {
        public WebApiDomainExceptionBase (string message) : base (message) { }
    }
}