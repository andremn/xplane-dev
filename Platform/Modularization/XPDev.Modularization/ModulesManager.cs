using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XPDev.Foundation.Logging;

namespace XPDev.Modularization
{
    /// <summary>
    /// Manages plugin modules.
    /// </summary>
    public class ModulesManager : IModulesManager
    {
        private readonly List<ModuleEntryBase> _modules;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger _logger;
        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ModulesManager"/> class with the specified <see cref="IServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to be passed to the modules.</param>
        public ModulesManager(IServiceProvider serviceProvider)
        {
            _modules = new List<ModuleEntryBase>();
            _serviceProvider = serviceProvider;
            _logger = LoggerManager.GetLoggerFor<ModulesManager>();
        }

        /// <summary>
        /// Adds modules to this manager.
        /// </summary>
        /// <param name="modules">The list of modules to add.</param>
        public void AddModules(IEnumerable<ModuleEntryBase> modules)
        {
            _modules.AddRange(modules);
        }

        /// <summary>
        /// Adds modules to this manager.
        /// </summary>
        /// <param name="modules">The list of modules to add.</param>
        public void AddModules(params ModuleEntryBase[] modules)
        {
            _modules.AddRange(modules);
        }

        /// <summary>
        /// Adds a module to this manager.
        /// </summary>
        /// <param name="module">The moduel to add.</param>
        public void AddModule(ModuleEntryBase module)
        {
            _modules.Add(module);
        }

        /// <summary>
        /// Calls <see cref="ModuleEntryBase.Run()"/> in all modules in the order they were added.
        /// </summary>
        public void Run()
        {
            if (!_isInitialized)
            {
                return;
            }

            var modules = _modules;

            _logger.Debug($"Updating {modules.Count} modules");

            foreach (var module in modules)
            {
                try
                {
                    _logger.Debug($"Calling Run() on module '{module.Name}'");
                    module.Run();
                }
                catch (Exception ex)
                {
                    _logger.Warning(ex, $"Unable to call Run() on module '{module.Name}'");
                }
            }
        }

        /// <summary>
        /// Starts all the loaded modules.
        /// </summary>
        /// <returns>A <see cref="Task"/> for the asynchronous modules start operation.</returns>
        public Task StartModulesAsync()
        {
            if (_isInitialized || _modules.Count == 0)
            {
                return Task.CompletedTask;
            }

            return Task.Run(() =>
            {
                var modules = _modules;

                foreach (var module in modules)
                {
                    try
                    {
                        module.Start(_serviceProvider);
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning(ex, $"Unable to start module '{module.Name}'");
                    }
                }

                _isInitialized = true;
            });
        }

        /// <summary>
        /// Stops all registered modules asynchronously in the reverse order they were added.
        /// </summary>
        /// <returns>A <see cref="Task"/> for the asynchronous modules de-initialization operation.</returns>
        public Task StopModulesAsync()
        {
            if (!_isInitialized || _modules.Count == 0)
            {
                return Task.CompletedTask;
            }

            return Task.Run(() =>
            {
                var reverseList = new List<IModuleEntry>(_modules);

                reverseList.Reverse();

                foreach (var module in reverseList)
                {
                    try
                    {
                        module.Stop();
                    }
                    catch (Exception ex)
                    {
                        _logger.Warning(ex, $"Unable to stop module '{module.Name}'");
                    }
                }

                _isInitialized = false;
            });
        }
    }
}
