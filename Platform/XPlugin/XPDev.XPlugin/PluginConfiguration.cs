using Newtonsoft.Json;

namespace XPDev.XPlugin
{
    /// <summary>
    /// Represents the Log section of the plugin configuration.
    /// </summary>
    internal class LogPluginConfigurationSection
    {
        /// <summary>
        /// Gets or sets the level of the log.
        /// </summary>
        public string Level { get; set; }

        /// <summary>
        /// Gets or sets the path of the log file.
        /// </summary>
        public string File { get; set; }
    }

    /// <summary>
    /// Represents the Entry section of the plugin configuration.
    /// </summary>
    internal class EntryPluginConfigurationSection
    {
        /// <summary>
        /// Gets or sets the name of the assembly where the plugin entry is declared.
        /// </summary>
        public string Assembly { get; set; }

        /// <summary>
        /// Gets or sets the name of the type that represents the plugin.
        /// </summary>
        public string Type { get; set; }
    }

    /// <summary>
    /// Represents the configuration of a plugin.
    /// </summary>
    internal class PluginConfiguration
    {
        /// <summary>
        /// Gets or sets the Log section of the plugin configuration.
        /// </summary>
        [JsonProperty("log")]
        public LogPluginConfigurationSection LogPluginConfigurationSection { get; set; }

        /// <summary>
        /// Gets or sets the Entry section of the plugin configuration.
        /// </summary>
        [JsonProperty("entry")]
        public EntryPluginConfigurationSection EntryPluginConfigurationSection { get; set; }
    }
}
