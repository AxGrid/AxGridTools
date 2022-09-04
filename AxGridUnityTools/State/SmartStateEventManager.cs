using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AxGrid.Model;
using AxGrid.Utils;
using log4net;
using SmartFormat;

namespace AxGrid.State
{
    public class SmartStateEventManager 
    {
        private static readonly ILog Log = LogManager.GetLogger("StateEventManager");
        
        readonly Dictionary<string, List<AsyncEventManager.MethodInfoObject>> _eventListeners = new Dictionary<string, List<AsyncEventManager.MethodInfoObject>>();
        
        public void AddEvent(string eventName)
        {
             ReflectionUtils.PartOfPath.Get(eventName);
        }

        public void Add(object o)
        {
            if (o == null) return;
            ReflectionUtils.GetAllMethodsInfo(o.GetType()).Where(mi => mi.GetCustomAttribute<SmartStateAttribute>() != null).ForEach(
                mi =>
                {
                    var a = mi.GetCustomAttribute<SmartStateAttribute>();
                    var mio = new AsyncEventManager.MethodInfoObject
                    {
                        Method = mi,
                        Target = o,
                    };
                    Add(a, mio);
                });
            
        }
        
        protected void Add(SmartStateAttribute bind, AsyncEventManager.MethodInfoObject mio, string realEventName=null)
        {
            var eventNames = !string.IsNullOrEmpty(realEventName) ? realEventName : bind.EventName ?? mio.Method.Name;
            if (eventNames.Contains("{") && mio.Target != null)
                eventNames = Smart.Format(eventNames, ReflectionUtils.GetAllFieldValues(mio.Target.GetType(), mio.Target));
            mio.EventName = eventNames;
            foreach (var eventName in ReflectionUtils.GetEvents(eventNames))
            {
                if (Log.IsDebugEnabled) Log.Debug($"Add state-event [{eventName}] for {mio.Target?.GetType().Name}");
                if (!_eventListeners.ContainsKey(eventName))
                    _eventListeners.Add(eventName, new List<AsyncEventManager.MethodInfoObject>());
                if (!_eventListeners[eventName].Contains(mio))
                    _eventListeners[eventName].Add(mio);
            }
        }
            
        public void Invoke(string eventName, object state)
        {
            if (!_eventListeners.ContainsKey(eventName)) return;
            foreach (var methodInfoObject in _eventListeners[eventName])
            {
                try
                {
                    
                    var oo = ReflectionUtils.Get(state, methodInfoObject.EventName);
                    if (Log.IsDebugEnabled)
                        Log.Debug($"Invoke({methodInfoObject.EventName}) with object {state} is {oo}");
                    methodInfoObject.Method.Invoke(methodInfoObject.Target, new[] { oo });
                }
                catch (Exception e)
                {
                    Log.Error($"Error in state-event {eventName} for {methodInfoObject.Target?.GetType().Name}", e);
                }
            }
        }
    }
    
    
    /// <summary>
    /// Пометить метод для вызова события
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple=true, Inherited = true)]
    public class SmartStateAttribute : Attribute
    {
        public string EventName { get; protected set; }
        
        public string[] Except { get; protected set; }
        
        /// <summary>
        /// Имя события будет имя метода маленькими буквами
        /// </summary>
        public SmartStateAttribute() {}

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="eventName">Имя события</param>
        public SmartStateAttribute(string eventName=null)
        {
            EventName = eventName;
        }
        
        
        public SmartStateAttribute(string eventName=null, string[] except = null)
        {
            EventName = eventName;
            Except = except;
        }
    }
    
}