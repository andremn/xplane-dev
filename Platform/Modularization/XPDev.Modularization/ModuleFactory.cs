using Microsoft.Extensions.DependencyInjection;
using System;

namespace XPDev.Modularization
{
    /// <summary>
    /// Creates instance of plugin modules.
    /// </summary>
    public class ModuleFactory
    {
        private readonly IServiceCollection _services;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModuleFactory"/> class with the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> used to provide services to the created modules.</param>
        public ModuleFactory(IServiceCollection services)
        {
            _services = services;
        }

        /// <summary>
        /// Creates an instance of <typeparamref name="TModule"/>.
        /// </summary>
        /// <typeparam name="TModule">The type of the module to create the instance.</typeparam>
        /// <returns>An instance of <typeparamref name="TModule"/>.</returns>
        public ModuleEntryBase CreateInstance<TModule>() where TModule : ModuleEntryBase
        {
            var moduleInstance = Activator.CreateInstance<TModule>();

            moduleInstance.OnRegisterModuleServices(_services);

            return moduleInstance;
        }
    }
}
