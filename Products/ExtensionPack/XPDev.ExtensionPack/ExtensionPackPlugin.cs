using Microsoft.Extensions.DependencyInjection;
using Nito.AsyncEx;
using System;
using System.IO;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using XPDev.ExtensionPack.Configuration;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement.Configuration;
using XPDev.Foundation;
using XPDev.Foundation.Logging;
using XPDev.Modularization;
using XPDev.XPlugin;
using XPDev.XPlugin.Api;

using FlightManagementModuleEntry = XPDev.FlightManagement.ModuleEntry;
using FlightSoundsManagementModuleEntry = XPDev.FlightSoundsManagement.ModuleEntry;

namespace XPDev.ExtensionPack
{
    /// <summary>
    /// Represents the operations of a plugin.
    /// </summary>
    public class ExtensionPackPlugin : Disposable, IXPlanePlugin
    {
        private const int InitialFightLoopIntervalSeconds = 60;
        private const int PausedFightLoopIntervalMilliseconds = 1000;
        private const string PluginConfigFileName = "ExtensionPackConfig.json";

        private readonly IXPlaneApi _api;
        private readonly ILogger _logger;

        private PluginDataRefs _dataRefs;
        private ExtensionPackConfiguration _pluginConfiguration;
        private PluginService _pluginService;
        private IModulesManager _modulesManager;
        private bool _isInitialized;
        private bool _lastPausedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExtensionPackPlugin"/> class with the specified <see cref="IXPlaneApi"/>.
        /// </summary>
        /// <param name="xPlaneApi">The <see cref="IXPlaneApi"/> to be used.</param>
        public ExtensionPackPlugin(IXPlaneApi xPlaneApi)
        {
            _api = xPlaneApi;
            _logger = LoggerManager.GetLoggerFor<ExtensionPackPlugin>();
        }

        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        public string Name => "Extension Pack for Airliners";

        /// <summary>
        /// Gets the signature of the plugin.
        /// </summary>
        public string Signature => "xpdev.plugin.extpack";

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        public string Description => "Extension Pack for airliners to provide utilities like passenger sounds, announcements, auto step climb, and more.";

        /// <summary>
        /// Called when the plugin is enabled.
        /// </summary>
        public bool OnEnabled()
        {
            _logger.Info($"Plugin enabled. ID is {_api.Utilities.GetPluginId()}");

            return true;
        }

        /// <summary>
        /// Called when the plugin is disabled.
        /// </summary>
        public void OnDisabled()
        {
            _api.Processing.UnregisterFlightLoopCallback(OnFlightLoop);
            _logger.Debug("Plugin disabled");
        }

        /// <summary>
        /// Called when a message is sent to the plugin from X-Plane.
        /// </summary>
        /// <param name="message">The id of the message.</param>
        /// <param name="parameter">The parameter of the message</param>
        public void OnMessageReceivedFromXPlane(XPlaneMessage message, IntPtr parameter)
        {
            if (!_isInitialized && message == XPlaneMessage.PlaneLoaded && IsUserAircraf(parameter))
            {
                _logger.Info($"Plane loaded, checking if {PluginConfigFileName} is present in the current aircraft folder");

                _pluginConfiguration = GetPluginConfiguration();

                if (_pluginConfiguration != null)
                {
                    _logger.Info($"{PluginConfigFileName} was found in the current aircraft folder, initializing");
                    _api.Processing.RegisterFlightLoopCallback(OnFlightLoop, TimeSpan.FromSeconds(InitialFightLoopIntervalSeconds));
                }
                else
                {
                    _logger.Info($"{PluginConfigFileName} was not found in current aircraft folder, plugin will not initialize");
                }
            }
        }

        /// <summary>
        /// Called when a message is sent to the plugin from another plugin.
        /// </summary>
        /// <param name="from">The id of the sender of the message.</param>
        /// <param name="message">The id of the message.</param>
        /// <param name="parameter">The parameter of the message</param>
        public void OnMessageReceivedFromPlugin(int from, int message, IntPtr parameter)
        {
            from.NotUsed();
            message.NotUsed();
            parameter.NotUsed();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _isInitialized = false;
            }

            base.Dispose(disposing);
        }

        private async Task InitializeAsync()
        {
            await LoadModulesAsync();
            _isInitialized = true;
            _logger.Info("Plugin initialized");
        }

