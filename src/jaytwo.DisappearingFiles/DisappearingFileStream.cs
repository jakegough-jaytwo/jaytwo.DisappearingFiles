using System;
using System.IO;
using System.Security;
using jaytwo.SuperDelete;

namespace jaytwo.DisappearingFiles
{
    public class DisappearingFileStream : FileStream
    {
        private bool _disposed = false;

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode)
            : base(path, mode)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access)
            : base(path, mode, access)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access, FileShare share)
            : base(path, mode, access, share)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            : base(path, mode, access, share, bufferSize)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            : base(path, mode, access, share, bufferSize, useAsync)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            : base(path, mode, access, share, bufferSize, options)
        {
        }

        // If disposing equals true:
        //   the method has been called directly or indirectly by a user's code. Managed and unmanaged resources can be disposed.
        // If disposing equals false:
        //   the method has been called by the runtime from inside the finalizer, and you should not reference other objects. Only unmanaged resources can be disposed.
        protected override void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            base.Dispose(disposing);
            _disposed = true;

            try
            {
                SuperDeleter.SuperDelete(Name);
            }
            catch
            {
            }
        }
    }
}
