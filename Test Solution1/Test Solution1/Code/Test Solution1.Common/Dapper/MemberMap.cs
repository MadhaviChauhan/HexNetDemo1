using Dapper;
using System;
using System.Reflection;

namespace Test_Solution1.Common.Dapper
{
    public class MemberMap : SqlMapper.IMemberMap
    {
        private readonly string columnName;
        private readonly MemberInfo member;

        public MemberMap(MemberInfo member, string columnName)
        {
            this.member = member;
            this.columnName = columnName;
        }

        public string ColumnName
        {
            get { return columnName; }
        }

        public FieldInfo Field
        {
            get { return member as FieldInfo; }
        }

        public Type MemberType
        {
            get
            {
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        return ((FieldInfo)member).FieldType;
                    case MemberTypes.Property:
                        return ((PropertyInfo)member).PropertyType;
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        public ParameterInfo Parameter
        {
            get { return null; }
        }

        public PropertyInfo Property
        {
            get { return member as PropertyInfo; }
        }
    }
}