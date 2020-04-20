using Domain.Exceptions;

namespace BusinessServices.Exceptions {
    public class InvalidInputException : WebApiDomainExceptionBase {
        public InvalidInputException (string message) : base (message) { }
    }
}