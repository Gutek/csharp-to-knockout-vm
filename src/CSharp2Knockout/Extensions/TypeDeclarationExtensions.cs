using System.Collections.Generic;
using System.Linq;
using ICSharpCode.NRefactory.CSharp;

namespace CSharp2Knockout.Extensions
{
    public static class TypeDeclarationExtensions
    {
        public static IEnumerable<PropertyDeclaration> GetProperties(this TypeDeclaration @this, bool publicOnly = true, bool withPublicGetter = true)
        {
            var types = @this.GetChildrenByRole(Roles.TypeMemberRole)
                       .Where(x => x.NodeType == NodeType.Member
                           && x is PropertyDeclaration)
                       .Cast<PropertyDeclaration>()
                       .Where(x => !publicOnly || (x.Modifiers == Modifiers.Public && publicOnly))
                       .Where(x => !withPublicGetter || ((x.Getter.Modifiers == Modifiers.None || x.Getter.Modifiers == Modifiers.Public) && withPublicGetter && !x.Getter.IsNull))
                       .ToList();

            return types;
        }
    }
}