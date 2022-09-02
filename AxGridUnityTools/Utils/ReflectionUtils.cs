using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AxGrid.Utils
{
    public static class ReflectionUtils
    {
        public static IEnumerable<MethodInfo> GetAllMethodsInfo(Type type, bool includeInheritedPrivate = true)
        {
            var res = new List<MethodInfo>();
            res.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            if (includeInheritedPrivate && type.BaseType != null)
                res.AddRange(GetAllMethodsInfo(type.BaseType));
            return res;
        }
        
        public static IEnumerable<FieldInfo> GetAllFieldsInfo(Type type, bool includeInheritedPrivate = true)
        {
            var res = new List<FieldInfo>();
            res.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            if (includeInheritedPrivate && type.BaseType != null)
                res.AddRange(GetAllFieldsInfo(type.BaseType));
            return res;
        }

        public static Dictionary<string, object> GetAllFieldValues(Type type, object inst)
        {
            var res = new Dictionary<string, object>();
            foreach (var fi in GetAllFieldsInfo(type).Where(fi => !fi.IsStatic).Where(fi =>
                     {
                         if (fi.IsPrivate)
                             return fi.GetCustomAttribute(typeof(SerializeField)) != null;
                         return true;
                     }))
            {
                object val = null;
                try
                {
                    val = fi.GetValue(inst);
                }
                catch (Exception)
                {
                    // ignored
                }

                if (res.ContainsKey(fi.Name))
                    res[fi.Name] = val;
                else
                    res.Add(fi.Name, val);
            }
            return res;
        }

        public static IEnumerable<PropertyInfo> GetAllPropertiesInfo(Type type, bool includeInheritedPrivate = true)
        {
            var res = new List<PropertyInfo>();
            res.AddRange(type.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            if (includeInheritedPrivate && type.BaseType != null)
                res.AddRange(GetAllPropertiesInfo(type.BaseType));
            return res;
        }

        public static bool IsSimpleType(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
                type = type.GetGenericArguments()[0];
 
            if (type.IsEnum)
                return true;
 
            if (type == typeof(Guid))
                return true;
 
            TypeCode tc = Type.GetTypeCode(type);
            switch (tc)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                case TypeCode.Char:
                case TypeCode.String:
                case TypeCode.Boolean:
                case TypeCode.DateTime:
                    return true;
                case TypeCode.Object:
                    return (typeof(TimeSpan) == type) || (typeof(DateTimeOffset) == type);
                default:
                    return false;
            }
        }
    }
}