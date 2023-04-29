using System.Numerics;

namespace XPDev.FlightManagement
{
    /// <summary>
    /// Represents the location of the camera.
    /// </summary>
    public class CameraLocation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CameraLocation"/> class.
        /// </summary>
        public CameraLocation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraLocation"/> class with the specified position and whether the camera is external.
        /// </summary>
        /// <param name="position">The position of the camera.</param>
        /// <param name="isExternal">Whether the camera is external view.</param>
        public CameraLocation(Vector3 position, bool isExternal)
        {
            Position = position;
            IsExternal = isExternal;
        }

        /// <summary>
        /// Gets or sets the position of the camera.
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets whether the camera is external.
        /// </summary>
        public bool IsExternal { get; set; }
    }
}
