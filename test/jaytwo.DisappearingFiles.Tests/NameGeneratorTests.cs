using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xunit;

namespace jaytwo.DisappearingFiles.Tests
{
    public class NameGeneratorTests
    {
        [Fact]
        public void GetFileNameWithExtension_generates_unique_name()
        {
            // arrange
            int iterations = 10;
            var names = new List<string>();

            for (int i = 0; i < iterations; i++)
            {
                names.Add(NameGenerator.GetFileNameWithExtension(".", ".tmp"));
                Thread.Sleep(TimeSpan.FromMilliseconds(1));
            }

            // act
            Assert.Equal(names.Count, names.Distinct().Count());
        }
    }
}
