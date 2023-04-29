using System;
using System.Diagnostics.CodeAnalysis;

namespace XPDev.Foundation
{
    /// <summary>
    /// Provides a default implementation for classes implementing the <see cref="IDisposable"/> pattern.
    /// </summary>
    public class Disposable : IDisposable
    {
        /// <summary>
        /// Indicates whether the object was already disposed.
        /// </summary>
        protected bool _isDisposed = false;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "The Dispose(bool) method is being called indirectly.")]
        public void Dispose()
        {
            InternalDispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// When overriden in a derived class, performs application-defined tasks associated with freeing, releasing, or resetting
        /// unmanaged resources.
        /// </summary>
        /// <param name="disposing">Indicates whether the method was called by user code (true) or by a finalizer (false).</param>
        protected virtual void Dispose(bool disposing)
        {
        }

        private void InternalDispose(bool disposing)
        {
            if (!_isDisposed)
            {
                Dispose(disposing);
            }
        }
    }
}
