using System.Numerics;

namespace XPDev.Audio.Fmod
{
    internal interface IFmodChannelFacade
    {
        int LoopCount { get; set; }

        IFmodSoundFacade CurrentSound { get; }

        bool IsPlaying { get; }

        float Volume { get; set; }

        float Occlusion { get; set; }

        bool Paused { get; set; }

        Vector3 Position3d { get; set; }

        void Stop();
    }
}
