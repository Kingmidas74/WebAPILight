namespace Domain.Exceptions
{
    public class NullException:GuardException
    {
        public NullException():base(typeof(NullException))
        {
        }        
        public NullException(string parameterName):base(typeof(NullException))
        {
            Properties.Add(nameof(parameterName),parameterName);
        }  
    }
}