using System.IO;

namespace MonkeyPants.Locating
{
    public class ProjectConfigurationLocater
    {
        private readonly string inputPath;
        private string initialDirectory;
        private readonly string searchPattern;

        public ProjectConfigurationLocater(string inputPath, string searchPattern)
        {
            this.inputPath = inputPath;
            this.searchPattern = searchPattern;
        }

        public string Locate()
        {
            if (Directory.Exists(inputPath))
            {
                return FindFileInDirectoryOrAncestors(inputPath);
            }
            
            if (File.Exists(inputPath))
            {
                initialDirectory = Path.GetDirectoryName(inputPath);
                return FindFileInDirectoryOrAncestors(initialDirectory);
            }
            
            throw new MonkeyPantsApplicationException("Input path not found: " + inputPath);
        }

        private string FindFileInDirectoryOrAncestors(string directory)
        {
            if (!Directory.Exists(directory))
            {
                throw new MonkeyPantsApplicationException("Directory not found: " + directory);
            }

            string[] strings = Directory.GetFiles(directory, searchPattern);
            if (strings.Length == 0)
            {
                DirectoryInfo directoryInfo = Directory.GetParent(directory);
                if (directoryInfo == null)
                {
                    // string.Format fails due to the \ in the path, so we just use +
                    throw new MonkeyPantsApplicationException(
                        "File matching '" + searchPattern + "' not found. Searched upward starting at " + 
                        initialDirectory + "and reaching " + directory);
                }
                return FindFileInDirectoryOrAncestors(directoryInfo.FullName);
            }

            if (strings.Length == 1)
            {
                return strings[0];
            }

            throw new MonkeyPantsApplicationException(
                string.Format("Found multiple configuration files in {0} matching {1}", directory, searchPattern));
        }
    }
}