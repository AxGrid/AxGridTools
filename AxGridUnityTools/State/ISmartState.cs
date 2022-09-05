using System;
using System.Collections.Generic;
using AxGrid.Model;

namespace AxGrid.State
{
    
    
    public interface ISmartState : ISmartStateEventManager
    {
        T GetState<T>();
        X Get<X>(string path, X defaultValue = default(X));
    }

    public delegate void DStateChanged<in T>(T newState);
    public interface ISmartState<T> : ISmartState
    {
        /// <summary>
        /// Состояние изменяется
        /// В этом событии можно вызывать сервер и передать в него новое сосоятоние
        /// </summary>
        DStateChanged<T> OnStateChanging  { get; set; }
        
        /// <summary>
        /// Состояние изменилось
        /// Тут можно узнать что состояние поменялось
        /// </summary>
        DStateChanged<T> OnStateChanged { get; set; }
        
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
        
        void AddAction(string eventName, Action method);
        void AddAction<X>(string eventName, DEventMethod<X> method);
        
        void RemoveAction(Action method);
        void RemoveAction<X>(DEventMethod<X> method);
        
        void RemoveAction(string eventName, Action method);
        void RemoveAction<X>(string eventName, DEventMethod<X> method);
        void Invoke(string eventName, object state);
    }
}