using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.String
{
    public class string_to_enum_tests : string_tests_base
    {
        [Fact]
        public void to_enum_when_provided_string_value_exists_in_selected_enum_should_return_enum_with_selected_value()
        {
            var enum_value = "Text";
            var result = enum_value.ToEnum<FakeEnum>();

            Assert.Equal(FakeEnum.Text, result);
        }

        [Fact]
        public void to_enum_when_provided_string_does_not_exists_in_selected_enum_type_should_trhow_exception()
        {
            var enum_value = "SOME_VALUE_THAT_DOES_NOT_EXISTS";

            Assert.Throws<ArgumentException>(() => enum_value.ToEnum<FakeEnum>());
        }

        public enum FakeEnum
        {
            Value,
            Text,
            Other
        }

    }

}