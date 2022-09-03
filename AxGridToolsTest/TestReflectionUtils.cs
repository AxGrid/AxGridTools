using System;
using System.Collections.Generic;
using System.Linq;
using AxGrid.Utils;
using KellermanSoftware.CompareNetObjects;
using NUnit.Framework;

namespace AxGridToolsTest
{
    [TestFixture]
    public class TestReflectionUtils
    {
        enum MyEnum
        {
            A,
            B,
            C
            
        }
        
        [Test]
        public void TestIsArrayType()
        {
            var list = new System.Collections.Generic.List<int>();
            var objects = new object[]
            {
                list,
                new int[] {1, 2},
                (ICollection<int>) list,
                (IEnumerable<int>) list,
                new Dictionary<string, object>(),
                MyEnum.B,
            };

            foreach (var obj in objects)
            {
                Type t = obj.GetType();
                Console.WriteLine($"{t.Name}: {ReflectionUtils.IsRepeatedField(t)}");
            }
            
        }

        class TC
        {
            public int A;
            public string B { get; set; }
            public string Z = "";
            public List<string> C = new List<string>();
            public List<TC> SubObjects = new List<TC>();
        }

        [Test]
        public void TestCompare()
        {
            
            var t2 = new TC{A = 5, B = "Name", Z = "Hello",  C = new List<string>{"World", "World"},  
                SubObjects = new List<TC>
                {
                    new TC{A = 5,  SubObjects = new List<TC>{ new TC{A = 5}},}, 
                    new TC{A = 7},
                }
                
            };
            var t1 = new TC{A = 5, B = "Name2", Z = "AX", C = new List<string>{"Hello", "World"}, 
                SubObjects = new List<TC>{
                    new TC{A = 5, SubObjects = new List<TC>{ new TC{A = 6}}, }, 
                    new TC{A = 5},
                    new TC{A = 8}}
            };
            
            
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.CompareProperties = true;
            compareLogic.Config.MaxDifferences = 1000;
            compareLogic.Config.ShowBreadcrumb = true;
            
            
            ComparisonResult result = compareLogic.Compare(t2, t1);
            if (!result.AreEqual)
                Console.WriteLine(result.DifferencesString);
            Console.WriteLine($"DCount:{result.Differences.Count}");
            foreach (var r in result.Differences)
            {
                Console.WriteLine($"{r.PropertyName}");
            }   
        }

        [Test]
        public void TestNullCompare()
        {
            TC t1 = null;
            TC t2 = new TC{A = 5, B = "Name", Z = "Hello",  C = new List<string>{"World", "World"},  
                SubObjects = new List<TC>
                {
                    new TC{A = 5,  SubObjects = new List<TC>{ new TC{A = 5}},}, 
                    new TC{A = 7},
                }
                
            };
            
            CompareLogic compareLogic = new CompareLogic();
            compareLogic.Config.CompareProperties = true;
            compareLogic.Config.MaxDifferences = 1000;
            compareLogic.Config.ShowBreadcrumb = true;
            
            
            ComparisonResult result = compareLogic.Compare(t1, t2);
            if (!result.AreEqual)
                Console.WriteLine(result.DifferencesString);
            Console.WriteLine($"DCount:{result.Differences.Count}");
            
            foreach (var r in result.Differences)
            {
                Console.WriteLine($"Changed:{r.PropertyName} {r.Object2Value} in {r.Object2}");
            }   
        }
        
        [Test]
        public void GetPartOfObject()
        {
            TC t2 = new TC{A = 5, B = "Name", Z = "Hello",  C = new List<string>{"World", "World2"},  
                SubObjects = new List<TC>
                {
                    new TC{A = 5,  SubObjects = new List<TC>{ new TC{A = 15}},}, 
                    new TC{A = 7},
                }
                
            };
            object res = ReflectionUtils.Get(t2, "A", 7);
            Assert.AreEqual(res, 5);
            Assert.AreEqual(ReflectionUtils.Get(t2, "L", 7),7);
            Assert.AreEqual(ReflectionUtils.Get(t2, "C[1]", "hello"), "World2");
            Assert.AreEqual(ReflectionUtils.Get(t2, "SubObjects[0].SubObjects[0].A", 0), 15);
        }


        [Test]
        public void TestStateEventsStrings()
        {
            var e1 = "A.SubObject[0].B";
            var ev = ReflectionUtils.GetEvents(e1);
            
            Assert.AreEqual(ev.Count, 4);
            Assert.AreEqual(ev[0], "A");
            Assert.AreEqual(ev[1], "A.SubObject");
            Assert.AreEqual(ev[2], "A.SubObject[0]");
            Assert.AreEqual(ev[3], "A.SubObject[0].B");
            
            e1 = "A[0]";
            ev = ReflectionUtils.GetEvents(e1);
            
            
            Assert.AreEqual(ev.Count, 2);
            Assert.AreEqual(ev[0], "A");
            Assert.AreEqual(ev[1], "A[0]");
            
            e1 = "A";
            ev = ReflectionUtils.GetEvents(e1);
            
            Assert.AreEqual(ev.Count, 1);
            Assert.AreEqual(ev[0], "A");

        }

        [Test]
        public void TestEventsStringsClear()
        {
            var eventList = new List<string>
            {
                "A.SubObject",
                "A.Count",
                "A.Count.C",
                "A.Elements.Internal[0].B",
                "A.SubObject[0]",
                "A.SubObject[0].B",
            };
            
            var res = ReflectionUtils.ClearEvents(eventList);
            Assert.AreEqual(res.Count, 3);
            Assert.AreEqual(res[0], "A.Count");
            Assert.AreEqual(res[1], "A.SubObject");
            Assert.AreEqual(res[2], "A.Elements.Internal[0].B");
            
        }
    }

}