using System;
using System.Collections;
using System.Linq.Expressions;

namespace Test_Solution1.Common.Expression
{
    public static class Restriction<T>
    {
        public static FilterExpression Between(Expression<Func<T, object>> param, object lo, object hi)
        {
            return GetFilter(param, FilterNames.Between, lo, hi);
        }

        public static FilterExpression Eq(Expression<Func<T, object>> param, object value)
        {
            return GetFilter(param, FilterNames.Eq, value, null);
        }

        public static FilterExpression NotEq(Expression<Func<T, object>> param, object value)
        {
            return GetFilter(param, FilterNames.NotEq, value, null);
        }

        public static FilterExpression Gt(Expression<Func<T, object>> param, object value)
        {
            return GetFilter(param, FilterNames.Gt, value, value.GetHigherValue());
        }

        public static FilterExpression Lt(Expression<Func<T, object>> param, object value)
        {
            return GetFilter(param, FilterNames.Lt, value.GetLowerValue(), value);
        }

        public static FilterExpression In(Expression<Func<T, object>> param, object[] values)
        {
            return GetFilter(param, FilterNames.In, values, null);
        }

        public static FilterExpression In(Expression<Func<T, object>> param, ICollection values)
        {
            return GetFilter(param, FilterNames.In, values, null);
        }

        public static FilterExpression Like(Expression<Func<T, object>> param, object value)
        {
            return GetFilter(param, FilterNames.Like, value, null);
        }

        public static FilterExpression Not(FilterExpression expression)
        {
            expression.IsNot = !expression.IsNot;
            return expression;
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

        public static string GetName(Expression<Func<T, object>> expression)
        {
            return expression.GetPropertyName();
        }
    }
}