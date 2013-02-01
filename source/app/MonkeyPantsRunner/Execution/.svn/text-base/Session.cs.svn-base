using System;
using System.IO;
using MonkeyPants.Configuration;
using MonkeyPants.Output;
using MonkeyPants.Reading;
using MonkeyPants.Results;

namespace MonkeyPants.Execution
{
    public class Session
    {
        public static Session Current { get; private set; }

        private readonly ProjectConfiguration configuration;
        private readonly Assemblies assemblies;
        private readonly TestReaders testReaders;
        private readonly IResultsWriter resultsWriter;
        private readonly TestRunner testRunner;

        public Session(
            ProjectConfiguration configuration, Assemblies assemblies, 
            TestReaders testReaders, IResultsWriter resultsWriter)
        {
            this.configuration = configuration;
            this.assemblies = assemblies;
            this.testReaders = testReaders;
            this.resultsWriter = resultsWriter;
            testRunner = new TestRunner(assemblies);
        }

        public void Run(string inputPath)
        {
            try
            {
                Current = this;

                resultsWriter.Open();
                RunTests(inputPath);
            }
            finally
            {
                resultsWriter.Close();
            }
        }

        public void RunSetup(string inputPath)
        {
            TestSetupResult result = new TestSetupResult(inputPath);
            resultsWriter.Begin(result);
            RunTests(inputPath);
            resultsWriter.End(result);
        }

        public void RunTests(string inputPath)
        {
            if (File.Exists(inputPath))
            {
                RunTestsInFile(inputPath);
            }
            else if (Directory.Exists(inputPath))
            {
                RunTestsInDirectory(inputPath);
            }
            else
            {
                throw new MonkeyPantsApplicationException(
                    string.Format("File not found for specified input path '{0}'", inputPath));
            }
        }

        // todo: configure ignore for some dirs, e.g. svn
        private void RunTestsInDirectory(string inputPath)
        {
            string[] files = Directory.GetFiles(inputPath);
            // don't run setups when running directory, since tests themselves will invoke the setups
            files = Array.FindAll(files, file => !testReaders.ShouldIgnoreInBulk(file));
            Array.ForEach(files, RunTestsInFile);

            string[] subdirs = Directory.GetDirectories(inputPath);
            Array.ForEach(subdirs, RunTestsInDirectory);
        }

        private void RunTestsInFile(string inputPath)
        {
            if (!testReaders.Supports(inputPath)) return;

            ITestFileReader reader = testReaders.For(inputPath);
            RawTest[] rawTests = reader.Read(inputPath);
            testRunner.Test(inputPath, rawTests, resultsWriter);
        }
    }
}