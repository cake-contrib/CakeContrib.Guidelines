using System;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks
{
    /// <summary>
    /// This Task calculates the ProjectType.
    /// </summary>
    /// <example><![CDATA[
    /// <Target Name="Test">
    ///   <PropertyGroup>
    ///     <ProjectType>unknown</ProjectType>
    ///   </PropertyGroup>
    ///   <ItemGroup>
    ///     <ProjectNames Include="$(AssemblyName)" />
    ///     <ProjectNames Include="$(PackageId)" />
    ///     <CakeRequiredReference Include="cake.core" />
    ///     <CakeRequiredReference Include="cake.common" />
    ///   </ItemGroup>
    ///   <CalculateProjectType
    ///      ProjectType="$(ProjectType)"
    ///      ProjectNames="@(ProjectNames)"
    ///      References="@(PackageReference)">
    ///     <Output ItemName="ProjectType" TaskParameter="Output"/>
    ///   </CalculateProjectType>
    ///
    ///   <Message Text="Project type: $(ProjectType)" />
    ///   </Target>
    /// ]]></example>
    public class CalculateProjectType : Task
    {
#if DEBUG
        private const MessageImportance LogLevel = MessageImportance.High;
#else
        private const MessageImportance LogLevel = MessageImportance.Low;
#endif

        /// <summary>
        /// Gets or sets the current ProjectType.
        /// If not empty, the task will return this value.
        /// </summary>
        public string ProjectType { get; set; }

        /// <summary>
        /// Gets or sets the Project names.
        /// E.g. add AssemblyName here or PackageId.
        /// These will be used to calculate the type.
        /// </summary>
        [Required]
        public ITaskItem[] ProjectNames { get; set; }

        /// <summary>
        /// Gets or sets the References.
        /// </summary>
        [Required]
        public ITaskItem[] References { get; set; }

        /// <summary>
        /// Gets or sets the CakeRequiredReference.
        /// </summary>
        [Required]
        public ITaskItem[] CakeRequiredReference { get; set; }

        /// <summary>
        /// Gets the output of this Task.
        /// </summary>
        [Output]
        public string Output { get; private set; }

        /// <inheritdoc cref="Task.Execute" />
        public override bool Execute()
        {
            if (!string.IsNullOrEmpty(ProjectType))
            {
                Log.LogMessage(
                    LogLevel,
                    $"Found existing ProjectType:{ProjectType}. Setting output to '{ProjectType}'.");
                Output = ProjectType;
                return true;
            }

            var requiredCakeReferences = CakeRequiredReference
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.ToLowerInvariant())
                .ToList();

            var references = References
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrEmpty(x))
                .Select(x => x.ToLowerInvariant())
                .ToList();

            if (!references.Any(x => requiredCakeReferences.Contains(x)))
            {
                Output = CakeProjectType.Other.ToString();
                Log.LogMessage(
                    LogLevel,
                    $"No reference to Cake found. Setting output to '{Output}'.");
                return true;
            }

            var names = ProjectNames
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var thisCouldHaveBeenAConfiguration = new[]
            {
                new Tuple<Predicate<string>, string>(
                    x =>
                        x.StartsWith("cake.", StringComparison.InvariantCultureIgnoreCase) &&
                        x.EndsWith(".module", StringComparison.InvariantCultureIgnoreCase),
                    CakeProjectType.Module.ToString()),
                new Tuple<Predicate<string>, string>(
                    x =>
                        x.StartsWith("cake.", StringComparison.InvariantCultureIgnoreCase) &&
                        x.EndsWith(".recipe", StringComparison.InvariantCultureIgnoreCase),
                    CakeProjectType.Recipe.ToString()),
                new Tuple<Predicate<string>, string>(
                    x =>
                        x.StartsWith("cake.", StringComparison.InvariantCultureIgnoreCase),
                    CakeProjectType.Addin.ToString()),
            };

            foreach (var tuple in thisCouldHaveBeenAConfiguration)
            {
                var match = names
                    .FirstOrDefault(x => tuple.Item1(x));
                if (match == null)
                {
                    continue;
                }

                Output = tuple.Item2;
                Log.LogMessage(
                    LogLevel,
                    $"The name '{match}' suggest a {Output} project. Setting output to '{Output}'.");
                return true;
            }

            Output = CakeProjectType.Other.ToString();
            Log.LogMessage(
                LogLevel,
                $"Setting output to the default of '{Output}'.");
            return true;
        }
    }
}
