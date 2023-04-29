using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace XPDev.XPlugin
{
    /// <summary>
    /// Parses JSON plugin configuration files.
    /// </summary>
    public static class PluginConfigurationParser
    {
        private static readonly ConcurrentDictionary<string, string> _plugingConfigurationTokenMap = new ConcurrentDictionary<string, string>();

        /// <summary>
        /// Adds or updates an existing token with the specified value.
        /// </summary>
        /// <param name="token">The token to be replaced with <paramref name="value"/>.</param>
        /// <param name="value">The value to repalace <paramref name="token"/> with.</param>
        public static void AddConfigurationToken(string token, string value)
        {
            _plugingConfigurationTokenMap.AddOrUpdate(token, value, (k, v) => value);
        }

        /// <summary>
        /// Deserializes the specified configuration file to an object of the type specified by <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the configuration object to be deserialized.</typeparam>
        /// <returns></returns>
        public static T Deserialize<T>(string configurationFilePath) where T : class, new()
        {
            if (string.IsNullOrEmpty(configurationFilePath))
            {
                throw new ArgumentException("Argument cannot be null or empty", nameof(configurationFilePath));
            }

            var configFileContents = new StringBuilder(File.ReadAllText(configurationFilePath));

            foreach (var tokenMap in _plugingConfigurationTokenMap.ToArray())
            {
                configFileContents.Replace(tokenMap.Key, tokenMap.Value.Replace(@"\", @"\\"));
            }

            return JsonConvert.DeserializeObject<T>(configFileContents.ToString());
        }
    }
}
