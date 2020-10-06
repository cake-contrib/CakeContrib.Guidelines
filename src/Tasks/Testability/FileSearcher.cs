using System.IO;

namespace CakeContrib.Guidelines.Tasks.Testability
{
    /// <inheritdoc />
    internal class FileSearcher : IFileSearcher
    {
        /// <inheritdoc />
        public bool HasFileInFolderStructure(string startFolder, string fileName)
        {
            var folder = startFolder;
            var found = false;
            while (folder != null && !found)
            {
                found = File.Exists(Path.Combine(folder, fileName));
                if (found)
                {
                    return true;
                }

                folder = Path.GetDirectoryName(folder);
            }

            return false;
        }
    }
}
