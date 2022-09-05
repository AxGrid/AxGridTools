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
    public class SmartStateEventManager : ISmartStateEventManager
    {
        private static readonly ILog Log = LogManager.GetLogger("StateEventManager");
        
        readonly Dictionary<string, List<AsyncEventManager.MethodInfoObject>> _eventListeners = new Dictionary<string, List<AsyncEventManager.MethodInfoObject>>();
        readonly Dictionary<object, List<string>> _objectEvents = new Dictionary<object, List<string>>();

        /// <summary>
        /// Возвращает все под-события которые должны вызыватся при изменении
        /// если вызвать A.SubObject[0].B
        /// то вернет
        /// ""
        /// "A"
        /// "A.SubObject"
        /// "A.SubObject[0]"
        /// "A.SubObject[0].B" 
        /// </summary>
        /// <param name="eventName">A.SubObject[0].B</param>
        /// <param name="mi"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        private List<string> GetAllBindEventForMethodInfo(string eventName, MethodInfo mi, object target = null)
        {
            var eventNames = string.IsNullOrEmpty(eventName) ?  mi.Name : eventName;
            if (eventNames.Contains("{") && target != null)
                eventNames = Smart.Format(eventNames, ReflectionUtils.GetAllFieldValues(target.GetType(), target)); //TODO: проверить
            return ReflectionUtils.GetEvents(eventNames);
        }

        public void AddAction(string eventName, Action method)=>
            AddAction(new AsyncEventManager.MethodInfoObject {Target = method.Target, Method = method.Method, EventName = eventName});

        public void AddAction<X>(string eventName, DEventMethod<X> method) =>
            AddAction(new AsyncEventManager.MethodInfoObject {Target = method.Target, Method = method.Method, EventName = eventName});

        private void AddAction(AsyncEventManager.MethodInfoObject mi)
        {
            var eventNames = GetAllBindEventForMethodInfo(mi.EventName, mi.Method, mi.Target);
            foreach (var eventName in eventNames)
            {
                if (!_eventListeners.ContainsKey(eventName))
                    _eventListeners.Add(eventName, new List<AsyncEventManager.MethodInfoObject>());
                _eventListeners[eventName].Add(mi);
            }
            if (!_objectEvents.ContainsKey(mi.Target))
                _objectEvents.Add(mi.Target, eventNames);
            else
                _objectEvents[mi.Target].AddRange(eventNames);            
        }
        
        public void Remove(object target)
        {
            if (_objectEvents.ContainsKey(target))
            {
                var eventNames = _objectEvents[target];
                foreach (var eventName in eventNames)
                {
                    if (_eventListeners.ContainsKey(eventName))
                    {
                        _eventListeners[eventName] = _eventListeners[eventName].Where(e => e.Target != target).ToList();
                        if (_eventListeners[eventName].Count == 0)
                            _eventListeners.Remove(eventName);
                    }
                }
                _objectEvents.Remove(target);
            }
        }

        public void RemoveAction(string name, Action method) =>
            RemoveAction(new AsyncEventManager.MethodInfoObject {Target = method.Target, Method = method.Method, EventName = name});

        public void RemoveAction<T>(string name, DEventMethod<T> method) =>
            RemoveAction(new AsyncEventManager.MethodInfoObject {Target = method.Target, Method = method.Method, EventName = name});

        public void RemoveAction(Action method) =>
            RemoveAction(new AsyncEventManager.MethodInfoObject {Target = method.Target, Method = method.Method, EventName = "" });
        
        public void RemoveAction<T>(DEventMethod<T> method) =>
            RemoveAction(new AsyncEventManager.MethodInfoObject {Target = method.Target, Method = method.Method, EventName = "" });

        
        private void RemoveAction(AsyncEventManager.MethodInfoObject mi)
        {
            var events = GetAllBindEventForMethodInfo(mi.EventName, mi.Method, mi.Target);
            foreach (var eventName in events)
            {
                if (_eventListeners.ContainsKey(eventName))
                {
                    _eventListeners[eventName] = _eventListeners[eventName].Where(e => e.Target != mi.Target && e.Method != mi.Method).ToList();
                    if (_eventListeners[eventName].Count == 0)
                        _eventListeners.Remove(eventName);
                }
            }
        }
        

        public void Add(object o)
        {
            if (o == null) return;
            ReflectionUtils.GetAllMethodsInfo(o.GetType()).Where(mi => mi.GetCustomAttribute<SmartStateAttribute>() != null).ForEach(
                mi =>
                {
                    var a = mi.GetCustomAttribute<SmartStateAttribute>();
                    var eventNames = a.EventName ?? mi.Name;
                    if (eventNames.Contains("{"))
                        eventNames = Smart.Format(eventNames, ReflectionUtils.GetAllFieldValues(o.GetType(), o));
                    
                    var mio = new AsyncEventManager.MethodInfoObject
                    {
                        Method = mi,
                        Target = o,
                        EventName = eventNames,
                    };
                    AddAction(mio);
                });
            
            ReflectionUtils.GetAllPropertiesInfo(o.GetType()).Where(pi => pi.GetCustomAttribute<SmartStateAttribute>() != null).ForEach(
                pi =>
                {
                    var a = pi.GetCustomAttribute<SmartStateAttribute>();
                    var eventNames = a.EventName ?? pi.Name;
                    if (eventNames.Contains("{"))
                        eventNames = Smart.Format(eventNames, ReflectionUtils.GetAllFieldValues(o.GetType(), o));
                    
                    var mio = new AsyncEventManager.MethodInfoObject
                    {
                        Method = pi.SetMethod,
                        Target = o,
                        EventName = eventNames,
                    };
                    AddAction(mio);
                });
        }

        
        // protected void Add(SmartStateAttribute bind, AsyncEventManager.MethodInfoObject mio, string realEventName=null)
        // {
        //     var eventNames = !string.IsNullOrEmpty(realEventName) ? realEventName : bind.EventName ?? mio.Method.Name;
        //     if (eventNames.Contains("{") && mio.Target != null)
        //         eventNames = Smart.Format(eventNames, ReflectionUtils.GetAllFieldValues(mio.Target.GetType(), mio.Target));
        //     mio.EventName = eventNames;
        //     if (Log.IsDebugEnabled) Log.Debug($"Add SmartStateEvent {eventNames}");
        //     foreach (var eventName in ReflectionUtils.GetEvents(eventNames))
        //     {
        //         if (Log.IsDebugEnabled) Log.Debug($"Add state-event [{eventName}] for {mio.Target?.GetType().Name}");
        //         if (!_eventListeners.ContainsKey(eventName))
        //             _eventListeners.Add(eventName, new List<AsyncEventManager.MethodInfoObject>());
        //         if (!_eventListeners[eventName].Contains(mio))
        //             _eventListeners[eventName].Add(mio);
        //     }
        // }
        //     
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