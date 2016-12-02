using System;
using System.Linq.Expressions;

namespace Test_Solution1.Common.Expression
{

    public static class Parameter<T>
    {
        public static FilterExpression Between(Expression<Func<T, object>> param, object lo, object hi)
        {
            return GetFilter(param, FilterNames.Between, lo, hi);
        }

        public static FilterExpression Add(Expression<Func<T, object>> param, object value)
        {
            return GetFilter(param, FilterNames.Eq, value, null);
        }

        public static FilterExpression NotEq(Expression<Func<T, object>> param, object value)
        {
            return GetFilter(param, FilterNames.NotEq, value, null);
        }

        private static FilterExpression GetFilter(Expression<Func<T, object>> param, FilterNames filterType,
                                                  object obj1, object obj2)
        {
            return new FilterExpression
            {
                Column = param.GetPropertyName(),
                Lower = obj1,
                Higher = obj2,
                Type = filterType,
                ColumnType = param.GetPropertyType().AssemblyQualifiedName
            };
        }

    }

}
