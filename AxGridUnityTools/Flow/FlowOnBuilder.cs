using System;

namespace AxGrid.Flow
{
    public class FlowOnBuilder<T, S, A> 
        where T : IFlowContext<S, A> 
        where S : struct
        where A : struct
    {
        private readonly Flow<T, S, A> flow;
        private readonly S state;
        private readonly FlowBuilder<T, S, A> builder;

        internal FlowOnBuilder(Flow<T, S, A> flow, S state, FlowBuilder<T,S,A> builder)
        {
            this.flow = flow;
            this.state = state;
            this.builder = builder;
        }

        public FlowOnBuilder<T, S, A> To(A action, S to, FlowBuilder<T, S, A>.DFlowCheck check = null, bool terminate = false)
        {
            builder.To(state, action, to, check, terminate);
            return this;
        }

        public FlowOnBuilder<T, S, A> When(A action, Flow<T, S, A>.DFlowAction method)
        {
            builder.When(action, method);
            return this;
        }
        
        public FlowOnBuilder<T, S, A> When(Flow<T, S, A>.DFlowAction method)
        {
            builder.When(method);
            return this;
        }
        
        public FlowOnBuilder<T, S, A> Catch(Type ex, Flow<T, S, A>.DFlowAction method, bool terminate = false)
        {
            builder.Catch(state, ex, method, terminate);
            return this;
        }
        
        public FlowOnBuilder<T, S, A> Catch(Flow<T, S, A>.DFlowAction method, bool terminate = false)
        {
            builder.Catch(state, typeof(Exception), method, terminate);
            return this;
        }


        public FlowOnBuilder<T, S, A> Catch(Type ex, Flow<T, S, A>.DFlowExceptionAction method,
            bool terminate = false)
        {
            builder.Catch(state, ex, method, terminate);
            return this;
        }
        
        public FlowOnBuilder<T, S, A> Catch(Flow<T, S, A>.DFlowExceptionAction method,
            bool terminate = false)
        {
            builder.Catch(state, typeof(Exception), method, terminate);
            return this;
        }
        
        public FlowOnBuilder<T, S, A> Catch(Type ex,S to, bool terminate = false)
        {
            builder.Catch(state, ex, to, terminate);
            return this;
        }
        
        public FlowOnBuilder<T, S, A> Catch(S to, bool terminate = false)
        {
            builder.Catch(state, typeof(Exception), to, terminate);
            return this;
        }
    }
}