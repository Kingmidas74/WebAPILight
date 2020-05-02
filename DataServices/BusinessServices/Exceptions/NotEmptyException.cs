
using System.Collections.Generic;

namespace BusinessServices.Exceptions
{
    public class NotEmptyException:GuardException
    {
        public NotEmptyException(IEnumerable<object> list):base(typeof(NotEmptyException))
        {
            Properties.Add(nameof(list),list);
        }
    }
}