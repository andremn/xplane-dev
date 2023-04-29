using System;
using System.IO;
using System.Runtime.InteropServices;
using XPDev.Foundation.Logging;
using XPDev.XPlugin.Api;
using XPDev.XPlugin.Interop;

namespace XPDev.XPlugin
{
    /// <summary>
    /// Entry points for native interaction.
    /// </summary>
    internal unsafe abstract class NativeEntryPoints
    {
        private const string DefaultLogFileName = "log.txt";

        private static PluginConfiguration _configuration;
        private static ILogger _logger;
        private static IXPlaneApi _api;
        private static IXPlanePlugin _plugin;

        /// <summary>
        /// Called when the plugin was started.
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = nameof(XPluginStart))]
        public static bool XPluginStart(IntPtr outName, IntPtr outSig, IntPtr outDesc)
        {
            try
            {
                _api = new XPlaneApi();
                _api.Utilities.WriteDebugLog($"[XPlugin] Starting XPlugin");

                LoadConfiguration();
                ConfigureLogger();

                _logger.Info("Starting X-Plane plugin");

                _plugin = PluginLoader.LoadPlugin(_api);

                if (_plugin != null)
                {
                    InteropHelper.CopyString(outName, _plugin.Name);
                    InteropHelper.CopyString(outSig, _plugin.Signature);
                    InteropHelper.CopyString(outDesc, _plugin.Description);

                    _logger.Debug($"Plugin found and loaded: {_plugin.Name}, {_plugin.Signature}, {_plugin.Description}");

                    return true;
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, $"Exception was thrown while trying to load plugin");
            }

            _logger.Info("Plugin could not be loaded");

            return false;

        }

        /// <summary>
        /// Called when the plugin was enabled.
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = nameof(XPluginEnable))]
        public static bool XPluginEnable()
        {
            _logger.Debug("X-Plane plugin enabled. Notifying user plugin...");

            try
            {
                return _plugin.OnEnabled();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Error trying to enable plugin");
                return false;
            }
        }

        /// <summary>
        /// Called when the plugin was disabled.
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = nameof(XPluginDisable))]
        public static void XPluginDisable()
        {
            _logger.Debug("X-Plane plugin disabled. Notifying user plugin...");

            try
            {
                _plugin.OnDisabled();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Error trying to disable plugin");
            }
        }

        /// <summary>
        /// Called when the plugin was stopped.
        /// </summary>
        [UnmanagedCallersOnly(EntryPoint = nameof(XPluginStop))]
        public static void XPluginStop()
        {
            _logger.Debug("X-Plane plugin stopped");

            try
            {
                _plugin.Dispose();
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Error trying to stop plugin");
            }
            finally
            {
                LoggerManager.Shutdown();
            }
        }

        /// <summary>
        /// Called when the plugin received a message.
        /// </summary>
        /// <param name="inFrom">The id of whom sent the message.</param>
        /// <param name="inMessage">The id of the received message.</param>
        /// <param name="inParam">The parameter of the received message.</param>
        [UnmanagedCallersOnly(EntryPoint = nameof(XPluginReceiveMessage))]
        public static void XPluginReceiveMessage(int inFrom, int inMessage, IntPtr inParam)
        {
            _logger.Debug($"X-Plane plugin received message '{inMessage}' from '{inFrom}'");

            try
            {
                if (Enum.IsDefined(typeof(XPlaneMessage), inMessage))
                {
                    var xplaneMessage = (XPlaneMessage)inMessage;

                    switch (xplaneMessage)
                    {
                        case XPlaneMessage.PlaneLoaded:
                        case XPlaneMessage.PlaneUnloaded:
                        case XPlaneMessage.PlaneLiveryLoaded:
                            inParam = GetAircraftNumberPtrFromIntPtr(inParam);
                            break;
                    }

                    _plugin.OnMessageReceivedFromXPlane((XPlaneMessage)inMessage, inParam);
                }
                else
                {
                    _plugin.OnMessageReceivedFromPlugin(inFrom, inMessage, inParam);
                }
            }
            catch (Exception ex)
            {
                _logger.Fatal(ex, "Error trying to send received message to plugin");
            }
        }

        private static IntPtr GetAircraftNumberPtrFromIntPtr(IntPtr intPtr)
        {
            // The parameter is actually an integer and not a pointer to one, so we need to convert it to int (like we we would read the address),
            // then convert it back to a valid pointer so the caller can read it like an address seamlessly
            var value = intPtr.ToInt32();

            return new IntPtr(&value);
        }

        private static void LoadConfiguration()
        {
            try
            {
                var xPlaneDirectory = AppContext.BaseDirectory;
                var pluginDirectory = _api.Utilities.GetPluginDirectory();
                var pluginConfigPath = Path.Combine(pluginDirectory, "pluginConfig.json");

                _api.Utilities.WriteDebugLog($"[XPlugin] Looking for configuration file in {pluginConfigPath}");

                PluginConfigurationParser.AddConfigurationToken("$(XPlaneFolder)", xPlaneDirectory);
                PluginConfigurationParser.AddConfigurationToken("$(PluginFolder)", pluginDirectory);

                _configuration = PluginConfigurationParser.Deserialize<PluginConfiguration>(pluginConfigPath);
                _api.Utilities.WriteDebugLog("[XPlugin] Configuration loaded");
            }
            catch (Exception ex)
            {
                _api.Utilities.WriteDebugLog($"[XPlugin] Error trying to load configuration: {ex.Message}");
            }
        }

        private static void ConfigureLogger()
        {
            if (_configuration?.LogPluginConfigurationSection == null)
            {
                _api.Utilities.WriteDebugLog($"[XPlugin] Cannot initialize logger, plugin configuration is not initialized");
                _logger = LoggerManager.GetLoggerFor<NativeEntryPoints>();
                return;
            }

            try
            {
                var logConfigurationSection = _configuration.LogPluginConfigurationSection;
                var loggerConfig = new LogConfigBuilder(logConfigurationSection.File)
                    .WithLogLevel(Enum.Parse<LogLevel>(logConfigurationSection.Level))
                    .WithCleanFile()
                    .Build();

                _api.Utilities.WriteDebugLog($"[XPlugin] Initializing logger. Log file will be created at {loggerConfig.FilePath}");
                LoggerManager.Initialize(loggerConfig);
            }
            catch (Exception ex)
            {
                _api.Utilities.WriteDebugLog($"[XPlugin] Error trying to initialize logger: {ex.Message}");
            }

            _logger = LoggerManager.GetLoggerFor<NativeEntryPoints>();
        }
    }
}
