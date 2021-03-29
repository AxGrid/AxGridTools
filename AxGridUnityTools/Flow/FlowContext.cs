using System;

namespace AxGrid.Flow
{
    public class FlowContext<S, A> : IFlowContext<S, A>
        where S : struct
        where A : struct
    {
        public S? State { get; set; }

        private A? __action;

        public A? Action
        {
            get => __action;
            set
            {
                this.LastAction = this.__action;
                this.__action = value;
            }
        }

        public A? LastAction { get; private set;  }
        public Exception Throwable { get; set; }
    }
}