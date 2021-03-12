using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace CakeContrib.Guidelines.Tasks.Testability
{
    /// <inheritdoc cref="IFileFacade"/>
    [ExcludeFromCodeCoverage]
    public class FileFacade : IFileFacade
    {
        /// <inheritdoc cref="IFileFacade.AreFilesSame"/>
        public bool AreFilesSame(string pathLhs, string pathRhs)
        {
            var lhs = new FileInfo(pathLhs);
            var rhs = new FileInfo(pathRhs);
            return lhs.Exists &&
                   rhs.Exists &&
                   File.ReadAllBytes(lhs.FullName).SequenceEqual(File.ReadAllBytes(rhs.FullName));
        }

        /// <inheritdoc cref="IFileFacade.Copy"/>
        public void Copy(string source, string destination)
        {
            File.Copy(source, destination, true);
        }
    }
}
