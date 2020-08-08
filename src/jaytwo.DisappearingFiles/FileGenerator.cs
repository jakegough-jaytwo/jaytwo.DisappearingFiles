using System;
using System.IO;

namespace jaytwo.DisappearingFiles
{
    internal class FileGenerator
    {
        private static object _padlock = new object();

        public static FileInfo CreateNewFile(Func<string> namingDelegate)
            => CreateNewFile(namingDelegate, 10);

        public static FileInfo CreateNewFile(Func<string> namingDelegate, int tries)
        {
            lock (_padlock)
            {
                for (int i = 0; i < tries; i++)
                {
                    var fileName = namingDelegate.Invoke();

                    try
                    {
                        return CreateNewFile(fileName);
                    }
                    catch (IOException)
                    {
                    }
                }
            }

            throw new Exception("Could not create new file");
        }

        public static FileInfo CreateNewFile(string fileName)
        {
            var fileInfo = new FileInfo(fileName);

            using (new FileStream(fileInfo.FullName, FileMode.CreateNew))
            {
            }

            fileInfo.Refresh();
            return fileInfo;
        }
    }
}
