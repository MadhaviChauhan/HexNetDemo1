using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Test_Solution1.Common.Extension
{
    public static class DictionaryExtension
    {
        public static Dictionary<string, object> MapToDictionary(this object obj, Dictionary<string, object> dict = null)
        {
            if (dict == null)
            {
                dict = new Dictionary<string, object>();
            }
            foreach (var property in obj.GetType().GetProperties())
            {
                var propertyValue = property.GetGetMethod().Invoke(obj, null);

                if (!IsUserDefinedType(property) && !IsGenericList(property))
                {
                    if (!dict.ContainsKey(property.Name))
                        dict.Add(property.Name, propertyValue == null ? null : propertyValue);
                    else
                        dict[property.Name] = propertyValue == null ? null : propertyValue;
                }
            }
            return dict;
        }

        public static T MapToObject<T>(this Dictionary<string, object> dict, T obj = null) where T : class, new()
        {
            if (obj == null)
                obj = new T();

            foreach (var property in obj.GetType().GetProperties())
            {
                var propertyValue = property.GetGetMethod().Invoke(obj, null);

                if (!IsUserDefinedType(property) && !IsGenericList(property))
                {
                    if (dict.ContainsKey(property.Name))
                    {
                        if (property.Name != "Id" && property.CanWrite)
                        {
                            var type = property.PropertyType;

                            if (dict[property.Name] != null)
                            {
                                type = Nullable.GetUnderlyingType(type) ?? type;
                                property.SetValue(obj, Convert.ChangeType(dict[property.Name], type));
                            }
                            else
                                property.SetValue(obj, dict[property.Name]);
                        }
                    }
                }
            }
            return obj;
        }

        private static Boolean IsUserDefinedType(PropertyInfo propInfo)
        {
            return
                !(propInfo.PropertyType.Namespace == "System" || propInfo.PropertyType.Namespace.StartsWith("System"));
        }

        private static Boolean IsGenericList(PropertyInfo propInfo)
        {
            return typeof(IList).IsAssignableFrom(propInfo.PropertyType) && propInfo.PropertyType.IsGenericType;
        }
    }
}