        private async Task LoadModulesAsync()
        {
            var services = new ServiceCollection();
            var moduleFactory = new ModuleFactory(services);

            _logger.Info("Registering services...");

            _dataRefs = new PluginDataRefs(_api.DataAccess);

            services.AddSingleton(_logger);
            services.AddSingleton<PluginService>();
            services.AddSingleton<IModulesManager, ModulesManager>();
            services.AddSingleton<IFlightSnapshotProvider>(_ => new FlightSnapshotProvider(_dataRefs));
            services.AddSingleton<IFlightSoundConfigurationProvider>(_ => new FlightSoundConfigurationProvider(AppContext.BaseDirectory, 
                GetPlaneCockpitDoorPosition(), TimeSpan.FromSeconds(5)));

            _logger.Info("Loading modules...");

            var flightManagementModule = moduleFactory.CreateInstance<FlightManagementModuleEntry>();
            var flightSoundsModule = moduleFactory.CreateInstance<FlightSoundsManagementModuleEntry>();
            var serviceProvider = services.BuildServiceProvider();

            _pluginService = serviceProvider.GetService<PluginService>();
            _modulesManager = serviceProvider.GetService<IModulesManager>();

            _modulesManager.AddModules(flightManagementModule, flightSoundsModule);

            await _modulesManager.StartModulesAsync();
            InitializePluginService();

            _logger.Info("Modules loaded");
        }

        private void InitializePluginService()
        {
            var pluginContext = new PluginContext
            {
                CurrentAircraftFolder = _api.Planes.GetCurrentAircraftFolderPath(),
                CurrentAircraftLiveryFolder = _dataRefs.PlaneLiveryDataRef?.Value,
                PluginConfiguration = _pluginConfiguration,
                PluginFolder = _api.Utilities.GetPluginDirectory()
            };

            // Todo: improve design
            _pluginService.Initialize(pluginContext);
        }

        private Vector3 GetPlaneCockpitDoorPosition()
        {
            var planeCockpitDoorPosition = _dataRefs.PlaneCockpitDoorPositionDataRef?.Value;

            if (planeCockpitDoorPosition?.Length < 3)
            {
                planeCockpitDoorPosition = new[] { 0f, 0f, 0f };
            }

            return new Vector3(planeCockpitDoorPosition[0], planeCockpitDoorPosition[1], planeCockpitDoorPosition[2]);
        }

        private ExtensionPackConfiguration GetPluginConfiguration()
        {
            var aircraftFolder = _api.Planes.GetCurrentAircraftFolderPath();
            var pluginConfigFilePath = Path.Combine(aircraftFolder, PluginConfigFileName);

            if (!File.Exists(pluginConfigFilePath))
            {
                return null;
            }

            PluginConfigurationParser.AddConfigurationToken("$(AircraftFolder)", aircraftFolder);

            return PluginConfigurationParser.Deserialize<ExtensionPackConfiguration>(pluginConfigFilePath);
        }

        private bool IsUserAircraf(IntPtr messageParameter)
        {
            return messageParameter != IntPtr.Zero && _api.Planes.IsUserAircraft(Marshal.ReadInt32(messageParameter));
        }

        private TimeSpan? OnFlightLoop(TimeSpan elapsedTimeSinceLastCall, TimeSpan elapsedTimeSinceLastFlightLoop, int flightLoopNumber)
        {
            try
            {
                if (!_isInitialized)
                {
                    AsyncContext.Run(InitializeAsync);
                }

                if (_dataRefs?.PausedDataRef?.Value ?? false)
                {
                    // If we are paused now, but wasn't paused before, it means the sim was just paused
                    if (!_lastPausedValue)
                    {
                        // Set the last paused to true so we don't get here until the sim is resumed and paused again
                        _lastPausedValue = true;
                        // Update the modules just one more time after the sim was paused so the modules are aware of it
                        _modulesManager.Run();
                    }

                    return TimeSpan.FromMilliseconds(PausedFightLoopIntervalMilliseconds);
                }

                _lastPausedValue = false;
                _logger.Debug($"Flight loop called (time since last flight loop: {elapsedTimeSinceLastFlightLoop}; time since last call: {elapsedTimeSinceLastCall})");
                _modulesManager.Run();
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, "Exception on flight loop");
            }

            return null;
        }
    }
}
