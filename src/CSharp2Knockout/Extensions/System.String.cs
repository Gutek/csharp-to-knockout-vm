using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CSharp2Knockout.Services;
using ICSharpCode.NRefactory.CSharp;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CSharp2Knockout.Extensions
{
    public static class StringExtensions
    {
        public static object ToKnockout(this string @this, TranslateOptions options)
        {
            if(string.IsNullOrWhiteSpace(@this))
            {
                return new
                {
                    success = false,
                    message = "I see dead code - add some and try again"
                };
            }

            try
            {
                var result = new StringBuilder();
                var textReader = new StringReader(@this);
                var parser = new CSharpParser();
                var cu = parser.Parse(textReader, "dummy.cs");

                bool shouldExit;
                var errors = cu.GetErrors(out shouldExit);

                if(shouldExit)
                {
                    return new
                    {
                        success = false,
                        message = "Error parsning code file",
                        errors = errors
                    };
                }

                var types = cu.GetUsableTypes(options.IncludeEnums.Value);
                foreach(var type in types)
                {
                    if(type.ClassType == ClassType.Enum)
                    {
                        var enumMembers = type.GetEnums();
                        dynamic enumExpando = enumMembers.SetMembers(options.SortProps);
                        string enumSerialized = JsonConvert.SerializeObject(enumExpando, Formatting.Indented, options.ToJsonSettings());
                        result.AppendLine(enumSerialized.ToJsEnum(type.Name));
                    }
                    else
                    {
                        var members = type.GetProperties(options.PublicOnly.Value, options.PublicGetter.Value).ToList();

                        dynamic expando = members.SetMembers(options.SortProps);
                        string serialized = JsonConvert.SerializeObject(expando, Formatting.Indented, options.ToJsonSettings());
                        result.AppendLine(serialized.ToKnockoutViewModel(options, type.Name, expando as IDictionary<string, object>));
                    }
                }

                return new
                {
                    success = true,
                    message = result.ToString(),
                    errors = errors
                };
            }
            catch(Exception ex)
            {
                return new
                {
                    success = false,
                    message = ex.Message
                };
            }
        }

        public static bool IsArray(this string @this)
        {
            return @this.StartsWith("Enumerable")
                   || @this.StartsWith("IEnumerable")
                   || @this.StartsWith("List")
                   || @this.StartsWith("IList")
                   || @this.StartsWith("Collection")
                   || @this.StartsWith("ICollection");
        }

        public static string ToKnockoutViewModel(this string @this, TranslateOptions options, string name, IDictionary<string, object> orginal = null)
        {
            JObject o = JObject.Parse(@this);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("var {0} = function (data) {{", name);
            sb.AppendLine();
            sb.AppendLine("\tvar self = this;");
            sb.AppendLine();
            sb.AppendLine("\tif (!data) {");
            sb.AppendLine("\t\tdata = { };");
            sb.AppendLine("\t}");
            sb.AppendLine();

            foreach(var item in o.Children())
            {
                var property = item.ToObject<JProperty>();
                string key = property.Name;

                if(orginal != null && options.CamelCase.Value && !options.ForceCamelCase.Value)
                {
                    var orginalKey = orginal
                        .Where(x => x.Key.Equals(key, StringComparison.OrdinalIgnoreCase))
                        .Select(x => x.Key)
                        .FirstOrDefault();

                    if(orginalKey.IsNotNullOrEmpty())
                    {
                        key = orginalKey;
                    }
                }

                if(o[property.Name] is JArray)
                {
                    sb.AppendFormat("\tself.{0} = ko.observableArray(data.{1});", property.Name, key);

                }
                else
                {
                    sb.AppendFormat("\tself.{0} = ko.observable(data.{1});", property.Name, key);

                }
                sb.AppendLine();
            }

            sb.AppendLine("};");

            return sb.ToString();
        }

        public static string ToJsEnum(this string @this, string name)
        {
            JObject o = JObject.Parse(@this);
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("var {0} = {{", name);
            sb.AppendLine();

            var children = o.Children().ToList();

            for(int i = 0; i < children.Count; i++)
            {
                var property = children[i].ToObject<JProperty>();

                sb.AppendFormat("\t{0}: '{1}'", property.Name, o[property.Name].Value<string>());
                if(i + 1 < children.Count)
                {
                    sb.Append(",");
                }
                sb.AppendLine();
            }

            sb.AppendLine("};");

            return sb.ToString();
        }
    }
}