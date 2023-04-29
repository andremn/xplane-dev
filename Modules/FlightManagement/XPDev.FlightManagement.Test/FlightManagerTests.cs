using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace XPDev.FlightManagement.Test
{
    public class FlightManagerTests
    {
        private FlightStateManagerTestWrapper Target { get; set; }

        public FlightManagerTests()
        {
            Target = new FlightStateManagerTestWrapper();
        }

        [Fact]
        public void CurrentStateShouldChangeToBoarding()
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftFrontLeftDoorState = DoorState.Open
            };

            Target.IsAircraftOnGround = true;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Boarding);
        }

        [Fact]
        public void CurrentStateShouldChangeToBoardingDone()
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 0,
                AircraftGroundSpeed = 0,
                AircraftFrontLeftDoorState = DoorState.Closed
            };

            Target.IsAircraftOnGround = true;
            Target.IsAircraftFronLeftDoorOpen = true;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.BoardingDone);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CurrentStateShouldChangeToPushback(bool hasChangedToTakeoff)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 0,
                AircraftGroundSpeed = 0,
                AircraftFrontLeftDoorState = DoorState.Closed,
                AircraftBeaconLightSwitchPosition = BeaconLightSwitchPosition.On
            };

            Target.IsAircraftOnGround = true;
            Target.IsAircraftFronLeftDoorOpen = false;
            Target.HasTakenOff = false;
            Target.HasChangedToTakeoff = hasChangedToTakeoff;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Pushback);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(10f)]
        [InlineData(25f)]
        public void CurrentStateShouldChangeToTaxiOut(float groundSpeed)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 0,
                AircraftGroundSpeed = groundSpeed,
                AircraftFrontLeftDoorState = DoorState.Closed,
                AircraftBeaconLightSwitchPosition = BeaconLightSwitchPosition.On,
                AircraftNoseLightSwitchPosition = NoseLightSwitchPosition.Taxi
            };

            Target.IsAircraftOnGround = true;
            Target.IsAircraftFronLeftDoorOpen = false;
            Target.HasTakenOff = false;
            Target.HasChangedToTakeoff = false;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.TaxiOut);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void CurrentStateShouldChangeToTakeoff(bool hasChangedToTakeoff)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 0,
                AircraftGroundSpeed = 40f,
                AircraftFrontLeftDoorState = DoorState.Closed,
                AircraftBeaconLightSwitchPosition = BeaconLightSwitchPosition.On,
                AircraftNoseLightSwitchPosition = NoseLightSwitchPosition.Takeoff
            };

            Target.IsAircraftOnGround = true;
            Target.IsAircraftFronLeftDoorOpen = false;
            Target.HasTakenOff = false;
            Target.HasChangedToTakeoff = hasChangedToTakeoff;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Takeoff);
        }

        [Theory]
        [InlineData(2200f, FmcFlightPhase.Climb)]
        [InlineData(0f, FmcFlightPhase.Climb)]
        [InlineData(-300f, FmcFlightPhase.Climb)]
        [InlineData(2200f, FmcFlightPhase.Undefined)]
        [InlineData(0f, FmcFlightPhase.Undefined)]
        [InlineData(-300f, FmcFlightPhase.Undefined)]
        public void CurrentStateShouldChangeToClimb(float verticalSpeed, FmcFlightPhase fmcFlightPhase)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 35500f,
                AircraftIndicatedAltitute = 35500f,
                AircraftCruiseAltitude = 37000f,
                AircraftVerticalSpeed = verticalSpeed,
                FmcFlightPhase = fmcFlightPhase
            };

            Target.HasReachedCruiseLevel = false;
            Target.HasTakenOff = true;
            Target.HasChangedToTakeoff = true;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Climb);
        }

        [Theory]
        [InlineData(34000f, 0f, true, FmcFlightPhase.Cruise)]
        [InlineData(34500f, 0f, true, FmcFlightPhase.Cruise)]
        [InlineData(35000f, 0f, true, FmcFlightPhase.Cruise)]
        [InlineData(35500f, 500f, true, FmcFlightPhase.Cruise)]
        [InlineData(36000f, -200, true, FmcFlightPhase.Cruise)]
        [InlineData(36000f, 0f, true, FmcFlightPhase.Cruise)]
        [InlineData(34000f, 0f, true, FmcFlightPhase.Descent)]
        [InlineData(34500f, 0f, true, FmcFlightPhase.Undefined)]
        public void CurrentStateShouldChangeToCruise(float indicatedAltitude, float verticalSpeed, bool hasReachedCruiseLevel, FmcFlightPhase fmcFlightPhase)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 32000f,
                AircraftIndicatedAltitute = indicatedAltitude,
                AircraftCruiseAltitude = 35000f,
                AircraftVerticalSpeed = verticalSpeed,
                FmcFlightPhase = fmcFlightPhase
            };

            Target.HasReachedCruiseLevel = hasReachedCruiseLevel;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Cruise);
        }

        [Theory]
        [InlineData(0f, FmcFlightPhase.Descent)]
        [InlineData(-1200, FmcFlightPhase.Descent)]
        [InlineData(0f, FmcFlightPhase.Undefined)]
        [InlineData(-1200, FmcFlightPhase.Undefined)]
        public void CurrentStateShouldChangeToDescent(float verticalSpeed, FmcFlightPhase fmcFlightPhase)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftCruiseAltitude = 35000f,
                AircraftAltituteAboveGroundLevel = 28000f,
                AircraftIndicatedAltitute = 31000f,
                AircraftVerticalSpeed = verticalSpeed,
                FmcFlightPhase = fmcFlightPhase
            };

            Target.HasReachedCruiseLevel = true;
            Target.HasTakenOff = true;
            Target.HasChangedToTakeoff = true;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Descent);
        }

        [Theory]
        [InlineData(0f, true)]
        [InlineData(0f, false)]
        [InlineData(-1200, true)]
        [InlineData(-1200, false)]
        public void CurrentStateShouldChangeToApproach(float verticalSpeed, bool hasReachedCruiseLevel)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 6000f,
                AircraftIndicatedAltitute = 9000f,
                AircraftVerticalSpeed = verticalSpeed,
                AircraftLandingLightsSwitchPosition = LandingLightsSwitchPosition.On
            };

            Target.HasReachedCruiseLevel = hasReachedCruiseLevel;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Approach);
        }

        [Theory]
        [InlineData(true, DayPeriod.DayTime, FmcFlightPhase.Approach)]
        [InlineData(false, DayPeriod.DayTime, FmcFlightPhase.Approach)]
        [InlineData(true, DayPeriod.NightTime, FmcFlightPhase.Approach)]
        [InlineData(false, DayPeriod.NightTime, FmcFlightPhase.Approach)]
        [InlineData(true, DayPeriod.DayTime, FmcFlightPhase.Undefined)]
        [InlineData(false, DayPeriod.DayTime, FmcFlightPhase.Undefined)]
        [InlineData(true, DayPeriod.NightTime, FmcFlightPhase.Undefined)]
        [InlineData(false, DayPeriod.NightTime, FmcFlightPhase.Undefined)]
        public void CurrentStateShouldChangeToLanding(bool hasReachedCruiseLevel, DayPeriod dayPeriod, FmcFlightPhase fmcFlightPhase)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 2500f,
                AircraftIndicatedAltitute = 5500f,
                AircraftVerticalSpeed = -700f,
                AircraftLandingLightsSwitchPosition = LandingLightsSwitchPosition.On,
                AircraftNoseLightSwitchPosition = NoseLightSwitchPosition.Takeoff,
                FmcFlightPhase = fmcFlightPhase,
                DayPeriod = dayPeriod
            };

            Target.IsAircraftOnGround = false;
            Target.HasTakenOff = true;
            Target.HasChangedToTakeoff = true;
            Target.HasReachedCruiseLevel = hasReachedCruiseLevel;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(dayPeriod == DayPeriod.DayTime ? FlightState.LandingDay : FlightState.LandingNight);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CurrentStateShouldChangeToRollout(bool hasReachedCruiseLevel)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftAltituteAboveGroundLevel = 0f,
                AircraftIndicatedAltitute = 0f,
                AircraftGroundSpeed = 110f,
                AircraftNoseLightSwitchPosition = NoseLightSwitchPosition.Takeoff,
                AircraftLandingLightsSwitchPosition = LandingLightsSwitchPosition.On,
                DayPeriod = DayPeriod.NightTime
            };

            Target.IsAircraftOnGround = true;
            Target.HasChangedToTakeoff = true;
            Target.HasTakenOff = true;
            Target.HasReachedCruiseLevel = hasReachedCruiseLevel;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Rollout);
        }

        [Theory]
        [InlineData(true, 0f)]
        [InlineData(true, 10f)]
        [InlineData(true, 25f)]
        [InlineData(false, 0f)]
        [InlineData(false, 10f)]
        [InlineData(false, 25f)]
        public void CurrentStateShouldChangeToTaxiIn(bool hasTakenOff, float groundSpeed)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftGroundSpeed = groundSpeed,
                AircraftNoseLightSwitchPosition = NoseLightSwitchPosition.Taxi
            };

            Target.IsAircraftOnGround = true;
            Target.HasChangedToTakeoff = true;
            Target.HasTakenOff = hasTakenOff;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.TaxiIn);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CurrentStateShouldChangeToUnboarding(bool hasTakenOff)
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftFrontLeftDoorState = DoorState.Open
            };

            Target.IsAircraftOnGround = true;
            Target.HasChangedToTakeoff = true;
            Target.HasTakenOff = hasTakenOff;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.Unboarding);
        }

        [Fact]
        public void CurrentStateShouldChangeToUnboardingDone()
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftFrontLeftDoorState = DoorState.Closed
            };

            Target.IsAircraftOnGround = true;
            Target.IsAircraftFronLeftDoorOpen = true;
            Target.HasChangedToTakeoff = true;
            Target.HasTakenOff = true;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.UnboardingDone);
        }

        [Fact]
        public void CurrentStateIsUnboardingDoneFieldsAreReset()
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftFrontLeftDoorState = DoorState.Closed
            };

            Target.IsAircraftOnGround = true;
            Target.IsAircraftFronLeftDoorOpen = true;
            Target.HasReachedCruiseLevel = true;
            Target.HasChangedToTakeoff = true;
            Target.HasTakenOff = true;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.CurrentFlightState.Should().Be(FlightState.UnboardingDone);
            Target.HasReachedCruiseLevel.Should().Be(false);
            Target.HasChangedToTakeoff.Should().Be(false);
            Target.HasTakenOff.Should().Be(false);
        }

        [Fact]
        public void HasReachedCruiseLevelShouldChangeToTrue()
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftVerticalSpeed = -1200f,
                FmcFlightPhase = FmcFlightPhase.Climb
            };

            Target.HasReachedCruiseLevel = false;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.HasReachedCruiseLevel.Should().Be(true);
        }

        [Fact]
        public void HasReachedCruiseLevelShouldChangeToFalse()
        {
            // Arrange
            var flightSnapshot = new FlightSnapshot
            {
                AircraftVerticalSpeed = 1200f,
                FmcFlightPhase = FmcFlightPhase.Cruise
            };

            Target.HasReachedCruiseLevel = true;

            // Act
            Target.UpdateState(flightSnapshot);

            // Assert
            Target.HasReachedCruiseLevel.Should().Be(false);
        }

        [Theory]
        [InlineData(FlightState.Climb)]
        [InlineData(FlightState.Cruise)]
        [InlineData(FlightState.Descent)]
        [InlineData(FlightState.Approach)]
        [InlineData(FlightState.LandingDay)]
        [InlineData(FlightState.LandingNight)]
        [InlineData(FlightState.Rollout)]
        [InlineData(FlightState.TaxiIn)]
        [InlineData(FlightState.Unboarding)]
        public void HasChangedToTakeoffAndHasTakenOffShouldChangeToTrue(FlightState flightState)
        {
            // Arrange
            Target.HasChangedToTakeoff = false;
            Target.HasTakenOff = false;

            // Act
            Target.SetState(flightState);

            // Assert
            Target.HasChangedToTakeoff.Should().Be(true);
            Target.HasTakenOff.Should().Be(true);
        }

        [Theory]
        [InlineData(FlightState.Cruise)]
        [InlineData(FlightState.Descent)]
        [InlineData(FlightState.Approach)]
        [InlineData(FlightState.LandingDay)]
        [InlineData(FlightState.LandingNight)]
        [InlineData(FlightState.Rollout)]
        [InlineData(FlightState.TaxiIn)]
        [InlineData(FlightState.Unboarding)]
        public void HasChangedToTakeoffAndHasTakenOffAndHasReachedCruiseLevelShouldChangeToTrue(FlightState flightState)
        {
            // Arrange
            Target.HasChangedToTakeoff = false;
            Target.HasTakenOff = false;
            Target.HasReachedCruiseLevel = false;

            // Act
            Target.SetState(flightState);

            // Assert
            Target.HasChangedToTakeoff.Should().Be(true);
            Target.HasTakenOff.Should().Be(true);
            Target.HasReachedCruiseLevel.Should().Be(true);
        }

        [Theory]
        [InlineData(FlightState.Parked, FlightState.Boarding)]
        [InlineData(FlightState.Boarding, FlightState.BoardingDone)]
        [InlineData(FlightState.BoardingDone, FlightState.Pushback)]
        [InlineData(FlightState.Pushback, FlightState.TaxiOut)]
        [InlineData(FlightState.TaxiOut, FlightState.Takeoff)]
        [InlineData(FlightState.Takeoff, FlightState.Climb, FlightState.TaxiIn, FlightState.Rollout)]
        [InlineData(FlightState.Climb, FlightState.Cruise, FlightState.Descent)]
        [InlineData(FlightState.Cruise, FlightState.Climb, FlightState.Descent)]
        [InlineData(FlightState.Descent, FlightState.Approach, FlightState.LandingDay, FlightState.LandingNight)]
        [InlineData(FlightState.LandingDay, FlightState.Climb, FlightState.Rollout)]
        [InlineData(FlightState.LandingNight, FlightState.Climb, FlightState.Rollout)]
        [InlineData(FlightState.Rollout, FlightState.TaxiIn)]
        [InlineData(FlightState.TaxiIn, FlightState.TaxiOut, FlightState.Pushback, FlightState.Unboarding)]
        [InlineData(FlightState.Unboarding, FlightState.UnboardingDone)]
        [InlineData(FlightState.UnboardingDone, FlightState.Parked)]
        public void ShouldChangeCurrentState(FlightState from, params FlightState[] acceptedStates)
        {
            // Arrange
            var allStates = Enum.GetValues(typeof(FlightState)).Cast<FlightState>();
            var changedStates = new List<FlightState>();

            Target.IsInitialized = true;
            Target.SetState(from);

            // Act
            foreach (var state in allStates)
            {
                Target.TryChangeToNextState(state);

                if (Target.CurrentFlightState != from)
                {
                    changedStates.Add(state);
                }

                Target.SetState(from);
            }

            // Assert
            Assert.True(changedStates.All(s => acceptedStates.Contains(s)));
        }
    }
}
