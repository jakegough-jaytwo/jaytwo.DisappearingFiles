using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xunit;

namespace jaytwo.DisappearingFiles.Tests
{
    public class DirectoryGeneratorTests
    {
        [Fact]
        public void CreateNewDirectory_with_existing_path_throws_IOException()
        {
            // arrange
            var path = Path.GetTempPath();

            // act && assert
            Assert.Throws<IOException>(() => DirectoryGenerator.CreateNewDirectory(path));
        }
    }
}
