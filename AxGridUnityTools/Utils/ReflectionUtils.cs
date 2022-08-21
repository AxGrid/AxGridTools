using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AxGrid.Utils
{
    public static class ReflectionUtils
    {
        public static List<MethodInfo> GetAllMethodsInfo(Type type, bool includeInheritedPrivate = true)
        {
            List<MethodInfo> res = new List<MethodInfo>();
            res.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            if (includeInheritedPrivate && type.BaseType != null)
                res.AddRange(GetAllMethodsInfo(type.BaseType));
            return res;
        }
        
        public static List<FieldInfo> GetAllFieldsInfo(Type type, bool includeInheritedPrivate = true)
        {
            List<FieldInfo> res = new List<FieldInfo>();
            res.AddRange(type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            if (includeInheritedPrivate && type.BaseType != null)
                res.AddRange(GetAllFieldsInfo(type.BaseType));
            return res;
        }

        public static Dictionary<string, object> GetAllFieldValues(Type type, object inst)
        {
            Dictionary<string, object> res = new Dictionary<string, object>();
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
    }
}