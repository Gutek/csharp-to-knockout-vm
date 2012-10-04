using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using CSharp2Knockout.Extensions;
using NLog;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSharp2Knockout.Services
{
    public interface IVisitorResultWritter
    {
        string Write(VisitorResult result, TranslateOptions options = null);
    }

    public class KnockoutWritter : IVisitorResultWritter
    {
        private TranslateOptions _options;

        public string Write(VisitorResult result, TranslateOptions options = null)
        {

            if(result == null)
            {
                return null;
            }

            if(result.Types.Count == 0)
            {
                return null;
            }

            if(options == null)
            {
                options = TranslateOptions.Defaults;
            }

            _options = options;

            IEnumerable<VisitorResult.TypeResult> types = result.Types;

            if(_options.SortProps.IsTrue())
            {
                types = types.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
            }

            foreach(var type in types)
            {
                if(_options.IncludeEmptyType.IsFalse() && type.Elements.Count == 0)
                {
                    continue;
                }

                if(type.IsEnum)
                {
                    WriteEnumDeclaration(type);
                }
                else
                {
                    WriteClassDeclaration(type);
                }
            }

            var code = _console.ToString();

            // remove last line
            code = code.RemoveEmptyLastLine();

            return code;
        }

        private void WriteEnumDeclaration(VisitorResult.TypeResult type)
        {
            _console.AppendFormat("var {0} = {{", type.Name);
            _console.AppendLine();

            IList<VisitorResult.TypeResult.SemanticElement> props = type.Elements;

            if(_options.SortProps.IsTrue())
            {
                props = props.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList();
            }

            for(var i = 0; i < props.Count; i++)
            {
                var propName = _options.UseCamelCaseForKo ? props[i].Name.ToCamelCase() : props[i].Name;
                _console.AppendFormat("{0}{1}: '{2}'", SetIdent(1), propName, props[i].Name);
                if(i + 1 < props.Count)
                {
                    _console.Append(",");
                }
                _console.AppendLine();
            }

            _console.AppendLine("};");
            _console.AppendLine();
        }

        private void WriteClassDeclaration(VisitorResult.TypeResult type)
        {
            _console.AppendFormat("var {0} = function (data) {{", type.Name);
            _console.AppendLine();
            _console.AppendFormat("{0}var self = this;", SetIdent(1));
            _console.AppendLine();
            _console.AppendLine();

            if(_options.IncludeDefaultData.IsTrue())
            {
                WritePropertyDeclarationComplex(type);
            }
            else
            {
                IEnumerable<VisitorResult.TypeResult.SemanticElement> props = type.Elements;

                if(_options.SortProps.IsTrue())
                {
                    props = props.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
                }

                _console.AppendFormat("{0}if (!data) {{", SetIdent(1));
                _console.AppendLine();
                _console.AppendFormat("{0}data = {{ }};", SetIdent(2));
                _console.AppendLine();
                _console.AppendFormat("{0}}}", SetIdent(1));
                _console.AppendLine();
                _console.AppendLine();

                foreach(var semantic in props)
                {
                    WritePropertyDeclarationSimple(semantic);
                }
            }

            _console.AppendLine("};");
            _console.AppendLine();
        }

        private void WritePropertyDeclarationSimple(VisitorResult.TypeResult.SemanticElement semanticElement, int identLevel = 1)
        {
            var koPropName = _options.UseCamelCaseForKo ? semanticElement.Name.ToCamelCase() : semanticElement.Name;
            var propName = _options.UseCamelCaseForAll ? semanticElement.Name.ToCamelCase() : semanticElement.Name;

            if(semanticElement.IsArray)
            {
                _console.AppendFormat("{0}self.{1} = ko.observableArray(data.{2});", SetIdent(identLevel), koPropName, propName);
            }
            else
            {
                _console.AppendFormat("{0}self.{1} = ko.observable(data.{2});", SetIdent(identLevel), koPropName, propName);
            }

            _console.AppendLine();
        }

        private void WritePropertyDefaultValue(VisitorResult.TypeResult.SemanticElement semanticElement, int identLevel = 1)
        {
            var koPropName = _options.UseCamelCaseForKo ? semanticElement.Name.ToCamelCase() : semanticElement.Name;

            if(semanticElement.IsArray)
            {
                _console.AppendFormat("{0}self.{1} = ko.observableArray([]);", SetIdent(identLevel), koPropName);
            }
            else
            {
                var objVal = JsonConvert.ToString(semanticElement.Value);

                if(objVal != null && objVal != "null")
                {
                    _console.AppendFormat("{0}self.{1} = ko.observable({2});", SetIdent(identLevel), koPropName, objVal);
                }
                else
                {
                    _console.AppendFormat("{0}self.{1} = ko.observable();", SetIdent(identLevel), koPropName);
                }

            }

            _console.AppendLine();
        }

        private void WritePropertyDeclarationComplex(VisitorResult.TypeResult type)
        {
            // serialize so we will have proper default values for .NET types including dates and stuff like that
            //var serialized = JsonConvert.SerializeObject(type, Formatting.Indented, _options.ToJsonSettings());
            //var serialized2 = new JavaScriptSerializer().Serialize(type);
            //var json = JObject.Parse(serialized);
            //var elements = json.Children<JProperty>().Where(x => x.Name == "elements").Children().FirstOrDefault() as JArray;
            //var objects = elements.Select(x => x.ToObject<JObject>()).ToList();

            IEnumerable<VisitorResult.TypeResult.SemanticElement> props = type.Elements;

            if(_options.SortProps.IsTrue())
            {
                props = props.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase);
            }

            _console.AppendFormat("{0}if (data) {{", SetIdent(1));
            _console.AppendLine();

            foreach(var semantic in props)
            {
                WritePropertyDeclarationSimple(semantic, 2);
            }

            _console.AppendFormat("{0}}} else {{", SetIdent(1));
            _console.AppendLine();

            foreach(var semantic in props)
            {
                WritePropertyDefaultValue(semantic, 2);
            }

            _console.AppendFormat("{0}}}", SetIdent(1));
            _console.AppendLine();
        }

        private string SetIdent(int identLevel)
        {
            return new string(' ', identLevel * 4);
            //StringBuilder ident = new StringBuilder();

            //for(int i = 0; i < identLevel; i++)
            //{
            //    ident.Append("\t");
            //}

            //return ident.ToString();
        }

        private readonly StringBuilder _console = new StringBuilder();
        private readonly Logger _log = LogManager.GetCurrentClassLogger();
    }
}