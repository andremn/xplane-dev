using XPDev.FlightSoundsSimulator.Model;
using System;
using XPDev.FlightManagement;
using XPDev.Foundation;
using System.Numerics;

namespace XPDev.FlightSoundsSimulator
{
    public class FlightParametersService : Disposable, IFlightParametersService
    {
        private static readonly CameraLocation _defaultCockpitCameraLocation = new CameraLocation(new Vector3(-2, 1, 5), false);
        private static readonly CameraLocation _defaultCabinPosition = new CameraLocation(new Vector3(-2, 1, 30), false);

        private FlightParameters _lastFlightParameters;

        public FlightParametersService()
        {
        }

        public DateTime LastUpdate { get; private set; }

        public FlightSnapshot TakeSnapshot()
        {
            LastUpdate = DateTime.Now;

            var flightParameters = _lastFlightParameters;

            if (flightParameters == null)
            {
                return new FlightSnapshot();
            }

            var cameraLocation = flightParameters.IsInsideCockpit ? _defaultCockpitCameraLocation : _defaultCabinPosition;

            cameraLocation.IsExternal = flightParameters.IsCameraExternal;

            return new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = flightParameters.AltitudeAboveGroundLevel,
                AircraftIndicatedAltitute = flightParameters.AltitudeAboveGroundLevel,
                AircraftCruiseAltitude = 39000,
                AircraftGroundSpeed = flightParameters.GroundSpeed,
                AircraftFrontLeftDoorState = flightParameters.FrontLeftDoorState,
                AircraftVerticalSpeed = flightParameters.IndicatedVerticalSpeed,
                AircraftBeaconLightSwitchPosition = flightParameters.BeaconLightsSwtich,
                AircraftLandingLightsSwitchPosition = flightParameters.LandingLightsSwitch,
                AircraftNoseLightSwitchPosition = flightParameters.NoseLightSwitch,
                AircraftSeatBeltsSignSwitchPosition = flightParameters.SeatBeltsSwitch,
                DayPeriod = flightParameters.DayPeriod,
                EnginesRunningStatus = ConvertToEngineRunningStatus(flightParameters.EnginesState),
                CockpitDoorState = DoorState.Closed,
                CameraLocation = cameraLocation,
                IsPaused = flightParameters.IsPaused
            };
        }

        public void UpdateFlightParameters(FlightParameters flightParameters)
        {
            _lastFlightParameters = flightParameters;
        }

        private static bool[] ConvertToEngineRunningStatus(EngineState engineState)
        {
            if (engineState == EngineState.Running)
            {
                return new [] { true };
            }

            return new[] { false };
        }
    }
}
