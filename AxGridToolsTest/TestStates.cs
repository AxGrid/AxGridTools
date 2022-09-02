using System;
using AxGrid.State;
using NUnit.Framework;

namespace AxGridToolsTest
{
    [TestFixture]
    public class TestStates
    {
        public class MySimpleState
        {
            public string name = "";
            public int Id { get; set; }
            public MySubClass Sub { get; set; } = new MySubClass();


            public class MySubClass
            {
                public string SubName { get; set; } = "SubName";
            

            public int SubClassId { get; set; } = -1;
                public System.Collections.Generic.List<string> Names { get; set; }
            }
        }

        [Test]
        public void TestStateDictionary()
        {
            MySimpleState state = new MySimpleState();
            var ss = new SmartState<MySimpleState>(state);
            Console.WriteLine("---> {0}", ss["Sub.SubName"].GetValue(state));
        }
        
    }
}