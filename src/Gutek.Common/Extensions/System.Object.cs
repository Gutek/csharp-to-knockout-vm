using System.Diagnostics;
using Newtonsoft.Json;

namespace System
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object @this, bool forceNoFormatting = false)
        {
            var formatting = Debugger.IsAttached && !forceNoFormatting ? Formatting.Indented : Formatting.None;

            return JsonConvert.SerializeObject(@this, formatting);
        }

        public static string ToFormattedJson(this object @this)
        {
            var formatting = Formatting.Indented;

            return JsonConvert.SerializeObject(@this, formatting);
        }
    }
}