using System;
using System.Reflection;

namespace AxGrid.Utils.Reflections
{
        public class PropertyOrField : IPropertyOrField
        {
            private PropertyInfo property;
            private FieldInfo field;

            public Type ValueType => GetValueType();
            public bool IsIndexed => false;
            
            public object GetValue(object obj)
            {
                if (field != null)
                    return field.GetValue(obj);
                if (property != null)
                {
                    Console.WriteLine($"{property.Name} Type:{property.PropertyType.Name} (obj:{obj?.GetType().Name}");
                    return property.GetValue(obj);
                }

                throw new Exception("Property or field is not set");
            }

            public void SetValue(object obj, object value)
            {
                if (field != null)
                    field.SetValue(obj, value);
                else if (property != null)
                    property.SetValue(obj, value);
            }

            public string Name { get; }

            // public object GetValue(object obj, object defaultValue = null, int? index = null)
            // {
            //     if (field != null)
            //     {
            //         if (index != null)
            //         {
            //             var arr = field.GetValue(obj) as IList;
            //             if (arr == null)
            //                 return defaultValue;
            //             return arr[index.Value];
            //         }
            //         return field.GetValue(obj);
            //     }
            //     
            //     if (property != null)
            //     {
            //         if (index != null)
            //         {
            //             var arr = property.GetValue(obj) as IList;
            //             if (arr == null)
            //                 return defaultValue;
            //             return arr[index.Value];
            //         }
            //         return property.GetValue(obj);
            //     }
            //
            //     return defaultValue;
            // }
            
            public Type GetValueType()
            {
                if (field != null)
                    return field.FieldType;
                if (property != null)
                    return property.PropertyType;
                throw new Exception("Property or field is not set");
            }

            public bool IsReadOnly { get; }

            public object GetDefault()
            {
                var t = GetValueType();
                if(t.IsValueType)
                {
                    return Activator.CreateInstance(t);
                }
                return null;
            }

            // public Type GetTypeOfGenericListItems(IList list)
            // {
            //     if (list == null)
            //         return null;
            //     var t = list.GetType();
            //     if (t.IsArray)
            //         return t.GetElementType();
            //     if (t.IsGenericType)
            //     {
            //         var args = t.GetGenericArguments();
            //         if (args.Length == 1)
            //             return args[0];
            //     }
            //     return null;
            // }
           
            
            // public object SetValue(object obj, object value, int? index = null)
            // {
            //     if (field != null)
            //     {
            //         var o = field.GetValue(obj);
            //         if (o == null)
            //         {
            //             o = field.GetValue(obj) ?? Activator.CreateInstance(property.PropertyType);
            //             field.SetValue(obj, o);
            //         }
            //         if (index != null)
            //         {
            //             var arr = o as IList;
            //             if (arr == null)
            //                 throw new Exception("Cannot set value to array");
            //             while(arr.Count < index)
            //             {
            //                 arr.Add(GetDefault());
            //             }
            //             arr[index.Value] = value;
            //         }else
            //             field.SetValue(obj, value);
            //     }
            //
            //     if (property != null)
            //     {
            //         if (index != null)
            //         {
            //             var o = property.GetValue(obj);
            //             if (o == null)
            //             {
            //                 o = property.GetValue(obj) ?? Activator.CreateInstance(property.PropertyType);
            //                 property.SetValue(obj, o);
            //             }
            //             var arr = o as IList;
            //             if (arr == null)
            //                 throw new Exception("Cannot set value to array");
            //             while (index >= arr.Count)
            //             {
            //                 arr.Add(Activator.CreateInstance(GetTypeOfGenericListItems(arr)));
            //             }
            //             arr[index.Value] = value;
            //         }
            //         else
            //         {
            //             property.SetValue(obj, value);
            //         }
            //     }
            //     return obj;
            // }
            
            public PropertyOrField(PropertyInfo pi)
            {
                property = pi;
            }

            public bool IsNull(object obj)
            {
                return GetValue(obj) == null;
            }
            
            public PropertyOrField(FieldInfo fi)
            {
                field = fi;
            }

            public object GetOrCreateValue(object obj)
            {
                var o = GetValue(obj);
                if (o != null) return o;
                o = Activator.CreateInstance(GetValueType());
                SetValue(obj, o);
                return o;
            }
            
            public PropertyOrField(Type t, string name)
            {
                Name = name;
                property = t.GetProperty(name);
                field = t.GetField(name);
                if (property == null && field == null)
                    throw new Exception($"Property or field {name} not found in type {t.Name}");
            }
            
            public PropertyOrField(object obj, string name)
            {
                Name = name;
                if (obj == null)
                    throw new ArgumentException("Object is null");
                var t = obj.GetType();
                property = t.GetProperty(name);
                if (property != null)
                    return;
                field = t.GetField(name);
                if (property != null)
                    return;
                throw new ArgumentException($"Property or field ({name}) on object ({obj}) not found");
            }
        }
}