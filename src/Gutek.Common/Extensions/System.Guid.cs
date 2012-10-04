namespace System
{
    public static class GuidExtensions
    {
        public static bool IsEmpty(this Guid @this)
        {
            return @this == Guid.Empty;
        }

        public static bool IsNotEmpty(this Guid @this)
        {
            return !@this.IsEmpty();
        }
    }
}