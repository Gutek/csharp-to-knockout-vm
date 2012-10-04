using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;

namespace CSharp2Knockout.Services.NRefactory
{
    public static class NRefactoryCodeTextExtensions
    {
        public static readonly int UsingLineCount = 9;

        public static List<string> GetErrors(this CompilationUnit @this, ErrorType errorType = ErrorType.Error)
        {
            var errors = new List<string>();

            if(@this.Errors.Count == 0)
            {
                return errors;
            }

            foreach(var error in @this.Errors.Where(x => x.ErrorType == errorType))
            {
                var line = String.Empty;

                if(error.Region.BeginLine > 0)
                {
                    var errorLine = "<a data-line=\"{0}\" href=\"javascript:void(0)\">Line {0}</a>: ".FormatWith(error.Region.BeginLine - UsingLineCount);
                    line += errorLine;
                }

                line += error.Message;

                errors.Add(line);
            }

            return errors;
        }

        public static string AddDefaultUsings(this string @this)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections;");
            sb.AppendLine("using System.Collections.Concurrent;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using System.Collections.ObjectModel;");
            sb.AppendLine("using System.Collections.Specialized;");
            sb.AppendLine("using System.Diagnostics;");
            sb.AppendLine("using System.Linq;");
            sb.AppendLine();
            sb.AppendLine(@this);

            return sb.ToString();
        }
    }
}