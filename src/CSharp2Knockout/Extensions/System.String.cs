﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.IO;
using System.Text;
using ICSharpCode.NRefactory;
using ICSharpCode.NRefactory.CSharp;
using ICSharpCode.NRefactory.TypeSystem;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace CSharp2Knockout.Extensions
{
    public static class StringExtensions
    {
        public static object ToKnockout(this string @this)
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

                var errors = new List<string>();

                if(cu.Errors != null && cu.Errors.Count > 0)
                {
                    foreach(var error in cu.Errors)
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

                    if(cu.Errors.Any(x => x.ErrorType == ErrorType.Error))
                    {
                        return new
                        {
                            success = false,
                            message = "Error parsning code file",
                            errors = errors
                        };

                    }
                }

                var types = cu.GetTypes(true);
                foreach(var type in types)
                {
                    var members = type
                        .GetChildrenByRole(Roles.TypeMemberRole)
                        .Where(x => x.NodeType == NodeType.Member && x is PropertyDeclaration)
                        .Cast<PropertyDeclaration>();

                    dynamic expando = new ExpandoObject();
                    var dict = expando as IDictionary<string, object>;

                    foreach(var member in members)
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

                    var serialized = JsonConvert.SerializeObject(expando, Formatting.Indented, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                        ,
                        NullValueHandling = NullValueHandling.Include
                        ,
                        Converters = new List<JsonConverter>
                        {
                            new StringEnumConverter()
                        }
                    });

                    JObject o = JObject.Parse(serialized);
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat("var {0} = function(data) {{", type.Name);
                    sb.AppendLine();
                    sb.AppendLine("     var self = this;");
                    sb.AppendLine();
                    sb.AppendLine("     if (!data) {");
                    sb.AppendLine("         data = { };");
                    sb.AppendLine("     }");
                    sb.AppendLine();

                    foreach(var item in o.Children())
                    {
                        var property = item.ToObject<JProperty>();

                        if(o[property.Name] is JArray)
                        {
                            sb.AppendFormat("   self.{0} = ko.observableArray(data.{0});", property.Name);
                        }
                        else
                        {
                            sb.AppendFormat("   self.{0} = ko.observable(data.{0});", property.Name);
                        }
                        sb.AppendLine();

                    }

                    sb.AppendLine("};");
                    sb.AppendLine();

                    result.AppendLine(sb.ToString());
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
    }
}