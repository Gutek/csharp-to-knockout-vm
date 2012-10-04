using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.String
{
    public class string_has_text_tests : string_tests_base
    {
        [Fact]
        public void has_no_text_should_return_true_when_string_is_null()
        {
            Assert.True(STRING_EMPTY.HasNoText());
        }

        [Fact]
        public void has_no_text_should_return_true_when_string_is_empty()
        {
            Assert.True(STRING_NULL.HasNoText());
        }

        [Fact]
        public void has_no_text_should_return_true_when_string_contains_only_spaces()
        {
            Assert.True(STRING_SPACES.HasNoText());
        }

        [Fact]
        public void has_no_text_should_return_true_when_string_contains_whitespaces()
        {
            Assert.True(STRING_WHITESPACES.HasNoText());
        }

        [Fact]
        public void has_no_text_should_return_false_when_string_is_not_empty()
        {
            Assert.False(STRING_VALUE.HasNoText());
        }

        [Fact]
        public void has_text_should_return_false_when_string_is_null()
        {
            Assert.False(STRING_EMPTY.HasText());
        }

        [Fact]
        public void has_text_should_return_false_when_string_is_empty()
        {
            Assert.False(STRING_NULL.HasText());
        }

        [Fact]
        public void has_text_should_return_false_when_string_contains_only_spaces()
        {
            Assert.False(STRING_SPACES.HasText());
        }

        [Fact]
        public void has_text_should_return_false_when_string_contains_whitespaces()
        {
            Assert.False(STRING_WHITESPACES.HasText());
        }

        [Fact]
        public void has_text_should_return_true_when_string_is_not_empty()
        {
            Assert.True(STRING_VALUE.HasText());
        }

    }
}