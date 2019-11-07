using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Timeta;

namespace Timeta.Tests
{
    public class TimeValidationRuleTests
    {
        [Theory]
        [InlineData("0", true)]
        [InlineData("10.0", false)]
        [InlineData("-1", false)]
        [InlineData("test", false)]
        [InlineData(null, false)]
        public void TestValidation(object value, bool expected)
        {
            var rule = new TimeValidationRule();
            var result = rule.Validate(value, null);
            Assert.Equal(result.IsValid, expected);
        }
    }
}
