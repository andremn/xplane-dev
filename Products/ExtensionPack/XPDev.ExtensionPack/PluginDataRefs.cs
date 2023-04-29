using XPDev.Foundation;
using XPDev.XPlugin.Api;

namespace XPDev.ExtensionPack
{
    internal class PluginDataRefs
    {
        private readonly DataRefHandler _indicatedAltitudeFtDataRef;
        private readonly DataRefHandler _indicatedAltitudeAboveGroundFtDataRef;
        private readonly DataRefHandler _indicatedVerticalSpeedFpmDataRef;
        private readonly DataRefHandler _indicatedGroundSpeedDataRef;
        private readonly DataRefHandler _fmcFlightPhaseDataRef;
        private readonly DataRefHandler _enginesN2RatePercentDataRef;
        private readonly DataRefHandler _flapsLeverPositionDataRef;
        private readonly DataRefHandler _gearLeverPositionDataRef;
        private readonly DataRefHandler _paxDoorsStatesDataRef;
        private readonly DataRefHandler _cockpitDoorStateDataRef;
        private readonly DataRefHandler _seatBeltsSwitchPositionDataRef;
        private readonly DataRefHandler _taxiLightSwitchPositionDataRef;
        private readonly DataRefHandler _noseLightSwitchPositionDataRef;
        private readonly DataRefHandler _beaconLightSwitchPositionDataRef;
        private readonly DataRefHandler _landingLightsSwitchPositionDataRef;
        private readonly DataRefHandler _cruiseLevelFtDataRef;
        private readonly DataRefHandler _planeCockpitDoorPositionDataRef;
        private readonly DataRefHandler _planeLiveryDataRef;
        private readonly DataRefHandler _dayPeriodDataRef;
        private readonly DataRefHandler _cameraPositionDataRef;
        private readonly DataRefHandler _externalViewDataRef;
        private readonly DataRefHandler _pausedDataRef;

        public PluginDataRefs(IXPlaneDataAccess dataAccess)
        {
            dataAccess.NotNull();            

            // Aircraft datarefs
            _indicatedAltitudeFtDataRef = dataAccess.FindDataRef(DataRefNames.IndicatedAltitudeFtDataRefName);
            _indicatedAltitudeAboveGroundFtDataRef = dataAccess.FindDataRef(DataRefNames.IndicatedAltitudeAboveGroundFtDataRefName);
            _indicatedVerticalSpeedFpmDataRef = dataAccess.FindDataRef(DataRefNames.IndicatedVerticalSpeedFpmDataRefName);
            _indicatedGroundSpeedDataRef = dataAccess.FindDataRef(DataRefNames.IndicatedGroundSpeedDataRefName);
            _fmcFlightPhaseDataRef = dataAccess.FindDataRef(DataRefNames.FmcFlightPhaseDataRefName);
            _enginesN2RatePercentDataRef = dataAccess.FindDataRef(DataRefNames.EnginesN2RatePercentDataRefName);
            _flapsLeverPositionDataRef = dataAccess.FindDataRef(DataRefNames.FlapsLeverPositionDataRefName);
            _gearLeverPositionDataRef = dataAccess.FindDataRef(DataRefNames.GearLeverPositionDataRefName);
            _paxDoorsStatesDataRef = dataAccess.FindDataRef(DataRefNames.PaxDoorsStatesDataRefName);
            _cockpitDoorStateDataRef = dataAccess.FindDataRef(DataRefNames.CockpitDoorStateDataRefName);
            _seatBeltsSwitchPositionDataRef = dataAccess.FindDataRef(DataRefNames.SeatBeltsSwtichPositionDataRefName);
            _taxiLightSwitchPositionDataRef = dataAccess.FindDataRef(DataRefNames.TaxiLightSwitchPositionDataRefName);
            _noseLightSwitchPositionDataRef = dataAccess.FindDataRef(DataRefNames.NoseLightSwitchPositionDataRefName);
            _beaconLightSwitchPositionDataRef = dataAccess.FindDataRef(DataRefNames.BeaconLightSwitchPositionDataRefName);
            _landingLightsSwitchPositionDataRef = dataAccess.FindDataRef(DataRefNames.LandingLightsSwitchPositionDataRefName);
            _cruiseLevelFtDataRef = dataAccess.FindDataRef(DataRefNames.CruiseLevelFtDataRefName);
            _planeLiveryDataRef = dataAccess.FindDataRef(DataRefNames.CurrentLoadedLiveryDataRefName);
            _planeCockpitDoorPositionDataRef = dataAccess.FindDataRef(DataRefNames.PlaneCockpitDoorPositionDataRefName);

            // Environment datarefs
            _dayPeriodDataRef = dataAccess.FindDataRef("");

            // Sim datarefs
            _cameraPositionDataRef = dataAccess.FindDataRef(DataRefNames.CameraPositionDataRefName);
            _externalViewDataRef = dataAccess.FindDataRef(DataRefNames.ExternalViewDataRefName);
            _pausedDataRef = dataAccess.FindDataRef(DataRefNames.PausedDataRefName);
        }

