using Microsoft.Build.Utilities;

namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class BaseBuildFixture<T>
        where T : Task, new()
    {
        public MockBuildEngine BuildEngine { get; }

        protected T Task { get; }

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
