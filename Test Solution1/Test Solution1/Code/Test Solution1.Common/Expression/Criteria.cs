using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Test_Solution1.Common.Expression
{
    [DataContract]
    public class Criteria
    {
        private IOrderExpression _order;
        private IPaginationExpression _pagination;


        public Criteria()
        {
            Expressions = new List<FilterExpression>();
        }

        [DataMember]
        public List<FilterExpression> Expressions { get; set; }

        public IPaginationExpression Pagination
        {
            get
            {
                return _pagination ??
                       (_pagination = Expressions.First(exp => exp.ExpressionType == ExpressionType.Pagination));
            }
        }

        public Criteria AddFilter(FilterExpression expression)
        {
            expression.ExpressionType = ExpressionType.Filter;
            Expressions.Add(expression);
            return this;
        }

        public Criteria AddPagination(int pageNumber, int pageSize)
        {
            var pagination = new FilterExpression
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                ExpressionType = ExpressionType.Pagination
            };

            Expressions.Add(pagination);

            return this;
        }

        [DataMember]
        public SortExpression Sort { get; set; }
        public Criteria AddOrder(string column, SortDirection direction)
        {
            Expressions.Add(new FilterExpression
            {
                Column = column,
                Direction = direction,
                ExpressionType = ExpressionType.Order
            });
            return this;
        }

        public Criteria EditPagination(int pageNumber, int pageSize)
        {
            var expression = Expressions.First(exp => exp.ExpressionType == ExpressionType.Pagination);

            expression.PageNumber = pageNumber;
            expression.PageSize = pageSize;

            return this;
        }

        public IPaginationExpression GetPagination()
        {
            return _pagination ??
                   (_pagination = Expressions.First(exp => exp.ExpressionType == ExpressionType.Pagination));
        }

        public IOrderExpression GetOrder()
        {
            return _order ?? (_order = Expressions.First(exp => exp.ExpressionType == ExpressionType.Order));
        }

        public static bool IsExpressionless(Criteria crit)
        {
            return !crit.Expressions.Any();
        }
    }
}