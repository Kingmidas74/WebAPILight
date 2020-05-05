using System;

namespace Domain.Exceptions
{
    public class GuardException:DomainException
    {
        public GuardException(Type exceptionType):base(exceptionType)
        {
            
        }        
    }
}