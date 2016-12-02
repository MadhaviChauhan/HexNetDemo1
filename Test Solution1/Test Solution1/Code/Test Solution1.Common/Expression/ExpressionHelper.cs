using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Test_Solution1.Common.Expression
{
    public static class ExpressionHelper
    {
        public static string GetPropertyName<T>(this Expression<Func<T, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("propertyRefExpr", "propertyRefExpr is null.");

            return expression.GetMemberExpression().Member.Name;

            throw new ArgumentException("No property reference expression was found.", "expression");
        }

        public static Type GetPropertyType<T>(this Expression<Func<T, object>> expression)
        {
            var memberExpr = expression.GetMemberExpression<T>();
            return memberExpr.Member is MethodInfo
                       ? ((MethodInfo)memberExpr.Member).ReturnType
                       : ((PropertyInfo)memberExpr.Member).PropertyType;

            throw new ArgumentException("No property reference expression was found.", "expression");
        }

        internal static object GetLowerValue(this object value)
        {
            return value != null
                       ? value.GetType().Equals(typeof(int))
                             ? int.MinValue
                             : value.GetType().Equals(typeof(DateTime)) ? DateTime.MinValue : value
                       : null;

            throw new ArgumentException("invalid value used. ", "value");
        }

        internal static object GetHigherValue(this object value)
        {
            return value != null
                       ? value.GetType().Equals(typeof(int))
                             ? int.MaxValue
                             : value.GetType().Equals(typeof(DateTime)) ? DateTime.MaxValue : value
                       : null;

            throw new ArgumentException("invalid value used. ", "value");
        }

        public static MemberExpression GetMemberExpression<T>(this Expression<Func<T, object>> expression)
        {
            if (expression == null)
                throw new ArgumentNullException("propertyRefExpr", "expression is null.");

            var memberExpr = expression.Body as MemberExpression;
            if (memberExpr == null)
            {
                var unaryExpr = expression.Body as UnaryExpression;
                if (unaryExpr != null && unaryExpr.NodeType == System.Linq.Expressions.ExpressionType.Convert)
                    memberExpr = unaryExpr.Operand as MemberExpression;
            }

            return memberExpr;
        }
    }
}