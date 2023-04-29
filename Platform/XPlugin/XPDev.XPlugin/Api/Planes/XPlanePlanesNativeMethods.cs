using System.Runtime.InteropServices;
using System.Text;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the native methods of the X-Plane Planes API.
    /// </summary>
    internal static class XPlanePlanesNativeMethods
    {
        /// <summary>
        /// Gets 
        /// </summary>
        [DllImport("*", CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi)]
        internal static extern void XPLMGetNthAircraftModel(int inIndex, StringBuilder outFileName, StringBuilder outPath);
    }
}
