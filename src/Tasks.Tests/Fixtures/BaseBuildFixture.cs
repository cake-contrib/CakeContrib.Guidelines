using System;
using System.Collections.Generic;

using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

using Moq;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class BaseBuildFixture<T>
        where T : Task, new()
    {
        public MockBuildEngine BuildEngine { get; }

        protected T Task { get; }

        protected Mock<ITaskItem> GetMockTaskItem(string identity, Dictionary<string, string> metadata = null)
        {
            var item = new Mock<ITaskItem>();
            item.Setup(x => x.ToString()).Returns(identity);
            item.Setup(x => x.GetMetadata("Identity")).Returns(identity);
            if (metadata != null)
            {
                // ReSharper disable once UseDeconstruction
                foreach (var kvp in metadata)
                {
                    var key = kvp.Key;
                    var val = kvp.Value;
                    item.Setup(x =>
                        x.GetMetadata(It.Is<string>(arg =>
                            arg.Equals(key, StringComparison.OrdinalIgnoreCase))))
                        .Returns(val);
                }
            }
            return item;
        }

        public BaseBuildFixture()
        {
            BuildEngine = new MockBuildEngine();
            Task = new T
            {
                BuildEngine = BuildEngine
            };
        }

        public virtual bool Execute()
        {
            return Task.Execute();
        }
    }
}
