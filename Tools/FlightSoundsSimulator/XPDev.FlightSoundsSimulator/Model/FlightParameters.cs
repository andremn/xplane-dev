using XPDev.FlightManagement;

namespace XPDev.FlightSoundsSimulator.Model
{
    public class FlightParameters
    {
        public SeatBeltsSignSwtichPosition SeatBeltsSwitch { get; set; }

        public NoseLightSwitchPosition NoseLightSwitch { get; set; }

        public LandingLightsSwitchPosition LandingLightsSwitch { get; set; }

        public BeaconLightSwitchPosition BeaconLightsSwtich { get; set; }

        public DoorState FrontLeftDoorState { get; set; }

        public DayPeriod DayPeriod { get; set; }

        public EngineState EnginesState { get; set; }

        public float GroundSpeed { get; set; }

        public float AltitudeAboveGroundLevel { get; set; }

        public float IndicatedVerticalSpeed { get; set; }

        public bool IsInsideCockpit { get; set; }

        public bool IsCameraExternal { get; set; }

        public bool IsPaused { get; set; }

        public override string ToString()
        {
            return $"SeatBeltsSwitch = {SeatBeltsSwitch}; NoseLightSwitch = {NoseLightSwitch}; LandingLightsSwitch = {LandingLightsSwitch}; " +
                $"BeaconLightsSwtich = {BeaconLightsSwtich}; FrontLeftDoorState = {FrontLeftDoorState}; DayPeriod = {DayPeriod}; " +
                $"EnginesRunning = {EnginesState}; GroundSpeed = {GroundSpeed}; AltitudeAboveGroundLevel = {AltitudeAboveGroundLevel}; " +
                $"IndicatedVerticalSpeed = {IndicatedVerticalSpeed}; IsInsideCockpit = {IsInsideCockpit}; IsCameraExternal = {IsCameraExternal}; " +
                $"IsPaused = {IsPaused}";
        }
    }
}
