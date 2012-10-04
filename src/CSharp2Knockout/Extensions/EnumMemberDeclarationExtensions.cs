using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace CSharp2Knockout.Extensions
{
    public static class EnumMemberDeclarationExtensions
    {
        public static dynamic SetMembers(this IEnumerable<EnumMemberDeclaration> @this, bool? sortProps)
        {
            dynamic expando = new ExpandoObject();
            var dict = expando as IDictionary<string, object>;

            if(sortProps.HasValue && sortProps.Value)
            {
                @this = @this.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
            }

            foreach(var member in @this)
            {
                dict[member.Name] = member.Name;
            }
            return expando;
        }
    }
}