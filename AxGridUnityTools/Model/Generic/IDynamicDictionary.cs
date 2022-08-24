using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AxGrid.Model
{
    public class DynamicDictionary<TKey, TValue> : IDictionary<TKey, TValue>, IDynamicObject
    {
        private Dictionary<TKey, TValue> baseDict;

        public DynamicModel ModelLink { get; set; }
        public string ModelField { get; set; }

        
        
       #region constructors

        public DynamicDictionary()
        {
            baseDict = new Dictionary<TKey, TValue>();
        }

        public DynamicDictionary(int capacity)
        {
            baseDict = new Dictionary<TKey, TValue>(capacity);
        }

        public DynamicDictionary(IEqualityComparer<TKey> comparer)
        {
            baseDict = new Dictionary<TKey, TValue>(comparer);
        }

        public DynamicDictionary(int capacity, IEqualityComparer<TKey> comparer)
        {
            baseDict = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        public DynamicDictionary(IDictionary<TKey, TValue> dictionary)
        {
            baseDict = new Dictionary<TKey, TValue>(dictionary);
        }

        public DynamicDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
        {
            baseDict = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public DynamicDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection) : this(collection, null) { }

        public DynamicDictionary(IEnumerable<KeyValuePair<TKey, TValue>> collection, IEqualityComparer<TKey> comparer) : this((collection as ICollection<KeyValuePair<TKey, TValue>>)?.Count ?? 0, comparer)
        {
            if (collection == null)
            {
                throw new ArgumentException("Collection is null");
            }

            foreach (KeyValuePair<TKey, TValue> pair in collection)
            {
                baseDict.Add(pair.Key, pair.Value);
            }
        }
        
        #endregion constructors

        public void Refresh()
        {
            ModelLink.Refresh(ModelField); 
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return baseDict.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return baseDict.GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            baseDict.Add(item.Key, item.Value);
            Refresh();
            ModelLink.EventManager.Invoke($"On{ModelField}DynamicChanged", item.Key, item.Value);
        }

        public void Clear()
        {
            var keys = this.Select(kv => kv.Key).ToList();
            baseDict.Clear();
            Refresh();
            keys.ForEach(k => ModelLink.EventManager.Invoke($"On{ModelField}DynamicChanged", k, null));    
            
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return baseDict.Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (baseDict.Remove(item.Key))
            {
                Refresh();
                ModelLink.EventManager.Invoke($"On{ModelField}DynamicChanged", item.Key, null);
                return true;
            }
            return false;
        }

        public int Count => baseDict.Count;
        public bool IsReadOnly => false;
        public void Add(TKey key, TValue value)
        {
            baseDict.Add(key, value);
            Refresh();
            ModelLink.EventManager.Invoke($"On{ModelField}DynamicChanged", key, value);
        }

        public bool ContainsKey(TKey key)
        {
            return baseDict.ContainsKey(key);
        }

        public bool Remove(TKey key)
        {
            if (baseDict.Remove(key))
            {
                Refresh();
                ModelLink.EventManager.Invoke($"On{ModelField}DynamicChanged", key, null);
                return true;
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return baseDict.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => baseDict[key];
            set
            {
                baseDict[key] = value;
                Refresh();
                ModelLink.EventManager.Invoke($"On{ModelField}DynamicChanged", key, value);
            }
        }

        public ICollection<TKey> Keys => baseDict.Keys;
        public ICollection<TValue> Values => baseDict.Values;
    }
}