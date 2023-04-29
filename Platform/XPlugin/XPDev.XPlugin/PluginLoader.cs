using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using XPDev.Foundation.Logging;
using XPDev.XPlugin.Api;

namespace XPDev.XPlugin
{
    /// <summary>
    /// Provides methods to load a plugin.
    /// </summary>
    internal static class PluginLoader
    {
        private static readonly ILogger _logger = LoggerManager.GetLoggerFor(typeof(PluginLoader));

        /// <summary>
        /// Loads an instance of <see cref="IXPlanePlugin"/>.
        /// </summary>
        /// <param name="xPlaneApi">The <see cref="IXPlaneApi"/> to be passed to the plugin instance.</param>
        /// <returns>An instance of <see cref="IXPlanePlugin"/> if it was found and loaded correctly or null otherwise.</returns>
        public static IXPlanePlugin LoadPlugin(IXPlaneApi xPlaneApi)
        {
            var currentDirectory = xPlaneApi.Utilities.GetPluginDirectory();
            var pluginConfigPath = Path.Combine(currentDirectory, "pluginConfig.json");

            _logger.Debug($"Looking for config file in current path: {currentDirectory}");

            var configFile = File.ReadAllText(pluginConfigPath);
            var pluginConfig = JsonConvert.DeserializeObject<PluginConfiguration>(configFile);
            var entryPluginConfigurationSection = pluginConfig?.EntryPluginConfigurationSection;
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assemblyNames = assemblies.ToDictionary(k => k.GetName(), v => v);

            _logger.Debug($"Loaded assemblies: {string.Join(Environment.NewLine, assemblies.Select(a => a.FullName))}");

            var pluginAssembly = assemblyNames.SingleOrDefault(x => x.Key.Name == entryPluginConfigurationSection.Assembly).Value;

            if (pluginAssembly == null)
            {
                _logger.Debug($"Could not find assembly '{entryPluginConfigurationSection.Assembly}'");
                return null;
            }

            var pluginEntry = pluginAssembly.GetTypes().SingleOrDefault(t => t.FullName == entryPluginConfigurationSection.Type);

            if (pluginEntry == null)
            {
                _logger.Debug($"Could not find type '{entryPluginConfigurationSection.Type}'");
                return null;
            }

            return Activator.CreateInstance(pluginEntry, new object[] { xPlaneApi }) as IXPlanePlugin;
        }
    }
}
