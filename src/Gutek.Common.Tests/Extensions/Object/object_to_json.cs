using System;
using Xunit;

namespace Gutek.Common.Tests.Extensions.Object
{
    public class object_to_json : object_tests_base
    {
        protected readonly Fake _fake_obj = new Fake { Name = "TEST" };
        protected readonly string _fake_json = "{\"Name\":\"TEST\"}";

        [Fact]
        public void to_json_should_return_null_string_for_null_object()
        {
            object obj = null;

            var result = ObjectExtensions.ToJson(obj);

            Assert.Equal("null", result);
        }

        [Fact]
        public void to_json_should_serialize_object()
        {
            var result = _fake_obj.ToJson();

            Assert.Equal(_fake_json, result);
        }

        public class Fake
        {
            public string Name { get; set; }
        }
    }
}