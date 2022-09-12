using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using AxGrid.Utils;
using AxGrid.Utils.Reflections;
using Google.Protobuf;

namespace AxGrid.State
{
    public class AxStateApplicator<T> where T : IDeepCloneable<T>
    {
        public T State { get; }

        private static readonly Regex OperatorRe = new Regex(@"^(?<field>\w+)(?<operator>\!)?(\[(?<index>[0-9]+)\])?$", RegexOptions.Compiled);
        private static readonly Regex PathRe = new Regex(@"^(?<field>\w+)(\[(?<index>[0-9]+)\])?$", RegexOptions.Compiled);
        
        public void ApplyDiff(T diff, List<string> paths)
        {
            if (diff == null) throw new ArgumentException("diff cannot be null");
            var operators = GetOperators(paths);
            foreach (var op in operators)
            {
                Apply(diff, op);
            }
        }

        public List<AxOperator> GetOperators(string path)
        {
            var list = new List<string> {path};
            return GetOperators(list);
        }
        
        public List<AxOperator> GetOperators(List<string> paths)
        {
            if (!paths.Any()) return new List<AxOperator>();
            var res = new List<AxOperator>();
            foreach (var line in paths)
            {
                string[] parts = line.Split('.');
                AxOperator op = new AxOperator
                {
                    Path = (line.Length > 1 ? parts.Take(parts.Length - 1).ToArray() : new string[0])
                        .Select(x => PathRe.Match(x))
                        .Select(x =>
                        {
                            if (!x.Success) throw new ArgumentException("Invalid path");
                            return x;
                        }).Select(m =>
                            new AxPath
                            {
                                Field = m.Groups["field"].Value,
                                IsArray = m.Groups["index"].Success,
                                Index = m.Groups["index"].Success ? int.Parse(m.Groups["index"].Value) : 0
                            }
                        ).ToArray(),
                };

                var fieldMatch = OperatorRe.Match(parts.Last());
                if (!fieldMatch.Success) throw new ArgumentException("Invalid path");
                op.Field = new AxPath
                {
                    Field = fieldMatch.Groups["field"].Value,
                    IsArray = fieldMatch.Groups["index"].Success,
                    Index = fieldMatch.Groups["index"].Success ? int.Parse(fieldMatch.Groups["index"].Value) : 0
                };
                op.Operator = AxOperator.GetOperator(fieldMatch.Groups["operator"].Value);
                res.Add(op);
            }

            return res;
        }

        // public AxContainer<T> GetPath(T diff, AxOperator op)
        // {
        //     var container = new AxContainer<T>
        //     {
        //         State = State,
        //         Diff = diff,
        //         StateObject = State,
        //         DiffObject = diff,
        //         
        //     };
        //     return GetPath(container, op);
        // }

        // public AxContainer<T> GetPath(AxContainer<T> container, AxOperator op)
        // {
        //     if (op.Path.Length > 0)
        //     {
        //         var path = op.Path[0];
        //         var field = new ReflectionUtils.PropertyOrField(container.StateObject, path.Field);
        //         object fieldValue = field.GetValue(container.StateObject);
        //         if (fieldValue == null)
        //         {
        //             fieldValue = Activator.CreateInstance(field.GetValueType());
        //         }
        //         field.SetValue(container.StateObject, fieldValue);
        //         
        //         if (path.IsArray)
        //         {
        //             
        //         }
        //     }
        // }
        
        private void OperatorSet(T diff, AxOperator op)
        {
            var path = op.PathsToString();
            object stateObject = State;
            object diffObject = diff;
            if (path != "")
            {
                stateObject = ReflectionUtils.GetPathOrCreate(State, path);
                diffObject = ReflectionUtils.GetPathOrCreate(diff, path);
            }
            
            var stateField = new PropertyOrField(stateObject, op.Field.Field);
            var diffField = new PropertyOrField(diffObject, op.Field.Field);
            Console.WriteLine($"Field: {op.Field.Field}[{op.Field.Index}] ");
            var value = op.Field.IsArray
                ? diffField.GetValue(diffObject, index: op.Field.Index)
                : diffField.GetValue(diffObject);
            Console.WriteLine($"Field.Value {value}");
            if (op.Field.IsArray)
            {
                stateField.SetValue(stateObject, diffField.GetValue(diffObject, index: op.Field.Index),
                    index: op.Field.Index);
            }
            else
            {
                stateField.SetValue(stateObject, diffField.GetValue(diffObject));
            }
            
        }
        
        private T Apply(T diff, AxOperator op)
        {
            switch (op.Operator)
            {
                case AxOperator.Operators.Set:
                    OperatorSet(diff, op);
                    break;
                case AxOperator.Operators.Remove:
                    break;
                default:
                    break;
            }

            return diff;
        }
       
      
        
        public AxStateApplicator(T state)
        {
            State = state;
        }
    }
    
    public class AxOperator
    {
        public enum Operators
        {
            Set,
            Remove
        }
        public static Operators GetOperator(string opString)
        {
            switch (opString)
            {
                default:
                    return Operators.Set;
                case "!":
                    return Operators.Remove;
            }
        }
        public AxPath[] Path { get; set; }
        public AxPath Field { get; set; }
        public Operators Operator { get; set; }

        public string PathsToString()
        {
            if (Path.Length == 0) return "";
            return Path.Select(item => $"{item.Field}{(item.IsArray ? $"[{item.Index}]" : "")}")
                .Aggregate((a, b) => $"{a}.{b}");
        }


        public override string ToString()
        {
            return "Path: " + string.Join(".", Path.Select(x => x.ToString())) + " Field: " + Field + " Operator: " + Operator;
        }
    }

    public class AxPath
    {
        public string Field { get; set; }
        public bool IsArray { get; set; }
        public int? Index { get; set; }

        public override string ToString()
        {
            return IsArray ? $"{Field}[{Index}]" : Field;
        }
    }

    public class AxContainer<T> where T : IDeepCloneable<T>
    {
        public T Diff { get; set; }
        public T State { get; set; }
        public object DiffObject { get; set; }
        public object StateObject { get; set; }
        public PropertyOrField DiffField { get; set; }
        public PropertyOrField StateField { get; set; }
    }
   
    
}