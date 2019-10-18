using System;
using System.Collections.Generic;

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
        Error
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

        

        
        private bool Loop { get; set; }
        
        public float DeltaF { get; protected set; }
        public double DeltaD { get; protected set; }
        public float PathStartTimeF { get; protected set; }
        public double PathStartTimeD { get; protected set; }

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


        public float StoredF { get; set; }
        
        public CPath Add(DPathActionContext action)
        {
            actions.Enqueue(new PathItem {Action1 = action});
            return this;
        }
        
        public CPath Add(DPathAction action)
        {
            actions.Enqueue(new PathItem {Action2 = action});
            return this;
        }

        public CPath Error(DPathActionContext action)
        {
            errors.Enqueue(new PathItem {Action1 = action, Error = true});
            return this;
        }
        
        public CPath Error(DPathAction action)
        {
            errors.Enqueue(new PathItem {Action2 = action, Error = true});
            return this;
        }

        public int Count
        {
            get { return actions.Count; }
        }

        public void Clear() {
            actions.Clear();
            errors.Clear();
            currentItem = null;
        }


        public bool IsPLaying
        {
            get { return Count > 0 || currentItem != null; }
        }

        private void ResetTimers()
        {
            DeltaF = StoredF;
            DeltaD = StoredF;
            StoredF = 0f;
        }

        public Status Now(float totalTime)
        {
            return Immediately(totalTime);
        }

        public Status Immediately(float totalTime)
        {
            StoredF = DeltaF - totalTime;
            return Status.Immediately;
        }

        public Status OK(float totalTime)
        {
            StoredF = DeltaF - totalTime;
            return Status.OK;
        }
        
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
            if (Count == 0 && currentItem == null)
                return;
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
                switch (currentItem.Invoke(this))
                {
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