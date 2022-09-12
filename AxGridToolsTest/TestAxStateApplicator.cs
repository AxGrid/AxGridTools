using System.Collections.Generic;
using AxGrid.Internal.Proto;
using AxGrid.State;
using log4net;
using log4net.Config;
using NUnit.Framework;

namespace AxGridToolsTest
{
    [TestFixture]
    public class TestAxStateApplicator
    {
        private readonly ILog _log = LogManager.GetLogger(typeof(TestAxStateApplicator));
        
        
        
        [SetUp]
        public void SetupBeforeEachTest()
        {
            BasicConfigurator.Configure();
        }

        [Test]
        public void SetOperators()
        {
            var mod = "Struct.SubStruct1[2].SubStruct2.SubStruct3.SubStruct4.Id";
            var applicator = new AxStateApplicator<PState>(new PState());
            var operators = applicator.GetOperators(mod);
            Assert.AreEqual(operators.Count, 1);
            _log.Debug($"Operators[0].Path:{operators[0]}");
            
            Assert.AreEqual(operators[0].Path.Length, 5);
            Assert.AreEqual(operators[0].Field.Field, "Id");
            Assert.AreEqual(operators[0].Field.IsArray, false);
            Assert.AreEqual(operators[0].Operator, AxOperator.Operators.Set);
            Assert.IsTrue(operators[0].Path[1].IsArray);
            Assert.AreEqual(operators[0].Path[1].Index, 2);
        }
        
        
        [Test]
        public void RemoveOperators()
        {
            var mod = "Struct.SubStruct1[2].SubStruct2.SubStruct3.SubStruct4.Id!";
            var applicator = new AxStateApplicator<PState>(new PState());
            var operators = applicator.GetOperators(mod);
            Assert.AreEqual(operators.Count, 1);
            _log.Debug($"Operators[0].Path:{operators[0]}");
            
            Assert.AreEqual(operators[0].Path.Length, 5);
            Assert.AreEqual(operators[0].Field.Field, "Id");
            Assert.AreEqual(operators[0].Field.IsArray, false);
            Assert.AreEqual(operators[0].Operator, AxOperator.Operators.Remove);
            Assert.IsTrue(operators[0].Path[1].IsArray);
            Assert.AreEqual(operators[0].Path[1].Index, 2);
        }
        
        [Test]
        public void RemoveOperatorsIndex()
        {
            var mod = "Struct.SubStruct1[2].SubStruct2.SubStruct3.SubStruct4.Id![4]";
            var applicator = new AxStateApplicator<PState>(new PState());
            var operators = applicator.GetOperators(mod);
            Assert.AreEqual(operators.Count, 1);
            _log.Debug($"Operators[0].Path:{operators[0]}");
            
            Assert.AreEqual(operators[0].Path.Length, 5);
            Assert.AreEqual(operators[0].Field.Field, "Id");
            Assert.AreEqual(operators[0].Field.IsArray, true);
            Assert.AreEqual(operators[0].Field.Index, 4);
            Assert.AreEqual(operators[0].Operator, AxOperator.Operators.Remove);
            Assert.IsTrue(operators[0].Path[1].IsArray);
            Assert.AreEqual(operators[0].Path[1].Index, 2);
        }

        [Test]
        public void ApplyDiff()
        {
            var state = new PState{Id = 5, Name = "Oops!"};
            var mod = "Id";
            var applicator = new AxStateApplicator<PState>(state);
            
            applicator.ApplyDiff(
                new PState{  Name = "Test", StateEnum = PStateEnum.Ok, Struct = new PSubStruct{Name = "SubTest"}, Structs = { new PSubStruct(), new PSubStruct{Id =15} }}, 
                new List<string>{ "Name", "StateEnum", "Struct", "Structs[1].Id" } 
            );
            Assert.AreEqual(applicator.State.Name, "Test");
            Assert.AreEqual(state.Name, applicator.State.Name);
            Assert.AreEqual(state.Id, 5);
            Assert.AreEqual(state.Name, "Test");
            Assert.AreEqual(state.StateEnum, PStateEnum.Ok);
            Assert.NotNull(state.Struct);
            Assert.AreEqual(state.Struct.Name, "SubTest");
            Assert.AreEqual(state.Structs.Count, 2);
            Assert.AreEqual(state.Structs[1].Id, 15);
        }
        

    }
}