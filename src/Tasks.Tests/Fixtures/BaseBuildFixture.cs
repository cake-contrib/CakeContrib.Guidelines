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

        protected Mock<ITaskItem> GetMockTaskItem(string identity)
        {
            var item = new Mock<ITaskItem>();
            item.Setup(x => x.ToString()).Returns(identity);
            item.Setup(x => x.GetMetadata("Identity")).Returns(identity);
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
