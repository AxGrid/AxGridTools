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

        public delegate void DFlowOnAction(FlowOnBuilder<T, S, A> builder);

        public FlowBuilder<T, S, A> On(S state, DFlowOnAction inState)
        {
            inState.Invoke(new FlowOnBuilder<T, S, A>(flow, state, this));
            return this;
        }
        
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

        
        public FlowBuilder<T, S, A> To(S state, A action, S to, DFlowCheck check = null, bool terminate = false)
        {
            return When(state, action, (ctx) =>
            {
                if (check != null && !check(ctx)) return;
                ctx.State = to;
                if (terminate) throw new FlowTerminateException();
            });
        }

        public FlowBuilder<T, S, A> Catch(S? state, Type ex, Flow<T, S, A>.DFlowAction method, bool terminate = false)
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
        

        public FlowBuilder<T, S, A> Catch(S? state, Type ex, Flow<T, S, A>.DFlowExceptionAction method, bool terminate = false)
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


        public FlowBuilder<T, S, A> Catch(Type ex, Flow<T, S, A>.DFlowAction method, bool terminate = false)
        {
            return Catch(null, ex, method, terminate);
        }

        public FlowBuilder<T, S, A> Catch(Flow<T, S, A>.DFlowAction method, bool terminate = false)
        {
            return Catch(null, typeof(Exception), method, terminate);
        }
        
        
        public FlowBuilder<T, S, A> Catch(S? state, Type ex, S to, bool terminate = false)
        {
            flow.AddExcept(state, ex, eMethod: (ctx, eArg) =>
            {
                ctx.State = to;
                if (terminate) throw new FlowTerminateException();
            });
            return this;
        }


        public FlowBuilder<T, S, A> Catch(Type ex, S to, bool terminate = false)
        {
            return Catch(null, ex, to, terminate);
        }

        public FlowBuilder<T, S, A> Catch(S to, bool terminate = false)
        {
            return Catch(null, typeof(Exception), to, terminate);
        }

        public FlowBuilder<T, S, A> terminate(S? state, A action, DFlowCheck check)
        {
            flow.Add(state, action, (ctx) =>
            {
                if (check.Invoke(ctx)) throw new FlowTerminateException();
            });
            return this;
        }
        
        public FlowBuilder<T, S, A> terminate(S? state, DFlowCheck check)
        {
            flow.Add(state, null, (ctx) =>
            {
                if (check.Invoke(ctx)) throw new FlowTerminateException();
            });
            return this;
        }
        
        public FlowBuilder<T, S, A> terminate(DFlowCheck check)
        {
            flow.Add(null, null, (ctx) =>
            {
                if (check.Invoke(ctx)) throw new FlowTerminateException();
            });
            return this;
        }
        
        internal FlowBuilder(Flow<T, S, A> flow)
        {
            this.flow = flow;
        }
        
    }
}