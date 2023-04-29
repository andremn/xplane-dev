using System.Collections.Generic;
using System.Threading.Tasks;
using XPDev.Foundation;

namespace XPDev.Modularization
{
    /// <summary>
    /// Loads plugin modules.
    /// </summary>
    public interface IModulesManager : IRunnable
    {
        /// <summary>
        /// Starts all the loaded modules.
        /// </summary>
        /// <returns>A <see cref="Task"/> for the asynchronous modules start operation.</returns>
        Task StartModulesAsync();

        /// <summary>
        /// Unloads (stops) all registered modules asynchronously in the reverse order they were added.
        /// </summary>
        /// <returns>A <see cref="Task"/> for the asynchronous modules de-initialization operation.</returns>
        Task StopModulesAsync();

        /// <summary>
        /// Adds modules to this manager.
        /// </summary>
        /// <param name="modules">The list of modules to add.</param>
        void AddModules(IEnumerable<ModuleEntryBase> modules);

        /// <summary>
        /// Adds modules to this manager.
        /// </summary>
        /// <param name="modules">The list of modules to add.</param>
        void AddModules(params ModuleEntryBase[] modules);

        /// <summary>
        /// Adds a module to this manager.
        /// </summary>
        /// <param name="module">The moduel to add.</param>
        void AddModule(ModuleEntryBase module);
    }
}