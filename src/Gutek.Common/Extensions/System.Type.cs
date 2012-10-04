using System.Linq;

namespace System
{
    public static class TypeExtensions
    {
        public static bool IsAssignableTo(this Type @this, Type other)
        {
            return other.IsAssignableFrom(@this);
        }

        public static bool IsAssignableTo<T>(this Type @this)
        {
            return @this.IsAssignableTo(typeof(T));
        }

        public static bool CanBeInstantiated(this Type @this)
        {
            return @this.IsClass && !@this.IsAbstract;
        }

        public static bool Is<TOther>(this Type @this)
        {
            return @this == (typeof(TOther));
        }

        public static bool IsNot<TOther>(this Type @this)
        {
            return !@this.Is<TOther>();
        }

        public static bool IsAssignableToGenericType(this Type @this, Type genericType)
        {
            if(@this == null || genericType == null)
            {
                return false;
            }

            return @this == genericType
              || @this.MapsToGenericTypeDefinition(genericType)
              || @this.HasInterfaceThatMapsToGenericTypeDefinition(genericType)
              || @this.BaseType.IsAssignableToGenericType(genericType);
        }

        public static Type GetUnderlyingTypeOrThis(this Type @this)
        {
            var nullableEnum = Nullable.GetUnderlyingType(@this);
            if(nullableEnum != null)
            {
                return nullableEnum;
            }

            return @this;
        }

        public static bool IsEnumOrNullableEnum(this Type @this)
        {
            return @this.GetUnderlyingTypeOrThis().IsEnum;
        }

        public static bool IsNullable(this Type @this)
        {
            return Nullable.GetUnderlyingType(@this) != null;
        }

        private static bool HasInterfaceThatMapsToGenericTypeDefinition(this Type @this, Type genericType)
        {
            return @this
              .GetInterfaces()
              .Where(it => it.IsGenericType)
              .Any(it => it.GetGenericTypeDefinition() == genericType);
        }

        private static bool MapsToGenericTypeDefinition(this Type @this, Type genericType)
        {
            return genericType.IsGenericTypeDefinition
              && @this.IsGenericType
              && @this.GetGenericTypeDefinition() == genericType;
        }
    }
}