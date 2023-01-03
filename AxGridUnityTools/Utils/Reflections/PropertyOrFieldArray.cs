using System;
using System.Collections;
using System.Reflection;

namespace AxGrid.Utils.Reflections
{
    public class PropertyOrFieldArray : IPropertyOrField
    {
        
        // private PropertyInfo property;
        // private FieldInfo field;
        
        public Type ValueType { get; set; }
        private Type ListItemType { get; set; }

        public bool IsIndexed => true;

        public bool IsReadOnly { get; } = false;

        public int Count(object obj)
        {
            var arr = obj as IList;
            return arr?.Count ?? 0;
        } 
        
        public PropertyOrFieldArray(object o):this(o.GetType()) { }
        public PropertyOrFieldArray(Type t)
        {
            // property = t.GetProperty(name);
            // if (property != null)
            // {
            //     IsReadOnly = property.CanWrite == false;
            // }
            // field = t.GetField(name);
            // if (property == null && field == null)
            //     throw new Exception($"Property or field {name} not found in type {t.Name}");
            // Console.WriteLine("Type: " + t.Name + " Name: " + name);
            ValueType = t;
            if (t.IsArray)
            {
                ListItemType = t.GetElementType();
            }
            else if (t.IsGenericType)
            {
                var ti = t.GetGenericArguments();
                if (ti.Length == 1)
                    ListItemType = ti[0];
            } else
                throw new ArgumentException("Object is not a list or array");
        }

        public object GetValue(object obj, int index)
        {
            
            var arr = obj as IList;
            if (arr == null)
                throw new ArgumentException("Field is not a list or array");
            return arr[index];
            
            // if (property != null)
            // {
            //     var arr = property.GetValue(obj) as IList;
            //     if (arr == null)
            //         throw new ArgumentException("Field is not a list or array");
            //     return arr[index];
            // }
            // else
            // {
            //     var arr = field.GetValue(obj) as IList;
            //     if (arr == null)
            //         throw new ArgumentException("Field is not a list or array");
            //     return arr[index];
            // }
        }
        
        public object GetDefaultItem()
        {
            return Activator.CreateInstance(ListItemType);
        }

        public void SetValue(object obj, int index, object value)
        {
            
            var arr = obj as IList;
            while(arr.Count < index)
            {
                arr.Add(GetDefaultItem());
            }
            if (arr.Count == index)
                arr.Add(value);
            else
                arr[index] = value;

            // if (property != null)
            // {
            //     var arr = property.GetValue(obj) as IList;
            //     if (arr == null)
            //         throw new Exception("Cannot set value to array");
            // }
            // else
            // {
            //     var arr = field.GetValue(obj) as IList;
            //     if (arr == null)
            //         throw new Exception("Cannot set value to array");
            //     while(arr.Count < index)
            //     {
            //         arr.Add(GetDefaultItem());
            //     }
            //     arr[index] = value;
            // }
            
        }

        public string Name { get; }
        
        public Type GetTypeOfGenericListItems(IList list)
        {
            if (list == null)
                return null;
            var t = list.GetType();
            if (t.IsArray)
                return t.GetElementType();
            if (t.IsGenericType)
            {
                var args = t.GetGenericArguments();
                if (args.Length == 1)
                    return args[0];
            }
            return null;
        }
        
    }
}