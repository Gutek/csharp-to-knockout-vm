using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.Bool
{
    public class bool_is_true_tests : bool_tests_base
    {
        [Fact]
        public void isTrue_should_return_false_for_null_bool()
        {
            var result = NullBool.IsTrue();

            Assert.False(result);
        }

        [Fact]
        public void isTrue_should_return_false_for_false_bool()
        {
            var result = False.IsTrue();

            Assert.False(result);
        }

        [Fact]
        public void isTrue_should_return_true_for_true_bool()
        {
            var result = True.IsTrue();

            Assert.True(result);
        }
    }
}