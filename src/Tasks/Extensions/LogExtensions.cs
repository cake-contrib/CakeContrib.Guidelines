using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks.Extensions
{
    /// <summary>
    /// Convenience methods for logging.
    /// </summary>
    internal static class LogExtensions
    {
        /// <summary>
        /// Writes a trace message
        /// This is a convenience-Method that wraps some always identical logLevel.
        /// </summary>
        /// <param name="log">The <see cref="TaskLoggingHelper"/> instance.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="importance"><see cref="MessageImportance"/>. Default is <see cref="MessageImportance.Low"/>.</param>
        [SuppressMessage("ReSharper", "RedundantAssignment", Justification = "only on DEBUG")]
        internal static void CcgTrace(
            this TaskLoggingHelper log,
            string message,
            MessageImportance importance = MessageImportance.Low)
        {
#if DEBUG
            importance = MessageImportance.High;
#endif
            log.LogMessage(importance, message);
        }

        /// <summary>
        /// Writes a suggestion. (Shows up as "message" in Visual Studio)
        /// This is a convenience-Method that wraps some always identical parameters.
        /// </summary>
        /// <param name="log">The <see cref="TaskLoggingHelper"/> instance.</param>
        /// <param name="ccgRuleId">The CCG Rule.</param>
        /// <param name="projectFile">The project file. May be null.</param>
        /// <param name="message">The message to show.</param>
        internal static void CcgSuggestion(this TaskLoggingHelper log, int ccgRuleId, string projectFile, string message)
        {
            var ccgRule = GetRule(ccgRuleId);
            var helpLink = GetHelpLink(ccgRuleId);
            message = $"{message} (see {helpLink})";

            log.LogCriticalMessage(
                null,
                ccgRule,
                string.Empty, // not usable anyway. See https://github.com/MicrosoftDocs/visualstudio-docs/issues/5894
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
        /// <param name="noWarn">list of rules for which warnings are suppressed.</param>
        /// <param name="warningsAsErrors">list of rules that should be raised as errors.</param>
        internal static void CcgWarning(
            this TaskLoggingHelper log,
            int ccgRuleId,
            string projectFile,
            string message,
            IEnumerable<string> noWarn,
            IEnumerable<string> warningsAsErrors)
        {
            var ccgRule = GetRule(ccgRuleId);
            if (noWarn != null && noWarn.Contains(ccgRule, StringComparer.OrdinalIgnoreCase))
            {
                log.CcgTrace($"{ccgRule} is set to noWarn.");
                return;
            }

            if (warningsAsErrors != null && warningsAsErrors.Contains(ccgRule, StringComparer.OrdinalIgnoreCase))
            {
                log.CcgTrace($"{ccgRule} is set to warningAsError.");
                log.CcgError(ccgRuleId, projectFile, message);
                return;
            }

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
