using System;
using System.Collections.Generic;
using System.Linq;

namespace Domain.Extensions {
    public static class IEnumerableExtensions {
        public static bool HaveDuplicates<TEntity, TProperty> (this IEnumerable<TEntity> list, Func<TEntity, TProperty> expression) {
            return list.Count () != list.Select (expression).Distinct ().Count ();
        }
    }
}