using XPDev.ExtensionPack.Configuration;

namespace XPDev.ExtensionPack
{
    internal class PluginContext
    {
        public ExtensionPackConfiguration PluginConfiguration { get; set; }

        public string PluginFolder { get; set; }

        public string CurrentAircraftFolder { get; set; }

        public string CurrentAircraftLiveryFolder { get; set; }
    }
}
