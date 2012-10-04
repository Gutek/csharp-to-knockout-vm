using System.Globalization;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace System
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string @this)
        {
            return string.IsNullOrEmpty(@this);
        }

        public static bool IsNotNullOrEmpty(this string @this)
        {
            return !@this.IsNullOrEmpty();
        }

        public static string FormatWith(this string @this, params object[] args)
        {
            return string.Format(@this, args);
        }

        public static bool HasText(this string @this)
        {
            return !HasNoText(@this);
        }

        public static bool HasNoText(this string @this)
        {
            return string.IsNullOrWhiteSpace(@this);
        }

        public static bool DoesNotContain(this string @this, string value)
        {
            return !@this.Contains(value);
        }

        public static TEnum ToEnum<TEnum>(this string @this) where TEnum : struct
        {
            return (TEnum)Enum.Parse(typeof(TEnum), @this, true);
        }

        public static TType FromJson<TType>(this string source)
            where TType : class
        {
            return JsonConvert.DeserializeObject<TType>(source);
        }

        public static string ToCamelCase(this string @this)
        {
            if(@this.IsNullOrEmpty() || !char.IsUpper(@this[0]))
            {
                return @this;
            }

            var camelCase = char.ToLower(@this[0], CultureInfo.InvariantCulture).ToString(CultureInfo.InvariantCulture);

            if(@this.Length > 1)
            {
                camelCase = camelCase + @this.Substring(1);
            }

            return camelCase;
        }

        public static string RemoveEmptyLastLine(this string @this)
        {
            return Regex.Replace(@this, @"^\r?\n?$", "", RegexOptions.Multiline);
        }
    }
}