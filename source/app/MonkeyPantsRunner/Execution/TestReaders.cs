using System.Collections.Generic;
using System.IO;
using MonkeyPants.Configuration;
using MonkeyPants.Reading;

namespace MonkeyPants.Execution
{
    public class TestReaders
    {
        private readonly Dictionary<string, ITestFileReader> readers = new Dictionary<string, ITestFileReader>();
        private readonly ProjectConfiguration configuration;

        public TestReaders(ProjectConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public void Map(string match, ITestFileReader reader)
        {
            readers.Add(match, reader);
        }

        public bool Supports(string path)
        {
            return ResolveReader(path) != null;
        }

        public ITestFileReader For(string path)
        {
            ITestFileReader reader = ResolveReader(path);
            if (reader == null)
            {
                throw new MonkeyPantsApplicationException(string.Format(
                                                              "No input reader configured for file '{0}'", path));
            }
            return reader;
        }

        public bool ShouldIgnoreInBulk(string path)
        {
            // don't read results files or setup when running bulk
            // todo: configurable setup pattern
            return configuration.IsResultsFile(path) || configuration.IsSetupFixture(path);
        }

        private ITestFileReader ResolveReader(string path)
        {
            string extension = Path.GetExtension(path);
            string key = FindKeyFor(extension);
            return key == null ? null : readers[key];
        }

        private string FindKeyFor(string extension)
        {
            if (string.Empty.Equals(extension)) return null;

            foreach (string key in readers.Keys)
            {
                // todo: support patterns, not just extensions
                if (key.EndsWith(extension))
                {
                    return key;
                }
            }
            return null;
        }
    }
}