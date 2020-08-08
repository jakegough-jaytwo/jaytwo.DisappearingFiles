using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace jaytwo.DisappearingFiles
{
    internal class NameGenerator
    {
        public static string GetFileNameWithExtension(string basePath, string extension)
            => Path.Combine(basePath, GetFileNameWithExtension(extension));

        public static string GetFileNameWithExtension(string extension)
            => GenerateRandomString(null, NormalizeExtension(extension));

        public static string GetFileName(string basePath)
            => Path.Combine(basePath, GetFileName());

        public static string GetFileName()
            => GenerateRandomString(null, ".tmp");

        public static string GenerateRandomPath(string basePath, string prefix, string suffix)
            => Path.Combine(basePath, GenerateRandomString(prefix, suffix));

        public static string GenerateRandomString(string prefix, string suffix)
            => prefix + GenerateRandomString() + suffix;

        public static string GenerateRandomString()
        {
            var result = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            result = Regex.Replace(result, "[^0-9A-Za-z]", string.Empty);
            return result;
        }

        public static string NormalizeExtension(string extension)
        {
            if (!string.IsNullOrEmpty(extension) && !extension.StartsWith("."))
            {
                return "." + extension;
            }

            return extension;
        }
    }
}
