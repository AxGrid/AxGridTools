using System;
using System.Collections.Generic;
using System.Reflection;

namespace AxGrid.Utils
{
    public static class MethodUtils
    {
        public static List<MethodInfo> GetAllMethodsInfo(Type type, bool includeInheritedPrivate = true)
        {
            List<MethodInfo> res = new List<MethodInfo>();
            res.AddRange(type.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance));
            if (includeInheritedPrivate && type.BaseType != null)
                res.AddRange(GetAllMethodsInfo(type.BaseType));
            return res;
        }
    }
}