using System;
using System.Collections.Generic;
using AxGrid.Flow;
using AxGrid.Utils;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace AxGridToolsTest
{
    [TestFixture]
    public class FlowTest
    {
        public enum States
        {
            First,
            Second,
            Error
        }

        public enum Action
        {
            Tick,
            Error
        }

        public class MyException : Exception
        {
            
        } 

        public class MySubException : MyException
        {
            
        }


        [Test]
        public void ExcTest()
        {
            var s = new MySubException();

            var t = typeof(MyException);
            Assert.True(t.IsInstanceOfType(s));
            Assert.True(typeof(Exception).IsInstanceOfType(s));
        }
        
        [Test]
        public void NullableEnumTest()
        {
            var d1 = new NullableDict<States?, int>();
            d1.Add(null, 3);
            d1.Add(States.First, 1);
            d1.Add(States.Second, 2);
            Assert.AreEqual(d1[null], 3);
            Assert.AreEqual(d1[States.First], 1);
            Assert.AreEqual(d1[States.Second], 2);
        }

        [Test]
        public void CreateFlowAndExecute()
        {
            var flow = Flow<FlowContext<States, Action>, States, Action>.create(States.First)
                .When(States.First, (c) => c.State = States.Second)
                .To(States.Second, Action.Tick, States.First)
                .On(States.First, (state) =>
                {
                    state
                        .Catch(typeof(MyException), States.Error, terminate: true)
                        .Catch((ctx, e) => Console.WriteLine("ERROR:{0}", e.Message))
                        .When(Action.Tick, (ctx) =>
                        {
                            Console.WriteLine("Tick in First");
                        }).When(Action.Error, (ctx) =>
                        {
                            throw new MySubException();
                        });
                })
                .Catch(typeof(Exception), (context) =>
                {
                    Console.WriteLine("Exception");
                })
                .Build();

            var myContext = new FlowContext<States, Action>();
            flow.Execute(myContext, Action.Tick);
            flow.Execute(myContext, Action.Error);
            Assert.AreEqual(myContext.State, States.Error);
        }
    }
}