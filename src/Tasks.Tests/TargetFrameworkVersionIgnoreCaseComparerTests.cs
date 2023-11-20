using System.Linq;

using CakeContrib.Guidelines.Tasks.Tests.Fixtures;

using Shouldly;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests
{
    public class TargetFrameworkVersionIgnoreCaseComparerTests
    {
        private TargetFrameworkVersions.IgnoreCaseComparer sut = new TargetFrameworkVersions.IgnoreCaseComparer();

        [Theory]
        [InlineData(null, "x", false)]
        [InlineData("x", null, false)]
        [InlineData("someValue", "someOtherValue", false)]
        [InlineData("someValue", "SoMeVaLuE", true)]
        [InlineData("someValue", "someValue", true)]
        [InlineData(null, null, true)]
        public void NormalUsageTheory(string lhs, string rhs, bool expected)
        {
            // given

            // when
            var actual = sut.Equals(lhs, rhs);

            // then
            actual.ShouldBe(expected);
        }
    }
}
