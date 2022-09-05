using System;
using System.Collections.Generic;
using AxGrid.State;
using AxGrid.Utils;
using log4net.Config;
using NUnit.Framework;

namespace AxGridToolsTest
{
    [TestFixture]
    public class TestStateManager
    {
        public class StateReceiverObject
        {
            public int count = 0;
            public string tx = "";

            public int subCount = 0;
            
            [SmartState("sub.subName")]
            public void Event1(string t)
            {
                Console.WriteLine($"t:{t}");
                tx = t;
                count++;
            }
            
             
            [SmartState("Subs")]
            public void Event2(List<StateObject.Sub> sub)
            {
                subCount++;
                Console.WriteLine($"Subs:{sub?.Count}");
            }
        }
        
        public class StateObject
        {
            public string name = "";
            public Sub sub = new Sub();
            public class Sub
            {
                public string subName = "test";
            }

            public List<Sub> Subs;
        }
        
        [SetUp]
        public void SetupBeforeEachTest()
        {
            BasicConfigurator.Configure();
        }

        [Test]
        public void TestSmartStateGet()
        {
            var so = new StateObject();
            Assert.AreEqual("test", ReflectionUtils.Get(so, "sub.subName"));
        }

        [Test]
        public void TestSmartState()
        {
            var state = new SmartState<StateObject>();
            var receiver = new StateReceiverObject();
            state.Add(receiver);
            Assert.Null(state.State);
            state.Apply(new StateObject());
            Assert.NotNull(state.State);
            Assert.AreEqual(receiver.subCount, 1);
            Assert.AreEqual(receiver.count, 1);
            Assert.AreEqual(receiver.tx, "test");
            state.Apply(new StateObject
            {
                sub = new StateObject.Sub
                {
                    subName = "oops!"
                }
            });
            Assert.AreEqual(receiver.count, 2);
            Assert.AreEqual(receiver.tx, "oops!");
            Assert.AreEqual(receiver.subCount, 1);
        }

        [Test]
        public void TestSmartStateEvents()
        {
            var so = new StateObject();
            var sro = new StateReceiverObject();
            var ssem = new SmartStateEventManager();
            ssem.Add(sro);
            
            ssem.Invoke("sub.subName", so);
            Assert.AreEqual(sro.count, 1);

            
            ssem.Invoke("sub", so);
            Assert.AreEqual(sro.count, 2);

            ssem.Invoke("test", so);
            Assert.AreEqual(sro.count, 2);

            ssem.Invoke("sub.subName.test", so);
            Assert.AreEqual(sro.count, 2);

        }
        
    }
}