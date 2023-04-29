using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane Utilities API.
    /// </summary>
    internal class XPlaneUtilities : IXPlaneUtilities
    {
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void XPLMDebugString([MarshalAs(UnmanagedType.LPStr)] string lpString);

        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern void XPLMGetPluginInfo(int inPlugin, StringBuilder outName, StringBuilder outFilePath, StringBuilder outSignature, StringBuilder outDescription);

        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        private static extern int XPLMGetMyID();

        /// <summary>
        /// Initializes a new instance of the <see cref="XPlaneUtilities"/> class.
        /// </summary>
        public XPlaneUtilities()
        {
        }

        /// <summary>
        /// Gets the ID of the current running X-Plane plugin.
        /// </summary>
        /// <returns>An integer number representing the current running X-Plane plugin.</returns>
        public int GetPluginId()
        {
            return XPLMGetMyID();
        }

        /// <summary>
        /// Gets the directory of the current running X-Plane plugin.
        /// </summary>
        /// <returns>The absolute path of the current running X-Plane plugin.</returns>
        public string GetPluginDirectory()
        {
            var outFilePath = new StringBuilder(256);

            XPLMGetPluginInfo(GetPluginId(), null, outFilePath, null, null);

            return Directory.GetParent(outFilePath.ToString()).Parent.FullName;
        }

        /// <summary>
        /// Writes the specified message to the X-Plane log file.
        /// </summary>
        /// <param name="message">The message to be logged.</param>
        public void WriteDebugLog(string message)
        {
            XPLMDebugString($"{message}\n");
        }
    }
}
