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
                       .Cast<PropertyDeclaration>();

            if(publicOnly)
            {
                types = types.Where(x => x.Modifiers == Modifiers.Public
                                 || x.Modifiers == (Modifiers.Public | Modifiers.Virtual)
                                 || x.Modifiers == (Modifiers.Public | Modifiers.New)
                                 || x.Modifiers == (Modifiers.Public | Modifiers.Override)
                                 || x.Modifiers == (Modifiers.Public | Modifiers.Abstract)
                                 || x.Modifiers == (Modifiers.Public | Modifiers.Sealed));
            }

            if(withPublicGetter)
            {
                types =
                    types.Where(
                        x =>
                        (x.Getter.Modifiers == Modifiers.None || x.Getter.Modifiers == Modifiers.Public) &&
                        !x.Getter.IsNull);
            }

            return types;
        }

        public static IEnumerable<EnumMemberDeclaration> GetEnums(this TypeDeclaration @this)
        {
            var types = @this.GetChildrenByRole(Roles.TypeMemberRole)
                       .Where(x => x.NodeType == NodeType.Member
                           && x is EnumMemberDeclaration)
                       .Cast<EnumMemberDeclaration>()
                       ;

            return types;
        }
    }
}