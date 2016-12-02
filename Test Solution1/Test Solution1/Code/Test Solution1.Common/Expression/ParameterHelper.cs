using System;
using System.ComponentModel;

namespace Test_Solution1.Common.Expression
{
    public static class ParameterHelper
    {
        public static Criteria New()
        {
            return new Criteria();
        }

        public static Criteria Add(this Criteria criteria, FilterExpression expression)
        {
            var columnType = Type.GetType(expression.ColumnType);

            if (!columnType.IsValueType)
            {
                if (Equals(expression.Lower, null) == false)
                    if (expression.Lower != null || expression.Higher != null)
                        if (expression.Lower.ToString() != "")
                            criteria = criteria.AddFilter(expression);
            }
            else criteria = criteria.AddFilter(expression);
            return criteria;
        }

        public static Criteria Add<T>(this Criteria criteria, string name, T value)
        {
            criteria = criteria.AddFilter(GetFilterExpression(name, value));
            return criteria;
        }

        /// <summary>
        /// Adds filters for paging to the criteria.
        /// </summary>
        /// <param name="criteria">The Criteria object to be updated</param>
        /// <param name="pageNumber">Number of the page to return</param>
        /// <param name="pageSize">The size of a page of results</param>
        /// <returns>The modified criteria</returns>
        public static Criteria AddPaging(this Criteria criteria, int pageNumber, int pageSize)
        {
            criteria.AddFilter(new FilterExpression { Column = "PageNumber", Lower = pageNumber, Type = FilterNames.Eq });
            criteria.AddFilter(new FilterExpression { Column = "PageSize", Lower = pageSize, Type = FilterNames.Eq });
            return criteria;
        }

        /// <summary>
        /// Adds a sort expression to the criteria.
        /// </summary>
        /// <param name="criteria">The Criteria object to be updated</param>
        /// <param name="column">Name of the column to sort on</param>
        /// <param name="direction">The sort direction</param>
        /// <returns>The modified criteria</returns>
        public static Criteria AddSort(this Criteria criteria, string column, ListSortDirection direction)
        {
            criteria.Sort = new SortExpression
            {
                Direction = direction,
                Column = column
            };
            return criteria;
        }

        private static FilterExpression GetFilterExpression<T>(string columnName, T value)
        {
            return new FilterExpression()
            {
                Column = columnName,
                ColumnType = typeof(T).AssemblyQualifiedName,
                Lower = value,
                Higher = null,
                Type = FilterNames.Eq
            };
        }
    }

}
