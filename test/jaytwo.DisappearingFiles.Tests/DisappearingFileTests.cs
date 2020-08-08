using System;
using System.IO;
using Xunit;

namespace jaytwo.DisappearingFiles.Tests
{
    public class DisappearingFileTests
    {
        [Fact]
        public void CreateTempFile_creates_a_file_in_system_temp_path()
        {
            // arrange

            // act
            using (var stream = DisappearingFile.CreateTempFile())
            {
                // assert
                Assert.True(File.Exists(stream.Name));
                Assert.StartsWith(Path.GetTempPath(), stream.Name);
            }
        }

        [Fact]
        public void Create_creates_a_file_in_specified_location()
        {
            // arrange
            var randomOtherDirectory = new DirectoryInfo(".");
            var randomFullFileName = Path.Combine(randomOtherDirectory.FullName, Guid.NewGuid().ToString());

            // act
            using (var stream = DisappearingFile.Create(randomFullFileName))
            {
                // assert
                Assert.True(File.Exists(randomFullFileName));
                Assert.StartsWith(randomOtherDirectory.FullName, stream.Name);
            }

            Assert.False(File.Exists(randomFullFileName));
        }

        [Theory]
        [InlineData("jake", ".jake")]
        [InlineData(".jake", ".jake")]
        public void CreateTempFile_with_extension_creates_file_with_extension_in_system_temp_path(string extension, string expectedExtension)
        {
            // arrange
            string generatedFileName;

            // act
            using (var stream = DisappearingFile.CreateTempFile(extension))
            {
                generatedFileName = stream.Name;

                // assert
                Assert.StartsWith(Path.GetTempPath(), stream.Name);
                Assert.EndsWith(extension, stream.Name);
                Assert.Equal(expectedExtension, Path.GetExtension(stream.Name));
            }

            Assert.False(File.Exists(generatedFileName));
        }

        [Fact]
        public void CreateTempFileIn_creates_a_file_in_specified_location()
        {
            // arrange
            var randomOtherDirectory = new DirectoryInfo(".");
            string generatedFileName;

            // act
            using (var stream = DisappearingFile.CreateTempFileIn(randomOtherDirectory))
            {
                generatedFileName = stream.Name;

                // assert
                Assert.StartsWith(randomOtherDirectory.FullName, stream.Name);
            }

            Assert.False(File.Exists(generatedFileName));
        }

        [Theory]
        [InlineData("jake", ".jake")]
        [InlineData(".jake", ".jake")]
        public void CreateTempFileIn_with_extension_creates_file_with_extension_in_system_temp_path(string extension, string expectedExtension)
        {
            // arrange
            var randomOtherDirectory = new DirectoryInfo(".");

            string generatedFileName;

            // act
            using (var stream = DisappearingFile.CreateTempFileIn(randomOtherDirectory, extension))
            {
                generatedFileName = stream.Name;

                // assert
                Assert.StartsWith(randomOtherDirectory.FullName, stream.Name);
                Assert.EndsWith(extension, stream.Name);
                Assert.Equal(expectedExtension, Path.GetExtension(stream.Name));
            }

            Assert.False(File.Exists(generatedFileName));
        }
    }
}
