using System;
using System.Collections.Generic;

namespace Domain.Exceptions
{
    internal class DuplicateException : GuardException
    {
        public DuplicateException(Type entityType, Type propertyType, dynamic list, dynamic expression)
            :base(typeof(DuplicateException))
        {
            Properties.Add(nameof(entityType),entityType);
            Properties.Add(nameof(propertyType),propertyType);
            Properties.Add(nameof(list),list);
            Properties.Add(nameof(expression),expression);
        }

        public DuplicateException(Type entityType, Type propertyType, dynamic list, dynamic expression, string fieldName)
            :base(typeof(DuplicateException))
        {
            Properties.Add(nameof(entityType),entityType);
            Properties.Add(nameof(propertyType),propertyType);
            Properties.Add(nameof(list),list);
            Properties.Add(nameof(expression),expression);
            Properties.Add(nameof(fieldName),fieldName);
        }
    }
}