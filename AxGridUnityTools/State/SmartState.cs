using System;
using System.Linq;
using System.Text.RegularExpressions;
using AxGrid.Model;
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

        public void Add(object o) => EventManager.Add(o);
        
        public void Update(T newState)
        {
            var result = SmartComparator.Compare(State, newState);
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
            State = state;
        }

        #region getters

        public object Get(string path, object defaultValue = null) => ReflectionUtils.Get(this.State, path, defaultValue);
        public object Get<A>(string path, A defaultValue = default(A)) => (A)ReflectionUtils.Get(this.State, path, defaultValue);
        
        
       

        #endregion
    }
}