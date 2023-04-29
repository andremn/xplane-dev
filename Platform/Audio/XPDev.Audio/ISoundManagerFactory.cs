namespace XPDev.Audio
{
    /// <summary>
    /// Creates instances of <see cref="ISoundManager"/>.
    /// </summary>
    internal interface ISoundManagerFactory
    {
        /// <summary>
        /// Creates a new instance of a <see cref="ISoundManager"/> based on the specified <see cref="SoundManagerEngine"/>.
        /// </summary>
        /// <param name="engine">The engine the <see cref="ISoundManager"/> will use.</param>
        /// <returns></returns>
        ISoundManager CreateSoundManager(SoundManagerEngine engine);
    }
}