using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
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

        public static bool IsRepeatedField(Type type)
        {
            if (type.IsArray)
                return true;
            if (typeof(IList).IsAssignableFrom(type))
                return true;
            return false;
        }
        

        
        public static object Get(object obj, string path, object defaultValue = null)
        {
            var parts = PartOfPath.Get(path);
            var current = obj;
            foreach (var part in parts)
            {
                if (current == null)
                    return defaultValue;
                var type = current.GetType();
                var field = type.GetField(part.Name);
                if (field != null)
                {
                    if (part.Index != null)
                    {
                        var arr = field.GetValue(current) as IList;
                        if (arr == null)
                            return defaultValue;
                        current = arr[part.Index.Value];
                    }
                    else
                    {
                        current = field.GetValue(current);
                    }
                    continue;
                }
                var property = type.GetProperty(part.Name);
                if (property != null)
                {
                    if (part.Index != null)
                    {
                        var arr = property.GetValue(current) as IList;
                        if (arr == null)
                            return defaultValue;
                        current = arr[part.Index.Value];
                    }
                    else
                    {
                        current = property.GetValue(current);
                    }
                    continue;
                }
                return defaultValue;
            }
            return current;
        }

        public class PartOfPath
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public int? Index { get; set; } = null;
            
            
            static Regex rx = new Regex(@"^(?<property>\w+)(\[(?<index>\w+)\])?$", RegexOptions.Compiled);
            public static List<PartOfPath> Get(string fullPath)
            {
                var res = new List<PartOfPath>();
                foreach (var str in fullPath.Split('.'))
                {
                    var m = rx.Match(str);
                    if (!m.Success)
                        throw new Exception($"Invalid path {str}");
                    var p = new PartOfPath
                    {
                        Path = str,
                        Name = m.Groups["property"].Value,
                        Index = m.Groups["index"].Success ? (int?)(int.Parse(m.Groups["index"].Value)) : null
                    };
                    res.Add(p);
                }
                return res;
            }
        }
    }
    
}