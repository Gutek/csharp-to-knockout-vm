using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.type
{
    public class type_is_nullable : type_tests_base
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
        public void nullable_type_should_be_nullable()
        {
            var type = get_nullable_enum_type();

            Assert.True(type.IsNullable());
        }

        [Fact]
        public void not_nullable_type_should_not_be_nullable()
        {
            var type = get_not_nullable_enum_type();

            Assert.False(type.IsNullable());
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