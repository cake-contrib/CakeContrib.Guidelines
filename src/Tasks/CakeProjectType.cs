using System;
using System.Linq;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// This is the outcome of the <see cref="CalculateProjectType"/> Task.
    /// </summary>
    public sealed class CakeProjectType
    {
        private readonly string id;

        private CakeProjectType(string id)
        {
            this.id = id;
        }

        /// <summary>
        /// Type: Addin.
        /// </summary>
        public static CakeProjectType Addin { get; } = new CakeProjectType("addin");

        /// <summary>
        /// Type: Module.
        /// </summary>
        public static CakeProjectType Module { get; } = new CakeProjectType("module");

        /// <summary>
        /// Type: Recipe.
        /// </summary>
        public static CakeProjectType Recipe { get; } = new CakeProjectType("recipe");

        /// <summary>
        /// Type: Other
        /// </summary>
        public static CakeProjectType Other { get; } = new CakeProjectType("other");

        /// <summary>
        /// Checks whether a calculated type is of this type.
        /// </summary>
        /// <param name="calculatedType">The msbuild calculated Type.</param>
        /// <returns><c>true</c>, if the calculated type is equal to this type.</returns>
        public bool Is(string calculatedType)
        {
            return id.Equals(calculatedType, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Checks whether a calculated type is one of the given types.
        /// </summary>
        /// <param name="calculatedType">the msbuild calculated type.</param>
        /// <param name="types">Types to check.</param>
        /// <returns><c>true</c>, if the calculated type <see cref="Is"/> one of the given types.</returns>
        public static bool IsOneOf(string calculatedType, params CakeProjectType[] types)
        {
            return types.Any(x => x.Is(calculatedType));
        }

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return id;
        }
    }
}
