using System;
using System.IO;
using System.Security;
using jaytwo.SuperDelete;

namespace jaytwo.DisappearingFiles
{
    public class DisappearingFileStream : FileStream
    {
        private const FileMode DefaultFileMode = FileMode.Open;
        private const FileShare DefaultFileShare = FileShare.None;
        private const int DefaultBufferSize = 8192;
        private const bool DefaultUseAsync = false;

        private bool _disposed = false;

        [SecuritySafeCritical]
        public DisappearingFileStream(FileInfo file)
            : this(file.FullName)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(FileInfo file, FileMode mode)
            : this(file.FullName, mode)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(FileInfo file, FileMode mode, FileAccess access)
            : this(file.FullName, mode, access)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(FileInfo file, FileMode mode, FileAccess access, FileShare share)
            : this(file.FullName, mode, access, share)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(FileInfo file, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            : this(file.FullName, mode, access, share, bufferSize)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(FileInfo file, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            : base(file.FullName, mode, access, share, bufferSize, useAsync)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(FileInfo file, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            : base(file.FullName, mode, access, share, bufferSize, options)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path)
            : this(path, DefaultFileMode, GetDefaultFileAccess(DefaultFileMode), DefaultFileShare, DefaultBufferSize, DefaultUseAsync)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode)
            : this(path, mode, GetDefaultFileAccess(mode), DefaultFileShare, DefaultBufferSize, DefaultUseAsync)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access)
            : this(path, mode, access, DefaultFileShare, DefaultBufferSize, DefaultUseAsync)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access, FileShare share)
            : this(path, mode, access, share, DefaultBufferSize, DefaultUseAsync)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize)
            : this(path, mode, access, share, bufferSize, DefaultUseAsync)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, bool useAsync)
            : base(path, mode, access, share, bufferSize, GetDefaultFileOptions(useAsync) | FileOptions.DeleteOnClose)
        {
        }

        [SecuritySafeCritical]
        public DisappearingFileStream(string path, FileMode mode, FileAccess access, FileShare share, int bufferSize, FileOptions options)
            : base(path, mode, access, share, bufferSize, options | FileOptions.DeleteOnClose)
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

        private static FileAccess GetDefaultFileAccess(FileMode mode) => mode == FileMode.Append ? FileAccess.Write : FileAccess.ReadWrite;

        private static FileOptions GetDefaultFileOptions(bool useAsync) => useAsync ? FileOptions.Asynchronous : FileOptions.None;
    }
}
