using System.Collections.Generic;
using System.IO;
using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Configuration
{
    public class ProjectConfiguration
    {
        public ProjectConfiguration(List<string> fixtureAssemblies, List<InputConfiguration> inputs, List<OutputConfiguration> outputs)
        {
            FixtureAssemblies = fixtureAssemblies;
            Inputs = inputs;
            Outputs = outputs;
        }

        public List<string> FixtureAssemblies { get; private set; }

        public List<InputConfiguration> Inputs { get; private set;}
        
        public List<OutputConfiguration> Outputs { get; private set; }

        public static ProjectConfiguration Load(string file)
        {
            return new ProjectConfigurationXmlReader().Load(file);
        }

        public bool IsSetupFixture(string path)
        {
            // todo: configurable regex
            return Path.GetFileName(path).StartsWith("Setup");
        }

        public bool IsResultsFile(string path)
        {
            List<string> resultsFiles = Outputs.ConvertAll(input => Path.GetFileName(input.Channel.File));
            return resultsFiles.Contains(Path.GetFileName(path));
        }

        public Session CreateSession()
        {
            TestReaders readers = LoadInputReaders();
            Assemblies assemblies = Assemblies.Load(FixtureAssemblies);
            MulticastResultsWriter writers = LoadResultsWriters();

            return new Session(this, assemblies, readers, writers);
        }

        private TestReaders LoadInputReaders()
        {
            TestReaders readers = new TestReaders(this);
            foreach (InputConfiguration inputConfig in Inputs)
            {
                readers.Map(inputConfig.Match, inputConfig.LoadReader());
            }
            return readers;
        }

        private MulticastResultsWriter LoadResultsWriters()
        {
            // todo: console coloring
            var multicastResultsWriter = new MulticastResultsWriter();
            foreach (OutputConfiguration outputConfig in Outputs)
            {
                IResultsWriter writer = outputConfig.LoadWriter();
                multicastResultsWriter.Add(writer);
            }
            return multicastResultsWriter;
        }
    }
}