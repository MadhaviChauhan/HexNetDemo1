using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Test_Solution1.TestFramework
{
    public static class TestExtension
    {
        public static string GetAccessorMemberFullName<TIn, TOut>(this Expression<Func<TIn, TOut>> expression)
        {
            // This method was created to get full name of a member in case of nested/complex type because rules violation
            // exception returns full member name and validation fails when comparing with accessor's member name.
            // This method was introduced to resolve the blocker faced during development, author of this class needs to validate and do changes accordingly.
            MemberExpression memberExpression;
            var propertyNames = new List<string>();
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var unaryExpression = expression.Body as UnaryExpression;
                    memberExpression = ((unaryExpression != null) ? unaryExpression.Operand : null) as MemberExpression;
                    break;
                default:
                    memberExpression = expression.Body as MemberExpression;
                    break;
            }

            while (memberExpression != null)
            {
                var propertyName = memberExpression.Member.Name;
                var propertyType = memberExpression.Type;

                propertyNames.Add(propertyName);

                memberExpression = memberExpression.Expression as MemberExpression;
            }

            propertyNames.Reverse();
            return string.Join(".", propertyNames);
        }
    }
}
