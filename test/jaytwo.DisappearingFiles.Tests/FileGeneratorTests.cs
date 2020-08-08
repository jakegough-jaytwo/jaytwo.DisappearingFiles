using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace jaytwo.DisappearingFiles.Tests
{
    public class FileGeneratorTests
    {
        [Fact]
        public void CreateNewFile_with_existing_path_throws_IOException()
        {
            // arrange
            using (var tempFile = DisappearingFile.CreateTempFile())
            {
                // act && assert
                Assert.Throws<IOException>(() => FileGenerator.CreateNewFile(tempFile.Name));
            }
        }
    }
}
