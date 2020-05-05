using System;

namespace Domain.Exceptions
{
    public class CountException : GuardException
    {
        public CountException(Type entityType, dynamic list, int maxElements, int listLength) : base(typeof(CountException))
        {
            Properties.Add(nameof(entityType),entityType);
            Properties.Add(nameof(list),list);
            Properties.Add(nameof(maxElements),maxElements);
            Properties.Add(nameof(listLength),listLength);
        }
    }
}