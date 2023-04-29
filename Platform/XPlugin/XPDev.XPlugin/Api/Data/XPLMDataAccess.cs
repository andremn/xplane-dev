using System;
using System.Collections.Concurrent;

namespace XPDev.XPlugin.Api
{
    /// <summary>
    /// Provides access to the X-Plane Data (DataRefs) API.
    /// </summary>
    internal class XPlaneDataAccess : IXPlaneDataAccess
    {
        private readonly ConcurrentDictionary<string, DataRefHandler> _dataRefCache;

        /// <summary>
        /// Initializes a new instance of the <see cref="XPlaneDataAccess"/> class.
        /// </summary>
        public XPlaneDataAccess()
        {
            _dataRefCache = new ConcurrentDictionary<string, DataRefHandler>();
        }

        /// <summary>
        /// Finds a data ref by the specified name and returns a handle to it if found.
        /// </summary>
        /// <param name="name">The name of the data ref to be looked up.</param>
        /// <returns>An instance of <see cref="DataRefHandler"/> if the data ref with the specified name was found; null otherwise.</returns>
        public DataRefHandler FindDataRef(string name)
        {
            return _dataRefCache.GetOrAdd(name, CreateDataRefHandler);
        }

        private DataRefHandler CreateDataRefHandler(string name)
        {
            var datarefPtr = XPLMDataAccessNativeMethods.XPLMFindDataRef(name);

            if (datarefPtr == IntPtr.Zero)
            {
                return null;
            }

            return new DataRefHandler(name, datarefPtr);
        }
    }
}
