using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.Bool
{
    public class bool_is_false_tests : bool_tests_base
    {
        [Fact]
        public void isFalse_should_return_false_for_null_bool()
        {
            var result = NullBool.IsFalse();

            Assert.False(result);
        }

        [Fact]
        public void isFalse_should_return_true_for_false_bool()
        {
            var result = False.IsFalse();

            Assert.True(result);
        }

        [Fact]
        public void isFalse_should_return_false_for_true_bool()
        {
            var result = True.IsFalse();

            Assert.False(result);
        }
    }
}