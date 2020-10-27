using System;

using CakeContrib.Guidelines.Tasks.Extensions;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests.Extensions
{
    public class VersionExtensionTests
    {
        [Fact]
        public void GreaterEqual_Is_True_For_Equal_Versions()
        {
            var a = new Version(1, 2, 3);
            var b = new Version(a.Major, a.Minor, a.Build);

            a.GreaterEqual(b).Should().BeTrue();
        }

        [Fact]
        public void GreaterEqual_Is_True_For_Greater_Versions()
        {
            var a = new Version(1, 2, 3);
            var b = new Version(a.Major, a.Minor, a.Build - 1);

            a.GreaterEqual(b).Should().BeTrue();
        }

        [Fact]
        public void GreaterEqual_Is_False_For_Lesser_Versions()
        {
            var a = new Version(1, 2, 3);
            var b = new Version(a.Major, a.Minor, a.Build + 1);

            a.GreaterEqual(b).Should().BeFalse();
        }

        [Fact]
        public void LessEqual_Is_True_For_Equal_Versions()
        {
            var a = new Version(1, 2, 3);
            var b = new Version(a.Major, a.Minor, a.Build);

            a.LessEqual(b).Should().BeTrue();
        }

        [Fact]
        public void LessEqual_Is_True_For_Lesser_Versions()
        {
            var a = new Version(1, 2, 3);
            var b = new Version(a.Major, a.Minor, a.Build + 1);

            a.LessEqual(b).Should().BeTrue();
        }

        [Fact]
        public void LessEqual_Is_False_For_Greater_Versions()
        {
            var a = new Version(1, 2, 3);
            var b = new Version(a.Major, a.Minor, a.Build - 1);

            a.LessEqual(b).Should().BeFalse();
        }
    }
}
