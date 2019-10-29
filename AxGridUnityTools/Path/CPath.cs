using System;
using System.Collections.Generic;
using AxGrid.Utils;

namespace AxGrid.Path
{

    public delegate Status DPathActionContext(CPath p);
    public delegate Status DPathAction();

    public enum Status
    {
        OK,
        Continue,
        Immediately,
        Now,
        Error,
        Stop
    }
    
    public class CPath
    {

        public CPath() : this(false) { }
        public CPath(bool loop)
        {
            actions = new Queue<PathItem>();
            errors = new Queue<PathItem>();
            Loop = loop;
        }

        public static CPath Create(){ return new CPath(); }
        public static CPath CreateLoop(){ return new CPath(true); }

        private Action StopAction = null;

        
        public bool Loop { get; set; }
        
        public float DeltaF { get; protected set; }
        public double DeltaD { get; protected set; }
        public float PathStartTimeF { get; protected set; }
        public double PathStartTimeD { get; protected set; }


        /// <summary>
        /// Остановить путь
        /// </summary>
        public void StopPath() {
            Clear();
            StopAction?.Invoke();
            StopAction = null;
        }
        
        /// <summary>
        /// Очистить путь
        /// </summary>
        public void Clear() {
            Loop = false;
            actions.Clear();
            errors.Clear();
            currentItem = null;
        }
        
        /// <summary>
        /// Сбросить таймеры
        /// </summary>
        public void ResetTimerVariables()
        {
            DeltaF = 0;
            DeltaD = 0;
        }
        
        private class PathItem
        {
            public DPathActionContext Action1 { get; set; }
            public DPathAction Action2 { get; set; }

            public bool Error { get; set; }
            
            public Status Invoke(CPath p)
            {
                return Action1 != null ? Action1.Invoke(p) : Action2.Invoke();
            }
        }

        private PathItem currentItem;
        
        private Queue<PathItem> actions;
        private Queue<PathItem> errors;

        
        /// <summary>
        /// Текущее засторенное время пути
        /// </summary>
        public float StoredF { get; protected set; }
        
        public CPath Add(DPathActionContext action)
        {
            actions.Enqueue(new PathItem {Action1 = action});
            return this;
        }
        
        /// <summary>
        /// Добавить в путь Действие
        /// </summary>
        /// <param name="action">Действие</param>
        /// <returns>Путь</returns>
        public CPath Add(DPathAction action)
        {
            actions.Enqueue(new PathItem {Action2 = action});
            return this;
        }

        /// <summary>
        /// Добавить в путь Действие в путь Ошибок
        /// </summary>
        /// <param name="action">Действие на ошибку</param>
        /// <returns>Путь</returns>
        public CPath Error(DPathActionContext action)
        {
            errors.Enqueue(new PathItem {Action1 = action, Error = true});
            return this;
        }
        
        /// <summary>
        /// Добавить подноразовое действие в путь Ошибок
        /// </summary>
        /// <param name="action">Действие</param>
        /// <returns>Путь</returns>
        public CPath Error(DPathAction action)
        {
            errors.Enqueue(new PathItem {Action2 = action, Error = true});
            return this;
        }

        /// <summary>
        /// Количество элементов в текущем пути
        /// </summary>
        public int Count => actions.Count;


        /// <summary>
        /// Путь в рабочем состоянии
        /// </summary>
        public bool IsPlaying
        {
            get { return Count > 0 || currentItem != null; }
        }

        private void ResetTimers()
        {
            DeltaF = StoredF;
            DeltaD = StoredF;
            StoredF = 0f;
        }

        /// <summary>
        /// Вернусть статус Now с установкой времени
        /// </summary>
        /// <param name="totalTime">Время</param>
        /// <returns>путь</returns>        
        public Status Now(float totalTime)
        {
            return Immediately(totalTime);
        }

        /// <summary>
        /// Вернусть статус Immediately с установкой времени
        /// </summary>
        /// <param name="totalTime">Время</param>
        /// <returns>путь</returns>        
        public Status Immediately(float totalTime)
        {
            StoredF = DeltaF - totalTime;
            return Status.Immediately;
        }

        /// <summary>
        /// Вернусть статус OK с установкой времени
        /// </summary>
        /// <param name="totalTime">Время</param>
        /// <returns>путь</returns>
        public Status OK(float totalTime)
        {
            StoredF = DeltaF - totalTime;
            return Status.OK;
        }
        
        /// <summary>
        /// Добавить ожидание
        /// </summary>
        /// <param name="time">время в сек</param>
        /// <returns></returns>
        public CPath Wait(float time)
        {
            return Add(p =>
            {
                if (p.DeltaF < time)
                    return Status.Continue;
                return Status.OK;
            });
            
        }


