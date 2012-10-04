using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.type
{
    public class type_is_tests : type_tests_base
    {
        [Fact]
        public void is_should_return_false__ifake_is_ifake2()
        {
            Assert.False(IFAKE_TYPE.Is<IFake2>());
        }

        [Fact]
        public void is_should_return_true__ifake_is_ifake()
        {
            Assert.True(IFAKE_TYPE.Is<IFake>());
        }

        [Fact]
        public void is_should_not_throw_exception_if_testing_on_null_type_and_should_return_false()
        {
            bool result = true;

            Assert.DoesNotThrow(() => result = NULL_TYPE.Is<IFake>());

            Assert.False(result);
        }

        [Fact]
        public void is_not_should_return_true__ifake_is_ifake2()
        {
            Assert.True(IFAKE_TYPE.IsNot<IFake2>());
        }

        [Fact]
        public void is_not_should_return_false__ifake_is_ifake()
        {
            Assert.False(IFAKE_TYPE.IsNot<IFake>());
        }

        [Fact]
        public void is_not_should_not_throw_exception_if_testing_on_null_type_and_should_return_false()
        {
            // explenation: we are giving falls, as for null type we can't determine if this is or this is not a type

            bool result = true;

            Assert.DoesNotThrow(() => result = NULL_TYPE.Is<IFake>());

            Assert.False(result);
        }
    }
}