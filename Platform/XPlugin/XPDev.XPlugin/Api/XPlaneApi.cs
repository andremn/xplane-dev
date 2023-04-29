namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane APIs.
    /// </summary>
    internal class XPlaneApi : IXPlaneApi
    {
        /// <summary>
        /// Gets the Data Acccess API.
        /// </summary>
        public IXPlaneDataAccess DataAccess { get; } = new XPlaneDataAccess();

        /// <summary>
        /// Gets the Processing API.
        /// </summary>
        public IXPlaneProcessing Processing { get; } = new XPLMProcessing();

        /// <summary>
        /// Gets the Utilities API.
        /// </summary>
        public IXPlaneUtilities Utilities { get; } = new XPlaneUtilities();

        /// <summary>
        /// Gets the Planes API.
        /// </summary>
        public IXPlanePlanes Planes { get; } = new XPlanePlanes();
    }
}
