namespace XPDev.Foundation
{
    /// <summary>
    /// Provides an object that runs an operation when requested.
    /// </summary>
    public interface IRunnable
    {
        /// <summary>
        /// Runs the operations of this <see cref="IRunnable"/>.
        /// </summary>
        void Run();
    }
}
