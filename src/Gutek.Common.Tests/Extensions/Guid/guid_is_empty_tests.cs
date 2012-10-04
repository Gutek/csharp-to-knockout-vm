using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.Guid
{
    public class guid_is_empty_tests : guid_tests_base
    {
        [Fact]
        public void is_empty_should_return_false_if_guid_is_provided()
        {
            System.Guid g = System.Guid.NewGuid();

            Assert.False(g.IsEmpty());
        }

        [Fact]
        public void is_empty_should_return_true_if_guid_is_set_to_empty()
        {
            System.Guid g = System.Guid.Empty;

            Assert.True(g.IsEmpty());
        }

        [Fact]
        public void is_not_empty_should_return_true_if_guid_is_provided()
        {
            System.Guid g = System.Guid.NewGuid();

            Assert.True(g.IsNotEmpty());
        }

        [Fact]
        public void is_not_empty_should_return_false_if_guid_is_set_to_empty()
        {
            System.Guid g = System.Guid.Empty;

            Assert.False(g.IsNotEmpty());
        }
    }
}