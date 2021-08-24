using System.Collections.Generic;
using UnityEngine;

namespace AxGrid.Tools
{
    
    public class AxSpriteObjectCollectionSimple<K> : ScriptableObject
    {
        public List<OKeySprite<K>> list = new List<OKeySprite<K>>();

        private readonly Dictionary<K, Sprite> map = new Dictionary<K, Sprite>();
        
        public Sprite get(K key, Sprite def = null)
        {
            if (map.ContainsKey(key))
                return map[key] ?? def;
            var kv = list.Find(item => item.OKey.Equals(key));
            if (kv != null) map.Add(kv.OKey, kv.OValue);
            else map.Add(key, null);
            return kv == null || kv.OValue == null ? def : kv.OValue;
        }

        public int Count => list.Count;
        
        
    }
}