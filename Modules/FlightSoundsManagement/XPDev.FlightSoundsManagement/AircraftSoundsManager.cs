using Nito.AsyncEx;
using System.Numerics;
using System.Threading.Tasks;
using XPDev.Audio;
using XPDev.FlightManagement;

namespace XPDev.FlightSoundsManagement
{
    /// <summary>
    /// Manages announcements logic.
    /// </summary>
    internal abstract class AircraftSoundsManager
    {
        protected readonly ISoundManager _soundManager;
        private readonly AsyncLock _asyncLock;
        private readonly AsyncAutoResetEvent _currentPlayingSoundResetEvent;

        protected FlightSoundDescriptorPair _currentPlayingFlightSound;
        protected FlightSnapshot _lastSnapshot;
        protected bool _lastPausedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="AircraftSoundsManager"/> class with the specified <see cref="ISoundManager"/>.
        /// </summary>
        /// <param name="soundManager">An instance of the <see cref="ISoundManager"/> interface.</param>
        public AircraftSoundsManager(ISoundManager soundManager)
        {
            _soundManager = soundManager;
            _asyncLock = new AsyncLock();
            _currentPlayingSoundResetEvent = new AsyncAutoResetEvent(false);
        }

        /// <summary>
        /// <inheritdoc/>
        /// </summary>
        public void ProcessFlightSnapshot(FlightSnapshot flightSnapshot)
        {
            _soundManager.Run();

            _lastSnapshot = flightSnapshot?.Clone();
            _soundManager.ListenerPosition = _lastSnapshot?.CameraLocation?.Position ?? Vector3.Zero;

            ProcessPlayingFlightSound(flightSnapshot);
        }

        protected async Task PlayFlightSoundAsync(FlightSoundDescriptorPair descriptorPair)
        {
            if (!descriptorPair.IsEmpty)
            {
                using (await _asyncLock.LockAsync())
                {
                    OnBeforeFlightSoundStartedPlaying(descriptorPair);
                    _currentPlayingFlightSound = descriptorPair;
                    _soundManager.PlaySound(descriptorPair.SoundDescriptor, descriptorPair.FlightSound.RepeatTimes);
                    OnAfterFlightSoundStartedPlaying(descriptorPair);
                    await _currentPlayingSoundResetEvent.WaitAsync();
                }
            }
        }

        protected virtual void ProcessPlayingFlightSound(FlightSnapshot flightSnapshot)
        {
            if (!_currentPlayingFlightSound.IsEmpty)
            {
                if (!_soundManager.IsPlaying(_currentPlayingFlightSound.SoundDescriptor))
                {
                    _currentPlayingFlightSound = FlightSoundDescriptorPair.Empty;
                    // Notifies the current sound finished playing
                    _currentPlayingSoundResetEvent.Set();
                }
                else
                {
                    var isPaused = flightSnapshot?.IsPaused ?? false;

                    if (isPaused && !_lastPausedValue)
                    {
                        _lastPausedValue = true;
                        _soundManager.PauseSound(_currentPlayingFlightSound.SoundDescriptor);
                    }
                    else if (!isPaused && _lastPausedValue)
                    {
                        _lastPausedValue = false;
                        _soundManager.ResumeSound(_currentPlayingFlightSound.SoundDescriptor);
                    }
                }
            }
        }

        protected virtual void OnBeforeFlightSoundStartedPlaying(FlightSoundDescriptorPair currentPlayingSound)
        {
        }

        protected virtual void OnAfterFlightSoundStartedPlaying(FlightSoundDescriptorPair currentPlayingSound)
        {
        }
    }
}
