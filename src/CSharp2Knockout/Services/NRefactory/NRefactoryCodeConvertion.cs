using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.CSharp.Resolver;
using ICSharpCode.NRefactory.CSharp.TypeSystem;
using ICSharpCode.NRefactory.TypeSystem;
using NLog;

namespace CSharp2Knockout.Services.NRefactory
{
    public class NRefactoryCodeConvertion : ICodeConvertion
    {
        public ConvertionResult ToKnockoutVm(string code, TranslateOptions options = null)
        {
            if(options == null)
            {
                options = TranslateOptions.Defaults;
            }

            _log.Debug("Converting code to Knockout VM. Options:\n{0}\nCode:\n{1}", options.ToFormattedJson(), code);

            Func<ConvertionResult, ConvertionResult> exit = r =>
            {
                _log.Debug("Returning Convertion Result: {0}", r.ToFormattedJson());
                return r;
            };

            var result = new ConvertionResult();

            if(code.IsNullOrEmpty())
            {
                result.Success = false;
                result.Message = "No code provided";
                return exit(result);
            }

            // do convertion
            code = code.AddDefaultUsings();
            var visitor = new NRefactoryVisitor(options);
            var textReader = new StringReader(code);
            var parser = new CSharpParser();
            var cu = parser.Parse(textReader, "dummy.cs");

            if(parser.HasErrors)
            {
                var errors = cu.GetErrors();
                var warnings = cu.GetErrors(ErrorType.Warning);
                result.Success = false;
                result.Message = "Error parsing code file.";
                result.Errors.AddRange(errors);
                result.Warnings.AddRange(warnings);
                return exit(result);
            }

            if(parser.HasWarnings)
            {
                var warnings = cu.GetErrors(ErrorType.Warning);
                result.Message = "Parsed code contains warnings!";
                result.Warnings.AddRange(warnings);
            }

            cu.AcceptVisitor(visitor);

            var writter = new KnockoutWritter();
            var convertedCode = writter.Write(visitor.Result, options);

            result.Success = true;
            result.Code = convertedCode;

            return exit(result);
        }

        private readonly Logger _log = LogManager.GetCurrentClassLogger();
    }

    public class NRefactoryVisitor : DepthFirstAstVisitor
    {
        public override void VisitTypeDeclaration(TypeDeclaration typeDeclaration)
        {
            if(IsClass(typeDeclaration) || IsStruct(typeDeclaration))
            {
                _currentType = typeDeclaration.Name;
                _visitorResult[_currentType] = new VisitorResult.TypeResult { Name = _currentType };
            }
            else if(_options.IncludeEnums.IsTrue() && IsEnum(typeDeclaration))
            {
                _currentType = typeDeclaration.Name;
                _visitorResult[_currentType] = new VisitorResult.TypeResult { Name = _currentType, IsEnum = true };
            }

            base.VisitTypeDeclaration(typeDeclaration);
        }

        public override void VisitEnumMemberDeclaration(EnumMemberDeclaration enumMemberDeclaration)
        {
            if(_options.IncludeEnums.IsTrue())
            {
                _visitorResult[_currentType][enumMemberDeclaration.Name] = new VisitorResult.TypeResult.SemanticElement
                                                                               {
                                                                                   Name = enumMemberDeclaration.Name,
                                                                                   Value = enumMemberDeclaration.Name
                                                                               };
            }

            base.VisitEnumMemberDeclaration(enumMemberDeclaration);
        }

        public override void VisitPropertyDeclaration(PropertyDeclaration propertyDeclaration)
        {
            if(IsAccessible(propertyDeclaration))
            {
                _visitorResult[_currentType][propertyDeclaration.Name] = SetSemanticElement(propertyDeclaration);
            }

            base.VisitPropertyDeclaration(propertyDeclaration);
        }

        private VisitorResult.TypeResult.SemanticElement SetSemanticElement(PropertyDeclaration propertyDeclaration)
        {
            var semanticInfo = new VisitorResult.TypeResult.SemanticElement
            {
                Name = propertyDeclaration.Name
            };

            Func<IType, object> getValue = t =>
            {
                var dotNetType = Type.GetType(t.ReflectionName);

                // it means that we have a code, that can be anything - we do not know what it is.
                // all we can do here is check if this is an array by some name convention
                if(dotNetType == null || t.IsReferenceType.IsTrue())
                {
                    return null;
                }

                // not good, but we do not know anything about that type.
                // string is filtered above but there could be other
                // type with no parameter less constructor
                try
                {
                    return Activator.CreateInstance(dotNetType);
                }
                catch
                {
                    return null;
                }
            };

            Func<IType, object> getEnumValue = t =>
            {
                var dotNetType = Type.GetType(t.ReflectionName);

                // it means that we have a code, that can be anything - we do not know what it is.
                // all we can do here is check if this is an array by some name convention
                if(dotNetType == null)
                {
                    // do a trick here and get enums
                    var typeDefinition = _compilation
                        .Assemblies
                        .Select(x => x.GetTypeDefinition(t.Namespace, t.Name, 0))
                        .FirstOrDefault(x => x != null);

                    if(typeDefinition == null || !typeDefinition.Fields.Any())
                    {
                        return 0;
                    }

                    return typeDefinition.Fields.First().Name;
                }


                return Enum.GetNames(dotNetType)[0];
            };

            var resolverResult = _resolver.Resolve(propertyDeclaration.ReturnType);
            var type = resolverResult.Type;

            if(type.Kind == TypeKind.Unknown)
            {
                semanticInfo.Value = null;
            }
            else if(type.Kind == TypeKind.Enum)
            {
                semanticInfo.Value = getEnumValue(type);
            }
            else
            {
                if(IsArray(type))
                {
                    semanticInfo.IsArray = true;
                    semanticInfo.Value = new int[0];
                }
                else
                {
                    semanticInfo.Value = getValue(type);
                }
            }

            return semanticInfo;
        }

