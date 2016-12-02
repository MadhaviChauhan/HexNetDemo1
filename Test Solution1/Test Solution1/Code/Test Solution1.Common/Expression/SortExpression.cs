using System.ComponentModel;
using System.Runtime.Serialization;

namespace Test_Solution1.Common.Expression
{
    /// <summary>
    /// A SortExpression is used to specify sorting parameters
    /// </summary>
    [DataContract]
    public class SortExpression
    {
        /// <summary>
        /// Specifies the direction of the sort
        /// </summary>
        [DataMember]
        public ListSortDirection Direction { get; set; }

        /// <summary>
        /// Specifies the column to be sorted on
        /// </summary>
        [DataMember]
        public string Column { get; set; }
    }
}
