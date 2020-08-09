using System;
using System.IO;
using Xunit;

namespace jaytwo.DisappearingFiles.Tests
{
    public class FileInfoExtensionsTests
    {
        [Fact]
        public void OpenDisappearingFileStream_opens_the_file_specified()
        {
            // arrange
            var fileInfo = new FileInfo(Path.GetTempFileName());

            // act
            using (var stream = fileInfo.OpenDisappearingFileStream())
            {
                // assert
                Assert.Equal(fileInfo.FullName, stream.Name);
            }

            Assert.False(File.Exists(fileInfo.FullName));
        }

        [Fact]
        public void OpenDisappearingFileStream_FileMode_Create_opens_the_file_specified()
        {
            // arrange
            var fileInfo = new FileInfo(Path.GetTempFileName());

            // act
            using (var stream = fileInfo.OpenDisappearingFileStream(FileMode.Create))
            {
                // assert
                Assert.Equal(fileInfo.FullName, stream.Name);
            }

            Assert.False(File.Exists(fileInfo.FullName));
        }

        [Fact]
        public void OpenDisappearingFileStream_FileMode_Create_FileAccess_ReadWrite_opens_the_file_specified()
        {
            // arrange
            var fileInfo = new FileInfo(Path.GetTempFileName());

            // act
            using (var stream = fileInfo.OpenDisappearingFileStream(FileMode.Create, FileAccess.ReadWrite))
            {
                // assert
                Assert.Equal(fileInfo.FullName, stream.Name);
            }

            Assert.False(File.Exists(fileInfo.FullName));
        }

        [Fact]
        public void OpenDisappearingFileStream_FileMode_Create_FileAccess_ReadWrite_FileShare_ReadWrite_opens_the_file_specified()
        {
            // arrange
            var fileInfo = new FileInfo(Path.GetTempFileName());

            // act
            using (var stream = fileInfo.OpenDisappearingFileStream(FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                // assert
                Assert.Equal(fileInfo.FullName, stream.Name);
            }

            Assert.False(File.Exists(fileInfo.FullName));
        }
    }
}
