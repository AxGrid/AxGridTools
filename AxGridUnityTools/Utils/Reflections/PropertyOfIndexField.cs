using System;
using System.Collections;

namespace AxGrid.Utils.Reflections
{
    public class PropertyOfIndexField
    {
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
        
        
        private object GetValue(object obj, string name, int index, object defaultValue = null)
        {
            var pof = new PropertyOrField(ListType, name);
            return pof.GetValue(obj, defaultValue, index);
        }
        
        public Type ListType { get; }
        
        public PropertyOfIndexField(object obj, )
        {
            ListType = GetTypeOfGenericListItems(obj as IList);
            POF = new PropertyOrField(ListType)
        }
    }
}