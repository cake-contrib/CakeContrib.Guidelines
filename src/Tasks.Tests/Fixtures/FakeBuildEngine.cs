using System;
using System.Collections;
using System.Collections.Generic;

using Microsoft.Build.Framework;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class MockBuildEngine : IBuildEngine
    {
        internal List<BuildErrorEventArgs> ErrorEvents { get; } =
            new List<BuildErrorEventArgs>();

        internal List<BuildMessageEventArgs> MessageEvents { get; } =
            new List<BuildMessageEventArgs>();

        internal List<CustomBuildEventArgs> CustomEvents { get; } =
            new List<CustomBuildEventArgs>();

        internal List<BuildWarningEventArgs> WarningEvents { get; } =
            new List<BuildWarningEventArgs>();

        public bool BuildProjectFile(
            string projectFileName, string[] targetNames,
            IDictionary globalProperties,
            IDictionary targetOutputs)
        {
            throw new NotImplementedException();
        }

        public bool ContinueOnError
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int ColumnNumberOfTaskNode { get; set; } = 0;

        public int LineNumberOfTaskNode { get; set; } = 0;

        public void LogCustomEvent(CustomBuildEventArgs e)
        {
            CustomEvents.Add(e);
        }

        public void LogErrorEvent(BuildErrorEventArgs e)
        {
            ErrorEvents.Add(e);
        }

        public void LogMessageEvent(BuildMessageEventArgs e)
        {
            MessageEvents.Add(e);
        }

        public void LogWarningEvent(BuildWarningEventArgs e)
        {
            WarningEvents.Add(e);
        }

        public string ProjectFileOfTaskNode { get; set; } = "Unittests.task";

    }
}
