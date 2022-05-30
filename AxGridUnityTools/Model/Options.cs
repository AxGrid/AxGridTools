using System;
using System.Collections;
using System.Collections.Generic;
using SmartFormat;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace AxGrid.Model {
    
    public class Options : IDictionary<string, object> {
        
        private readonly Dictionary<string, object> dataObject;
        private readonly List<string> saveKeys = new List<string>();

        public List<string> SaveKeys => saveKeys;
        
        public string Toggle(string key, params string[] objects)
        {
            if (objects.Length == 0)
                return null;

            string o = Get<string>(key);
            if (String.IsNullOrEmpty(o))
            {
                Set(key, objects[0]);
                return objects[0];
            }
            
            List<string> l = new List<string>(objects);
            int index = l.IndexOf(o);
            if (index == -1)
            {
                Set(key, objects[0]);
                return objects[0];
            }
            index++;
            Set(key, objects[index % objects.Length]);
            return objects[index % objects.Length];
        }


        public Options SaveKey(string key) {
            saveKeys.Add(key);
            return this;
        } 
        
        public void Add(string key, object value)
        {
            Set(key, value);
        }

        public bool ContainsKey(string key)
        {
            return dataObject.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            if (dataObject.ContainsKey(key))
            {
                dataObject.Remove(key);
                return true;
            } 
            return false;
        }

        
        
        public bool TryGetValue(string key, out object value)
        {
            return dataObject.TryGetValue(key, out value);
        }

        public object this[string name]
        {
            get => dataObject[name];
            set => Set(name, value);
        }

        public ICollection<string> Keys => dataObject.Keys;

        public ICollection<object> Values => dataObject.Values;
        
        public Options Set(string name, object obj)
        {
            
            if (dataObject.ContainsKey(name))
                dataObject[name] = obj;
            else
                dataObject.Add(name, obj);
            return this;
        }
        
        public Options SetString(string fieldName, string format, params object[] args)
        {
            Set(fieldName, Smart.Format(format, args));
            return this;
        }
        
        public int GetInt(string name, int def = default(int))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return Convert.ToInt32(dataObject[name]);
        }
        
        public bool GetBool(string name, bool def = default(bool))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return Convert.ToBoolean(dataObject[name]);
        }
        
        public double GetDouble(string name, double def = default(double))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return Convert.ToDouble(dataObject[name]);
        }
        
        public float GetFloat(string name, float def = default(float))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return Convert.ToSingle(dataObject[name]);
        }
        
        public Options GetOptions(string name, Options def = default(Options))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return (Options)dataObject[name];
        }
        
        public long GetLong(string name, long def = default(long))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return Convert.ToInt64(dataObject[name]);
        }

        public string GetString(string name, string def = default(string))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null || dataObject[name].ToString() == "")
                return def;
            return Convert.ToString(dataObject[name]);
        }
        
        [Preserve]
        public T Get<T>(string name, T def = default(T))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return (T)dataObject[name];
        }

        [Preserve]
        public object Get(string name, object def = null)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return dataObject[name];
        }

        
        public List<T> GetList<T>(string name, List<T> def = default(List<T>))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return (List<T>)dataObject[name];
        }
        
        public void Inc(string name, int value = 1)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
            {
                Set(name, value);
            }
            else
            {
                var o = dataObject[name];
                if (o is long)
                    Set(name, (long) o+ value);
                if (o is int)
                    Set(name, (int) o+ value);
            }
        }

        public void Dec(string name, int value = 1)
        {
            Inc(name, -value);
        }
        
        public Options(): this( new Dictionary<string, object>()) {}
        public Options(Dictionary<string, object> construct)
        {
            dataObject = construct;
        }
        
        public IEnumerator<KeyValuePair<string, object>> GetEnumerator() => dataObject.GetEnumerator();
        public void Add(KeyValuePair<string, object> item) => Set(item.Key, item.Value);
        public void Clear() => dataObject.Clear();

        public bool Contains(KeyValuePair<string, object> item) => dataObject.Contains(item);

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
        }

        public bool Remove(KeyValuePair<string, object> item) => Remove(item.Key);

        public int Count => dataObject.Count;

        public bool IsReadOnly => false;

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        
        static Serializer _serializer = new SerializerBuilder()
            .WithNamingConvention(new CamelCaseNamingConvention())
            .Build();

        private static Deserializer _deserializer = new DeserializerBuilder()
            .WithNamingConvention(new CamelCaseNamingConvention())
            .Build();
        
        /// <summary>
        /// Сохранить в Unity3d PlayerPrefs
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static Options LoadPrefs(string name = "yopts") {
            var str = PlayerPrefs.GetString(name);
            return LoadFromString(str);
        }
        
        public static Options LoadFromString(string str) {
            if (string.IsNullOrEmpty(str))
                return new Options();
            return new Options(_deserializer.Deserialize<Dictionary<string, object>>(str));
        }

        /// <summary>
        /// Считать из Unity3d PlayerPrefs
        /// </summary>
        /// <param name="name"></param>
        /// <param name="save"></param>
        public void SavePrefs(string name = "yopts", bool allKeys = false, bool save = true) {
            var s = SaveAsString(allKeys);
            PlayerPrefs.SetString(name, s);
            if (save) PlayerPrefs.Save();
        }

        public string SaveAsString(bool allKeys = false)
        {
            var d = allKeys
                ? dataObject
                : dataObject.Where(i => SaveKeys.Contains(i.Key)).ToDictionary(i => i.Key, i => i.Value);
            return _serializer.Serialize(d);
        } 
    }
}