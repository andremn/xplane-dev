using System;
using System.Numerics;
using static XPDev.FlightSoundsManagement.Sound.Fmod.ErrorHelper;

namespace XPDev.Audio.Fmod
{
    internal class FmodSystemFacade : IFmodSystemFacade
    {
        private const int MaxDefaultChannels = 32;

        private readonly int _maxChannels;

        private FMOD.System _fmodSystem;
        private bool _isInitialized;

        public FmodSystemFacade(int maxChannels = MaxDefaultChannels)
        {
            _maxChannels = maxChannels;
        }

        public Vector3 ListenerPosition
        {
            get
            {
                ThrowIfNotInitialized();
                ThrowIfResultIsError(_fmodSystem.get3DListenerAttributes(0, out var position, out var _, out var _, out var _));

                return new Vector3(position.x, position.y, position.z);
            }
            set
            {
                ThrowIfNotInitialized();

                var fmodVector = new FMOD.VECTOR { x = value.X, y = value.Y, z = value.Z };
                var fmodZeroVector = new FMOD.VECTOR();
                var fmodForwardVector = new FMOD.VECTOR { z = 1 };
                var fmodUpVector = new FMOD.VECTOR { y = 1 };

                ThrowIfResultIsError(_fmodSystem.set3DListenerAttributes(0, ref fmodVector, ref fmodZeroVector, ref fmodForwardVector, ref fmodUpVector));
            }
        }

        public void Initialize()
        {
            if (!_isInitialized)
            {
                ThrowIfResultIsError(FMOD.Factory.System_Create(out _fmodSystem));
                ThrowIfResultIsError(_fmodSystem.init(_maxChannels, FMOD.INITFLAGS.NORMAL, IntPtr.Zero));

                _isInitialized = true;
            }
        }

        public IFmodSoundFacade CreateSound(string filename, FMOD.MODE mode = FMOD.MODE.DEFAULT)
        {
            ThrowIfNotInitialized();

            ThrowIfResultIsError(_fmodSystem.createSound(filename, FMOD.MODE._3D, out var fmodSound));

            return new FmodSoundFacade(fmodSound);
        }

        public IFmodChannelFacade PlaySound(IFmodSoundFacade sound, bool paused = false)
        {
            ThrowIfNotInitialized();
            ThrowIfResultIsError(_fmodSystem.playSound(sound.FmodSound, new FMOD.ChannelGroup(), paused, out var fmodChannel));

            return new FmodChannelFacade(fmodChannel);
        }

        public void Update()
        {
            ThrowIfNotInitialized();
            ThrowIfResultIsError(_fmodSystem.update());
        }

        private void ThrowIfNotInitialized()
        {
            if (!_isInitialized)
            {
                throw new InvalidOperationException($"Fmod is not initialize. Call '{nameof(Initialize)}' to initialize it.");
            }
        }
    }
}
