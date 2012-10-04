using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CSharp2Knockout.Services
{
    public interface ICodeConvertion
    {
        ConvertionResult ToKnockoutVm(string code, TranslateOptions options);
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class ConvertionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }
        public List<string> Warnings { get; set; }
        public string Code { get; set; }

        public ConvertionResult()
        {
            Errors = new List<string>();
            Warnings = new List<string>();
        }

        private string DebuggerDisplay
        {
            get
            {
                return "Success = {0}, Errors = {1}".FormatWith(Success, Errors.Count);
            }
        }
    }

    [DebuggerDisplay("{DebuggerDisplay,nq}")]
    public class VisitorResult
    {
        public IList<TypeResult> Types { get; set; }

        public TypeResult this[string name]
        {
            get
            {
                var item = Types.FirstOrDefault(x => string.Equals(name, x.Name, StringComparison.OrdinalIgnoreCase));

                return item;
            }
            set
            {
                var item = Types.FirstOrDefault(x => string.Equals(name, x.Name, StringComparison.OrdinalIgnoreCase));

                if(item == null)
                {
                    Types.Add(value);
                }
                else
                {
                    item = value;
                }
            }
        }

        public VisitorResult()
        {
            Types = new List<TypeResult>();
        }

        [DebuggerDisplay("{DebuggerDisplay,nq}")]
        public class TypeResult
        {
            public string Name { get; set; }
            public bool IsEnum { get; set; }
            public IList<SemanticElement> Elements { get; set; }

            public SemanticElement this[string name]
            {
                get
                {
                    var item = Elements.FirstOrDefault(x => string.Equals(name, x.Name, StringComparison.OrdinalIgnoreCase));

                    if(item == null)
                    {
                        item = new SemanticElement();
                        item.Name = name;
                        Elements.Add(item);
                    }

                    return item;
                }
                set
                {
                    var item = Elements.FirstOrDefault(x => string.Equals(name, x.Name, StringComparison.OrdinalIgnoreCase));

                    if(item == null)
                    {
                        Elements.Add(value);
                    }
                    else
                    {
                        item = value;
                    }
                }
            }

            public TypeResult()
            {
                Elements = new List<SemanticElement>();
            }

            [DebuggerDisplay("Name = {Name}, Is Array: {IsArray}, Value = {Value}")]
            public class SemanticElement
            {
                public string Name { get; set; }
                public bool IsArray { get; set; }
                public object Value { get; set; }
            }

            private string DebuggerDisplay
            {
                get { return "Name: {0}; Elements: {1}".FormatWith(Name, Elements.Count); }
            }
        }

        private string DebuggerDisplay
        {
            get { return "Types: {0}".FormatWith(Types.Count); }
        }
    }
}