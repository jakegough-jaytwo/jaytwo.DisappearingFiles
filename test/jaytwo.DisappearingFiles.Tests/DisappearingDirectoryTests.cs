using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xunit;

namespace jaytwo.DisappearingFiles.Tests
{
    public class DisappearingDirectoryTests
    {
        [Fact]
        public void CreateInTempPath_creates_folder_in_temp_path()
        {
            // arrange

            // act
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // assert
                Assert.StartsWith(Path.GetTempPath(), workspace.Path);
            }
        }

        [Fact]
        public void Directory_exists_after_DisappearingDirectory_is_created()
        {
            // arrange

            // act
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // assert
                Assert.True(Directory.Exists(workspace.Path));
            }
        }

        [Fact]
        public void Directory_does_not_exist_after_DisappearingDirectory_is_disposed()
        {
            // arrange
            var workspace = DisappearingDirectory.CreateInTempPath();

            // act
            workspace.Dispose();

            // assert
            Assert.False(Directory.Exists(workspace.Path));
        }

        [Fact]
        public void SubDirectories_does_not_exist_after_DisappearingDirectory_is_disposed()
        {
            // arrange
            var workspace = DisappearingDirectory.CreateInTempPath();
            var subDirectory1 = workspace.CreateNewSubdirectory();
            var subDirectory2 = subDirectory1.CreateSubdirectory("hello");

            // act
            workspace.Dispose();

            // assert
            Assert.False(Directory.Exists(workspace.Path));
            Assert.False(Directory.Exists(subDirectory1.FullName));
            Assert.False(Directory.Exists(subDirectory2.FullName));
        }

        [Fact]
        public void Files_do_not_exist_after_DisappearingDirectory_is_disposed()
        {
            // arrange
            var workspace = DisappearingDirectory.CreateInTempPath();
            var file = workspace.CreateNewFileWithExtension(".hello");

            // act
            workspace.Dispose();

            // assert
            Assert.False(Directory.Exists(workspace.Path));
            Assert.False(File.Exists(file.FullName));
        }

        [Fact]
        public void Create_creates_directory_in_system_temp_path()
        {
            // arrange

            // act
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // assert
                Assert.StartsWith(Path.GetTempPath(), workspace.Path);
            }
        }

        [Fact]
        public void CreateIn_creates_directory_in_specified_location()
        {
            // arrange
            var randomOtherDirectory = new DirectoryInfo(".");

            // act
            using (var workspace = DisappearingDirectory.CreateIn(randomOtherDirectory))
            {
                // assert
                Assert.StartsWith(randomOtherDirectory.FullName, workspace.Path);
            }
        }

        [Fact]
        public void CreateSubdirectory_creates_directory_within_DisappearingDirectory()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var workspaceDirectory = workspace.CreateNewSubdirectory();

                // assert
                Assert.True(Directory.Exists(workspaceDirectory.FullName));
                Assert.StartsWith(workspace.Path, workspaceDirectory.FullName);
            }
        }

        [Theory]
        [InlineData("abc.", ".xyz")]
        [InlineData("abc", "xyz")]
        [InlineData(null, ".xyz")]
        [InlineData(null, "xyz")]
        [InlineData("abc.", null)]
        [InlineData("abc", null)]
        public void CreateSubdirectory_creates_file_with_prefix_and_suffix(string prefix, string suffix)
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var workspaceDirectory = workspace.CreateNewSubdirectory(prefix, suffix);

                // assert
                Assert.True(Directory.Exists(workspaceDirectory.FullName));
                if (prefix != null)
                {
                    Assert.StartsWith(prefix, workspaceDirectory.Name);
                }

                if (suffix != null)
                {
                    Assert.EndsWith(suffix, workspaceDirectory.Name);
                }
            }
        }

        [Fact]
        public void CreateFile_creates_file_within_DisappearingDirectory()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var createdFile = workspace.CreateNewFile();

                // assert
                Assert.True(File.Exists(createdFile.FullName));
                Assert.StartsWith(workspace.Path, createdFile.FullName);
            }
        }

        [Theory]
        [InlineData("abc.", ".xyz")]
        [InlineData("abc", "xyz")]
        [InlineData(null, ".xyz")]
        [InlineData(null, "xyz")]
        [InlineData("abc.", null)]
        [InlineData("abc", null)]
        public void CreateFile_creates_file_with_prefix_and_suffix(string prefix, string suffix)
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var createdFile = workspace.CreateNewFile(prefix, suffix);

                // assert
                Assert.True(File.Exists(createdFile.FullName));
                if (prefix != null)
                {
                    Assert.StartsWith(prefix, createdFile.Name);
                }

                if (suffix != null)
                {
                    Assert.EndsWith(suffix, createdFile.Name);
                }
            }
        }

        [Fact]
        public void GetFiles_returns_files_in_DisappearingDirectory()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                var file1 = workspace.CreateNewFile();

                // act
                var files = workspace.GetFiles();

                // assert
                Assert.Equal(file1.FullName, files.Single().FullName);
            }
        }

        [Fact]
        public void GetFiles_with_pattern_returns_files_in_DisappearingDirectory()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                var file1 = workspace.CreateNewFile();

                // act
                var files = workspace.GetFiles("*.*");

                // assert
                Assert.Equal(file1.FullName, files.Single().FullName);
            }
        }

        [Fact]
        public void GetFiles_with_pattern_and_SearchOption_returns_files_in_DisappearingDirectory()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                var file1 = workspace.CreateNewFile();

                // act
                var files = workspace.GetFiles("*.*", SearchOption.AllDirectories);

                // assert
                Assert.Equal(file1.FullName, files.Single().FullName);
            }
        }

        [Fact]
        public void GetDirectories_returns_directories_in_DisappearingDirectory()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                var directory1 = workspace.CreateNewSubdirectory();

                // act
                var directories = workspace.GetDirectories();

                // assert
                Assert.Equal(directory1.FullName, directories.Single().FullName);
            }
        }

        [Fact]
        public void GetDirectories_with_pattern_returns_directories_in_DisappearingDirectory()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                var directory1 = workspace.CreateNewSubdirectory();

                // act
                var directories = workspace.GetDirectories("*.*");

                // assert
                Assert.Equal(directory1.FullName, directories.Single().FullName);
            }
        }

        [Fact]
        public void GetDirectories_with_pattern_and_SearchOption_returns_directories_in_DisappearingDirectory()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                var directory1 = workspace.CreateNewSubdirectory();

                // act
                var directories = workspace.GetDirectories("*.*", SearchOption.AllDirectories);

                // assert
                Assert.Equal(directory1.FullName, directories.Single().FullName);
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_stream_writes_expected_content()
        {
            // arrange
            var contentText = "Hello World";
            var contentBytes = Encoding.UTF8.GetBytes(contentText);

            using (var contentStream = new MemoryStream(contentBytes))
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentStream);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_stream_and_file_name_writes_expected_content()
        {
            // arrange
            var fileName = "helloWorld.txt";
            var contentText = "Hello World";
            var contentBytes = Encoding.UTF8.GetBytes(contentText);

            using (var contentStream = new MemoryStream(contentBytes))
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentStream, fileName);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.Equal(fileName, file1.Name);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_stream_and_prefix_and_suffix_writes_expected_content()
        {
            // arrange
            var prefix = "abc-";
            var suffix = "-xyz";
            var contentText = "Hello World";
            var contentBytes = Encoding.UTF8.GetBytes(contentText);

            using (var contentStream = new MemoryStream(contentBytes))
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentStream, prefix, suffix);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.StartsWith(prefix, file1.Name);
                Assert.EndsWith(suffix, file1.Name);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileWithExtensionAsync_with_stream_writes_expected_content()
        {
            // arrange
            var fileExtension = ".txt";
            var contentText = "Hello World";
            var contentBytes = Encoding.UTF8.GetBytes(contentText);

            using (var contentStream = new MemoryStream(contentBytes))
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileWithExtensionAsync(contentStream, fileExtension);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.Equal(fileExtension, file1.Extension);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_byte_array_writes_expected_content()
        {
            // arrange
            var contentText = "Hello World";
            var contentBytes = Encoding.UTF8.GetBytes(contentText);

            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentBytes);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_byte_array_and_file_name_writes_expected_content()
        {
            // arrange
            var fileName = "helloWorld.txt";
            var contentText = "Hello World";
            var contentBytes = Encoding.UTF8.GetBytes(contentText);

            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentBytes, fileName);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.Equal(fileName, file1.Name);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_byte_array_and_prefix_and_suffix_writes_expected_content()
        {
            // arrange
            var prefix = "abc-";
            var suffix = "-xyz";
            var contentText = "Hello World";
            var contentBytes = Encoding.UTF8.GetBytes(contentText);

            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentBytes, prefix, suffix);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.StartsWith(prefix, file1.Name);
                Assert.EndsWith(suffix, file1.Name);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileWithExtensionAsync_with_byte_array_writes_expected_content()
        {
            // arrange
            var fileExtension = ".txt";
            var contentText = "Hello World";
            var contentBytes = Encoding.UTF8.GetBytes(contentText);

            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileWithExtensionAsync(contentBytes, fileExtension);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.Equal(fileExtension, file1.Extension);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_string_writes_expected_content()
        {
            // arrange
            var contentText = "Hello World";

            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentText);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_string_and_file_name_writes_expected_content()
        {
            // arrange
            var fileName = "helloWorld.txt";
            var contentText = "Hello World";

            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentText, fileName);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.Equal(fileName, file1.Name);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileAsync_with_string_and_prefix_and_suffix_writes_expected_content()
        {
            // arrange
            var prefix = "abc-";
            var suffix = "-xyz";
            var contentText = "Hello World";

            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileAsync(contentText, prefix, suffix);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.StartsWith(prefix, file1.Name);
                Assert.EndsWith(suffix, file1.Name);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Fact]
        public async Task WriteToNewFileWithExtensionAsync_with_string_writes_expected_content()
        {
            // arrange
            var fileExtension = ".txt";
            var contentText = "Hello World";

            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var file1 = await workspace.WriteToNewFileWithExtensionAsync(contentText, fileExtension);

                // assert
                Assert.True(File.Exists(file1.FullName));
                Assert.Equal(fileExtension, file1.Extension);
                Assert.NotEqual(0, file1.Length);
                Assert.Equal(contentText, File.ReadAllText(file1.FullName));
            }
        }

        [Theory]
        [InlineData("a", "{0}a")]
        [InlineData("/a", "{0}a")]
        [InlineData("\\a", "{0}a")]
        [InlineData("a/b/c", "{0}a{0}b{0}c")]
        [InlineData("/a/b/c", "{0}a{0}b{0}c")]
        [InlineData("a\\b\\c", "{0}a{0}b{0}c")]
        [InlineData("\\a\\b\\c", "{0}a{0}b{0}c")]
        [InlineData("/a\\b/c", "{0}a{0}b{0}c")]
        public void GetFullPath_returns_normalized_paths(string path, string expectedEndingFormat)
        {
            var expectedEnding = string.Format(expectedEndingFormat, Path.DirectorySeparatorChar);

            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var fullPath = workspace.GetFullPath(path);

                // assert
                Assert.EndsWith(expectedEnding, fullPath);
            }
        }

        [Fact]
        public void GetDirectoryInfo_returns_expected_path()
        {
            // arrange
            using (var workspace = DisappearingDirectory.CreateInTempPath())
            {
                // act
                var directoryInfo = workspace.GetDirectoryInfo();

                // assert
                Assert.Equal(workspace.Path, directoryInfo.FullName);
            }
        }
    }
}
