using MonkeyPants.Execution;
using MonkeyPants.Reading;

namespace MonkeyPants.Configuration
{
    public class InputConfiguration
    {
        public string Match { get; set; }

        public string Reader { get; set; }
        public string ReaderAssembly { get; set; }

        public ITestFileReader LoadReader()
        {
            string readerTypeName = Reader;
            string readerAssembly = ReaderAssembly;
            return Assemblies.Instantiate<ITestFileReader>(readerAssembly, readerTypeName);
        }
    }
}