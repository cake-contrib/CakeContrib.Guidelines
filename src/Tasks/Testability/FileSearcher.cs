using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace CakeContrib.Guidelines.Tasks.Testability
{
    /// <inheritdoc />
    [ExcludeFromCodeCoverage]
    internal class FileSearcher : IFileSearcher
    {
        /// <inheritdoc />
        public bool HasFileInFolderStructure(string startFolder, string fileName)
        {
            var folder = startFolder;
            do
            {
                var found = File.Exists(Path.Combine(folder, fileName));
                if (found)
                {
                    return true;
                }

                folder = Path.GetDirectoryName(folder);
            }
            while (folder != null);

            return false;
        }
    }
}
