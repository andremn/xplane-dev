using System;
using XPDev.Foundation;

namespace XPDev.Modularization
{
    /// <summary>
    /// Represents the entry point of a module.
    /// </summary>
    public interface IModuleEntry : IRunnable, IDisposable
    {
        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets whether the module is started.
        /// </summary>
        bool IsStarted { get; }

        /// <summary>
        /// Starts the module with the specified <see cref="IServiceProvider"/>.
        /// </summary>
        void Start(IServiceProvider serviceProvider);

        /// <summary>
        /// Stops the module.
        /// </summary>
        void Stop();
    }
}
