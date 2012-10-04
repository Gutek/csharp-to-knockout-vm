using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.String
{
    public class string_is_null_or_empty_tests : string_tests_base
    {
        [Fact]
        public void is_null_or_empty_should_return_true_when_string_is_null()
        {
            Assert.True(STRING_EMPTY.IsNullOrEmpty());
        }

        [Fact]
        public void is_null_or_empty_should_return_true_when_string_is_empty()
        {
            Assert.True(STRING_NULL.IsNullOrEmpty());
        }

        [Fact]
        public void is_null_or_empty_should_return_false_when_string_is_not_empty()
        {
            Assert.False(STRING_VALUE.IsNullOrEmpty());
        }

        [Fact]
        public void is_not_null_or_empty_should_return_false_when_string_is_null()
        {
            Assert.False(STRING_EMPTY.IsNotNullOrEmpty());
        }

        [Fact]
        public void is_not_null_or_empty_should_return_false_when_string_is_empty()
        {
            Assert.False(STRING_NULL.IsNotNullOrEmpty());
        }

        [Fact]
        public void is_not_null_or_empty_should_return_true_when_string_is_not_empty()
        {
            Assert.True(STRING_VALUE.IsNotNullOrEmpty());
        }
    }
}