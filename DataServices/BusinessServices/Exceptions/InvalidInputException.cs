namespace BusinessServices.Exceptions
{
    public class InvalidInputException : BusinessException {
        public InvalidInputException (string message) : base (message) { }
    }
}