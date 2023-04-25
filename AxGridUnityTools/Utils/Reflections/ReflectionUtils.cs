using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;

namespace AxGrid.Utils.Reflections
{
    public static class ReflectionUtils
    {
        static Regex rx = new Regex(@"^(?<property>\w+)(\[(?<index>\w+)\])?$", RegexOptions.Compiled);

        public static object CloneObject(this object objSource)
        {
            //step : 1 Get the type of source object and create a new instance of that type
            Type typeSource = objSource.GetType();
            object objTarget = Activator.CreateInstance(typeSource);
            //Step2 : Get all the properties of source object type
            PropertyInfo[] propertyInfo = typeSource.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            //Step : 3 Assign all source property to taget object 's properties
            foreach (PropertyInfo property in propertyInfo)
            {
                //Check whether property can be written to
                if (property.CanWrite)
                {
                    //Step : 4 check whether property type is value type, enum or string type
                    if (property.PropertyType.IsValueType || property.PropertyType.IsEnum || property.PropertyType.Equals(typeof(System.String)))
                    {
                        property.SetValue(objTarget, property.GetValue(objSource, null), null);
                    }
                    //else property type is object/complex types, so need to recursively call this method until the end of the tree is reached
                    else
                    {
                        object objPropertyValue = property.GetValue(objSource, null);
                        if (objPropertyValue == null)
                        {
                            property.SetValue(objTarget, null, null);
                        }
                        else
                        {
                            property.SetValue(objTarget, CloneObject(objPropertyValue), null);
                        }
                    }
                }
            }
            return objTarget;
        }
        
        public static IEnumerable<MethodInfo> GetAllMethodsInfo(Type type, bool includeInheritedPrivate = true)
        {
            var res = new List<MethodInfo>();
            res.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            if (includeInheritedPrivate && type.BaseType != null)
            {
                res.AddRange(GetAllMethodsInfo(type.BaseType).Where(baseMethod =>
                    !res.Any(derivedMethod => derivedMethod.Name == baseMethod.Name 
                                              && derivedMethod.GetBaseDefinition() == baseMethod.GetBaseDefinition())));
            }
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

        public static object GetPathOrCreate(object obj, string path)
        {
            var parts = PartOfPath.Get(path);
            var current = obj;
            if (current == null)
                throw new AggregateException("Object is null");
        
            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part.Name)) continue;
                var pof = new PropertyOrField(current, part.Name);
                object item = pof.GetValue(obj);
                if (item == null)
                {
                    item = Activator.CreateInstance(pof.GetValueType());
                    pof.SetValue(obj, item);
                }
                current = item;
            }

            return current;
        }


        
        public static object Get(object obj, string path, object defaultValue = null)
        {
            var parts = PartOfPath.Get(path);
            
            var current = obj;
            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part.Name)) continue;
                
               
                if (current == null)
                    return defaultValue;
                var type = current.GetType();
                var field = type.GetField(part.Name);
                if (field != null)
                {
                    
                    if (part.Index.HasValue)
                    {
                        var arr = field.GetValue(current) as IList;
                        if (arr == null)
                            return defaultValue;
                        if (part.Index >= arr.Count || part.Index < 0)
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
                        if (part.Index >= arr.Count || part.Index < 0)
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

        
        public static List<string> ClearEvents(List<string> events)
        {
            var res = new List<string>();
            foreach (var eventName in events.OrderBy(i => i.Length))
            {
                if (res.Any(i => eventName.StartsWith(i)))
                    continue;
                res.Add(eventName);
            }

            return res;
        }

        public static List<string> GetEvents(string path)
        {
            var res = new List<string>();
            var currentEvent = new List<string>();
            res.Add("");
            if (string.IsNullOrEmpty(path))
                return res;
            foreach (var str in path.Split('.'))
            {
                var m = rx.Match(str);
                if (!m.Success)
                    throw new Exception($"Invalid path {path}");
                
                if (m.Groups["index"].Success)
                {
                    currentEvent.Add(m.Groups["property"].Value);
                    res.Add(currentEvent.Aggregate((a,b) => a+"."+b));
                    currentEvent.RemoveAt(currentEvent.Count-1);
                    currentEvent.Add(m.Groups["property"].Value + "[" + m.Groups["index"].Value + "]");
                    res.Add(currentEvent.Aggregate((a,b) => a+"."+b));
                }
                else
                {
                    currentEvent.Add(m.Groups["property"].Value);
                    res.Add(currentEvent.Aggregate((a,b) => a+"."+b));
                }
            }

            return res;
        }
        
        public class PartOfPath
        {
            public string Path { get; set; }
            public string Name { get; set; }
            public int? Index { get; set; } = null;

            public static PartOfPath _empty = new PartOfPath { Path = "", Name = ""};
           
            public static List<PartOfPath> Get(string fullPath)
            {
                
                var res = new List<PartOfPath>();
                res.Add(_empty);
                if (string.IsNullOrEmpty(fullPath))
                    return res;
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