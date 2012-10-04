using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.type
{
    public class type_is_assignable_to : type_tests_base
    {
        [Fact]
        public void is_assignable_to_should_return_false__ifake_to_ifake2()
        {
            Assert.False(IFAKE_TYPE.IsAssignableTo<IFake2>());
        }

        [Fact]
        public void is_assignable_to_should_return_false__ifake_to_iinheritesifake()
        {
            Assert.False(IFAKE_TYPE.IsAssignableTo<IInheritesIFake>());
        }

        [Fact]
        public void is_assignable_to_should_return_true__iinheritesifake_ifake()
        {
            Assert.True(IIHERITES_IFAKE_TYPE.IsAssignableTo<IFake>());
        }

        [Fact]
        public void is_assignable_to_should_not_throw_exception_if_testing_on_null_type_and_should_return_false()
        {
            bool result = true;

            Assert.DoesNotThrow(() => result = NULL_TYPE.IsAssignableTo<IFake>());

            Assert.False(result);
        }

        [Fact]
        public void should_be_assignable_from_open_generic_type_to_concrete_open_generic_type()
        {
            Assert.True(typeof(Foo<>).IsAssignableToGenericType(typeof(IFoo<>)));
        }

        [Fact]
        public void should_be_assignable_from_open_generic_type_to_generic_interface_type()
        {
            Assert.True(typeof(IFoo<int>).IsAssignableToGenericType(typeof(IFoo<>)));
        }

        [Fact]
        public void should_be_assignable_from_open_generic_type_to_itself()
        {
            Assert.True(typeof(IFoo<>).IsAssignableToGenericType(typeof(IFoo<>)));
        }

        [Fact]
        public void should_be_assignable_from_open_generic_type_to_concrete_generic_type()
        {
            Assert.True(typeof(Foo<int>).IsAssignableToGenericType(typeof(IFoo<>)));
        }

        [Fact]
        public void should_be_assignable_from_open_generic_type_to_nongeneric_concrete_type()
        {
            Assert.True(typeof(Bar).IsAssignableToGenericType(typeof(IFoo<>)));
        }

        [Fact]
        public void assignable_to_generic_type_should_return_false_for_null_type()
        {
            var result = TypeExtensions.IsAssignableToGenericType(null, typeof(IFoo<>));

            Assert.False(result);
        }

        public interface IFoo<T> { }
        public class Foo<T> : IFoo<T> { }
        public class Bar : IFoo<int> { }
    }
}