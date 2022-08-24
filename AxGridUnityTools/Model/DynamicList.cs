using System.Collections.Generic;

namespace AxGrid.Model
{
    public class DynamicList<T> : List<T>, IDynamicObject
    {
        public DynamicModel ModelLink { get; set; }
        public string ModelField { get; set; }
        
        
        public new void Add(T item)
        {
            base.Add(item);
            ModelLink?.Refresh(ModelField);
        }

        public new void Remove(T item)
        {
            base.Remove(item);
            ModelLink?.Refresh(ModelField);
        }
    }

}