namespace System
{
    public static class BoolExtensions
    {
        public static bool IsTrue(this bool? @this)
        {
            return @this.HasValue && @this.Value;
        }

        public static bool IsFalse(this bool? @this)
        {
            return @this.HasValue && !@this.Value;
        }

        public static bool OrDefault(this bool? @this, bool defaultValue)
        {
            return @this.GetValueOrDefault(defaultValue);
        }
    }
}