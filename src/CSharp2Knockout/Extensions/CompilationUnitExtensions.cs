using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;

namespace CSharp2Knockout.Extensions
{
    public static class CompilationUnitExtensions
    {
        public static IEnumerable<TypeDeclaration> GetUsableTypes(this CompilationUnit @this, bool includeEnums = false, bool includeInterfaces = false)
        {
            return @this.GetTypes(true)
                .Where(x => (x.ClassType != ClassType.Enum && !includeEnums) || includeEnums)
                .Where(x => (x.ClassType != ClassType.Interface && !includeInterfaces) || includeInterfaces);
        }

        public static List<string> GetErrors(this CompilationUnit @this, out bool shouldExit)
        {
            var errors = new List<string>();

            if(@this.Errors.Count == 0)
            {
                shouldExit = false;
                return errors;
            }

            foreach(var error in @this.Errors)
            {
                var line = string.Empty;
                if(error.ErrorType == ErrorType.Warning)
                {
                    line += "<strong>Warning</strong>: ";
                }
                else
                {
                    line += "<strong>Error</strong>: ";
                }

                line += error.Message;
                errors.Add(line);
            }

            if(@this.Errors.Any(x => x.ErrorType == ErrorType.Error))
            {
                shouldExit = true;
            }
            else
            {
                shouldExit = false;
            }

            return errors;
        }
    }
}