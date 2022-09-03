using System;
using System.Text.RegularExpressions;
using AxGrid.Model;

namespace AxGrid.State
{
    /// <summary>
    /// Smart state object
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SmartState<T>
    {
        public string StateName { get; set; } = "State";
        public IEventManagerInvoke EventManager { get; set; } = new SettingsEventManager();
        public T State { get; private set; }


        
        public void Update(T newState)
        {
            var result = SmartComparator.Compare(State, newState);
            foreach (var r in result.Differences)
                EventManager.Invoke($"On{StateName}{r.PropertyName}Updated", r.Object2Value, r.Object1Value, r.Object2, r.Object1);
            State = newState;
        }

        public SmartState(T state = default(T))
        {
            State = state;
        }
    }
}