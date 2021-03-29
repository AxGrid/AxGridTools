using System;

namespace AxGrid.Flow
{
    public class FlowBuilder<T, S, A> 
        where T : IFlowContext<S, A> 
        where S : struct
        where A : struct
    {
        private readonly Flow<T, S, A> flow;
        public Flow<T, S, A> Build() => flow;

        public delegate bool DFlowCheck(T ctx);
        
        public FlowBuilder<T, S, A> When(S state, A action, Flow<T, S, A>.DFlowAction method)
        {
            flow.Add(state, action, method);
            return this;
        }

        public FlowBuilder<T, S, A> When(S state, Flow<T, S, A>.DFlowAction method)
        {
            flow.Add(state, default, method);
            return this;
        }

        public FlowBuilder<T, S, A> When(A action, Flow<T, S, A>.DFlowAction method)
        {
            flow.AddAll(action, method);
            return this;
        }

        public FlowBuilder<T, S, A> When(Flow<T, S, A>.DFlowAction method)
        {
            flow.AddAll(default, method);
            return this;
        }

        
        public FlowBuilder<T, S, A> Transition(S state, A action, S to, DFlowCheck check = null, bool terminate = false)
        {
            return When(state, action, (ctx) =>
            {
                if (check != null && !check(ctx)) return;
                ctx.State = to;
                if (terminate) throw new FlowTerminateException();
            });
        }

        public FlowBuilder<T, S, A> Exception(S? state, Type ex, Flow<T, S, A>.DFlowAction method, bool terminate = false)
        {
            if (!terminate)
                flow.AddExcept(state, ex, method);
            else
                flow.AddExcept(state, ex, (ctx) =>
                {
                    method.Invoke(ctx);
                    throw new FlowTerminateException();
                });
            return this;
        }
        

        public FlowBuilder<T, S, A> Exception(S? state, Type ex, Flow<T, S, A>.DFlowExceptionAction method, bool terminate = false)
        {
            if (!terminate)
                flow.AddExcept(state, ex, eMethod: method);
            else
                flow.AddExcept(state, ex, eMethod: (ctx, eArg) =>
                {
                    method.Invoke(ctx, eArg);
                    throw new FlowTerminateException();
                });
            return this;
        }

        
        
        
        internal FlowBuilder(Flow<T, S, A> flow)
        {
            this.flow = flow;
        }
        
    }
}