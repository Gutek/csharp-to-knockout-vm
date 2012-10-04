using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.type
{
    public class type_is_enum : type_tests_base
    {
        private Type get_nullable_enum_type()
        {
            var fake = new UsingFakeEnum();
            var type = fake.GetType();

            return type.GetProperty("NullableEnum").PropertyType;
        }

        private Type get_not_nullable_enum_type()
        {
            var fake = new UsingFakeEnum();
            var type = fake.GetType();

            return type.GetProperty("Enum").PropertyType;
        }

        [Fact]
        public void nullable_enum_is_enum()
        {
            var type = get_nullable_enum_type();

            Assert.True(type.IsEnumOrNullableEnum());
        }

        [Fact]
        public void not_nullable_enum_is_enum()
        {
            var type = get_not_nullable_enum_type();

            Assert.True(type.IsEnumOrNullableEnum());
        }


        public enum FakeEnum
        {
            Value1,
            Value2
        }

        public class UsingFakeEnum
        {
            public FakeEnum? NullableEnum { get; set; }
            public FakeEnum Enum { get; set; }
        }
    }
}