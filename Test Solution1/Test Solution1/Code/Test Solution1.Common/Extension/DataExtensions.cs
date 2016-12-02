using Test_Solution1.Common.Expression;
using Dapper;
using System.Collections;
using System.Text;

namespace Test_Solution1.Common.Extension
{
    public static class DataExtensions
    {
        public static DynamicParameters ToDynamicParamaters(this Criteria criteria)
        {
            var param = new DynamicParameters();
            criteria.Expressions.ForEach(item => ConvertExpression(param, item));
            return param;
        }

        private static void ConvertExpression(DynamicParameters param, FilterExpression item)
        {
            if (item.Type == FilterNames.Between ||
                item.Type == FilterNames.Gt ||
                item.Type == FilterNames.Lt)
                AddMultipleParameters(param, item.Column, item.Lower, item.Higher);
            else if (item.Type == FilterNames.In)
                AddCollectionParameters(param, item.Column, item.Lower);
            else if (item.Type == FilterNames.NotEq)
                AddNotEqParameters(param, item.Column, item.Lower);
            else
                param.Add(item.Column, item.Lower);
        }

        private static void AddNotEqParameters(DynamicParameters param, string column, object lower)
        {
            param.Add(string.Format("NotEq{0}", column), lower);
        }

        private static void AddMultipleParameters(DynamicParameters param, string column, object lower, object higher)
        {
            param.Add(string.Format("L{0}", column), lower);
            param.Add(string.Format("H{0}", column), higher);
        }

        private static void AddCollectionParameters(DynamicParameters param, string column, object lower)
        {
            var sb = new StringBuilder();
            if (lower is ICollection)
            {
                var enumerator = ((ICollection)lower).GetEnumerator();
                while (enumerator.MoveNext())
                {
                    sb.Append(enumerator.Current == null ? "" : string.Format("{0}{1}", enumerator.Current, ","));
                }
            }
            param.Add(column, sb.ToString().TrimEnd(','));
        }
    }
}