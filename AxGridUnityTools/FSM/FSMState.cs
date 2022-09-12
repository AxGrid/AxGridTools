using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AxGrid.Base;
using AxGrid.Model;
using AxGrid.Utils;
using AxGrid.Utils.Reflections;
using log4net;
using SmartFormat;
using UnityEngine.Scripting;


namespace AxGrid.FSM
{
    /// <summary>
    /// Состояние
    /// </summary>
    [Preserve]
    public abstract class FSMState : IState
    {


        private AsyncEventManager asm;

        protected ILog _log;        
        private ILog Log
        {
            get { return _log ?? (_log = LogManager.GetLogger(GetType().Name)); }
        }

        /// <summary>
        /// Proxy
        /// </summary>
        [Preserve]
        protected DynamicModel Model => Settings.Model;

        /// <summary>
        /// proxy Event Manager
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="args">Argse</param>
        protected void Invoke(string name, params object[] args) => Model.EventManager.Invoke(name, args);
        
        /// <summary>
        /// Методинвокер
        /// </summary>
        private class MethodInfoObject
        {
            public MethodInfo Method { get; set; }
            public object Target { get; set; }
            public int Priority { get; set; }
            
            protected ILog _log;        
            private ILog Log
            {
                get { return _log ?? (_log = LogManager.GetLogger(GetType().Name)); }
            }


            public void Invoke(params object[] args)
            {
                if (Target == null) return;
                try
                {
                    Method.Invoke(Target, args);
                }
                catch (Exception e)
                {
                    Exception thr = e;
                    while (thr.InnerException != null)
                        thr = thr.InnerException;
                    Log.Error(thr);
                    throw thr;
                }
            }

            public override string ToString()
            {
                return Smart.Format("{Target.GetType().Name}.{Method.Name}", this);
            }

            public override bool Equals(object obj)
            {
                if (obj == null || obj.GetType() != typeof(MethodInfoObject))
                    return false;
                return Equals((MethodInfoObject) obj);
            }

