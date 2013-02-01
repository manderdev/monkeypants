using System.IO;

namespace MonkeyPants.Extensions
{
    public static class PathExtensions
    {
        public static string EnsureIsRooted(string path, string defaultRoot)
        {
            return Path.IsPathRooted(path) ? path : Path.Combine(defaultRoot, path);
        }
    }
}