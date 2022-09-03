using System;
using System.Collections.Generic;
using System.Reflection;
using AxGrid.Model;
using AxGrid.Utils;
using log4net;

namespace AxGrid.State
{
    public class StateEventManager : IEventManagerInvoke
    {
        private static readonly ILog Log = LogManager.GetLogger("StateEventManager");
        
        readonly Dictionary<string, List<AsyncEventManager.MethodInfoObject>> _eventListeners = new Dictionary<string, List<AsyncEventManager.MethodInfoObject>>();

        readonly Dictionary<string, List<string>> events = new Dictionary<string, List<string>>();

        public void AddEvent(string eventName)
        {
             ReflectionUtils.PartOfPath.Get(eventName);
        }
            
            
        public void Invoke(string eventName, params object[] args)
        {
            
        }
    }
    
    
    /// <summary>
    /// Пометить метод для вызова события
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple=true, Inherited = true)]
    public class State : Attribute
    {
        public string EventName { get; protected set; }
        
        public string[] Except { get; protected set; }

        public bool Global { get; protected set; } = false;
        
        /// <summary>
        /// Имя события будет имя метода маленькими буквами
        /// </summary>
        public State() {}

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="eventName">Имя события</param>
        public State(string eventName=null)
        {
            EventName = eventName;
        }
        
        
        public State(string eventName=null, string[] except = null, bool global = false)
        {
            EventName = eventName;
            Except = except;
            Global = global;
        }
    }
}