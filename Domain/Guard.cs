using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Domain.Exceptions;
using Domain.Extensions;

namespace Domain {
    public static class Guard {
        public static void NotNull (object value) {
            if (value != null) return;
            throw new NullException();
        }

        public static void NotNull (object value, string name) {
            if (value != null) return;
            throw new NullException(name);
        }

        public static void DontHaveDuplicates<TEntity, TProperty> (List<TEntity> list, Func<TEntity, TProperty> expression, string fieldName = null) {
            if (!list.HaveDuplicates (expression)) return;
            throw !String.IsNullOrEmpty(fieldName) 
                ? new DuplicateException(typeof(TEntity), typeof(TProperty), list, expression, nameof(fieldName))
                : new DuplicateException(typeof(TEntity), typeof(TProperty), list, expression);
        }

        public static void MaxLengthRestricted (string input, int inputLength) {
            if (String.IsNullOrEmpty(input) || input.Length <= inputLength) return;
            throw new LengthException (input,inputLength);
        }

        public static void MaxCountRestricted<TEntity> (List<TEntity> list, int maxElements) {
            if (list.Count <= maxElements) return;
            throw new CountException (typeof(TEntity),list,maxElements,list.Count);            
        }

        public static void FutureDate (DateTime datetime, string name = null) {
            if (datetime.Date >= DateTime.Now.Date) return;
            throw name != null ? new FutureDateException (datetime.Date, name)
                                : new FutureDateException (datetime.Date);            
        }

        public static void ValidateExpression<TEntity> (TEntity entity, Expression<Func<TEntity, bool>> expression) {
            if (expression.Compile ().Invoke (entity)) return;
            throw new ValidateByExpressionException (typeof(TEntity), entity, expression.Body);
        }

        public static void NotEmptyCollection (IEnumerable<object> value) {
            if (value != null && value.Any ()) return;
            throw new NotEmptyException (value);
            
        }
    }
}