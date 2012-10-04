using System;
using System.Collections.Generic;
using Xunit;

namespace Gutek.Common.Tests.Extensions.String
{
    public class string_from_json : string_tests_base
    {
        private const string FAKE_JSON = @"{
    'Property': 10,
    'Tab': [10,10]
}";

        private const string REAL_JSON = @"{
    'Age': 10,
    'Children': [10,10]
}";

        [Fact]
        public void from_json_should_return_object_with_filled_properties__real_json()
        {
            var json = REAL_JSON.FromJson<RealJson>();

            Assert.NotNull(json);
            Assert.Equal(10, json.Age);
            Assert.NotEmpty(json.Children);
            Assert.Equal(2, json.Children.Count);
        }

        [Fact]
        public void from_json_should_return_object_without_filled_properties__fakse_json()
        {
            var json = REAL_JSON.FromJson<FakeJson>();

            Assert.NotNull(json);
            Assert.Null(json.Name);
        }

        [Fact]
        public void from_json_should_throw_exception_when_trying_to_convert_null_string()
        {
            Assert.Throws<ArgumentNullException>(() => STRING_NULL.FromJson<FakeJson>());
        }

        public class FakeJson
        {
            public string Name { get; set; }
        }

        public class RealJson
        {
            public int Age { get; set; }
            public IList<int> Children { get; set; }
        }
    }
}