        private void SetCurrentItem()
        {
            if (Loop && currentItem != null)
                actions.Enqueue(currentItem);
            
            if (actions.Count > 0)
                currentItem = actions.Dequeue();
            else
                currentItem = null;
        }

        /// <summary>
        /// Добавить действие остановки пути и в конец пути
        /// </summary>
        /// <param name="a">Действие</param>
        /// <param name="onlyOnStopPath">Не добавлять в конец пути</param>
        /// <returns></returns>
        public CPath Stop(Action a, bool onlyOnStopPath = false) {
            StopAction = a;
            if (!onlyOnStopPath) Action(StopAction);
            return this;
        }
        
        /// <summary>
        /// Добавить одноразовое действие
        /// </summary>
        /// <param name="a">Действие</param>
        /// <returns></returns>
        public CPath Action(Action a)
        {
            Add(() =>
            {
                a.Invoke();
                return Status.OK;
            });
            return this;
        }
        
        public void Update(float deltaTime)
        {
            if (Count == 0 && currentItem == null) {
                StopAction = null;
                return;
            }

            DeltaD += deltaTime;
            DeltaF += deltaTime;
            PathStartTimeF += deltaTime;
            PathStartTimeD += deltaTime;

            if (currentItem == null)
            {
                SetCurrentItem();
                Update(0f);
            }
            else
            {
                try {
                    switch (currentItem.Invoke(this)) {
                        case Status.Continue:
                            return;
                        case Status.OK:
                            SetCurrentItem();
                            ResetTimers();
                            return;
                        case Status.Now:
                        case Status.Immediately:
                            SetCurrentItem();
                            ResetTimers();
                            Update(0);
                            return;
                        case Status.Error:
                            actions = errors;
                            errors = new Queue<PathItem>();
                            SetCurrentItem();
                            ResetTimers();
                            Loop = false;
                            Update(0);
                            return;
                        case Status.Stop:
                            StopPath();
                            return;
                    }
                }
                catch (Exception e) {
                    var ex = e;
                    if (e.InnerException != null) ex = e.InnerException;
                    Log.Error(ex);
                    currentItem = null;
                    actions = errors;
                    errors = new Queue<PathItem>();
                    SetCurrentItem();
                    ResetTimers();
                    Loop = false;
                    Update(0);
                }
            }
        }

        public string Combine(string streamingAssetsPath, string keyTxt)
        {
            throw new NotImplementedException();
        }
    }


    public delegate void CPathEasingAction(float value);

    public static class EasingPath
    {

        private delegate float DEasing(float delta, float from, float to, float time);

        private static CPath Apply(CPath path, CPathEasingAction action, DEasing method, float from, float to, float time)
        {
            return path.Add(p =>
            {
                if (p.DeltaF < time)
                {
                    action.Invoke(method.Invoke(p.DeltaF, from, to, time));
                    return Status.Continue;
                }
                action.Invoke(to);
                return Status.OK;
            });
        }
        
        public static CPath EasingLinear(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.Linear, from, to, time);
        }
        
        public static CPath EasingCircEaseIn(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.CircEaseIn, from, to, time);            
        }

        public static CPath EasingCircEaseInOut(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.CircEaseInOut, from, to, time);            
        }

        
        public static CPath EasingCircEaseOut(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.CircEaseOut, from, to, time);            
        }

        public static CPath EasingCubicEaseIn(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.CubicEaseIn, from, to, time);            
        }

        public static CPath EasingCubicEaseInOut(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.CubicEaseInOut, from, to, time);            
        }

        
        public static CPath EasingCubicEaseOut(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.CubicEaseOut, from, to, time);            
        }
        
        public static CPath EasingQuadEaseIn(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.QuadEaseIn, from, to, time);            
        }

        public static CPath EasingQuadEaseInOut(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.QuadEaseInOut, from, to, time);            
        }

        
        public static CPath EasingQuadEaseOut(this CPath path, float time, float from, float to, CPathEasingAction action)
        {
            return Apply(path, action, EasingTo.QuadEaseOut, from, to, time);            
        }
    }

    public static class TimePath
    {
        public static CPath TimeAction(this CPath path, float time, DPathActionContext action)
        {
            return path.Add(p =>
            {
                if (p.DeltaF < time)
                    return Status.Continue;
                var status = action.Invoke(p);
                switch (status)
                {
                    case Status.Continue:
                        p.ResetTimerVariables();
                        return Status.Continue;
                    default:
                        return status;
                }
            });
        }
     
        public static CPath TimeAction(this CPath path, float time, DPathAction action)
        {
            return path.Add(p =>
            {
                if (p.DeltaF < time)
                    return Status.Continue;
                var status = action.Invoke();
                switch (status)
                {
                    case Status.Continue:
                        p.ResetTimerVariables();
                        return Status.Continue;
                    default:
                        return status;
                }
            });
        }

        
    }
    
}