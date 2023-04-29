using System;

namespace XPDev.FlightSoundsManagement.Sound.Fmod
{
    internal class FmodException : Exception
    {
        public FmodException(FMOD.RESULT errorCode) : base($"Error code {errorCode}")
        {
            ErrorCode = errorCode;
        }

        public FMOD.RESULT ErrorCode { get; set; }
    }

    internal static class ErrorHelper
    {
        public static void ThrowIfResultIsError(FMOD.RESULT fmodResult)
        {
            if (fmodResult != FMOD.RESULT.OK)
            {
                throw new FmodException(fmodResult);
            }
        }
    }
}
