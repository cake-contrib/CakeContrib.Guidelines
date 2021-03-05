namespace CakeContrib.Guidelines.Tasks.Tests.Fixtures
{
    public class EnsureCakeContribIconUrlFixture : BaseBuildFixture<EnsureCakeContribIconUrl>
    {
        private readonly string cakeContribDefaultIcon;

        public EnsureCakeContribIconUrlFixture()
        {
            cakeContribDefaultIcon = "http://somewhere.is.the/icon.png";
        }

        public string PackageIconUrlOutput { get; private set; }

        public override bool Execute()
        {
            Task.CakeContribIconUrl = cakeContribDefaultIcon;
            var result = base.Execute();

            PackageIconUrlOutput = Task.PackageIconUrlOutput;
            return result;
        }

        public void WithPackageIconUrl(string packageIconUrl)
        {
            Task.PackageIconUrl = packageIconUrl;
        }

        public void WithOmitIconImport()
        {
            Task.OmitIconImport = true;
        }

        public void WithProjectFile(string fileName)
        {
            Task.ProjectFile = fileName;
        }
    }
}
