using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Xunit;

namespace jaytwo.DisappearingFiles.Tests
{
    public class DisappearingFileStreamTests
    {
        [Fact]
        public void File_does_exist_before_stream_is_disposed()
        {
            // arrange

            // act
            using (var stream = DisappearingFile.CreateTempFile())
            {
                // assert
                Assert.True(File.Exists(stream.Name));
            }
        }

        [Fact]
        public void File_does_not_exist_after_stream_is_disposed()
        {
            // arrange
            var stream = DisappearingFile.CreateTempFile();

            // act
            stream.Dispose();

            // assert
            Assert.False(File.Exists(stream.Name));
        }

        [Fact]
        public void FileMode_Open()
        {
            // arrange
            var tempFile = Path.GetTempFileName();
            var stream = new DisappearingFileStream(tempFile, FileMode.Open);

            // act
            stream.Dispose();

            // assert
            Assert.False(File.Exists(stream.Name));
        }

        [Fact]
        public void FileMode_OpenOrCreate()
        {
            // arrange
            var tempFile = Path.GetTempFileName();
            var stream = new DisappearingFileStream(tempFile, FileMode.OpenOrCreate);

            // act
            stream.Dispose();

            // assert
            Assert.False(File.Exists(stream.Name));
        }

        [Fact]
        public void FileMode_OpenOrCreate_FileAccess_Read()
        {
            // arrange
            var tempFile = Path.GetTempFileName();
            var bytes = new byte[] { 1, 2, 3 };

            using (var stream = new DisappearingFileStream(tempFile, FileMode.Open, FileAccess.Read))
            {
                // act && assert
                var exception = Assert.Throws<NotSupportedException>(() => stream.Write(bytes, 0, bytes.Length));
            }

            Assert.False(File.Exists(tempFile));
        }

        [Fact]
        public void FileMode_OpenOrCreate_FileAccess_ReadWrite()
        {
            // arrange
            var tempFile = Path.GetTempFileName();
            var bytes = new byte[] { 1, 2, 3 };

            using (var stream = new DisappearingFileStream(tempFile, FileMode.Open, FileAccess.ReadWrite))
            {
                // act
                stream.Write(bytes, 0, bytes.Length);
                stream.Position = 0;
                stream.ReadByte();
            }

            // assert
            Assert.False(File.Exists(tempFile));
        }
    }
}
