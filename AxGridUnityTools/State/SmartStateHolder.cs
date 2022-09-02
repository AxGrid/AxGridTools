using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Reflection;

namespace AxGrid.State
{

    public delegate object DPathAction(object obj);
    public class SmartStatePropertyHolder : ISmartStateHolder
    {
        public DPathAction GetObject { get; set; }

        public string Path { get; set; }
        public string Name { get; set; }
        public PropertyInfo Property { get; set; }
        public object GetValue(object obj)
        {
            return obj == null ? null : Property.GetValue(GetObject(obj));
        }
    }

    public interface ISmartStateHolder
    {
        DPathAction GetObject { get; }
        string Path { get; }
        string Name { get; }
        object GetValue(object obj);
    }
}