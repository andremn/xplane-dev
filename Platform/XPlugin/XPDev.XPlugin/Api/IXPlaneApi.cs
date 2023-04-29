namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane APIs.
    /// </summary>
    public interface IXPlaneApi
    {
        /// <summary>
        /// Gets the Data Access API.
        /// </summary>
        IXPlaneDataAccess DataAccess { get; }

        /// <summary>
        /// Gets the Processing API.
        /// </summary>
        IXPlaneProcessing Processing { get; }

        /// <summary>
        /// Gets the Utilities API.
        /// </summary>
        IXPlaneUtilities Utilities { get; }

        /// <summary>
        /// Gets the Planes API.
        /// </summary>
        IXPlanePlanes Planes { get; }
    }
}