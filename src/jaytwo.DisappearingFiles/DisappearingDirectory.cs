using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using jaytwo.SuperDelete;

namespace jaytwo.DisappearingFiles
{
    public class DisappearingDirectory : IDisposable
    {
        private bool _disposed = false;

        internal DisappearingDirectory(DirectoryInfo directoryInfo)
        {
            // passing through DirectoryInfo to ensure a clean path name
            Path = directoryInfo.FullName;
        }

        public string Path { get; }

        public static DisappearingDirectory Create() => CreateIn(System.IO.Path.GetTempPath());

        public static DisappearingDirectory CreateIn(DirectoryInfo directoryInfo) => CreateIn(directoryInfo.FullName);

        public static DisappearingDirectory CreateIn(string basePath)
        {
            var directoryInfo = DirectoryGenerator.CreateNewDirectory(() =>
            {
                var randomName = NameGenerator.GenerateRandomString();
                return System.IO.Path.Combine(basePath, randomName);
            });

            return new DisappearingDirectory(directoryInfo);
        }

        public FileInfo CreateNewFile()
            => CreateNewFileWithExtension(".tmp");

        public FileInfo CreateNewFile(string name)
            => FileGenerator.CreateNewFile(System.IO.Path.Combine(Path, name));

        public FileInfo CreateNewFile(string prefix, string suffix)
            => FileGenerator.CreateNewFile(() => GenerateRandomName(prefix, suffix));

        public FileInfo CreateNewFileWithExtension(string extension)
            => FileGenerator.CreateNewFile(() => GenerateRandomNameWithExtension(extension));

        public Task<FileInfo> WriteToNewFileAsync(Stream stream)
            => WriteToNewFileAsync(stream, () => CreateNewFile());

        public Task<FileInfo> WriteToNewFileAsync(Stream stream, string name)
            => WriteToNewFileAsync(stream, () => CreateNewFile(name));

        public Task<FileInfo> WriteToNewFileAsync(Stream stream, string prefix, string suffix)
            => WriteToNewFileAsync(stream, () => CreateNewFile(prefix, suffix));

        public Task<FileInfo> WriteToNewFileWithExtensionAsync(Stream stream, string extension)
            => WriteToNewFileAsync(stream, () => CreateNewFileWithExtension(extension));

        public Task<FileInfo> WriteToNewFileAsync(byte[] bytes)
            => WriteToNewFileAsync(bytes, () => CreateNewFile());

        public Task<FileInfo> WriteToNewFileAsync(byte[] bytes, string name)
            => WriteToNewFileAsync(bytes, () => CreateNewFile(name));

        public Task<FileInfo> WriteToNewFileAsync(byte[] bytes, string prefix, string suffix)
            => WriteToNewFileAsync(bytes, () => CreateNewFile(prefix, suffix));

        public Task<FileInfo> WriteToNewFileWithExtensionAsync(byte[] bytes, string extension)
            => WriteToNewFileAsync(bytes, () => CreateNewFileWithExtension(extension));

        public Task<FileInfo> WriteToNewFileAsync(string text)
            => WriteToNewFileAsync(text, () => CreateNewFile());

        public Task<FileInfo> WriteToNewFileAsync(string text, string name)
            => WriteToNewFileAsync(text, () => CreateNewFile(name));

        public Task<FileInfo> WriteToNewFileAsync(string text, string prefix, string suffix)
            => WriteToNewFileAsync(text, () => CreateNewFile(prefix, suffix));

        public Task<FileInfo> WriteToNewFileWithExtensionAsync(string text, string extension)
            => WriteToNewFileAsync(text, () => CreateNewFileWithExtension(extension));

        public DirectoryInfo CreateNewSubdirectory()
            => CreateNewSubdirectory("dir.", null);

        public DirectoryInfo CreateNewSubdirectory(string name)
            => DirectoryGenerator.CreateNewDirectory(System.IO.Path.Combine(Path, name));

        public DirectoryInfo CreateNewSubdirectory(string prefix, string suffix)
            => DirectoryGenerator.CreateNewDirectory(() => GenerateRandomName(prefix, suffix));

        public DirectoryInfo CreateSubdirectoryWithPrefix(string prefix)
            => CreateNewSubdirectory(prefix, null);

        public string GetFullPath(string path) => System.IO.Path.Combine(Path, path);

        public FileInfo[] GetFiles() =>
            new DirectoryInfo(Path).GetFiles();

        public FileInfo[] GetFiles(string searchPattern) =>
            new DirectoryInfo(Path).GetFiles(searchPattern);

        public FileInfo[] GetFiles(string searchPattern, SearchOption searchOption) =>
            new DirectoryInfo(Path).GetFiles(searchPattern, searchOption);

        public DirectoryInfo[] GetDirectories() =>
            new DirectoryInfo(Path).GetDirectories();

        public DirectoryInfo[] GetDirectories(string searchPattern) =>
            new DirectoryInfo(Path).GetDirectories(searchPattern);

        public DirectoryInfo[] GetDirectories(string searchPattern, SearchOption searchOption) =>
            new DirectoryInfo(Path).GetDirectories(searchPattern, searchOption);

        public string GenerateRandomNameWithExtension(string extension)
        {
            if (!extension.StartsWith("."))
            {
                extension = "." + extension;
            }

            return GenerateRandomName(prefix: null, suffix: extension);
        }

        public string GenerateRandomName(string prefix, string suffix)
            => System.IO.Path.Combine(Path, NameGenerator.GenerateRandomString(prefix, suffix));

        public void Dispose() => Dispose(true);

        // If disposing equals true:
        //   the method has been called directly or indirectly by a user's code. Managed and unmanaged resources can be disposed.
        // If disposing equals false:
        //   the method has been called by the runtime from inside the finalizer, and you should not reference other objects. Only unmanaged resources can be disposed.
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                SuperDeleter.SuperDelete(Path);
            }
            catch
            {
            }
        }

        private async Task<FileInfo> WriteToNewFileAsync(Stream stream, Func<FileInfo> createFileDelegate)
        {
            var newFile = createFileDelegate.Invoke();

            using (stream)
            using (var fileStream = newFile.Open(FileMode.Create))
            {
                await stream.CopyToAsync(fileStream);
            }

            return newFile;
        }

        private async Task<FileInfo> WriteToNewFileAsync(byte[] bytes, Func<FileInfo> createFileDelegate)
        {
            var newFile = createFileDelegate.Invoke();

            using (var fileStream = newFile.Open(FileMode.Create))
            {
                await fileStream.WriteAsync(bytes, 0, bytes.Length);
            }

            return newFile;
        }

        private async Task<FileInfo> WriteToNewFileAsync(string text, Func<FileInfo> createFileDelegate)
        {
            var newFile = createFileDelegate.Invoke();

            using (var fileStream = newFile.Open(FileMode.Create))
            using (var writer = new StreamWriter(fileStream))
            {
                await writer.WriteAsync(text);
            }

            return newFile;
        }
    }
}
