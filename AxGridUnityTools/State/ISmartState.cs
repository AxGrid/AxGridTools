using System;
using System.Collections.Generic;

namespace AxGrid.State
{
    
    
    public interface ISmartState : ISmartStateEventManager
    {
        T State<T>();
        X Get<X>(string path, X defaultValue = default(X));
    }

    public delegate void DStateChanged<in T>(T newState);
    public interface ISmartState<T> : ISmartState
    {
        /// <summary>
        /// Состояние изменяется
        /// В этом событии можно вызывать сервер и передать в него новое сосоятоние
        /// </summary>
        event DStateChanged<T> OnStateChanging;
        
        /// <summary>
        /// Состояние изменилось
        /// Тут можно узнать что состояние поменялось
        /// </summary>
        event DStateChanged<T> OnStateChanged;
        
        /// <summary>
        /// Применить новое полученное состояние
        /// </summary>
        /// <param name="newState">новое состояние</param>
        /// <param name="saveState">сохранить состояние</param>
        void Apply(T newState, bool saveState = true);
        
        /// <summary>
        /// Получить список событий изменения состояния
        /// </summary>
        /// <param name="newState">новое состояние</param>
        /// <param name="saveState">сохранить состояние</param>
        /// <returns>список событий</returns>
        IEnumerable<StateEvent> GetEvents(T newState, bool saveState = true);
        
        /// <summary>
        /// Состояние
        /// </summary>
        T State { get; }
    }

    public interface ISmartStateEventManager
    {
        void Add(object o);
        void Remove(object o);
        
        void AddAction(string name, Action action);
        void AddAction<X>(string name, Action<X> action);
        
        void RemoveAction(Action action);
        void RemoveAction<X>(Action<X> action);
        
        void RemoveAction(string name, Action action);
        void RemoveAction<X>(string name, Action<X> action);
    }
}