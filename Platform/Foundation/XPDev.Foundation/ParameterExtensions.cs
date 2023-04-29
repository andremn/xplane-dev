using System;

namespace XPDev.Foundation
{
    /// <summary>
    /// Provides extension methods to be used with method parameters.
    /// </summary>
    public static class ParameterExtensions
    {
        /// <summary>
        /// Provides a way to remove warning for not used parameters. This method does nothing.
        /// </summary>
        /// <param name="parameter">The parameter to be ignored.</param>
#pragma warning disable IDE0060 // Remove unused parameter
        public static void NotUsed(this object parameter)
#pragma warning restore IDE0060 // Remove unused parameter
        {
            // Do nothing
        }

        /// <summary>
        /// Throws an <see cref="ArgumentNullException"/> if the specified parameter is null.
        /// </summary>
        /// <param name="parameter">The parameter to check for null.</param>
        /// <param name="exceptionMessage">The message to be passed to the exception.</param>
        /// <returns>The instance for <paramref name="parameter"/>.</returns>
        public static T NotNull<T>(this T parameter, string exceptionMessage = "")
        {
            if (parameter == null)
            {
                throw new ArgumentNullException(nameof(parameter), exceptionMessage);
            }

            return parameter;
        }
    }
}
