using System;
using System.Collections.Concurrent;
using System.Numerics;
using XPDev.Foundation;

namespace XPDev.Audio.Fmod
{
    /// <summary>
    /// Represents a manager for FMOD sounds.
    /// </summary>
    internal class FmodSoundManager : Disposable, ISoundManager
    {
        private ConcurrentDictionary<string, CacheItem> _soundCache;
        private readonly Lazy<IFmodSystemFacade> _fmodSystemFacade;

        /// <summary>
        /// Initializes a new instance of the <see cref="FmodSoundManager"/> class with the specified <see cref="IFmodSystemFacade"/>.
        /// </summary>
        /// <param name="fmodSystemFacade">The <see cref="IFmodSystemFacade"/> to be used to communicate with the FMOD system.</param>
        public FmodSoundManager(IFmodSystemFacade fmodSystemFacade)
        {
            if (fmodSystemFacade == null)
            {
                throw new ArgumentNullException(nameof(fmodSystemFacade));
            }

            _soundCache = new ConcurrentDictionary<string, CacheItem>();
            _fmodSystemFacade = new Lazy<IFmodSystemFacade>(() =>
            {
                fmodSystemFacade.Initialize();
                return fmodSystemFacade;
            });
        }

        /// <summary>
        /// Gets or sets the position of the listener.
        /// </summary>
        public Vector3 ListenerPosition { get; set; }

        private IFmodSystemFacade FmodSystemFacade => _fmodSystemFacade.Value;

        /// <summary>
        /// Gets or creates a sound for the given file path and returns a descriptor for it.
        /// </summary>
        /// <param name="path">The path of the sound file to be created.</param>
        /// <returns>An instance of <see cref="ISoundDescriptor"/>.</returns>
        public ISoundDescriptor GetOrCreateSound(string path)
        {
            var fmodSoundDescriptor = _soundCache.AddOrUpdate(path, CreateCacheItem, 
                (soundPath, sound) => CreateCacheItem(soundPath));

            return fmodSoundDescriptor.Descriptor;
        }

        /// <summary>
        /// Updates all internal sounds based on the sound descriptors.
        /// </summary>
        public void Run()
        {
            var items = _soundCache.Values;

            foreach (var fmodDescriptor in items)
            {
                if (fmodDescriptor.IsInitialized && !fmodDescriptor.IsPlaying)
                {
                    if (_soundCache.TryRemove(fmodDescriptor.Descriptor.Path, out var descriptorToRemove))
                    {
                        descriptorToRemove.Sound?.Release();
                    }
                }
                else if (fmodDescriptor.IsInitialized)
                {
                    fmodDescriptor.Update();
                }
            }

            FmodSystemFacade.ListenerPosition = ListenerPosition;
            FmodSystemFacade.Update();
        }

        /// <summary>
        /// Plays the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to play the related sound.</param>
        public void PlaySound(ISoundDescriptor soundDescriptor)
        {
            PlaySound(soundDescriptor, SoundRepeatTimes.Once);
        }

        /// <summary>
        /// Plays the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to play the related sound.</param>
        /// <param name="repeatTimes">The repeat of the sound.</param>
        public void PlaySound(ISoundDescriptor soundDescriptor, SoundRepeatTimes repeatTimes)
        {
            if (_soundCache.TryGetValue(soundDescriptor.Path, out var item) && !item.IsPlaying)
            {
                if (item.Sound == null)
                {
                    var mode = repeatTimes > SoundRepeatTimes.Once ? FMOD.MODE._3D | FMOD.MODE.LOOP_NORMAL : FMOD.MODE._3D | FMOD.MODE.LOOP_OFF;

                    item.Sound = FmodSystemFacade.CreateSound(item.Descriptor.Path, mode);
                    item.Sound.Set3DMinMaxDistance(100, 100);
                }

                item.Sound.LoopCount = repeatTimes.NumberOfRepeats;
                item.Channel = FmodSystemFacade.PlaySound(item.Sound, paused: true);
                item.Update();
                item.Channel.Paused = false;
            }
        }

        /// <summary>
        /// Resumes playing the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to resume the related sound.</param>
        public void ResumeSound(ISoundDescriptor soundDescriptor)
        {
            if (_soundCache.TryGetValue(soundDescriptor.Path, out var item) && item.Channel != null)
            {
                item.Channel.Paused = false;
            }
        }

        /// <summary>
        /// Pauses the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to pause the related sound.</param>
        public void PauseSound(ISoundDescriptor soundDescriptor)
        {
            if (_soundCache.TryGetValue(soundDescriptor.Path, out var item) && item.Channel != null)
            {
                item.Channel.Paused = true;
            }
        }

        /// <summary>
        /// Stops the sound specified by the provided descriptor.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to be used to stop the related sound.</param>
        public void StopSound(ISoundDescriptor soundDescriptor)
        {
            if (_soundCache.TryGetValue(soundDescriptor.Path, out var item))
            {
                item.Channel?.Stop();
            }
        }

        /// <summary>
        /// Gets whether the specified sound is playing.
        /// </summary>
        /// <param name="soundDescriptor">The descriptor to verify if the associated sound is playing.</param>
        /// <returns>True if the sound is playing; false otherwise.</returns>
        public bool IsPlaying(ISoundDescriptor soundDescriptor)
        {
            return _soundCache.TryGetValue(soundDescriptor.Path, out var item) && item.IsPlaying;
        }

        /// <summary>
        /// Releases resources used by this <see cref="FmodSoundManager"/>.
        /// </summary>
        /// <param name="disposing">Indicates whether the method was called by user code (true) or by a finalizer (false).</param>
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (_soundCache != null)
                {
                    _soundCache.Clear();
                    _soundCache = null;
                }
            }
        }

        private CacheItem CreateCacheItem(string path)
        {
            return new CacheItem { Descriptor = new SoundDescriptor(path) };
        }

        private class CacheItem
        {
            public IFmodSoundFacade Sound { get; set; }

            public IFmodChannelFacade Channel { get; set; }

            public SoundDescriptor Descriptor { get; set; }

            public bool IsInitialized => Sound != null && Channel != null && Descriptor != null;

            public bool IsPlaying => Channel?.IsPlaying ?? false;

            public void Update()
            {
                if (Channel != null && Sound != null && Descriptor != null)
                {
                    Channel.Volume = Descriptor.Volume / 100f;

                    if (Sound.Mode.HasFlag(FMOD.MODE._3D))
                    {
                        Channel.Position3d = Descriptor.Position;
                        Channel.Occlusion = Descriptor.Occlusion;
                    }
                }
            }
        }
    }
}
