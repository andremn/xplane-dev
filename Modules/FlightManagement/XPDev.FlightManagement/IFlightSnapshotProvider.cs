using System;

namespace XPDev.FlightManagement
{
    /// <summary>
    /// A provider for flight snapshots.
    /// </summary>
    public interface IFlightSnapshotProvider
    {
        /// <summary>
        /// Gets the timestamp of the last update.
        /// </summary>
        DateTime LastUpdate { get; }

        /// <summary>
        /// Takes a snapshot of the current flight conditions.
        /// </summary>
        /// <returns>An instance of <see cref="FlightSnapshot"/> representing the current flight conditions.</returns>
        FlightSnapshot TakeSnapshot();
    }
}