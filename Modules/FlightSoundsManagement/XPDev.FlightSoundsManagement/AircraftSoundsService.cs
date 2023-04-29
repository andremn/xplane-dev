using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using XPDev.Audio;
using XPDev.FlightManagement;
using XPDev.FlightSoundsManagement.Configuration;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Manages aircraft sounds.
    /// </summary>
    internal class AircraftSoundsService : FlightSoundsServiceBase, IAircraftSoundsService
    {
        private readonly ConcurrentDictionary<FlightSound, ISoundDescriptor> _aircraftSounds;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftSoundsService"/> class with the specified 
        /// <see cref="IFlightSnapshotProvider"/>, <see cref="ISoundManager"/>, and <see cref="IFlightSoundConfiguration"/>.
        /// </summary>
        /// <param name="snapshotProvider">The <see cref="IFlightSnapshotProvider"/> to get flight info from.</param>
        /// <param name="soundManager">The <see cref="ISoundManager"/> to manage sounds.</param>
        /// <param name="soundConfigurationProvider">The <see cref="IFlightSoundConfiguration"/> provider to be used to create a <see cref="IFlightSoundConfiguration"/>.</param>
        public AircraftSoundsService(IFlightSnapshotProvider snapshotProvider, ISoundManager soundManager, IFlightSoundConfigurationProvider soundConfigurationProvider)
            : base(snapshotProvider, soundManager, soundConfigurationProvider.GetFlightSoundConfiguration())
        {
            _aircraftSounds = new ConcurrentDictionary<FlightSound, ISoundDescriptor>();
        }

        /// <summary>
        /// Adds the specified aircraft sound to be played when the condition is met.
        /// </summary>
        /// <param name="aircraftSound">The aircraft sound to be added.</param>
        public void AddAircraftSound(FlightSound aircraftSound)
        {
            if (aircraftSound == null)
            {
                throw new ArgumentNullException(nameof(aircraftSound));
            }

            _aircraftSounds[aircraftSound] = _soundManager.GetOrCreateSound(aircraftSound.FilePath);
        }

        /// <summary>
        /// Plays scheduled sounds.
        /// </summary>
        protected override Task OnUpdateAsync()
        {
            return Task.Run(() =>
            {
                var aircraftSounds = _aircraftSounds;

                foreach (var keyValuePair in aircraftSounds)
                {
                    var aircraftSound = keyValuePair.Key;
                    var soundDescriptor = keyValuePair.Value;
                    var isExternalView = _lastSnapshot?.CameraLocation?.IsExternal ?? false;

                    soundDescriptor.Volume = isExternalView ? 0f : aircraftSound.Volume;
                    soundDescriptor.Occlusion = aircraftSound.Occlusion;
                    soundDescriptor.Position = aircraftSound.Position;

                    _soundManager.PlaySound(soundDescriptor, aircraftSound.RepeatTimes);

                    if (aircraftSound.RepeatTimes == SoundRepeatTimes.Once)
                    {
                        aircraftSounds.TryRemove(aircraftSound, out var _);
                    }
                }
            });
        }
    }
}
