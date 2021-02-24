using System.Globalization;

using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks.Extensions
{
    /// <summary>
    /// Convenience methods for logging.
    /// </summary>
    internal static class LogExtensions
    {
        /// <summary>
        /// Writes a warning.
        /// This is a convenience-Method that wraps some always identical parameters.
        /// </summary>
        /// <param name="log">The <see cref="TaskLoggingHelper"/> instance.</param>
        /// <param name="ccgRuleId">The CCG Rule.</param>
        /// <param name="projectFile">The project file. May be null.</param>
        /// <param name="message">The message to show.</param>
        internal static void CcgWarning(this TaskLoggingHelper log, int ccgRuleId, string projectFile, string message)
        {
            var ccgRule = GetRule(ccgRuleId);
            var helpLink = GetHelpLink(ccgRuleId);
            message = $"{message} (see {helpLink})";

            log.LogWarning(
                null,
                ccgRule,
                string.Empty, // not usable anyway. See https://github.com/MicrosoftDocs/visualstudio-docs/issues/5894
                helpLink,
                projectFile ?? string.Empty,
                0,
                0,
                0,
                0,
                message);
        }

        /// <summary>
        /// Writes a warning.
        /// This is a convenience-Method that wraps some always identical parameters.
        /// </summary>
        /// <param name="log">The <see cref="TaskLoggingHelper"/> instance.</param>
        /// <param name="ccgRuleId">The CCG Rule.</param>
        /// <param name="projectFile">The project file. May be null.</param>
        /// <param name="message">The message to show.</param>
        internal static void CcgError(this TaskLoggingHelper log, int ccgRuleId, string projectFile, string message)
        {
            var ccgRule = GetRule(ccgRuleId);
            var helpLink = GetHelpLink(ccgRuleId);
            message = $"{message} (see {helpLink})";

            log.LogError(
                null,
                ccgRule,
                string.Empty, // not usable anyway. See https://github.com/MicrosoftDocs/visualstudio-docs/issues/5894
                helpLink,
                projectFile ?? string.Empty,
                0,
                0,
                0,
                0,
                message);
        }

        private static string GetRule(int ruleId)
        {
            return "CCG" + ruleId.ToString("D4", CultureInfo.InvariantCulture);
        }

        private static string GetHelpLink(int ruleId)
        {
            var ccgRule = GetRule(ruleId);

#pragma warning disable CA1308
            return $"https://cake-contrib.github.io/CakeContrib.Guidelines/rules/{ccgRule.ToLowerInvariant()}";
#pragma warning restore CA1308
        }
    }
}
