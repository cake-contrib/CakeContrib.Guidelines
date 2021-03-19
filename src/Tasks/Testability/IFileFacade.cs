namespace CakeContrib.Guidelines.Tasks.Testability
{
    /// <summary>
    /// Compares two files.
    /// </summary>
    public interface IFileFacade
    {
        /// <summary>
        /// Compares the two files.
        /// </summary>
        /// <param name="pathLhs">Path to the (left) File.</param>
        /// <param name="pathRhs">Path to the (right) File.</param>
        /// <returns>true, if both files exist and are binary equal.</returns>
        bool AreFilesSame(string pathLhs, string pathRhs);

        /// <summary>
        /// copy source to destination.
        /// </summary>
        /// <param name="source">The source to copy from.</param>
        /// <param name="destination">The destination to copy to.</param>
        void Copy(string source, string destination);
    }
}
