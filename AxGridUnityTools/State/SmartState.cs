using System;
using System.Collections.Generic;
using System.Linq;
using AxGrid.Compare;
using AxGrid.Utils;


namespace AxGrid.State
{
    /// <summary>
    /// Smart state object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartState<T>
    {
        public string StateName { get; set; } = "State";
        public SmartStateEventManager EventManager { get; set; } = new SmartStateEventManager();
        public T State { get; private set; }

        public CompareLogic CompareLogic { get; }
        
        public void Add(object o) => EventManager.Add(o);
        
        public void Remove(object o) => EventManager.Remove(o);

        protected IEnumerable<StateEvent> GetEvents(T newState, bool saveState = true)
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
        
        public void Update(T newState)
        {
            var result = CompareLogic.Compare(State, newState);
            Console.WriteLine(result.DifferencesString);

            foreach (var rs in result.Differences)
            {
                Console.WriteLine($"PN:{rs.ActualName}");
            }

            State = newState;
            ReflectionUtils.ClearEvents(result.Differences.Select(i => i.PropertyName).ToList())
                .Select(s =>
                {
                    Console.WriteLine($"Clearing event: [{s}]");
                    return s;
                })
                .ForEach(ev =>
                {
                    Console.WriteLine($"Invoke event: {ev}");
                    EventManager.Invoke($"{ev}", State);
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
        public object Get<A>(string path, A defaultValue = default(A)) => (A)ReflectionUtils.Get(this.State, path, defaultValue);
        
        
       

        #endregion
    }
    
   
}