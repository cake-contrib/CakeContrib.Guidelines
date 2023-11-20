using System;

namespace CakeContrib.Guidelines.Tasks
{
    internal static class CakeVersions
    {
        // Cake 0.26.0
        internal static readonly Version Vo26 = new Version(0, 26, 0);

        // Cake 1.0.0
        internal static readonly Version V1 = new Version(1, 0, 0);

        // Cake 2.0.0
        internal static readonly Version V2 = new Version(2, 0, 0);

        // Cake 3.0.0
        internal static readonly Version V3 = new Version(3, 0, 0);

        // Cake 4.0.0
        public static readonly Version V4 = new Version(4, 0, 0);

        // The next, currently non-existing cake version
        public static Version VNext = new Version(5, 0, 0);
    }
}
