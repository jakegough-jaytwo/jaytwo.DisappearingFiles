using System;
using System.IO;

namespace jaytwo.DisappearingFiles
{
    public static class FileInfoExtensions
    {
        public static DisappearingFileStream OpenDisappearingFileStream(this FileInfo fileInfo)
            => new DisappearingFileStream(fileInfo);

        public static DisappearingFileStream OpenDisappearingFileStream(this FileInfo fileInfo, FileMode fileMode)
            => new DisappearingFileStream(fileInfo, fileMode);

        public static DisappearingFileStream OpenDisappearingFileStream(this FileInfo fileInfo, FileMode fileMode, FileAccess fileAccess)
            => new DisappearingFileStream(fileInfo, fileMode, fileAccess);

        public static DisappearingFileStream OpenDisappearingFileStream(this FileInfo fileInfo, FileMode fileMode, FileAccess fileAccess, FileShare fileShare)
            => new DisappearingFileStream(fileInfo, fileMode, fileAccess, fileShare);
    }
}
