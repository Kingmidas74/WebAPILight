using Domain.Exceptions;

namespace DataAccess.Exceptions
{
    public class ProcedureNotFoundException: WebApiDomainExceptionBase {        
        public ProcedureNotFoundException (string message) : base (message) { }
    }
}