        private string _currentType = string.Empty;
        private IProjectContent _project;
        private ICompilation _compilation;
        private CompilationUnit _compilationUnit;
        private CSharpParsedFile _parsedFile;
        private CSharpAstResolver _resolver;

        private readonly TranslateOptions _options;
        private readonly VisitorResult _visitorResult = new VisitorResult();

        public VisitorResult Result
        {
            get { return _visitorResult; }
        }

        public NRefactoryVisitor(TranslateOptions options)
        {
            _options = options;
        }

        #region Helpers (Visitors like CU or methods)

        Lazy<IList<IUnresolvedAssembly>> builtInLibs = new Lazy<IList<IUnresolvedAssembly>>(
            delegate
            {
                Assembly[] assemblies = {
					typeof(object).Assembly, // mscorlib
					typeof(Uri).Assembly, // System.dll
					typeof(System.Linq.Enumerable).Assembly, // System.Core.dll
//					typeof(System.Xml.XmlDocument).Assembly, // System.Xml.dll
//					typeof(System.Drawing.Bitmap).Assembly, // System.Drawing.dll
//					typeof(Form).Assembly, // System.Windows.Forms.dll
					typeof(ICSharpCode.NRefactory.TypeSystem.IProjectContent).Assembly,
				};
                IUnresolvedAssembly[] projectContents = new IUnresolvedAssembly[assemblies.Length];
                Parallel.For(
                    0, assemblies.Length,
                    delegate(int i)
                    {
                        CecilLoader loader = new CecilLoader();
                        projectContents[i] = loader.LoadAssemblyFile(assemblies[i].Location);
                    });

                return projectContents;
            });

        public override void VisitCompilationUnit(CompilationUnit unit)
        {
            IProjectContent project = new CSharpProjectContent();
            _parsedFile = unit.ToTypeSystem();
            project = project.UpdateProjectContent(null, _parsedFile);
            project = project.AddAssemblyReferences(builtInLibs.Value);
            _compilation = project.CreateCompilation();
            _compilationUnit = unit;
            _resolver = new CSharpAstResolver(_compilation, _compilationUnit, _parsedFile);

            base.VisitCompilationUnit(unit);
        }

        private bool IsAccessible(PropertyDeclaration propertyDeclaration)
        {
            var publicOnly = true;
            var publicGetter = true;

            if(_options.PublicOnly.IsTrue())
            {
                publicOnly = propertyDeclaration.Modifiers == Modifiers.Public
                             || propertyDeclaration.Modifiers == (Modifiers.Public | Modifiers.Virtual)
                             || propertyDeclaration.Modifiers == (Modifiers.Public | Modifiers.New)
                             || propertyDeclaration.Modifiers == (Modifiers.Public | Modifiers.Override)
                             || propertyDeclaration.Modifiers == (Modifiers.Public | Modifiers.Abstract)
                             || propertyDeclaration.Modifiers == (Modifiers.Public | Modifiers.Sealed);
            }

            if(_options.PublicGetter.IsTrue())
            {
                publicGetter = propertyDeclaration.Getter.IsNull == false
                               && (
                                (propertyDeclaration.Getter.Modifiers == Modifiers.None && publicOnly)
                                ||
                                propertyDeclaration.Getter.Modifiers == Modifiers.Public
                               );
            }

            return publicOnly && publicGetter;
        }

        private bool IsClass(TypeDeclaration typeDeclaration)
        {
            return typeDeclaration.ClassType == ClassType.Class;
        }

        private bool IsEnum(TypeDeclaration typeDeclaration)
        {
            return typeDeclaration.ClassType == ClassType.Enum;
        }

        private bool IsStruct(TypeDeclaration typeDeclaration)
        {
            return typeDeclaration.ClassType == ClassType.Struct;
        }

        private bool IsArray(IType type)
        {
            // special case, string is array of chars!
            if(type.FullName == "System.String")
            {
                return false;
            }

            if(type.Kind == TypeKind.Array)
            {
                return true;
            }

            if(type.DirectBaseTypes.Any(x => x.Namespace == "System.Collections.Generic" || x.FullName == "System.Collections.IEnumerable"))
            {
                return true;
            }

            return false;
        }

        #endregion Helpers (Visitors like CU or methods)
    }
}