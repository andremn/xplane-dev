using Nito.AsyncEx;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using XPDev.Audio;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement.Configuration;
using XPDev.FlightSoundsManagement.Helpers;
using XPDev.Foundation;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Manages announcements logic.
    /// </summary>
    internal class AnnouncementManager : AircraftSoundsManager, IAnnouncementManager
    {
        private const int ClimbAnnouncementMinAltitude = 23000;

        private readonly IFlightStateService _flightStateService;
        private readonly ConcurrentDictionary<FlightAction, ConcurrentBag<AnnouncementSound>> _announcementsMap;
        private readonly ConcurrentDictionary<FlightAction, FlightSoundDescriptorPair> _scheduledAnnouncements;
        private readonly Vector3 _cockpitDoorPosition;

        private SeatBeltsSignSwtichPosition _lastSeatBelts;
        private SeatBeltsSignSwtichPosition? _lastPlayedSeatBeltsAnnouncement;

        /// <summary>
        /// Initializes a new instance of the <see cref="AnnouncementManager"/> class with the 
        /// specified <see cref="IFlightStateService"/>, <see cref="ISoundManager"/>, and <see cref="IFlightSoundConfiguration"/>.
        /// </summary>
        /// <param name="flightStateService">An instance of the <see cref="IFlightStateService"/> interface.</param>
        /// <param name="soundManager">An instance of the <see cref="ISoundManager"/> interface.</param>
        /// <param name="soundConfigurationProvider">The <see cref="IFlightSoundConfiguration"/> provider to be used to create a <see cref="IFlightSoundConfiguration"/>.</param>
        public AnnouncementManager(IFlightStateService flightStateService, ISoundManager soundManager, IFlightSoundConfigurationProvider soundConfigurationProvider)
            : base(soundManager)
        {
            flightStateService.NotNull();
            soundConfigurationProvider.NotNull();

            var soundConfiguration = soundConfigurationProvider.GetFlightSoundConfiguration();

            _flightStateService = flightStateService;
            _announcementsMap = new ConcurrentDictionary<FlightAction, ConcurrentBag<AnnouncementSound>>();
            _scheduledAnnouncements = new ConcurrentDictionary<FlightAction, FlightSoundDescriptorPair>();
            _cockpitDoorPosition = soundConfiguration.CockpitDoorPosition;

            _flightStateService.FlightStateChanged += OnFlightStateChanged;

            DelayToPlayAnnouncement = soundConfiguration.DelayBeforePlayingAnnouncement;
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public TimeSpan DelayToPlayAnnouncement { get; set; }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void SetAnnouncementSoundFileForFlightAction(string soundFilePath, FlightAction flightAction)
        {
            if (string.IsNullOrEmpty(soundFilePath))
            {
                throw new ArgumentException("The path cannot be null or empty.", nameof(soundFilePath));
            }

            if (flightAction == FlightAction.SeatBeltsSignTurnedOff)
            {
                var flightSound = new AnnouncementSound(soundFilePath, SoundRepeatTimes.Forever, flightAction)
                {
                    LocationType = SoundLocationType.PassengerCabinOnly,
                    ConditionToPlay = () => CanPlaySeatBeltsSignAnnouncement(SeatBeltsSignSwtichPosition.Off)
                };

                _scheduledAnnouncements.AddOrUpdate(flightAction, CreateAnnouncement(flightSound), (k, v) => CreateAnnouncement(flightSound));
            }
            else if (flightAction == FlightAction.SeatBeltsSignTurnedOn)
            {
                var flightSound = new AnnouncementSound(soundFilePath, SoundRepeatTimes.Forever, flightAction)
                {
                    LocationType = SoundLocationType.PassengerCabinOnly,
                    ConditionToPlay = () => CanPlaySeatBeltsSignAnnouncement(SeatBeltsSignSwtichPosition.On)
                };

                _scheduledAnnouncements.AddOrUpdate(flightAction, CreateAnnouncement(flightSound), (k, v) => CreateAnnouncement(flightSound));
            }
            else if (flightAction == FlightAction.ClimbingSeatBeltsOn)
            {
                var flightSound = new AnnouncementSound(soundFilePath, SoundRepeatTimes.Once, flightAction)
                {
                    LocationType = SoundLocationType.PassengerCabinOnly,
                    ConditionToPlay = CanPlayClimbingSeatBeltsOnAnnouncement
                };

                _scheduledAnnouncements.AddOrUpdate(flightAction, CreateAnnouncement(flightSound), (k, v) => CreateAnnouncement(flightSound));
            }
            else if (flightAction == FlightAction.ArrivedAtGate)
            {
                var flightSound = new AnnouncementSound(soundFilePath, SoundRepeatTimes.Once, flightAction)
                {
                    LocationType = SoundLocationType.PassengerCabinOnly,
                    ConditionToPlay = CanPlayArrivedAtGateAnnouncement
                };

                _scheduledAnnouncements.AddOrUpdate(flightAction, CreateAnnouncement(flightSound), (k, v) => CreateAnnouncement(flightSound));
            }
            else
            {
                var locationType = SoundLocationType.PassengerCabinOnly;

                if (flightAction == FlightAction.BoardingFinished || flightAction == FlightAction.UnboardingFinished)
                {
                    locationType = SoundLocationType.CockpitOnly;
                }

                var announcementSound = new AnnouncementSound(soundFilePath, SoundRepeatTimes.Once, flightAction)
                {
                    LocationType = locationType
                };

                _announcementsMap.AddOrUpdate(flightAction,
                    new ConcurrentBag<AnnouncementSound> { announcementSound },
                    (_, announcements) =>
                    {
                        announcements.Add(announcementSound);
                        return announcements;
                    });
            }
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        /// <returns></returns>
        public async Task ProcessAnnouncementsAsync()
        {
            var entries = _scheduledAnnouncements.ToArray();

            foreach (var entry in entries)
            {
                var announcement = entry.Value;

                if (!announcement.IsEmpty && (announcement.FlightSound?.ConditionToPlay?.Invoke() ?? false))
                {
                    if (announcement.FlightSound.RepeatTimes == SoundRepeatTimes.Once)
                    {
                        _scheduledAnnouncements.TryRemove(entry.Key, out _);
                    }

                    await PlayFlightSoundAsync(announcement);
                }
            }
        }

        protected override void OnBeforeFlightSoundStartedPlaying(FlightSoundDescriptorPair currentPlayingSound)
        {
            AsyncContext.Run(async () => await Task.Delay(DelayToPlayAnnouncement));
            UpdateSoundAnnouncement(currentPlayingSound, _lastSnapshot);
        }

        protected override void OnAfterFlightSoundStartedPlaying(FlightSoundDescriptorPair currentPlayingSound)
        {
            if (currentPlayingSound.IsEmpty || currentPlayingSound.FlightSound is not AnnouncementSound announcement)
            {
                return;
            }

            if (announcement.FlightAction is FlightAction.SeatBeltsSignTurnedOff or FlightAction.SeatBeltsSignTurnedOn)
            {
                _lastPlayedSeatBeltsAnnouncement = announcement.FlightAction == FlightAction.SeatBeltsSignTurnedOff ?
                    SeatBeltsSignSwtichPosition.Off : SeatBeltsSignSwtichPosition.On;
            }
        }

        protected override void ProcessPlayingFlightSound(FlightSnapshot flightSnapshot)
        {
            _lastSeatBelts = flightSnapshot?.AircraftSeatBeltsSignSwitchPosition ?? SeatBeltsSignSwtichPosition.Off;

            UpdateSoundAnnouncement(_currentPlayingFlightSound, flightSnapshot);

            base.ProcessPlayingFlightSound(flightSnapshot);
        }

        protected FlightSoundDescriptorPair CreateAnnouncement(FlightSound flightSound)
        {
            var soundDescriptor = _soundManager.GetOrCreateSound(flightSound.FilePath);

            soundDescriptor.Volume = flightSound.Volume;

            return new FlightSoundDescriptorPair(soundDescriptor, flightSound);
        }

        protected IList<FlightSoundDescriptorPair> CreateAnnouncements(FlightAction flightAction)
        {
            var announcements = new List<FlightSoundDescriptorPair>();

            if (_announcementsMap.TryGetValue(flightAction, out var flightSounds))
            {
                foreach (var flightSound in flightSounds)
                {
                    announcements.Add(CreateAnnouncement(flightSound));
                }
            }

            return announcements;
        }

        private void UpdateSoundAnnouncement(FlightSoundDescriptorPair announcement, FlightSnapshot flightSnapshot)
        {
            if (!announcement.IsEmpty && flightSnapshot != null)
            {
                var flightSound = announcement.FlightSound;
                var soundDescriptor = announcement.SoundDescriptor;

                // The announcement is not played from a single source, so make it follow the camera position to sound equal everywhere
                soundDescriptor.Position = flightSnapshot?.CameraLocation?.Position ?? Vector3.Zero;
                soundDescriptor.Volume = FlightSoundsHelper.IsCameraExternal(flightSnapshot?.CameraLocation) ? 0f : flightSound.Volume;
                soundDescriptor.Occlusion = FlightSoundsHelper.GetOcclusion(flightSound.LocationType, _lastSnapshot?.CameraLocation,
                    _cockpitDoorPosition, flightSnapshot?.CockpitDoorState == DoorState.Open);
            }
        }

        private bool CanPlaySeatBeltsSignAnnouncement(SeatBeltsSignSwtichPosition seatBeltsSignSwtichPosition)
        {
            if (_lastSnapshot != null && !_lastSnapshot.IsAircraftOnGround && _lastSeatBelts != _lastPlayedSeatBeltsAnnouncement)
            {
                return seatBeltsSignSwtichPosition == _lastSnapshot.AircraftSeatBeltsSignSwitchPosition;
            }

            return false;
        }

        private bool CanPlayClimbingSeatBeltsOnAnnouncement()
        {
            return _flightStateService.CurrentFlightState == FlightState.Climb
                && _lastSnapshot?.AircraftIndicatedAltitute >= ClimbAnnouncementMinAltitude;
        }

        private bool CanPlayArrivedAtGateAnnouncement()
        {
            return _flightStateService.CurrentFlightState >= FlightState.TaxiIn
                && (_lastSnapshot?.EnginesRunningStatus.All(x => !x) ?? false);
        }

        private static FlightAction? GetFlightActionForFlightState(FlightState flightState)
        {
            return flightState switch
            {
                FlightState.Boarding => FlightAction.BoardingStarted,
                FlightState.BoardingDone => FlightAction.BoardingFinished,
                FlightState.Pushback => FlightAction.ReadyToPushback,
                FlightState.Climb => FlightAction.ClimbingSeatBeltsOn,
                FlightState.Cruise => FlightAction.LevelOff,
                FlightState.Descent => FlightAction.DescentStarted,
                FlightState.Approach => FlightAction.Approaching,
                FlightState.LandingDay => FlightAction.ClearedToLandDay,
                FlightState.LandingNight => FlightAction.ClearedToLandNight,
                FlightState.TaxiIn => FlightAction.TaxingToGate,
                FlightState.Unboarding => FlightAction.UnboardingStarted,
                FlightState.UnboardingDone => FlightAction.UnboardingFinished,
                _ => null,
            };
        }

        private async void OnFlightStateChanged(object sender, FlightStateChangedEventArgs args)
        {
            var newFlightState = args.NewFlightState;
            var flightAction = GetFlightActionForFlightState(newFlightState);

            if (flightAction != null)
            {
                var announcements = CreateAnnouncements(flightAction.Value);

                foreach (var announcement in announcements)
                {
                    await PlayFlightSoundAsync(announcement);
                }
            }
        }

        protected class AnnouncementSound : FlightSound
        {
            public AnnouncementSound(string filePath, SoundRepeatTimes repeatTimes, FlightAction flightAction) : base(filePath, repeatTimes)
            {
                FlightAction = flightAction;
            }

            public FlightAction FlightAction { get; }
        }
    }
}
