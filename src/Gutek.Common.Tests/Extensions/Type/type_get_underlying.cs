using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.type
{
    public class type_get_underlying : type_tests_base
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
        public void underlying_type_of_nullable_should_be_equal_to_type()
        {
            var type = get_nullable_enum_type();
            var sourceType = typeof(FakeEnum);

            Assert.Equal(sourceType, type.GetUnderlyingTypeOrThis());
        }

        [Fact]
        public void underlying_type_of_not_nullable_should_be_equal_to_type()
        {
            var type = get_not_nullable_enum_type();
            var sourceType = typeof(FakeEnum);

            Assert.Equal(sourceType, type.GetUnderlyingTypeOrThis());
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