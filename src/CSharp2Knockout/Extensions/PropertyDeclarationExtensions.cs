using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace CSharp2Knockout.Extensions
{
    public static class PropertyDeclarationExtensions
    {
        public static dynamic SetMembers(this IEnumerable<PropertyDeclaration> @this, bool? sortProps)
        {
            dynamic expando = new ExpandoObject();
            var dict = expando as IDictionary<string, object>;

            if(sortProps.HasValue && sortProps.Value)
            {
                @this = @this.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
            }

            foreach(var member in @this)
            {
                var type = member.ReturnType;

                //var isPrimitive = type as PrimitiveType;
                //var isSimpleType = type as SimpleType;

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