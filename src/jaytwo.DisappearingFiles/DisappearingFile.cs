using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace jaytwo.DisappearingFiles
{
    public class DisappearingFile
    {
        private const FileMode DefaultFileMode = FileMode.Create;

        public static DisappearingFileStream CreateTempFile()
            => Create(Path.GetTempFileName());

        public static DisappearingFileStream CreateTempFile(string extension)
            => CreateTempFileIn(Path.GetTempPath(), extension);

        public static DisappearingFileStream Create(string path)
            => new DisappearingFileStream(path, DefaultFileMode);

        public static DisappearingFileStream CreateTempFileIn(string basePath)
            => CreateTempFileIn(basePath, null);

        public static DisappearingFileStream CreateTempFileIn(string basePath, string extension)
            => new DisappearingFileStream(NameGenerator.GetFileNameWithExtension(basePath, extension), DefaultFileMode);

        public static DisappearingFileStream CreateTempFileIn(DirectoryInfo baseDirectory)
            => CreateTempFileIn(baseDirectory.FullName);

        public static DisappearingFileStream CreateTempFileIn(DirectoryInfo baseDirectory, string extension)
            => CreateTempFileIn(baseDirectory.FullName, extension);
    }
}
