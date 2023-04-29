using XPDev.FlightSoundsSimulator.Model;
using XPDev.FlightManagement;
using XPDev.Modularization;

namespace XPDev.FlightSoundsSimulator
{
    public interface IFlightParametersService : IFlightSnapshotProvider
    {
        void UpdateFlightParameters(FlightParameters flightParameters);
    }
}
