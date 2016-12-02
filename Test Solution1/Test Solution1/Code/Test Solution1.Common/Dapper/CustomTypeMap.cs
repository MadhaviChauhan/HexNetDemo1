using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Test_Solution1.Common.Dapper
{
    public class CustomTypeMap<T> : SqlMapper.ITypeMap
    {
        private readonly Dictionary<string, SqlMapper.IMemberMap> members
            = new Dictionary<string, SqlMapper.IMemberMap>(StringComparer.OrdinalIgnoreCase);

        private readonly SqlMapper.ITypeMap tail;
        private readonly Type type;

        public CustomTypeMap()
        {
            type = typeof(T);

            tail = SqlMapper.GetTypeMap(typeof(T));
        }

        public Type Type
        {
            get { return type; }
        }

        public ConstructorInfo FindConstructor(string[] names, Type[] types)
        {
            return tail.FindConstructor(names, types);
        }

        public SqlMapper.IMemberMap GetConstructorParameter(
            ConstructorInfo constructor, string columnName)
        {
            return tail.GetConstructorParameter(constructor, columnName);
        }

        public SqlMapper.IMemberMap GetMember(string columnName)
        {
            SqlMapper.IMemberMap map;
            if (!members.TryGetValue(columnName, out map))
            {
                // you might want to return null if you prefer not to fallback to the
                // default implementation
                map = tail.GetMember(columnName);
            }
            return map;
        }

        public void Map(string columnName, string memberName)
        {
            members[columnName] = new MemberMap(type.GetMember(memberName).Single(), columnName);
        }


        public ConstructorInfo FindExplicitConstructor()
        {
            return tail.FindExplicitConstructor();
        }
    }
}