namespace XPDev.Audio.Fmod
{
    internal interface IFmodSoundFacade
    {
        int LoopCount { get; set; }

        FMOD.MODE Mode { get; set; }

        string Name { get; }

        FMOD.Sound FmodSound { get; }

        void Get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume);

        void Get3DMinMaxDistance(out float min, out float max);

        void Set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume);

        void Set3DMinMaxDistance(float min, float max);

        void Release();
    }
}
