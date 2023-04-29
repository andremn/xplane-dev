namespace XPDev.Audio.Fmod
{
    internal class FmodSoundFacade : IFmodSoundFacade
    {
        private const int MaxSoundFileNameLength = 256;

        public FmodSoundFacade(FMOD.Sound fmodSound)
        {
            FmodSound = fmodSound;
        }

        public int LoopCount
        {
            get
            {
                FmodSound.getLoopCount(out var loopCount);

                return loopCount;
            }
            set => FmodSound.setLoopCount(value);
        }

        public FMOD.MODE Mode 
        { 
            get
            {
                FmodSound.getMode(out var mode);

                return mode;
            }
            set => FmodSound.setMode(value);
        }

        public string Name
        {
            get
            {
                FmodSound.getName(out var name, MaxSoundFileNameLength);

                return name;
            }
        }

        public FMOD.Sound FmodSound { get; }

        public void Get3DConeSettings(out float insideconeangle, out float outsideconeangle, out float outsidevolume)
        {
            FmodSound.get3DConeSettings(out insideconeangle, out outsideconeangle, out outsidevolume);
        }

        public void Get3DMinMaxDistance(out float min, out float max)
        {
            FmodSound.get3DMinMaxDistance(out min, out max);
        }

        public void Set3DConeSettings(float insideconeangle, float outsideconeangle, float outsidevolume)
        {
            FmodSound.set3DConeSettings(insideconeangle, outsideconeangle, outsidevolume);
        }

        public void Set3DMinMaxDistance(float min, float max)
        {
            FmodSound.set3DMinMaxDistance(min, max);
        }

        public void Release()
        {
            FmodSound.release();
        }
    }
}
