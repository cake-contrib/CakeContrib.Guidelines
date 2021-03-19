using System;
using System.Collections.Generic;

using CakeContrib.Guidelines.Tasks.Extensions;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests.Extensions
{
    public class VersionExtensionTests
    {
        [Theory]
        [MemberData(nameof(GetGreaterEqualData))]
        public void GreaterEqualTheory(Version lhs, Version rhs, bool expected)
        {
            lhs.GreaterEqual(rhs).Should().Be(expected);
        }

        public static IEnumerable<object[]> GetGreaterEqualData()
        {
            yield return new object[] { new Version(1, 2, 3), new Version(1, 2, 3), true };
            yield return new object[] { new Version(1, 2, 3), new Version(1, 2, 2), true };
            yield return new object[] { new Version(1, 2, 2), new Version(1, 2, 3), false };
        }

        [Theory]
        [MemberData(nameof(GetLessEqualData))]
        public void LessEqualTheory(Version lhs, Version rhs, bool expected)
        {
            lhs.LessEqual(rhs).Should().Be(expected);
        }

        public static IEnumerable<object[]> GetLessEqualData()
        {
            yield return new object[] { new Version(1, 2, 3), new Version(1, 2, 3), true };
            yield return new object[] { new Version(1, 2, 3), new Version(1, 2, 2), false };
            yield return new object[] { new Version(1, 2, 2), new Version(1, 2, 3), true };
        }

        [Theory]
        [MemberData(nameof(GetGreaterThanData))]
        public void GreaterThanTheory(Version lhs, Version rhs, bool expected)
        {
            lhs.GreaterThan(rhs).Should().Be(expected);
        }

        public static IEnumerable<object[]> GetGreaterThanData()
        {
            yield return new object[] { new Version(1, 2, 3), new Version(1, 2, 3), false };
            yield return new object[] { new Version(1, 2, 3), new Version(1, 2, 2), true };
            yield return new object[] { new Version(1, 2, 2), new Version(1, 2, 3), false };
        }

        [Theory]
        [MemberData(nameof(GetLessThanData))]
        public void LessThanTheory(Version lhs, Version rhs, bool expected)
        {
            lhs.LessThan(rhs).Should().Be(expected);
        }

        public static IEnumerable<object[]> GetLessThanData()
        {
            yield return new object[] { new Version(1, 2, 3), new Version(1, 2, 3), false };
            yield return new object[] { new Version(1, 2, 3), new Version(1, 2, 2), false };
            yield return new object[] { new Version(1, 2, 2), new Version(1, 2, 3), true };
        }
    }
}
