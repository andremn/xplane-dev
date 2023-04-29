using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("XPDev.FlightManagement.Test")]
namespace XPDev.FlightManagement
{
    /// <summary>
    /// Manages the state of a flight.
    /// </summary>
    internal class FlightStateManager : IFlightStateManager
    {
        private readonly object _syncLock = new object();

        protected bool _isInitialized;
        protected DoorState _lastAircraftFrontLeftDoorStateValue;
        protected IList<FlightState> _possibleNextFlightStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="FlightStateManager"/> class.
        /// </summary>
        public FlightStateManager()
        {
            _lastAircraftFrontLeftDoorStateValue = DoorState.Closed;
            _possibleNextFlightStates = new List<FlightState>();
        }

        /// <summary>
        /// Gets the current flight state.
        /// </summary>
        public FlightState CurrentFlightState { get; protected set; }

        /// <summary>
        /// Gets or sets if the aircraft has changed to takeoff state.
        /// </summary>
        public bool HasChangedToTakeoff { get; set; }

        /// <summary>
        /// Gets or sets if the aircraft has reached the cruise level.
        /// </summary>
        public bool HasReachedCruiseLevel { get; set; }

        /// <summary>
        /// Gets or sets if the aircraft is on ground.
        /// </summary>
        public bool IsAircraftOnGround { get; set; } = true;

        /// <summary>
        /// Gets or sets if the aircraft has taken off.
        /// </summary>
        public bool HasTakenOff { get; set; }

        /// <summary>
        /// Gets or sets if the aircraft front left door is open.
        /// </summary>
        public bool IsAircraftFronLeftDoorOpen
        {
            get => _lastAircraftFrontLeftDoorStateValue == DoorState.Open;
            set => _lastAircraftFrontLeftDoorStateValue = value ? DoorState.Open : DoorState.Closed;
        }

        /// <summary>
        /// Updates the current flight state based on the specified <see cref="FlightSnapshot"/>.
        /// </summary>
        /// <param name="flightSnapshot">The flight snapshot to be used to update the flight state.</param>
        public void UpdateState(FlightSnapshot flightSnapshot)
        {
            if (flightSnapshot == null)
            {
                return;
            }

            lock (_syncLock)
            {
                var nextState = GetFlightState(flightSnapshot);

                if (_isInitialized && nextState != CurrentFlightState)
                {
                    TryChangeToNextState(nextState);
                }
                else if (!_isInitialized)
                {
                    SetState(nextState);
                    _isInitialized = true;
                }

                // If we the plane was climbing but started to descent, set the HasReachedCruiseLevel to true since we won't reach cruise level to do so
                if (CurrentFlightState == FlightState.Climb && flightSnapshot.AircraftVerticalSpeed <= Constants.MinDescentVerticalSpeed && !HasReachedCruiseLevel)
                {
                    HasReachedCruiseLevel = true;
                }
                // This is to support step climb, which happens when the plane is at cruise level and starts climbing again
                else if (CurrentFlightState == FlightState.Cruise && flightSnapshot.AircraftVerticalSpeed >= Constants.MinClimbVerticalSpeed && HasReachedCruiseLevel)
                {
                    HasReachedCruiseLevel = false;
                }

                _lastAircraftFrontLeftDoorStateValue = flightSnapshot.AircraftFrontLeftDoorState;
                IsAircraftOnGround = flightSnapshot.IsAircraftOnGround;
            }
        }

        /// <summary>
        /// Tries to change the current flight state to <paramref name="nextState"/>.
        /// </summary>
        /// <param name="nextState">The fligh state to change the current state to.</param>
        protected void TryChangeToNextState(FlightState nextState)
        {
            var possibleNextStates = GetPossibleNextFlightStates();

            // Only change the state if the next state is a valid next state for the current one
            if (possibleNextStates.Contains(nextState))
            {
                SetState(nextState);
            }
        }

        /// <summary>
        /// Gets the possible flight states after the current one, if any.
        /// </summary>
        /// <returns>A <see cref="IList{T}"/> containing the possible flight states or empty if the states could not be determined.</returns>
        protected IList<FlightState> GetPossibleNextFlightStates()
        {
            return CurrentFlightState switch
            {
                FlightState.Parked => new List<FlightState> { FlightState.Boarding },
                FlightState.Boarding => new List<FlightState> { FlightState.BoardingDone },
                FlightState.BoardingDone => new List<FlightState> { FlightState.Pushback },
                FlightState.Pushback => new List<FlightState> { FlightState.TaxiOut },
                FlightState.TaxiOut => new List<FlightState> { FlightState.Takeoff },
                FlightState.Takeoff => new List<FlightState> { FlightState.Climb, FlightState.TaxiIn, FlightState.Rollout },
                FlightState.Climb => new List<FlightState> { FlightState.Cruise, FlightState.Descent },
                FlightState.Cruise => new List<FlightState> { FlightState.Climb, FlightState.Descent },
                FlightState.Descent => new List<FlightState> { FlightState.Approach, FlightState.LandingDay, FlightState.LandingNight },
                FlightState.Approach => new List<FlightState> { FlightState.LandingDay, FlightState.LandingNight },
                FlightState.LandingDay => new List<FlightState> { FlightState.Climb, FlightState.Rollout },// Todo: add support for go arounds
                FlightState.LandingNight => new List<FlightState> { FlightState.Climb, FlightState.Rollout },
                FlightState.Rollout => new List<FlightState> { FlightState.TaxiIn },
                FlightState.TaxiIn => new List<FlightState> { FlightState.TaxiOut, FlightState.Pushback, FlightState.Unboarding },
                FlightState.Unboarding => new List<FlightState> { FlightState.UnboardingDone },
                FlightState.UnboardingDone => new List<FlightState> { FlightState.Parked },
                _ => new List<FlightState>(0),
            };
        }

