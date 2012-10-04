using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.String
{
    public class string_format_with_tests : string_tests_base
    {
        [Fact]
        public void format_with_should_return_string_provided_if_parameters_placeholder_were_not_provided_even_when_extra_arguments_that_should_be_injected_are_provided()
        {
            var result = STRING_WITHOUT_PLACEHOLDERS.FormatWith(STRING_VALUE, STRING_VALUE);

            Assert.Equal(STRING_WITHOUT_PLACEHOLDERS, result);
        }

        [Fact]
        public void format_with_should_return_string_provided_if_parameters_placeholder_were_not_provided_and_no_extra_arguments_were_provided_too()
        {
            var result = STRING_WITHOUT_PLACEHOLDERS.FormatWith();

            Assert.Equal(STRING_WITHOUT_PLACEHOLDERS, result);
        }

        [Fact]
        public void format_with_should_throw_exception_if_parameters_placeholder_were_not_provided_and_null_has_been_provided_as_argument()
        {
            Assert.Throws<ArgumentNullException>(() => STRING_WITHOUT_PLACEHOLDERS.FormatWith(null));
        }

        [Fact]
        public void format_with_should_throw_exception_if_string_provided_is_null_even_when_extra_arguments_that_should_be_injected_are_provided()
        {
            Assert.Throws<ArgumentNullException>(() => STRING_NULL.FormatWith(STRING_VALUE));
        }

        [Fact]
        public void format_with_should_throw_exception_if_string_provided_is_null_and_extra_arguments_that_should_be_injected_are_not_provided()
        {
            Assert.Throws<ArgumentNullException>(() => STRING_NULL.FormatWith());
        }

        [Fact]
        public void format_with_should_return_combined_string_when_string_provided_contains_placeholders_and_arguments_to_be_injected_are_provided()
        {
            var result = STRING_WITH_PLACEHOLDERS.FormatWith(STRING_VALUE, STRING_VALUE);

            Assert.Equal(STRING_WITH_PLACEHOLDERS_FILLED, result);
        }

        [Fact]
        public void format_with_should_throw_exception_when_arguments_are_not_provided()
        {
            Assert.Throws<FormatException>(() => STRING_WITH_PLACEHOLDERS.FormatWith());
        }
    }
}