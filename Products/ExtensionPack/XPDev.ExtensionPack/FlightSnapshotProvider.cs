using XPDev.FlightManagement;
using System;
using System.Numerics;
using XPDev.Foundation.Logging;
using System.Reflection;
using System.Linq;
using XPDev.Foundation;

namespace XPDev.ExtensionPack
{
    internal class FlightSnapshotProvider : IFlightSnapshotProvider
    {
        private readonly PluginDataRefs _dataRefs;
        private readonly ILogger _logger;

        public FlightSnapshotProvider(PluginDataRefs knownDataRefs)
        {
            knownDataRefs.NotNull();

            _dataRefs = knownDataRefs;
            _logger = LoggerManager.GetLoggerFor<FlightSnapshotProvider>();
        }

        public DateTime LastUpdate { get; private set; }

        public FlightSnapshot TakeSnapshot()
        {
            LastUpdate = DateTime.UtcNow;

            var snapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = _dataRefs.IndicatedAltitudeAboveGroundFtDataRef?.Value ?? 0f,
                AircraftBeaconLightSwitchPosition = GetBeaconLightSwitchPosition(),
                AircraftCruiseAltitude = _dataRefs.CruiseLevelFtDataRef?.Value ?? 0f,
                AircraftFrontLeftDoorState = GetPaxDoorState(),
                CockpitDoorState = GetCockpitDoorState(),
                AircraftGroundSpeed = _dataRefs.IndicatedGroundSpeedDataRef?.Value ?? 0f,
                FmcFlightPhase = GetFmcFlightPhase(),
                AircraftIndicatedAltitute = _dataRefs.IndicatedAltitudeFtDataRef?.Value ?? 0f,
                AircraftLandingLightsSwitchPosition = GetLandingLightsSwitchPosition(),
                AircraftNoseLightSwitchPosition = GetNoseLightSwitchPosition(),
                AircraftSeatBeltsSignSwitchPosition = GetSeatBeltsSignSwtichPosition(),
                AircraftVerticalSpeed = _dataRefs.IndicatedVerticalSpeedFpmDataRef?.Value ?? 0f,
                CameraLocation = GetCameraLocation(),
                IsPaused = _dataRefs.PausedDataRef?.Value ?? false
            };

            if (_logger.IsEnabled(LogLevel.Debug))
            {
                DebugLogSnapshot(snapshot);
            }

            return snapshot;
        }

        private FmcFlightPhase GetFmcFlightPhase()
        {
            var value = _dataRefs.FmcFlightPhaseDataRef?.Value ?? 0;

            return (FmcFlightPhase)value;
        }

        private BeaconLightSwitchPosition GetBeaconLightSwitchPosition()
        {
            var value = _dataRefs.BeaconLightSwitchPositionDataRef?.Value ?? 0f;

            return value == 0f ? BeaconLightSwitchPosition.Off : BeaconLightSwitchPosition.On;
        }

        private DoorState GetPaxDoorState()
        {
            var value = _dataRefs.PaxDoorsStatesDataRef?.Value;

            if (value == null)
            {
                return DoorState.Closed;
            }

            var allDoorsClosed = value.All(v => v == 0f);

            return allDoorsClosed ? DoorState.Closed : DoorState.Open;
        }

        private DoorState GetCockpitDoorState()
        {
            var value = _dataRefs.CockpitDoorStateDataRef?.Value;

            if (value == null || value == 0f)
            {
                return DoorState.Closed;
            }

            return DoorState.Open;
        }

        private LandingLightsSwitchPosition GetLandingLightsSwitchPosition()
        {
            var value = _dataRefs.LandingLightsSwitchPositionDataRef?.Value ?? 0f;

            if (value == 0f)
            {
                return LandingLightsSwitchPosition.Off;
            }

            return value == 1f ? LandingLightsSwitchPosition.Retracted : LandingLightsSwitchPosition.On;
        }

        private NoseLightSwitchPosition GetNoseLightSwitchPosition()
        {
            var noseLightValue = _dataRefs.NoseLightSwitchPositionDataRef?.Value ?? 0f;
            var taxiLightValue = _dataRefs.TaxiLightSwitchPositionDataRef?.Value ?? 0f;

            if (noseLightValue == 0f && taxiLightValue == 0f)
            {
                return NoseLightSwitchPosition.Off;
            }

            if (noseLightValue == 0f && taxiLightValue == 1f)
            {
                return NoseLightSwitchPosition.Taxi;
            }

            if (noseLightValue == 1f && taxiLightValue == 0f)
            {
                return NoseLightSwitchPosition.Takeoff;
            }

            return NoseLightSwitchPosition.Takeoff;
        }

        private SeatBeltsSignSwtichPosition GetSeatBeltsSignSwtichPosition()
        {
            var value = _dataRefs.SeatBeltsSwitchPositionDataRef?.Value ?? 0f;

            if (value == 0f)
            {
                return SeatBeltsSignSwtichPosition.Off;
            }

            return value == 1f ? SeatBeltsSignSwtichPosition.Auto : SeatBeltsSignSwtichPosition.On;
        }

        private CameraLocation GetCameraLocation()
        {
            var value = _dataRefs.CameraPositionDataRef?.Value;
            var isExternalView = _dataRefs.ExternalViewDataRef?.Value ?? false;

            if (value == null || value.Length < 3)
            {
                return new CameraLocation(Vector3.Zero, isExternalView);
            }

            return new CameraLocation(new Vector3(value[0], value[1], value[2]), isExternalView);
        }

        private void DebugLogSnapshot(FlightSnapshot flightSnapshot)
        {
            var properties = flightSnapshot.GetType().GetTypeInfo().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            var flightSnapshotProps = string.Join(";", properties.Select(p => $"{p.Name} = {p.GetValue(flightSnapshot)}"));

            _logger.Debug($"Last flight snapshot:{Environment.NewLine}{flightSnapshotProps}");
        }
    }
}
