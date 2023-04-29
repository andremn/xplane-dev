using System;

namespace XPDev.XPlugin
{
    /// <summary>
    /// Represents an X-Plane plugin.
    /// </summary>
    public interface IXPlanePlugin : IDisposable
    {
        /// <summary>
        /// Gets the name of the plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the signature of the plugin.
        /// </summary>
        string Signature { get; }

        /// <summary>
        /// Gets the description of the plugin.
        /// </summary>
        string Description { get; }

        /// <summary>
        /// Called when the plugin is enabled.
        /// </summary>
        bool OnEnabled();

        /// <summary>
        /// Called when the plugin is disabled.
        /// </summary>
        void OnDisabled();

        /// <summary>
        /// Called when a message is sent to the plugin from X-Plane.
        /// </summary>
        /// <param name="message">The id of the message.</param>
        /// <param name="parameter">The parameter of the message</param>
        void OnMessageReceivedFromXPlane(XPlaneMessage message, IntPtr parameter);

        /// <summary>
        /// Called when a message is sent to the plugin from another plugin.
        /// </summary>
        /// <param name="from">The id of the sender of the message.</param>
        /// <param name="message">The id of the message.</param>
        /// <param name="parameter">The parameter of the message</param>
        void OnMessageReceivedFromPlugin(int from, int message, IntPtr parameter);
    }
}
