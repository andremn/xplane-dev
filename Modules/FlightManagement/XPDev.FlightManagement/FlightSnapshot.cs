using System.Numerics;

namespace XPDev.FlightManagement
{
    /// <summary>
    /// A snapshot of the current situation of the flight.
    /// </summary>
    public class FlightSnapshot
    {
        /// <summary>
        /// Gets or sets the current aircraft indicated altitude, in feet.
        /// </summary>
        public float AircraftIndicatedAltitute { get; set; }

        /// <summary>
        /// Gets or sets the current aircraft altitude above ground level, in feet.
        /// </summary>
        public float AircraftAltituteAboveGroundLevel { get; set; }

        /// <summary>
        /// Gets or sets the current aircraft indicated vertical speed, in feet per minute.
        /// </summary>
        public float AircraftVerticalSpeed { get; set; }

        /// <summary>
        /// Gets or sets the selected aircraft cruise altitude, in feet.
        /// </summary>
        public float AircraftCruiseAltitude { get; set; }

        /// <summary>
        /// Gets or sets the current aircraft ground speed, in knots.
        /// </summary>
        public float AircraftGroundSpeed { get; set; }

        /// <summary>
        /// Gets or sets the running status (on or off) for the available engines.
        /// </summary>
        public bool[] EnginesRunningStatus { get; set; }

        /// <summary>
        /// Gets whether the aircraft is on the ground.
        /// </summary>
        public bool IsAircraftOnGround => AircraftAltituteAboveGroundLevel < 1f;

        /// <summary>
        /// Gets or sets the state of the aircraft front left door.
        /// </summary>
        public DoorState AircraftFrontLeftDoorState { get; set; }

        /// <summary>
        /// Gets or sets the FMC flight phase.
        /// </summary>
        public FmcFlightPhase FmcFlightPhase { get; set; }

        /// <summary>
        /// Gets or sets the state of the cockpit door.
        /// </summary>
        public DoorState CockpitDoorState { get; set; }

        /// <summary>
        /// Gets or sets the position of the seat belts sign swtich.
        /// </summary>
        public SeatBeltsSignSwtichPosition AircraftSeatBeltsSignSwitchPosition { get; set; }

        /// <summary>
        /// Gets or sets the position of the beacon lights swtich.
        /// </summary>
        public BeaconLightSwitchPosition AircraftBeaconLightSwitchPosition { get; set; }

        /// <summary>
        /// Gets or sets the position of the nose light swtich.
        /// </summary>
        public NoseLightSwitchPosition AircraftNoseLightSwitchPosition { get; set; }

        /// <summary>
        /// Gets or sets the position of the landing lights swtich.
        /// </summary>
        public LandingLightsSwitchPosition AircraftLandingLightsSwitchPosition { get; set; }

        /// <summary>
        /// Gets or sets the position of the flap lever.
        /// </summary>
        public FlapsLeverPosition FlapsLeverPosition { get; set; }

        /// <summary>
        /// Gets or sets the position of the gear lever.
        /// </summary>
        public GearLeverPosition GearLeverPosition { get; set; }

        /// <summary>
        /// Gets or sets the period of the day.
        /// </summary>
        public DayPeriod DayPeriod { get; set; }

        /// <summary>
        /// Gets or sets the current location of the camera.
        /// </summary>
        public CameraLocation CameraLocation { get; set; }

        /// <summary>
        /// Gets or sets the positions of the cockpit door.
        /// </summary>
        public Vector3 CockpitDoorPosition { get; set; }

        /// <summary>
        /// Gets or sets if the flight is paused.
        /// </summary>
        public bool IsPaused { get; set; }

        /// <summary>
        /// Creates a new instance of the <see cref="FlightSnapshot"/> class which has the same property values as the current instance.
        /// </summary>
        /// <returns>An instance of <see cref="FlightSnapshot"/> with the same property values of the current instance.</returns>
        public FlightSnapshot Clone()
        {
            return new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = AircraftAltituteAboveGroundLevel,
                AircraftBeaconLightSwitchPosition = AircraftBeaconLightSwitchPosition,
                AircraftCruiseAltitude = AircraftCruiseAltitude,
                AircraftFrontLeftDoorState = AircraftFrontLeftDoorState,
                AircraftGroundSpeed = AircraftGroundSpeed,
                AircraftIndicatedAltitute = AircraftIndicatedAltitute,
                AircraftLandingLightsSwitchPosition = AircraftLandingLightsSwitchPosition,
                AircraftNoseLightSwitchPosition = AircraftNoseLightSwitchPosition,
                AircraftSeatBeltsSignSwitchPosition = AircraftSeatBeltsSignSwitchPosition,
                AircraftVerticalSpeed = AircraftVerticalSpeed,
                CameraLocation = CameraLocation,
                CockpitDoorPosition = CockpitDoorPosition,
                CockpitDoorState = CockpitDoorState,
                DayPeriod = DayPeriod,
                EnginesRunningStatus = EnginesRunningStatus,
                IsPaused = IsPaused,
                FlapsLeverPosition = FlapsLeverPosition,
                FmcFlightPhase = FmcFlightPhase,
                GearLeverPosition = GearLeverPosition
            };
        }
    }
}