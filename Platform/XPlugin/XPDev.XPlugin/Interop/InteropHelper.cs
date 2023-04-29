using System;
using System.Text;

namespace XPDev.XPlugin.Interop
{
    /// <summary>
    /// Helper methods to interop operations.
    /// </summary>
    public unsafe static class InteropHelper
    {
        /// <summary>
        /// Copies the given .Net string into the given byte buffer as a null-terminated ASCII C-string.
        /// </summary>
        /// <param name="destinationPointer">The pointer to the destination buffer.</param>
        /// <param name="source">The string to be used to read the bytes from and copy to the destination buffer.</param>
        public static void CopyString(IntPtr destinationPointer, string source)
        {
            CopyToCString((byte*)destinationPointer.ToPointer(), source);
        }

        /// <summary>
        /// Copies the given .Net string into the given byte buffer as a null-terminated ASCII C-string.
        /// </summary>
        /// <param name="destinationPointer">The pointer to the destination buffer.</param>
        /// <param name="source">The string to be used to read the bytes from and copy to the destination buffer.</param>
        public static void CopyToCString(byte* destinationPointer, string source)
        {
            var stringBytes = Encoding.ASCII.GetBytes(source);
            var numberOfBytesToCopy = stringBytes.Length;

            fixed (byte* sourcePointerStart = &stringBytes[0])
            {
                var sourcePointer = sourcePointerStart;

                for (var i = 0; i < numberOfBytesToCopy; ++i, ++destinationPointer, ++sourcePointer)
                {
                    *destinationPointer = *sourcePointer;
                }

                *destinationPointer = 0;
            }
        }
    }
}