        /// <summary>
        /// Sets the current flight state to the value specified by <paramref name="flightState"/>.
        /// </summary>
        /// <param name="flightState">The flight state to set the current flight state to.</param>
        protected void SetState(FlightState flightState)
        {
            if (flightState is >= FlightState.Climb and < FlightState.UnboardingDone)
            {
                HasChangedToTakeoff = true;
                HasTakenOff = true;

                if (flightState >= FlightState.Cruise)
                {
                    HasReachedCruiseLevel = true;
                }
            }
            else if (flightState == FlightState.UnboardingDone)
            {
                // End of flight, reset fields
                HasTakenOff = false;
                HasChangedToTakeoff = false;
                HasReachedCruiseLevel = false;
            }

            CurrentFlightState = flightState;
        }

        protected FlightState GetFlightState(FlightSnapshot flightSnapshot)
        {
            if (IsBoarding(flightSnapshot))
            {
                return FlightState.Boarding;
            }
            
            if (IsBoardingDone(flightSnapshot))
            {
                return FlightState.BoardingDone;
            }
            
            if (IsPushingBack(flightSnapshot))
            {
                return FlightState.Pushback;
            }
            
            if (IsTaxingOut(flightSnapshot))
            {
                return FlightState.TaxiOut;
            }
            
            if (IsTakingOff(flightSnapshot))
            {
                HasChangedToTakeoff = true;
                return FlightState.Takeoff;
            }
            
            if (IsClimbing(flightSnapshot))
            {
                return FlightState.Climb;
            }
            
            if (IsCrusing(flightSnapshot))
            {
                return FlightState.Cruise;
            }
            
            if (IsDescending(flightSnapshot))
            {
                return FlightState.Descent;
            }
            
            if (IsApproaching(flightSnapshot))
            {
                return FlightState.Approach;
            }
            
            if (IsLanding(flightSnapshot))
            {
                return flightSnapshot.DayPeriod == DayPeriod.DayTime ? FlightState.LandingDay : FlightState.LandingNight;
            }
            
            if (IsRollingOut(flightSnapshot))
            {
                return FlightState.Rollout;
            }
            
            if (IsTaxingIn(flightSnapshot))
            {
                return FlightState.TaxiIn;
            }
            
            if (IsUnboarding(flightSnapshot))
            {
                return FlightState.Unboarding;
            }
            
            if (IsUnboardingDone(flightSnapshot))
            {
                return FlightState.UnboardingDone;
            }

            return FlightState.Parked;
        }

        protected bool IsBoarding(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.IsAircraftOnGround && IsAircraftOnGround
                && !HasChangedToTakeoff
                && _lastAircraftFrontLeftDoorStateValue == DoorState.Closed 
                && flightSnapshot.AircraftFrontLeftDoorState == DoorState.Open;
        }

        protected bool IsBoardingDone(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.IsAircraftOnGround && IsAircraftOnGround
                && !HasChangedToTakeoff
                && _lastAircraftFrontLeftDoorStateValue == DoorState.Open 
                && flightSnapshot.AircraftFrontLeftDoorState == DoorState.Closed;
        }

        protected bool IsPushingBack(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.IsAircraftOnGround && IsAircraftOnGround
                && !HasTakenOff
                && flightSnapshot.AircraftFrontLeftDoorState == DoorState.Closed
                && flightSnapshot.AircraftGroundSpeed < Constants.MinTaxiSpeed
                && flightSnapshot.AircraftNoseLightSwitchPosition == NoseLightSwitchPosition.Off
                && flightSnapshot.AircraftBeaconLightSwitchPosition == BeaconLightSwitchPosition.On;
        }

        protected bool IsTaxingOut(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.IsAircraftOnGround
                && IsAircraftOnGround
                && !HasTakenOff && !HasChangedToTakeoff
                && flightSnapshot.AircraftNoseLightSwitchPosition == NoseLightSwitchPosition.Taxi;
        }

        protected bool IsTakingOff(FlightSnapshot flightSnapshot)
        {
            // TODO: check engines instead of ground speed? 
            return flightSnapshot.IsAircraftOnGround
                && !HasTakenOff
                && IsAircraftOnGround 
                && flightSnapshot.AircraftGroundSpeed >= Constants.MinTakeoffSpeed
                && flightSnapshot.AircraftNoseLightSwitchPosition == NoseLightSwitchPosition.Takeoff;
        }

