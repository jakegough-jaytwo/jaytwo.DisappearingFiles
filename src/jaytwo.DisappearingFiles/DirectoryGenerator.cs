using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jaytwo.DisappearingFiles
{
    internal class DirectoryGenerator
    {
        private static object _padlock = new object();

        public static DirectoryInfo CreateNewDirectory(Func<string> namingDelegate)
            => CreateNewDirectory(namingDelegate, 10);

        public static DirectoryInfo CreateNewDirectory(Func<string> namingDelegate, int tries)
        {
            lock (_padlock)
            {
                for (int i = 0; i < tries; i++)
                {
                    var directoryName = namingDelegate.Invoke();

                    try
                    {
                        return CreateNewDirectory(directoryName);
                    }
                    catch (IOException)
                    {
                    }
                }
            }

            throw new Exception("Could not create new directory");
        }

        public static DirectoryInfo CreateNewDirectory(string directoryName)
        {
            var directory = new DirectoryInfo(directoryName);
            if (directory.Exists)
            {
                throw new IOException("Directory already exists: " + directoryName);
            }

            directory.Create();
            directory.Refresh();
            return directory;
        }
    }
}
