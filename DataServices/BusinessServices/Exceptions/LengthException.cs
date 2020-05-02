using System;

namespace BusinessServices.Exceptions
{
    public class LengthException : GuardException
    {
        public LengthException(string input, int inputLength) : base(typeof(LengthException))
        {
            Properties.Add(nameof(input),input);
            Properties.Add(nameof(inputLength),inputLength);
        }
    }
}