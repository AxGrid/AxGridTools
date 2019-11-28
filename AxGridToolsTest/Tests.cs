using System;
using AxGrid.Model;
using NUnit.Framework;

namespace AxGridToolsTest {
    [TestFixture]
    public class Tests {
        [Test]
        public void Test1() {
            var opt = new Options();
            opt.Set("test-key", "test-value");
            var text = opt.SaveAsString(allKeys:true);
            Console.WriteLine(text);
            var newOpts = Options.LoadFromString(text);
            Assert.True(newOpts.Count == 1);
            Assert.True(newOpts.GetString("test-key") == opt.GetString("test-key"));
            
        }
        
        [Test]
        public void Test2() {
            var opt = new Options();
            opt.Set("test-key", "test-value");
            opt.Set("test-key3", "test-value3");
            opt.SaveKey("test-key");
            var text = opt.SaveAsString();
            Console.WriteLine(text);
            var newOpts = Options.LoadFromString(text);
            Assert.True(newOpts.Count == 1);
            Assert.True(newOpts.GetString("test-key") == opt.GetString("test-key"));
            
        }
    }
}