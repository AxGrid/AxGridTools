using System;
using System.Collections.Generic;
using AxGrid.Utils;
using log4net;

namespace AxGrid.FSM
{
    /// <summary>
    /// Машина состояний
    /// </summary>
    public class FSM : IDisposable
    {
        public static bool ShowFsmEnterState { get; set; }
        public static bool ShowFsmExitState { get; set; }


        private string name = "";

        public string Name
        {
            get => name;
            set
            {
                name = value;
                Log = LogManager.GetLogger($"FSM.{name}");
            }
        }
        
        private readonly Dictionary<string, IState> _states = new Dictionary<string, IState>();
        private IState currentState;
        
        private Dictionary<string, Dictionary<string,string>> ExitActions = new Dictionary<string, Dictionary<string, string>>();

        public ILog Log { get; private set;  } = LogManager.GetLogger("FSM");
        
        public void OverrideExit(string state, string exit, string newExit) {
            if (!ExitActions.ContainsKey(state))
                ExitActions.Add(state, new Dictionary<string, string>());
            if (ExitActions[state].ContainsKey(exit))
                ExitActions[state][exit] = newExit;
            else
                ExitActions[state].Add(exit, newExit);
        }


        public string GetOverrideExit(string state, string exit) {
            if (state == null || !ExitActions.ContainsKey(state))
                return exit;
            return !ExitActions[state].ContainsKey(exit) ? exit : ExitActions[state][exit];
        }
        
        public string CurrentStateName { get; protected set; }
        //private static readonly ILog Log = new ILog();

        /// <summary>
        /// Содержит состояние
        /// </summary>
        /// <param name="stateName"></param>
        /// <returns></returns>
        public bool ContainsState(string stateName)
        {
            return _states.ContainsKey(stateName);
        }
        
        /// <summary>
        /// Добавить новые состояния
        /// </summary>
        /// <param name="states"></param>
        public void Add(params IState[] states)
        {
            foreach (var state in states)
            {
                bool found = false;
                foreach (object attr in state.GetType().GetCustomAttributes(typeof(State), false))
                {
                    Add(state, ((State) attr).Name);
                    found = true;
                }
                if (!found)
                    Log.Error($"FSMState {Name} doesn't have attribute State");
            }
        }
        
        /// <summary>
        /// Заменить состояния на новые
        /// </summary>
        /// <param name="states"></param>
        public void Replace(params IState[] states)
        {
            foreach (var state in states)
            {
                bool found = false;
                foreach (object attr in state.GetType().GetCustomAttributes(typeof(State), false))
                {
                    Replace(state, ((State) attr).Name);
                    found = true;
                }
                if (!found)
                    Log.Error($"FSMState {Name} doesn't have attribute State");
            }
        }

        /// <summary>
        /// Добавить новое состояние
        /// </summary>
        /// <param name="state"></param>
        /// <param name="stateName"></param>
        public void Add(IState state, string stateName)
        {
            if (_states.ContainsKey(stateName))
            {
                Log.Warn($"Duplicate state with name {stateName}. Use method Replace");
                _states.Remove(stateName);
            }
            state.Parent = this;
            _states.Add(stateName, state);
        }
        

        /// <summary>
        /// Заменить состояние на новое
        /// </summary>
        /// <param name="state"></param>
        /// <param name="stateName"></param>

        public void Replace(IState state, string stateName)
        {
            if (_states.ContainsKey(stateName))
                _states.Remove(stateName);
            state.Parent = this;
            _states.Add(stateName, state);
        }

        public void Start(string stateName)
        {
            if (currentState == null)
                Change(stateName);
        }

        /// <summary>
        /// Перейти из одного состояния в другое
        /// </summary>
        /// <param name="name">имя состояния для перехода</param>
        /// <param name="direct">не обращать внимание на перекрытые переходы</param>
        public void Change(string name, bool direct = false)
        {
            if (!direct) name = GetOverrideExit(CurrentStateName, name);
            if (ShowFsmExitState) Log.Debug($"--- EXIT {CurrentStateName} ---");
            currentState?.__ExitState();
            if (!_states.ContainsKey(name))
                throw new Exception($"Key {name} not found in fsm {Name}!");
            currentState = _states[name];
            CurrentStateName = name;
            
            if (ShowFsmEnterState) Log.Debug($"--- ENTER {CurrentStateName} ---");
            currentState.__EnterState(); 
        }
        
        /// <summary>
        /// Перезайти в текущее состояние
        /// </summary>
        public void ReEnter() {
            var name = this.CurrentStateName;
            if (ShowFsmExitState) Log.Debug($"--- EXIT {CurrentStateName} ---");
            currentState?.__ExitState();
            if (!_states.ContainsKey(name))
                throw new Exception($"Key {name} not found!");
            currentState = _states[name];
            CurrentStateName = name;
            if (ShowFsmEnterState) Log.Debug($"--- ENTER {CurrentStateName} ---");
            currentState.__EnterState(); 
        }

        /// <summary>
        /// Update loop
        /// </summary>
        /// <param name="deltaTime"></param>
        public void Update(float deltaTime)
        {
            currentState?.__UpdateState(deltaTime);
        }

        /// <summary>
        /// События
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void Invoke(string eventName, params object[] args)
        {
            currentState?.__Invoke(eventName, args);
        }
        
        
        /// <summary>
        /// События
        /// </summary>
        /// <param name="eventName"></param>
        /// <param name="args"></param>
        public void InvokeDelayAsync(float delay, string eventName, params object[] args)
        {
            currentState?.__InvokeDelayAsync(delay, eventName, args);
        }

        public void Dispose()
        {
            foreach (var kv in _states)
                kv.Value.Dispose();
            _states.Clear();
        }
    }
}