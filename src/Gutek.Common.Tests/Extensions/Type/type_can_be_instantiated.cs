using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.type
{
    public class type_can_be_instantiated
    {
        [Fact]
        public void can_be_instantiated_should_return_false_for_interface()
        {
            var type = typeof(INo);

            Assert.False(type.CanBeInstantiated());
        }

        [Fact]
        public void can_be_instantiated_should_return_false_for_abstract_class()
        {
            var type = typeof(NoStruct);

            Assert.False(type.CanBeInstantiated());
        }

        [Fact]
        public void can_be_instantiated_should_return_false_for_struct()
        {
            var type = typeof(NoAbstract);

            Assert.False(type.CanBeInstantiated());
        }

        [Fact]
        public void can_be_instantiated_should_return_false_for_enum()
        {
            var type = typeof(NoEnum);

            Assert.False(type.CanBeInstantiated());
        }

        [Fact]
        public void can_be_instantiated_should_return_true_for_class_delivered_from_abstract_class()
        {
            var type = typeof(YesFromAbstract);

            Assert.True(type.CanBeInstantiated());
        }

        [Fact]
        public void can_be_instantiated_should_return_true_for_class()
        {
            var type = typeof(Yes);

            Assert.True(type.CanBeInstantiated());
        }

        [Fact]
        public void can_be_instantiated_should_return_true_for_class_implementing_interface()
        {
            var type = typeof(YesFromInterface);

            Assert.True(type.CanBeInstantiated());
        }

        [Fact]
        public void can_be_instantiated_should_return_true_for_class_that_have_private_constructor()
        {
            var type = typeof(DontKnowPrivate);

            Assert.True(type.CanBeInstantiated());
        }

        public interface INo { }
        public struct NoStruct { }
        public enum NoEnum { }
        public abstract class NoAbstract { }

        public class Yes { }
        public class YesFromAbstract : NoAbstract { }
        public class YesFromInterface : INo { }

        public class DontKnowPrivate
        {
            private DontKnowPrivate()
            {
            }
        }
    }
}