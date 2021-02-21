using System;

namespace CakeContrib.Guidelines.Tasks.Extensions
{
    /// <summary>
    /// internal extensions to <see cref="Version"/>.
    /// </summary>
    internal static class VersionExtensions
    {
        /// <summary>
        /// compares two versions.
        /// </summary>
        /// <param name="baseVersion">the version that is extended.</param>
        /// <param name="compareTo">the version to compare to.</param>
        /// <returns>
        /// <c>true</c>, if <paramref name="compareTo"/> is greater or equal to <paramref name="baseVersion"/>.
        /// <c>false</c>, otherwise.
        /// </returns>
        public static bool GreaterEqual(this Version baseVersion, Version compareTo)
        {
            if (baseVersion == null)
            {
                throw new ArgumentNullException(nameof(baseVersion));
            }

            if (compareTo == null)
            {
                throw new ArgumentNullException(nameof(compareTo));
            }

            return baseVersion.CompareTo(compareTo) > -1;
        }

        /// <summary>
        /// compares two versions.
        /// </summary>
        /// <param name="baseVersion">the version that is extended.</param>
        /// <param name="compareTo">the version to compare to.</param>
        /// <returns>
        /// <c>true</c>, if <paramref name="compareTo"/> is greater than <paramref name="baseVersion"/>.
        /// <c>false</c>, otherwise.
        /// </returns>
        public static bool GreaterThan(this Version baseVersion, Version compareTo)
        {
            if (baseVersion == null)
            {
                throw new ArgumentNullException(nameof(baseVersion));
            }

            if (compareTo == null)
            {
                throw new ArgumentNullException(nameof(compareTo));
            }

            return baseVersion.CompareTo(compareTo) > 0;
        }

        /// <summary>
        /// compares two versions.
        /// </summary>
        /// <param name="baseVersion">the version that is extended.</param>
        /// <param name="compareTo">the version to compare to.</param>
        /// <returns>
        /// <c>true</c>, if <paramref name="compareTo"/> is less or equal to <paramref name="baseVersion"/>.
        /// <c>false</c>, otherwise.
        /// </returns>
        public static bool LessEqual(this Version baseVersion, Version compareTo)
        {
            if (baseVersion == null)
            {
                throw new ArgumentNullException(nameof(baseVersion));
            }

            if (compareTo == null)
            {
                throw new ArgumentNullException(nameof(compareTo));
            }

            return baseVersion.CompareTo(compareTo) < 1;
        }

        /// <summary>
        /// compares two versions.
        /// </summary>
        /// <param name="baseVersion">the version that is extended.</param>
        /// <param name="compareTo">the version to compare to.</param>
        /// <returns>
        /// <c>true</c>, if <paramref name="compareTo"/> is less than <paramref name="baseVersion"/>.
        /// <c>false</c>, otherwise.
        /// </returns>
        public static bool LessThan(this Version baseVersion, Version compareTo)
        {
            if (baseVersion == null)
            {
                throw new ArgumentNullException(nameof(baseVersion));
            }

            if (compareTo == null)
            {
                throw new ArgumentNullException(nameof(compareTo));
            }

            return baseVersion.CompareTo(compareTo) < 0;
        }
    }
}
