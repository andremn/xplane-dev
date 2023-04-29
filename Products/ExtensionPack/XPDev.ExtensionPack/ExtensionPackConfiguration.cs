using Newtonsoft.Json;
using System.Collections.Generic;

namespace XPDev.ExtensionPack.Configuration
{
    internal class ExtensionPackFlightSoundsConfigurationSection
    {
        [JsonProperty("enabled")]
        public bool IsEnabled { get; set; }

        [JsonProperty("directoryPath")]
        public string SoundsFolderPath { get; set; }
    }

    internal class ExtensionPackAnnouncementsConfigurationSection : ExtensionPackFlightSoundsConfigurationSection
    {
        [JsonProperty("liveries")]
        public IDictionary<string, string> LiveriesPaths { get; set; }
    }

    internal class ExtensionPackConfiguration
    {
        [JsonProperty("flightSounds")]
        public ExtensionPackFlightSoundsConfigurationSection FlightSoundsConfigurationSection { get; set; }

        [JsonProperty("announcements")]
        public ExtensionPackAnnouncementsConfigurationSection AnnouncementsConfigurationSection { get; set; }
    }
}
