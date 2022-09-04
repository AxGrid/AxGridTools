using System;
using System.Collections.Generic;
using AxGrid.Utils;

namespace AxGridToolsConsole
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            TC t2 = new TC{
                A = 5, 
                B = "Name", 
                Z = "Hello",  
                //C = new List<string>{"World", "World2"},  
                SubObjects = new List<TC>
                {
                    new TC{A = 5,  SubObjects = new List<TC>{ new TC{A = 15}},}, 
                    new TC{A = 7},
                }
                
            };
            object res = ReflectionUtils.Get(t2, "A", 0);
            Console.WriteLine(res);
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
}