            protected bool Equals(MethodInfoObject other)
            {
                return Equals(Method, other.Method) && Equals(Target, other.Target);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Method != null ? Method.GetHashCode() : 0) * 397) ^ (Target != null ? Target.GetHashCode() : 0);
                }
            }
        }

        private class TimingObject {
            private float _currentTime = 0.0f;
            private float _resetTime = 0.0f;
            private float _startTime = 0.0f;
            private static readonly ILog Log = LogManager.GetLogger("TimingObject");// LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
            public float Time
            {
                get { return _resetTime; }
            }

            public bool Loop { get; protected set; }
            public bool Enable { get; protected set; }
            public object Target { get; set; }
            public MethodInfo Method { get; set; }
            public object[] Args = new object[0];
            public int Priority = 0;
            public string Name { get; set; }

            public void Reset()
            {
                _currentTime = _startTime;
                Enable = true;
            }

            public void Stop()
            {
                Enable = false;
            }

            public void Update(float deltaTime)
            {
                if (!Enable)
                    return;
                _currentTime -= deltaTime;
                if (_currentTime > 0)
                    return;
            
                try
                {
                    Method.Invoke(Target, Args);
                }
                catch (TargetInvocationException e)
                {
                    Exception thr = e;
                
                    while (thr.InnerException != null) {
                        Log.Error($"Inner:{thr.Message}\n{thr.StackTrace}");
                        thr = thr.InnerException;
                    }
                    Log.Error($"Final:{thr.Message}\n{thr.StackTrace}");
                }
            
                if (Loop)
                    _currentTime += _resetTime;
                else
                    Enable = false;
            }

            public static TimingObject fromAttribute(Timing attr) {
                return new TimingObject(attr.ResetTime, attr.CurrentTime, attr.Loop) {
                    Name = attr.Name, Priority = attr.Priority, Enable = attr.Enable
                };
            }
            
            public static TimingObject fromMonoBehaviourAttribute(OnRefresh attr) {
                return new TimingObject(attr.Interval, attr.currentInterval, true);
            }

            public static TimingObject fromMonoBehaviourAttribute(OnDelay attr)
            {
                return new TimingObject(attr.interval, attr.currentInterval, false);
            }

            
            public TimingObject(float resetTime, float currentTime, bool loop)
            {
           
                _resetTime = resetTime;
                _currentTime = currentTime;
                _startTime = currentTime;
                Loop = loop;
            }

        }
        
        private readonly List<MethodInfoObject> enters;
        private readonly List<MethodInfoObject> exits;
        private readonly List<MethodInfoObject> updates;
        private readonly Dictionary<string, TimingObject> timersDictionary;
        private readonly List<TimingObject> timers;
        
        public FSM Parent { get; set; }
        
        
        
        protected FSMState()
        {
            asm = new AsyncEventManager();
            enters = new List<MethodInfoObject>();
            exits = new List<MethodInfoObject>();
            updates = new List<MethodInfoObject>();
            timersDictionary = new Dictionary<string, TimingObject>();
            
            foreach (var methodInfo in ReflectionUtils.GetAllMethodsInfo(GetType()))
            {
                foreach (var o in methodInfo.GetCustomAttributes(false))
                {
                    if (o.GetType() == typeof(Enter))
                    {
                        var enter = (Enter) o;
                        enters.Add(new MethodInfoObject {Method = methodInfo, Target = this, Priority = enter.Priority});
                    }else if (o.GetType() == typeof(Exit))
                    {
                        var exit = (Exit) o;
                        exits.Add(new MethodInfoObject {Method = methodInfo, Target = this, Priority = exit.Priority});
                    }else if (o.GetType() == typeof(Loop))
                    {
                        var loop = TimingObject.fromAttribute((Loop) o);
                        if (loop.Time == 0)
                            updates.Add(new MethodInfoObject
                            {
                                Method = methodInfo,
                                Target = this,
                                Priority = loop.Priority
                            });
                        else
                        {
                            loop.Method = methodInfo;
                            loop.Target = this;
                            timersDictionary.Add(loop.Name ?? methodInfo.Name, loop);
                        }
                        //break; //TODO: Чет это странный break
                    }else if (o.GetType() == typeof(One))
                    {
                        var one = TimingObject.fromAttribute((One) o);
                        timersDictionary.Add(one.Name ?? methodInfo.Name, one);
                        one.Method = methodInfo;
                        one.Target = this;
                    }else if (o.GetType() == typeof(OnRefresh))
                    {
                        var loop = TimingObject.fromMonoBehaviourAttribute((OnRefresh) o);
                        if (loop.Time == 0)
                            updates.Add(new MethodInfoObject
                            {
                                Method = methodInfo,
                                Target = this,
                                Priority = loop.Priority
                            });
                        else
                        {
                            loop.Method = methodInfo;
                            loop.Target = this;
                            timersDictionary.Add(loop.Name ?? methodInfo.Name, loop);
                        }
                    }else if (o.GetType() == typeof(OnDelay))
                    {
                        var one = TimingObject.fromMonoBehaviourAttribute((OnDelay) o);
                        timersDictionary.Add(one.Name ?? methodInfo.Name, one);
                        one.Method = methodInfo;
                        one.Target = this;
                    }
                }
            }
            
            enters = enters.OrderBy(i => i.Priority).ToList();
            exits = exits.OrderBy(i => i.Priority).ToList();
            updates = updates.OrderBy(i => i.Priority).ToList();
            timers = timersDictionary.Select(i => i.Value).OrderBy(i => i.Priority).ToList();
            asm.Add(this);
        }

        public void __Invoke(string eventName, object[] args)
        {
            asm.Invoke(eventName, args);
        }
        
        public void __InvokeDelayAsync(float delay, string eventName, object[] args)
        {
            asm.InvokeDelayAsync(delay, eventName, args);
        }

        public void Dispose()
        {
            asm.Dispose();
            enters.Clear();
            exits.Clear();
            updates.Clear();
        }

        public void __EnterState()
        {
            timers.ForEach(t => t.Reset());
            foreach (var m in enters)
                m.Invoke();
        }
        
        public void __ExitState()
        {
            foreach (var m in exits)
                m.Invoke();
        }

        public void __UpdateState(float deltaTime)
        {
            foreach (var m in updates)
                m.Invoke(deltaTime);
            foreach (var t in timers)
                t.Update(deltaTime);
        }

    }
    
    [AttributeUsage(AttributeTargets.Class) ]
    public class State : Attribute
    {
        public string Name { get; protected set; }
        
        public State(string name)
        {
            Name = name;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method) ]
    public class Enter : Attribute
    {
        public int Priority { get; protected set;  }
        
        public Enter(){}

        public Enter(int priority)
        {
            Priority = priority;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method) ]
    public class Exit : Attribute
    {
        public int Priority { get; protected set; }
        
        public Exit(){}

        public Exit(int priority)
        {
            Priority = priority;
        }
    }
    
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class One : Timing
    {
        public One(float time) : this(time, null){ }
        
        public One(float time, string name) : this(time, name, 0){}
        
        public One(float time, string name, int priority) : base(time, time, false){ 
            Name = name;
            Priority = priority;
        }
    }
    

    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class Loop : Timing
    {
        public Loop(float time) : this(time, 0){ }
        
        public Loop(float time, float startTime) : this(time, startTime, null, 0){}
        
        public Loop(float time, float startTime, string name) : this(time, startTime, name, 0){}
        
        public Loop(float time, float startTime, string name, int priority) : base(time, startTime, true){ 
            Name = name;
            Priority = priority;
        }
    }

    
    /// <summary>
    /// Таймер
    /// </summary>
    public class Timing : Attribute
    {
        public float CurrentTime { get; protected set; } = 0.0f;
        public float ResetTime { get; protected set; } = 0.0f;
        public float StartTime { get; protected set; } = 0.0f;
        
        public float Time
        {
            get { return ResetTime; }
        }

        public bool Loop { get; protected set; }
        public bool Enable { get; protected set; }
        public object Target { get; set; }
        public MethodInfo Method { get; set; }
        public object[] Args = new object[0];
        
        public int Priority = 0;
        public string Name { get; set; }

//        public void Reset()
//        {
//            _currentTime = _startTime;
//            Enable = true;
//        }
//
//        public void Stop()
//        {
//            Enable = false;
//        }
//
//        public void Update(float deltaTime)
//        {
//            if (!Enable)
//                return;
//            _currentTime -= deltaTime;
//            if (_currentTime > 0)
//                return;
//            
//            try
//            {
//                Method.Invoke(Target, Args);
//            }
//            catch (TargetInvocationException e)
//            {
//                Exception thr = e;
//                
//                while (thr.InnerException != null) {
//                    Log.Error($"Inner:{thr.Message}\n{thr.StackTrace}");
//                    thr = thr.InnerException;
//                }
//                Log.Error($"Final:{thr.Message}\n{thr.StackTrace}");
//            }
//            
//            if (Loop)
//                _currentTime += _resetTime;
//            else
//                Enable = false;
//        }

        public Timing(float resetTime, float currentTime, bool loop)
        {
           
            ResetTime = resetTime;
            CurrentTime = currentTime;
            StartTime = currentTime;
            Loop = loop;
        }
        
    }

    public interface IState : IDisposable
    {
        void __EnterState();
        void __ExitState();
        void __UpdateState(float deltaTime);
        FSM Parent { get; set; }
        void __Invoke(string eventName, object[] args);
        void __InvokeDelayAsync(float delay, string eventName, object[] args);
    }
    
}