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
    ///     <ProjectType></ProjectType>
    ///   </PropertyGroup>
    ///   <ItemGroup>
    ///     <ProjectNames Include="$(AssemblyName)" />
    ///     <ProjectNames Include="$(PackageId)" />
    ///   </ItemGroup>
    ///   <CalculateProjectType
    ///      ProjectType="$(ProjectType)"
    ///      ProjectNames="@(ProjectNames)">
    ///     <Output ItemName="ProjectType" TaskParameter="Output"/>
    ///   </CalculateProjectType>
    ///
    ///   <Message Text="Project type: $(ProjectType)" />
    ///   </Target>
    /// ]]></example>
    public class CalculateProjectType : Task
    {
        private const string TypeAddin = "Addin";
        private const string TypeModule = "Module";

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

            var names = ProjectNames
                .Select(x => x.ToString())
                .Where(x => !string.IsNullOrEmpty(x))
                .ToList();

            var match = names
                .FirstOrDefault(x => x.EndsWith(".module", StringComparison.InvariantCultureIgnoreCase));
            if (match != null)
            {
                Log.LogMessage(
                    LogLevel,
                    $"The name '{match}' suggest a module project. Setting output to '{TypeModule}'.");
                Output = TypeModule;
                return true;
            }

            Log.LogMessage(
                LogLevel,
                $"Setting output to the default of '{TypeAddin}'.");
            Output = TypeAddin;
            return true;
        }
    }
}
