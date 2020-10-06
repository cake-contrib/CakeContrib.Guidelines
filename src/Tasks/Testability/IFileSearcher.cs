namespace CakeContrib.Guidelines.Tasks.Testability
{
    /// <summary>
    /// Search for files.
    /// </summary>
    public interface IFileSearcher
    {
        /// <summary>
        /// Searches for the existence of <paramref name="fileName"/> up the folder structure, starting with <paramref name="startFolder"/>.
        /// </summary>
        /// <param name="startFolder">The start folder.</param>
        /// <param name="fileName">Name of the file.</param>
        /// <returns>
        ///   <c>true</c> if <paramref name="fileName"/> exists; otherwise, <c>false</c>.
        /// </returns>
        bool HasFileInFolderStructure(string startFolder, string fileName);
    }
}
