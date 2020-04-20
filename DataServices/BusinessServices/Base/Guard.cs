using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BusinessServices.Exceptions;
using BusinessServices.Extensions;
using Domain.Exceptions;

namespace BusinessServices.Base {
    public static class Guard {
        public static void NotNull (object value) {
            if (value == null) {
                throw new InvalidInputException ("Входной параметр должен иметь значение");
            }
        }

        public static void NotNull (object value, string name) {
            if (value == null) {
                throw new InvalidInputException ("Параметр " + name + " должен иметь значение");
            }
        }

        public static void DontHaveDuplicates<TEntity, TProperty> (List<TEntity> list, Func<TEntity, TProperty> expression, string fieldName = null) {
            if (list.HaveDuplicates (expression)) {
                if (fieldName != null) {
                    throw new WebApiDomainExceptionBase ("В списке не может быть двух одинаковых записей с ключем " + fieldName);
                }
                throw new WebApiDomainExceptionBase ("В списке не может быть двух одинаковых записей");
            }
        }

        public static void MaxLengthRestricted (string input, int inputLength) {
            if (input != null && input.Length > inputLength) {
                throw new InvalidInputException ("Значение поля не должно превышать " + inputLength + " символов");
            }
        }

        public static void MaxCountRestricted<TEntity> (List<TEntity> list, int maxElements, string message = "Превышено допустимое количество элементов в списке") {
            if (list.Count > maxElements) {
                throw new WebApiDomainExceptionBase (message);
            }
        }

        public static void FutureDate (DateTime datetime, string name = null) {
            if (!(datetime.Date >= DateTime.Now.Date)) {
                if (name != null) {
                    throw new InvalidInputException (string.Format ("{0} меньше текущей даты: {1} : {2}", name, datetime.Date, DateTime.Now.Date));
                }
                throw new InvalidInputException ("Дата меньше текущей даты");
            }
        }

        public static void ValidateExpression<TEntity> (TEntity entity, Expression<Func<TEntity, bool>> expression, string text = null) {
            var valid = expression.Compile ().Invoke (entity);
            if (!valid) {
                if (text != null) {
                    throw new InvalidInputException (text);
                }
                throw new InvalidInputException ("Условие " + expression.Body.ToString () + " не выполненно");
            }
        }

        public static void NotEmptyCollection (IEnumerable<object> value, string text = null) {
            if (value == null || !value.Any ()) {
                throw new InvalidInputException (text == null ? "Коллекция не может быть пустой" : string.Format ("Коллекция {0} не может быть пустой", text));
            }
        }
    }
}