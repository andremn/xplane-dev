using System.Numerics;
using XPDev.Audio;
using XPDev.FlightManagement;

namespace XPDev.FlightSoundsManagement.Helpers
{
    internal static class FlightSoundsHelper
    {
        /// <summary>
        /// Gets whether the specified <see cref="CameraLocation"/> is external.
        /// </summary>
        /// <param name="cameraLocation">The camera location.</param>
        /// <returns><see langword="true"/> if the specified <see cref="CameraLocation"/> is external; <see langword="false"/> otherwise.</returns>
        public static bool IsCameraExternal(CameraLocation cameraLocation)
        {
            return cameraLocation?.IsExternal ?? false;
        }

        /// <summary>
        /// Gets the occlusion of a sound bases on the specified location type.
        /// </summary>
        /// <param name="locationType">The location type of the sound to calculate the occlusion.</param>
        /// <param name="cameraLocation">The location of the camera.</param>
        /// <param name="cockpitDoorPosition">The position of the cockpit door.</param>
        /// <param name="isCockpitDoorOpened">Whether the cockpit door is open.</param>
        /// <returns>A float value representing the occlusion.</returns>
        public static float GetOcclusion(SoundLocationType locationType, CameraLocation cameraLocation, Vector3 cockpitDoorPosition, bool isCockpitDoorOpened)
        {
            var isListenerInsideCockpit = IsInsideCockpit(cameraLocation?.Position ?? Vector3.Zero, cockpitDoorPosition);
            var isSoundInsideCockpit = locationType == SoundLocationType.Both || locationType == SoundLocationType.CockpitOnly;

            if ((isListenerInsideCockpit && isSoundInsideCockpit) || (!isListenerInsideCockpit && !isSoundInsideCockpit))
            {
                return FlightSound.MinOcclusion;
            }

            if (!isListenerInsideCockpit && isSoundInsideCockpit)
            {
                return isCockpitDoorOpened ? FlightSound.MinOcclusion : FlightSound.MaxOcclusion;
            }

            if (isListenerInsideCockpit && !isSoundInsideCockpit)
            {
                return isCockpitDoorOpened ? FlightSound.MaxOcclusion * 0.2f : FlightSound.MaxOcclusion * 0.95f;
            }

            return FlightSound.MinOcclusion;
        }

        /// <summary>
        /// Gets whether the specified camera position is inside the cockpit.
        /// </summary>
        /// <param name="cameraPosition">The position to check.</param>
        /// <param name="cockpitDoorPosition">The position of the cockpit door.</param>
        /// <returns><see langword="true"/> if the specified position is inside the cockpit; <see langword="false"/> otherwise.</returns>
        public static bool IsInsideCockpit(Vector3 cameraPosition, Vector3 cockpitDoorPosition)
        {
            return cameraPosition.Z < cockpitDoorPosition.Z;
        }
    }
}
