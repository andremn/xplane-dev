using System.Numerics;

namespace XPDev.Audio.Fmod
{
    internal interface IFmodSystemFacade
    {
        Vector3 ListenerPosition { get; set; }

        void Initialize();

        IFmodSoundFacade CreateSound(string filename, FMOD.MODE mode = FMOD.MODE.DEFAULT);

        IFmodChannelFacade PlaySound(IFmodSoundFacade sound, bool paused = false);

        void Update();
    }
}
