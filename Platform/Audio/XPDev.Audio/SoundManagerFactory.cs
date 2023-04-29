using System;
using XPDev.Audio.Fmod;

namespace XPDev.Audio
{
    /// <summary>
    /// Creates instances of <see cref="ISoundManager"/>.
    /// </summary>
    public class SoundManagerFactory : ISoundManagerFactory
    {
        /// <summary>
        /// Creates a new instance of a <see cref="ISoundManager"/> based on the specified <see cref="SoundManagerEngine"/>.
        /// </summary>
        /// <param name="engine">The engine the <see cref="ISoundManager"/> will use.</param>
        /// <returns></returns>
        public ISoundManager CreateSoundManager(SoundManagerEngine engine)
        {
            return engine switch
            {
                SoundManagerEngine.Fmod => new FmodSoundManager(new FmodSystemFacade(32)),
                _ => throw new ArgumentOutOfRangeException(nameof(engine)),
            };
        }
    }
}
