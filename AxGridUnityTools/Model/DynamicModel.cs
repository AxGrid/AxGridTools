using System;
using System.Collections;
using System.Collections.Generic;
using SmartFormat;
using System.Linq;
using AxGrid.Utils;
using log4net;
using UnityEngine.Scripting;

namespace AxGrid.Model
{
    /// <summary>
    /// Модель основанная на дикте с динамичискими полями
    /// </summary>
    public abstract class DynamicModel : IDictionary<string, object>, IDisposable
    {
        private readonly Dictionary<string, object> dataObject;
        private readonly AsyncEventManager aem;

        public AsyncEventManager EventManager => aem;


        private ILog _log;

        protected ILog Log => _log ?? (_log = LogManager.GetLogger(GetType().Name));


        public void Refresh(string key)
        {
            if (!ContainsKey(key))
                return;
            SendEvents(key, this[key], this[key]);
        }

        public void RefreshAll()
        {   
            dataObject.ToList().ForEach(kv => SendEvents(kv.Key, kv.Value, kv.Value));
        }

        public void Dispose()
        {
            aem.Dispose();
            dataObject.Clear();
        }

        public bool ToggleBool(string key, bool def)
        {
            var v = GetBool(key, def);
            Set(key, !v);
            return !v;
        }
 
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
                var before = dataObject[key];
                dataObject.Remove(key);
                SendEvents(key, null, before);
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

        protected virtual void SendEvents(string name, object value, object before)
        {
            aem.Invoke("On"+name+"Changed", value, before, this);
            aem.Invoke("ModelChanged", name, this);
        }
        
        
        
        protected virtual void SendEventsAsync(string name, object value, object before)
        {
            aem.InvokeAsync("On"+name+"Changed", value, before, this);
            aem.InvokeAsync("ModelChanged", name, this);
        }

        public DynamicModel SilentSet(string name, object obj)
        {
            if (dataObject.ContainsKey(name))
            {
                if (dataObject[name] != obj)
                {
                    var before = dataObject[name]; 
                    dataObject[name] = obj;
                }
            }
            else
            {
                dataObject.Add(name, obj);
            }

            return this;
        }
        
        [Bind("OnModelFieldDelaySet")]
        public DynamicModel Set(string name, object obj)
        {
            
            if (dataObject.ContainsKey(name))
            {
                if (dataObject[name] != obj)
                {
                    var before = dataObject[name];
                    if (obj is IDynamicObject dObj)
                    {
                        dObj.ModelLink = this;
                        dObj.ModelField = name;
                    }
                    dataObject[name] = obj;
                    SendEvents(name, obj, before);
                }
            }
            else
            {
               
                if (obj is IDynamicObject dObj)
                {
                    dObj.ModelLink = this;
                    dObj.ModelField = name;
                }
                dataObject.Add(name, obj);
                SendEvents(name, obj, null);
            }

            return this;
        }
        
        
        public DynamicModel SetAsync(string name, object obj)
        {
            
            if (dataObject.ContainsKey(name))
            {
                if (dataObject[name] != obj)
                {
                    var before = dataObject[name]; 
                    if (obj is IDynamicObject dObj)
                    {
                        dObj.ModelLink = this;
                        dObj.ModelField = name;
                    }
                    dataObject[name] = obj;
                    SendEventsAsync(name, obj, before);
                }
            }
            else
            {
                if (obj is IDynamicObject dObj)
                {
                    dObj.ModelLink = this;
                    dObj.ModelField = name;
                }
                dataObject.Add(name, obj);
                SendEventsAsync(name, obj, null);
            }
            return this;
        }

        public DynamicModel SetDelay(float delay, string fiendName, object value)
        {
            aem.InvokeDelayAsync(delay, "OnModelFieldDelaySet", fiendName, value);
            return this;

        }

        public DynamicModel SetString(string fieldName, string format, params object[] args)
        {
            Set(fieldName, Smart.Format(format, args));
            return this;
        }

        public DynamicModel SetStringAsync(string fieldName, string format, params object[] args)
        {
            SetAsync(fieldName, Smart.Format(format, args));
            return this;
        }

        
        public int GetInt(string name, int def = default(int))
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return Convert.ToInt32(dataObject[name]);
        }

        public int[] GetArrayOfInt(string name, int[] def = default)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return ConvertUtils.ConvertToInt(dataObject[name]);
        } 

        public uint[] GetArrayOfUInt(string name, uint[] def = default)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return ConvertUtils.ConvertToUInt(dataObject[name]);
        } 

        public long[] GetArrayOfLong(string name, long[] def = default)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return ConvertUtils.ConvertToLong(dataObject[name]);
        } 

        public ulong[] GetArrayOfULong(string name, ulong[] def = default)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return ConvertUtils.ConvertToULong(dataObject[name]);
        } 
        
        public string[] GetArrayOfString(string name, string[] def = default)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return ConvertUtils.ConvertToString(dataObject[name]);
        } 
        
        public bool[] GetArrayOfBool(string name, bool[] def = default)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return ConvertUtils.ConvertToBool(dataObject[name]);
        } 
        
        public float[] GetArrayOfFloat(string name, float[] def = default)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return ConvertUtils.ConvertToFloat(dataObject[name]);
        } 
        
        public double[] GetArrayOfDouble(string name, double[] def = default)
        {
            if (!dataObject.ContainsKey(name) || dataObject[name] == null)
                return def;
            return ConvertUtils.ConvertToDouble(dataObject[name]);
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

        public DynamicModel(): this( new Dictionary<string, object>()) {}
        public DynamicModel(Dictionary<string, object> construct)
        {
            dataObject = construct;
            aem = new AsyncEventManager();
            aem.Add(this);
        }

        public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
        {
            return dataObject.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dataObject.GetEnumerator();
        }

        public void Add(KeyValuePair<string, object> item)
        {
            Set(item.Key, item.Value);
        }

        public void Clear()
        {
            dataObject.Clear();
        }

        public bool Contains(KeyValuePair<string, object> item)
        {
            return dataObject.Contains(item);
        }

        public void CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
        }

        public bool Remove(KeyValuePair<string, object> item)
        {
            return Remove(item.Key);
        }

        public int Count => dataObject.Count;

        public bool IsReadOnly => false;

        public void Update(float deltaTime)
        {
            aem.Update(deltaTime);
        }
        
    }
}