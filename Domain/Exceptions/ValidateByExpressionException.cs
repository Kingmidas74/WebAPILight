using System;
using System.Linq.Expressions;

namespace Domain.Exceptions
{
    public class ValidateByExpressionException:GuardException
    {
        public ValidateByExpressionException(Type entityType,dynamic entity, Expression expression):base(typeof(ValidateByExpressionException))
        {
            Properties.Add(nameof(entityType), entityType);
            Properties.Add(nameof(entity), entity);
            Properties.Add(nameof(expression), expression);
        }
    }
}