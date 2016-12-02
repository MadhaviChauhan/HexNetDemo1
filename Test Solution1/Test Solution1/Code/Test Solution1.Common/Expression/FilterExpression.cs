using System.Runtime.Serialization;

namespace Test_Solution1.Common.Expression
{
    [DataContract]
    public class FilterExpression : IFilterExpression,
                                    IOrderExpression,
                                    IPaginationExpression
    {
        [DataMember]
        public ExpressionType ExpressionType { get; set; }

        [DataMember]
        public string Column { get; set; }

        [DataMember]
        public FilterNames Type { get; set; }

        [DataMember]
        public object Lower { get; set; }

        [DataMember]
        public object Higher { get; set; }

        [DataMember]
        public bool IsNot { get; set; }

        [DataMember]
        public string ColumnType { get; set; }

        [DataMember]
        public SortDirection Direction { get; set; }

        [DataMember]
        public int PageNumber { get; set; }

        [DataMember]
        public int PageSize { get; set; }
    }
}