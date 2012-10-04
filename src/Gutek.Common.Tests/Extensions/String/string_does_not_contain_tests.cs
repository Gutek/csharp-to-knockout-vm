using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.String
{
    public class string_does_not_contain_tests : string_tests_base
    {
        public void does_not_contain_should_return_true_if_string_does_not_contain_word_value()
        {
            Assert.True("SOME_STRING".DoesNotContain(STRING_VALUE));
        }

        public void does_not_contain_should_return_false_if_string_does_contain_word_value()
        {
            Assert.True(STRING_VALUE.DoesNotContain(STRING_VALUE));
        }

        public void does_not_contain_should_trhow_exception_if_source_string_is_null()
        {
            Assert.Throws<ArgumentNullException>(() => STRING_NULL.DoesNotContain(STRING_VALUE));
        }
    }
}