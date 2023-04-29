using System;
using System.Threading.Tasks;

namespace XPDev.Modularization
{
    /// <summary>
    /// Represents a service of a module.
    /// </summary>
    public interface IModuleService : IDisposable
    {
        /// <summary>
        /// Updates the service asynchronously.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        Task UpdateAsync();
    }
}
