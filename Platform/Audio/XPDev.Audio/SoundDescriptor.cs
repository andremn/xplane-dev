using System.Numerics;

namespace XPDev.Audio
{
    internal class SoundDescriptor : ISoundDescriptor
    {
        public SoundDescriptor(string path)
        {
            Path = path;
        }

        public string Path { get; }

        public float Volume { get; set; }

        public bool IsLoopEnabled { get; set; }

        public Vector3 Position { get; set; }

        public float Occlusion { get; set; }
    }
}
