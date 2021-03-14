// unset

using System.Collections.Generic;
using System.IO;

using CakeContrib.Guidelines.Tasks.Extensions;

using FluentAssertions;

using Xunit;

namespace CakeContrib.Guidelines.Tasks.Tests.Extensions
{
    public class StringExtensionTests
    {
        [Theory]
        [MemberData(nameof(NormalizePathSeparatorsData))]
        public void NormalizePathSeparators(string actual, string expected)
        {
            // This is not a good tests, as the outcome
            // will be different on different OS.
            actual.NormalizePathSeparators().Should().Be(expected);
        }

        public static IEnumerable<object[]> NormalizePathSeparatorsData()
        {
            yield return new object[] { "Foo/Bar\\baz", Path.Combine("Foo", "Bar", "baz")  };
            yield return new object[] { "C:\\Foo\\Bar/baz.txt", Path.Combine($"C:{Path.DirectorySeparatorChar}Foo", "Bar", "baz.txt")  };
        }
    }
}
