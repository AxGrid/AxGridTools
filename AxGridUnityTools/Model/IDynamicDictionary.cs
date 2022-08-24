using System.Collections.Generic;

namespace AxGrid.Model
{
    public class IDynamicDictionary<K, V> : Dictionary<K, V>, IDynamicObject
    {
        public DynamicModel ModelLink { get; set; }
        public string ModelField { get; set; }

        public new void Add(K k, V v)
        {
            base.Add(k, v);
        }
    }
}