        protected bool IsClimbing(FlightSnapshot flightSnapshot)
        {
            if (flightSnapshot.FmcFlightPhase == FmcFlightPhase.Climb)
            {
                return true;
            }

            return flightSnapshot.FmcFlightPhase == FmcFlightPhase.Undefined 
                && !flightSnapshot.IsAircraftOnGround
                && !HasReachedCruiseLevel
                && HasTakenOff && HasChangedToTakeoff
                && !IsAircraftWithinCruiseAltitude(flightSnapshot)
                && flightSnapshot.AircraftVerticalSpeed > Constants.MinDescentVerticalSpeed;
        }

        protected bool IsCrusing(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.FmcFlightPhase == FmcFlightPhase.Cruise || (!flightSnapshot.IsAircraftOnGround && IsAircraftWithinCruiseAltitude(flightSnapshot));
        }

        protected bool IsDescending(FlightSnapshot flightSnapshot)
        {
            if (flightSnapshot.FmcFlightPhase == FmcFlightPhase.Descent)
            {
                return true;
            }

            return flightSnapshot.FmcFlightPhase == FmcFlightPhase.Undefined
                && !flightSnapshot.IsAircraftOnGround 
                && HasTakenOff && HasChangedToTakeoff
                && HasReachedCruiseLevel
                && !IsAircraftWithinCruiseAltitude(flightSnapshot)
                && flightSnapshot.AircraftVerticalSpeed < Constants.MinClimbVerticalSpeed
                && flightSnapshot.AircraftIndicatedAltitute < flightSnapshot.AircraftCruiseAltitude
                && flightSnapshot.AircraftIndicatedAltitute > Constants.LimitationAltitude;
        }

        protected bool IsApproaching(FlightSnapshot flightSnapshot)
        {
            if (flightSnapshot.FmcFlightPhase == FmcFlightPhase.Approach)
            {
                // If the FMC tell us we are in the Approach phase, but the takeoff lights are on, it means we are cleared to land
                // So we are not approaching anymore, we are probably in Landing(Day|Night) state
                return flightSnapshot.AircraftNoseLightSwitchPosition != NoseLightSwitchPosition.Takeoff;
            }

            return !flightSnapshot.IsAircraftOnGround
                && flightSnapshot.AircraftNoseLightSwitchPosition != NoseLightSwitchPosition.Takeoff
                && flightSnapshot.AircraftLandingLightsSwitchPosition == LandingLightsSwitchPosition.On 
                && flightSnapshot.AircraftIndicatedAltitute <= Constants.LimitationAltitude;
        }

        protected bool IsLanding(FlightSnapshot flightSnapshot)
        {
            if (flightSnapshot.FmcFlightPhase == FmcFlightPhase.Approach)
            {
                return true;
            }

            return !flightSnapshot.IsAircraftOnGround
                && HasTakenOff 
                && HasChangedToTakeoff
                && flightSnapshot.AircraftLandingLightsSwitchPosition == LandingLightsSwitchPosition.On
                && flightSnapshot.AircraftNoseLightSwitchPosition == NoseLightSwitchPosition.Takeoff
                && flightSnapshot.AircraftIndicatedAltitute <= Constants.LimitationAltitude
                && flightSnapshot.AircraftVerticalSpeed <= Constants.MinDescentVerticalSpeed;
        }

        protected bool IsRollingOut(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.IsAircraftOnGround
                && HasTakenOff 
                && HasChangedToTakeoff
                && flightSnapshot.AircraftLandingLightsSwitchPosition == LandingLightsSwitchPosition.On
                && flightSnapshot.AircraftNoseLightSwitchPosition == NoseLightSwitchPosition.Takeoff;
        }

        protected bool IsTaxingIn(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.IsAircraftOnGround
                && IsAircraftOnGround
                && flightSnapshot.AircraftNoseLightSwitchPosition == NoseLightSwitchPosition.Taxi
                && HasChangedToTakeoff;
        }

        protected bool IsUnboarding(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.IsAircraftOnGround
                && HasChangedToTakeoff
                && flightSnapshot.AircraftFrontLeftDoorState == DoorState.Open
                && _lastAircraftFrontLeftDoorStateValue == DoorState.Closed;
        }

        protected bool IsUnboardingDone(FlightSnapshot flightSnapshot)
        {
            return flightSnapshot.IsAircraftOnGround
                && HasChangedToTakeoff
                && _lastAircraftFrontLeftDoorStateValue == DoorState.Open
                && flightSnapshot.AircraftFrontLeftDoorState == DoorState.Closed;
        }

        protected bool IsAircraftWithinCruiseAltitude(FlightSnapshot flightSnapshot)
        {
            var cruiseAlt = flightSnapshot.AircraftCruiseAltitude;

            if (cruiseAlt <= 0)
            {
                return false;
            }

            var currentAlt = flightSnapshot.AircraftIndicatedAltitute;

            return currentAlt <= cruiseAlt + Constants.CruiseToleranceAltitude && currentAlt >= cruiseAlt - Constants.CruiseToleranceAltitude;
        }
    }
}
