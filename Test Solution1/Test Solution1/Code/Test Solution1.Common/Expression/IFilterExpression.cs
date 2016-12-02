namespace Test_Solution1.Common.Expression
{
    public interface IFilterExpression
    {
        string Column { get; set; }
        FilterNames Type { get; set; }
        string ColumnType { get; set; }
        object Higher { get; set; }
        bool IsNot { get; set; }
        object Lower { get; set; }
    }
}