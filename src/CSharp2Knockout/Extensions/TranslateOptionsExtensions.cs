using System.Collections.Generic;
using CSharp2Knockout.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace CSharp2Knockout.Extensions
{
    public static class TranslateOptionsExtensions
    {
        public static JsonSerializerSettings ToJsonSettings(this TranslateOptions @this)
        {
            var settings = new JsonSerializerSettings();

            if(@this.CamelCase.Value || @this.ForceCamelCase.Value)
            {
                settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }

            settings.NullValueHandling = NullValueHandling.Include;

            settings.Converters = new List<JsonConverter> { new StringEnumConverter() };

            return settings;
        }
    }
}