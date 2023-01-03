using System;
using System.Collections.Generic;
using AxGrid.Compare;
using AxGrid.Internal.Proto;
using AxGrid.Utils.Reflections;
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
            public TC[] SubArray = null;
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
        public void TestGet()
        {
            var t = new TC
            {
                A = 1,
                B = "2",
                Z = "3",
                SubObjects = new List<TC>
                {
                    new TC
                    {
                        A = 3,
                        B = "4",
                        Z = "5",
                        SubObjects = new List<TC>
                        {
                            new TC
                            {
                                A = 9,
                            }
                        }
                    }
                },
                SubArray = new[]
                {
                    new TC
                    {
                        A = 6,
                        B = "7",
                        Z = "8",
                    }
                }
            };
            
            Assert.AreEqual(ReflectionUtils.Get(t, "A"), 1);
            Assert.AreEqual(ReflectionUtils.Get(t, "B"), "2");
            Assert.AreEqual(ReflectionUtils.Get(t, "Z"), "3");
            Assert.AreEqual(ReflectionUtils.Get(t, "SubObjects").GetType(), typeof(List<TC>));
            Assert.AreEqual(ReflectionUtils.Get(t, "SubObjects.Count"), 1);
            Assert.AreEqual(ReflectionUtils.Get(t, "SubObjects[0].A"), 3);
            Assert.AreEqual(ReflectionUtils.Get(t, "SubObjects[0].B"), "4");
            Assert.AreEqual(ReflectionUtils.Get(t, "SubObjects[0].Z"), "5");
            
            Assert.AreEqual(ReflectionUtils.Get(t, "SubArray.Length"), 1);
            Assert.AreEqual(ReflectionUtils.Get(t, "SubArray[0].A"), 6);
            Assert.AreEqual(ReflectionUtils.Get(t, "SubArray[0].B"), "7");
            Assert.AreEqual(ReflectionUtils.Get(t, "SubArray[0].Z"), "8");
            
            Assert.AreEqual(ReflectionUtils.Get(t, "SubObjects[0].SubObjects[0].A"), 9);
            
        }

        [Test]
        public void TestPartOfPath()
        {
            string path = "SubObjects[0].B.Count";
            var pop = ReflectionUtils.PartOfPath.Get(path);
            Assert.AreEqual(pop.Count, 4);
            
            
            Assert.AreEqual(pop[1].Path, "SubObjects[0]");
            Assert.AreEqual(pop[2].Path, "B");
            Assert.AreEqual(pop[3].Path, "Count");
        }
        
        [Test]
        public void GetPartOfObject()
        {
            TC t2 = new TC{
                A = 5, 
                B = "Name", 
                Z = "Hello",  
                C = new List<string>{"World", "World2"},  
                SubObjects = new List<TC>
                {
                    new TC{A = 5,  SubObjects = new List<TC>{ new TC{A = 15}},}, 
                    new TC{A = 7},
                }
            };
            object res = ReflectionUtils.Get(t2, "A", 0);
            Assert.AreEqual(res, 5);
            Assert.AreEqual(ReflectionUtils.Get(t2, "L", 7),7);
            Assert.AreEqual(ReflectionUtils.Get(t2, "C[1]", "hello"), "World2");
            Assert.AreEqual(ReflectionUtils.Get(t2, "SubObjects[0].SubObjects[0].A", 0), 15);
            Assert.AreEqual(ReflectionUtils.Get(t2, "SubObjects[2].SubObjects[0].A", 0), 0);
            
            t2 = new TC
            {
                SubArray = new TC[0],
            };
            
            Assert.AreEqual(ReflectionUtils.Get(t2, "L", 7),7);
            Assert.AreEqual(ReflectionUtils.Get(t2, "C[1]", "hello"), "hello");
            Assert.AreEqual(ReflectionUtils.Get(t2, "SubObjects[0].SubObjects[0].A", 0), 0);
            Assert.AreEqual(ReflectionUtils.Get(t2, "SubObjects[2].SubObjects[0].A", 0), 0);
            
            Assert.AreEqual(ReflectionUtils.Get(t2, "SubArray[2].SubObjects[0].A", 0), 0);

        }


        [Test]
        public void TestStateEventsStrings()
        {
            var e1 = "A.SubObject[0].B";
            var ev = ReflectionUtils.GetEvents(e1);
            
            Assert.AreEqual(ev.Count, 5);
            Assert.AreEqual(ev[0], "");
            Assert.AreEqual(ev[1], "A");
            Assert.AreEqual(ev[2], "A.SubObject");
            Assert.AreEqual(ev[3], "A.SubObject[0]");
            Assert.AreEqual(ev[4], "A.SubObject[0].B");
            
            e1 = "A[0]";
            ev = ReflectionUtils.GetEvents(e1);
            
            
            Assert.AreEqual(ev.Count, 3);
            Assert.AreEqual(ev[0], "");
            Assert.AreEqual(ev[1], "A");
            Assert.AreEqual(ev[2], "A[0]");
            
            e1 = "A";
            ev = ReflectionUtils.GetEvents(e1);
            
            Assert.AreEqual(ev.Count, 2);
            Assert.AreEqual(ev[0], "");
            Assert.AreEqual(ev[1], "A");

            
            
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

        [Test]
        public void TestGetOrCreatePath()
        {
            var state = new PState();
            var path = "Struct.Name";
            var obj = ReflectionUtils.GetPathOrCreate(state, path);
            

        }

        [Test]
        public void TestDeep()
        {
            PState state = new PState
            {
                Id = 1,
                Name = "Name",
                StateEnums = {PStateEnum.Ok, PStateEnum.Error, PStateEnum.Ok},
                StateEnum = PStateEnum.Error,
                Strings = {"Hello", "World"},
                Struct = new PSubStruct
                {
                    Id = 2,
                    Name = "Sub Name",
                    SubStruct = new PSubStruct
                    {
                        Id = 3,
                    },
                    Structs =
                    {
                        new PSubStruct {Id = 4,},
                        new PSubStruct {Id = 5,},
                    },
                },
                Structs =
                {
                    new PSubStruct {Id = 6, Name = "Subs Name"},
                    new PSubStruct {Id = 7,},
                }
            };
            var state2 = state.Clone();
            Assert.AreEqual(state.Id, state2.Id);
        }

        
        
        [Test]
        public void PropertyOrFieldTest()
        {
            PState state = new PState();
            PropertyOrField pof = new PropertyOrField(state, "Name");
            Assert.AreEqual(pof.GetValueType(), typeof(string));
            pof.SetValue(state, "Hello");
            Assert.AreEqual(state.Name, "Hello");
            
            var pofa = new PropertyOrFieldArray(state.Structs);
            pofa.SetValue(state.Structs, 3, new PSubStruct {Id = 6, Name = "Subs Name"});
            Assert.IsNotNull(state.Structs);
            Assert.AreEqual(state.Structs.Count, 4);
            Assert.IsNotNull(state.Structs[3]);
            Assert.AreEqual(state.Structs[3].Name, "Subs Name");
            Assert.AreEqual(state.Structs[3].Id, 6);
            
            Assert.AreEqual((pofa.GetValue(state.Structs,3) as PSubStruct).Id, 6);
            Assert.AreEqual((pofa.GetValue(state.Structs,3) as PSubStruct).Name, "Subs Name");
            

            
            

            
            
            //
            // pof = new PropertyOrField(state, "StateEnum");
            // Assert.AreNotEqual(state.StateEnum, PStateEnum.Ok);
            // pof.SetValue(state, PStateEnum.Ok);
            // Assert.AreEqual(state.StateEnum, PStateEnum.Ok);
            //
            // Assert.AreEqual(state.StateEnums.Count, 0);
            // pof = new PropertyOrField(state, "StateEnums");
            // pof.SetValue(state, PStateEnum.Error, 3);
            // Assert.AreEqual(state.StateEnums.Count, 4);
            // Assert.AreEqual(state.StateEnums[3], PStateEnum.Error);

            
        }
    }

}