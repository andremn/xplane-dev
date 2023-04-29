using Microsoft.Extensions.DependencyInjection;
using System;
using XPDev.Foundation;
using XPDev.Foundation.Logging;

namespace XPDev.Modularization
{
    /// <summary>
    /// Base class for classes implementing <see cref="IModuleEntry"/>.
    /// </summary>
    public abstract class ModuleEntryBase : Disposable, IModuleEntry
    {
        /// <summary>
        /// Initializes a new instance of the implementing class.
        /// </summary>
        protected ModuleEntryBase(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Name cannot be null or empty", nameof(name));
            }

            Name = name;
            Logger = LoggerManager.GetLoggerFor(GetType());
        }

        /// <summary>
        /// Gets the name of the module.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets whether the module is started.
        /// </summary>
        public bool IsStarted { get; protected set; }

        /// <summary>
        /// Gets the <see cref="ILogger"/> for this type.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets or sets the <see cref="IServiceProvider"/>.
        /// </summary>
        private IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="IServiceCollection"/>.
        /// </summary>
        private IServiceCollection ServiceCollection { get; set; }

        /// <summary>
        /// Updates the module.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Starts the module with the specified <see cref="IServiceProvider"/>.
        /// </summary>
        public void Start(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            OnStarted();
            IsStarted = true;
            Logger.Info($"Module '{Name}' started");
        }

        /// <summary>
        /// Stops the module.
        /// </summary>
        public void Stop()
        {
            OnStopped();
            IsStarted = false;
            Logger.Info($"Module '{Name}' stopped");
        }

        /// <summary>
        /// Called when the module is started.
        /// </summary>
        protected abstract void OnStarted();

        /// <summary>
        /// Called when the module is stopped.
        /// </summary>
        protected abstract void OnStopped();

        /// <summary>
        /// Called when the services are ready to be registered.
        /// </summary>
        protected abstract void OnRegisterServices();

        /// <summary>
        /// Called when the module services should be registered.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> to register the module services to.</param>
        internal void OnRegisterModuleServices(IServiceCollection services)
        {
            ServiceCollection = services ?? throw new ArgumentNullException(nameof(services));
            OnRegisterServices();
        }

        /// <summary>
        /// Registers a service with the specified interface and implementation types.
        /// </summary>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        /// <typeparam name="TImplementation">The type of the implementation.</typeparam>
        protected void RegisterService<TInterface, TImplementation>() where TInterface : class where TImplementation : class, TInterface
        {
            ServiceCollection.AddSingleton<TInterface, TImplementation>();
        }

        /// <summary>
        /// Registers a service with the specified implementation factory to create the service instance.
        /// </summary>
        /// <param name="implementationFactory">The factory that creates the service.</param>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        protected void RegisterService<TInterface>(Func<IServiceProvider, TInterface> implementationFactory)
            where TInterface : class
        {
            ServiceCollection.AddSingleton(implementationFactory);
        }

        /// <summary>
        /// Registers a service with the specified interface and implementation instance.
        /// </summary>
        /// <param name="instance">The instance of the service.</param>
        /// <typeparam name="TInterface">The type of the interface.</typeparam>
        protected void RegisterService<TInterface>(TInterface instance) where TInterface : class
        {
            ServiceCollection.AddSingleton(instance);
        }

        /// <summary>
        /// Gets the service with the specified type.
        /// </summary>
        /// <typeparam name="TService">The type of the service to get.</typeparam>
        /// <returns>An instance of <typeparamref name="TService"/> if the service exists; null otherwise.</returns>
        protected TService GetService<TService>() where TService : class
        {
            return ServiceProvider.GetService<TService>();
        }
    }
}
