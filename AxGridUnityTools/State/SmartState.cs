using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace AxGrid.State
{
    /// <summary>
    /// Smart state object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartState<T>
    {

        private readonly Dictionary<string, ISmartStateHolder> _elements = new Dictionary<string, ISmartStateHolder>();

        public ISmartStateHolder this[string key]
        {
            get
            {
                if (!_elements.ContainsKey(key))
                {
                    throw new KeyNotFoundException($"Key {key} not found");
                }
                return _elements[key];
            }
        }
        
        //Reflection scan all Properties and fields and store full path into Dictionary
        private static void GetAllElementsOfType(Dictionary<string, ISmartStateHolder> res, DPathAction lastAction, Type type, string startPath = "")
        {
            if (lastAction == null)
                lastAction = (inObject) => inObject;
            var properties = type.GetProperties(BindingFlags.Instance | BindingFlags.Public).ToList();
            
            foreach(var property in properties)
            {
                var path = $"{startPath}{property.Name}";
                var holder = new SmartStatePropertyHolder
                {
                    Name = property.Name,
                    Path = path,
                    GetObject = lastAction,
                    Property = property
                };
                res.Add(path, holder);
                Console.WriteLine($"{path} ({property.PropertyType})");
                if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    GetAllElementsOfType(res, (o) => holder.GetValue(o), property.PropertyType, path+ ".");
                }
            }
        }

        public void GetValues(T obj)
        {
            foreach (var element in _elements)
            {
                var property = element.Value as PropertyInfo;
                if (property != null)
                {
                    property.SetValue(obj, property.GetValue(obj));
                }
                else
                {
                    var field = element.Value as FieldInfo;
                    if (field != null)
                    {
                        field.SetValue(obj, field.GetValue(obj));
                    }
                }
            }
        }
        
        void Init()
        {
            GetAllElementsOfType(_elements, null, typeof(T));
        }
        
        public SmartState(T state)
        {
            Init();
        }
    }
}