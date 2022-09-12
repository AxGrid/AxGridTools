using System;
using System.Collections.Generic;
using System.Linq;
using AxGrid.Compare;
using AxGrid.Utils;
using AxGrid.Utils.Reflections;


namespace AxGrid.State
{
    /// <summary>
    /// Smart state object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartState<T> : SmartStateEventManager, ISmartState<T>
    {
        public string StateName { get; set; } = "State";
        //public SmartStateEventManager EventManager { get; set; } = new SmartStateEventManager();
        public T State { get; private set; }
        public X GetState<X>() => (X)(object)State;

        public CompareLogic CompareLogic { get; }
        
        // public void Add(object o) => EventManager.Add(o);
        //
        // public void Remove(object o) => EventManager.Remove(o);

        public IEnumerable<StateEvent> GetEvents(T newState, bool saveState = true)
        {
            var result = CompareLogic.Compare(State, newState);
            if (saveState) State = newState;
            return ReflectionUtils.ClearEvents(result.Differences.Select(i => i.PropertyName).ToList())
                .Select(ev => new StateEvent
                {
                    EventName = $"{ev}",
                    State = newState,
                }).ToList();
        }

        public DStateChanged<T> OnStateChanging { get; set; }
        public DStateChanged<T> OnStateChanged { get; set; }

        public void Apply(T newState, bool needSave = true)
        {
            var result = CompareLogic.Compare(State, newState);
            Console.WriteLine(result.DifferencesString);

            foreach (var rs in result.Differences)
            {
                Console.WriteLine($"PN:{rs.ActualName}");
            }
            if (needSave)
                State = newState;
            ReflectionUtils.ClearEvents(result.Differences.Select(i => i.PropertyName).ToList())
                .ForEach(ev =>
                {
                    Invoke($"{ev}", newState);
                });
        }

        public SmartState(T state = default(T))
        {
            CompareLogic = new CompareLogic
            {
                Config =
                {
                    CompareProperties = true,
                    MaxDifferences = 1000,
                    ShowBreadcrumb = true
                }
            };
            State = state;
        }

        #region getters

        public object Get(string path, object defaultValue = null) => ReflectionUtils.Get(this.State, path, defaultValue);
        public A Get<A>(string path, A defaultValue = default(A)) => (A)ReflectionUtils.Get(this.State, path, defaultValue);
        
        #endregion
    }
    
   
}