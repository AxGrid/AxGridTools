using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using AxGrid.Utils;
using JetBrains.Annotations;

namespace AxGrid.Flow
{
    
    public class Flow<T, S, A> 
        where T : IFlowContext<S, A> 
        where S : struct
        where A : struct
    {

        public delegate void DFlowAction(T c);
        public delegate void DFlowExceptionAction(T c, Exception e = null);

        private int __id = 0;
        private readonly NullableDict<S?, NullableDict<A?, List<ActionHolder>>> actions =
            new NullableDict<S?, NullableDict<A?, List<ActionHolder>>>();

        private readonly NullableDict<S?, List<ExceptionHolder>> exceptions =
            new NullableDict<S?, List<ExceptionHolder>>();
        
        private S? startState;

        internal void Add(S? state, A? action, DFlowAction method)
        {
            
            if (state == null)
            {
                AddAll(action, method);
                return;
            }
            if (!actions.ContainsKey(state)) actions.Add(state, new NullableDict<A?, List<ActionHolder>>());
            if (!actions[state].ContainsKey(action)) actions[state].Add(action, new List<ActionHolder>());
            actions[state][action].Add(new ActionHolder(__id++, method));
            actions[state][action].Sort((a,b) => b.Id - a.Id);
        }

        internal void AddAll(A? action, DFlowAction method)
        {
            foreach (var state in actions.Keys)
            {
                if (!actions[state].ContainsKey(action)) actions[state].Add(action, new List<ActionHolder>());
                actions[state][action].Add(new ActionHolder(__id++, method));
                actions[state][action].Sort((a,b) => b.Id - a.Id);
            }
        }

        internal void AddExcept(S? state, Type throwable, DFlowAction method = null, DFlowExceptionAction eMethod = null)
        {
            if (state == null)
            {
                AddAllExcept(throwable, method, eMethod); 
                return;
            }
            if (method != null) exceptions[state].Add(new ExceptionHolder(throwable, method));
            if (eMethod != null) exceptions[state].Add(new ExceptionHolder(throwable, eMethod));
        }

        internal void AddAllExcept(Type throwable,  DFlowAction method = null, DFlowExceptionAction eMethod = null)
        {
            var h = method != null ? new ExceptionHolder(throwable, method) : null;
            var eh = eMethod != null ? new ExceptionHolder(throwable, eMethod) : null;
            foreach (var list in exceptions.Values)
            {
                if (h!=null) list.Add(h);
                if (eh != null) list.Add(eh);
            }
        }

        
        public void Execute(T ctx, A? action)
        {
            if (ctx.State == null) ctx.State = startState;
            ctx.Action = action;
            foreach (var act in GetAllActions(ctx.State, action))
            {
                try
                {
                    act.Method.Invoke(ctx);
                }
                catch (FlowTerminateException)
                {
                    break;
                }
                catch (Exception e)
                {
                    Except(ctx, e);
                }
            }
        }

        private void Except(T ctx, Exception e)
        {
            foreach (var eh in exceptions[ctx.State].Where(eh => eh.Throwable == null || eh.Throwable.IsInstanceOfType(e)))
            {
                try
                {
                    eh.Action?.Invoke(ctx);
                    eh.EAction?.Invoke(ctx, e);
                }
                catch (FlowTerminateException)
                {
                    break;                        
                }
            }
        }

        private IEnumerable<ActionHolder> GetAllActions(S? state, A? action)
        {
            if (action == null) return actions[state][null];
            var empty = actions[state][null];
            var notEmpty = actions[state][action];
            return empty.Concat(notEmpty).OrderBy(item => item.Id);
        }
        
        public static FlowBuilder<T, S, A> create(S startState)
        {
            return new FlowBuilder<T, S, A>(new Flow<T, S, A>(startState));
        }

        private Flow(S startState)
        {
            foreach(var value in Enum.GetValues(typeof(S)))
                actions.Add((S)value, new NullableDict<A?, List<ActionHolder>>());
        }

        public class ActionHolder
        {
            public int Id { get; }
            public DFlowAction Method { get; }
            public ActionHolder(int id, DFlowAction method)
            {
                Id = id;
                Method = method;
            }
        }
        
        public class ExceptionHolder
        {
            public Type Throwable { get; }
            public DFlowAction Action { get; }
            public DFlowExceptionAction EAction { get; }

            public ExceptionHolder(Type throwable, DFlowAction action)
            {
                Action = action;
                Throwable = throwable;
                EAction = null;
            }
            
            public ExceptionHolder(Type throwable, DFlowExceptionAction action)
            {
                Action = null;
                Throwable = throwable;
                EAction = action;
            }
            
        }
    }

}