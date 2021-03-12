using System;
using System.IO;
using System.Linq;

namespace CakeContrib.Guidelines.Tasks.Extensions
{
    /// <summary>
    /// INTERNAL USE.
    /// </summary>
    internal static class StringExtensions
    {
        /// <summary>
        /// Normalizes DirectorySeparators to <see cref="Path.DirectorySeparatorChar"/>, so
        /// they are comparable and Path.GetFileName etc works nicely.
        /// </summary>
        /// <param name="path">The path to work on.</param>
        /// <returns>Resulting path.</returns>
        internal static string NormalizePathSeparators(this string path)
        {
            var directorySeparator = Path.DirectorySeparatorChar.ToString();
            var variants = new[]
            {
                Path.AltDirectorySeparatorChar.ToString(),
                "\\",
                "/",
            }
                .Distinct()
                .Where(x => !x.Equals(directorySeparator, StringComparison.Ordinal))
                .ToArray();

            var result = path;
            foreach (var sep in variants)
            {
                if (result.IndexOf(sep, StringComparison.Ordinal) > -1)
                {
                    result = result.Replace(sep, directorySeparator);
                }
            }

            return result;
        }
    }
}