        public IDataRef<float> IndicatedAltitudeFtDataRef => _indicatedAltitudeFtDataRef?.AsFloat();

        public IDataRef<float> IndicatedAltitudeAboveGroundFtDataRef => _indicatedAltitudeAboveGroundFtDataRef?.AsFloat();

        public IDataRef<float> IndicatedVerticalSpeedFpmDataRef => _indicatedVerticalSpeedFpmDataRef?.AsFloat();

        public IDataRef<float> IndicatedGroundSpeedDataRef => _indicatedGroundSpeedDataRef?.AsFloat();

        public IDataRef<int> FmcFlightPhaseDataRef => _fmcFlightPhaseDataRef?.AsInt();

        public IDataRef<float> EnginesN2RatePercentDataRef => _enginesN2RatePercentDataRef?.AsFloat();

        public IDataRef<float> FlapsLeverDataRef => _flapsLeverPositionDataRef?.AsFloat();

        public IDataRef<bool> GearLeverPositionDataRef => _gearLeverPositionDataRef?.AsBool();

        public IDataRef<float[]> PaxDoorsStatesDataRef => _paxDoorsStatesDataRef?.AsFloatArray();

        public IDataRef<float> CockpitDoorStateDataRef => _cockpitDoorStateDataRef?.AsFloat();

        public IDataRef<int> SeatBeltsSwitchPositionDataRef => _seatBeltsSwitchPositionDataRef?.AsInt();

        public IDataRef<int> TaxiLightSwitchPositionDataRef => _taxiLightSwitchPositionDataRef?.AsInt();

        public IDataRef<int> NoseLightSwitchPositionDataRef => _noseLightSwitchPositionDataRef?.AsInt();

        public IDataRef<int> BeaconLightSwitchPositionDataRef => _beaconLightSwitchPositionDataRef?.AsInt();

        public IDataRef<int> LandingLightsSwitchPositionDataRef => _landingLightsSwitchPositionDataRef?.AsInt();

        public IDataRef<float> CruiseLevelFtDataRef => _cruiseLevelFtDataRef?.AsFloat();

        public IDataRef<string> PlaneLiveryDataRef => _planeLiveryDataRef?.AsString();

        public IDataRef<float[]> PlaneCockpitDoorPositionDataRef => _planeCockpitDoorPositionDataRef?.AsFloatArray();

        public IDataRef<bool> DayPeriodDataRef => _dayPeriodDataRef?.AsBool();

        public IDataRef<float[]> CameraPositionDataRef => _cameraPositionDataRef?.AsFloatArray();

        public IDataRef<bool> ExternalViewDataRef => _externalViewDataRef?.AsBool();

        public IDataRef<bool> PausedDataRef => _pausedDataRef?.AsBool();
    }
}
