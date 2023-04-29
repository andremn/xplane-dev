using System.Numerics;
using static XPDev.FlightSoundsManagement.Sound.Fmod.ErrorHelper;

namespace XPDev.Audio.Fmod
{
    internal class FmodChannelFacade : IFmodChannelFacade
    {
        private readonly FMOD.Channel _fmodChannel;

        public FmodChannelFacade(FMOD.Channel fmodChannel)
        {
            _fmodChannel = fmodChannel;
        }

        public int LoopCount
        {
            get
            {
                ThrowIfResultIsError(_fmodChannel.getLoopCount(out var loopCount));

                return loopCount;
            }
            set => ThrowIfResultIsError(_fmodChannel.setLoopCount(value));
        }

        public IFmodSoundFacade CurrentSound
        {
            get
            {
                ThrowIfResultIsError(_fmodChannel.getCurrentSound(out var currentSound));

                return new FmodSoundFacade(currentSound);
            }
        }

        public bool IsPlaying
        {
            get
            {
                ThrowIfResultIsError(_fmodChannel.isPlaying(out var isPlaying));

                return isPlaying;
            }
        }

        public float Volume
        {
            get
            {
                ThrowIfResultIsError(_fmodChannel.getVolume(out var volume));

                return volume;
            }
            set => ThrowIfResultIsError(_fmodChannel.setVolume(value));
        }

        public bool Paused
        {
            get
            {
                ThrowIfResultIsError(_fmodChannel.getPaused(out var isPaused));

                return isPaused;
            }
            set => ThrowIfResultIsError(_fmodChannel.setPaused(value));
        }

        public float Occlusion 
        { 
            get
            {
                ThrowIfResultIsError(_fmodChannel.get3DOcclusion(out var occlusion, out var _));

                return occlusion;
            }
            set => ThrowIfResultIsError(_fmodChannel.set3DOcclusion(value, value));
        }

        public Vector3 Position3d
        {
            get
            {
                ThrowIfResultIsError(_fmodChannel.get3DAttributes(out var position3d, out var _));

                return new Vector3(position3d.x, position3d.y, position3d.z);
            }
            set
            {
                var fmodPosition3d = new FMOD.VECTOR { x = value.X, y = value.Y, z = value.Z };
                var fmodZeroVector = new FMOD.VECTOR();

                ThrowIfResultIsError(_fmodChannel.set3DAttributes(ref fmodPosition3d, ref fmodZeroVector));
            }
        }

        public void Stop()
        {
            ThrowIfResultIsError(_fmodChannel.stop());
        }
    }
}
