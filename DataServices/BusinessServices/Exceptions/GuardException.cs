using System;

namespace BusinessServices.Exceptions
{
    public class GuardException:BusinessException
    {
        public GuardException(Type exceptionType):base(exceptionType)
        {
            
        }        
    }
}