using System.Collections.Generic;
using System.Dynamic;
using ICSharpCode.NRefactory.CSharp;

namespace CSharp2Knockout.Extensions
{
    public static class PropertyDeclarationExtensions
    {
        public static dynamic SetMembers(this IEnumerable<PropertyDeclaration> @this)
        {
            dynamic expando = new ExpandoObject();
            var dict = expando as IDictionary<string, object>;

            foreach(var member in @this)
            {
                var text = member.ReturnType.GetText();
                if(text.IsArray())
                {
                    dict[member.Name] = new int[0];
                }
                else
                {
                    dict[member.Name] = null;
                }
            }

            return expando;
        }
    }
}