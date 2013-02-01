using MonkeyPants.Output;
using MonkeyPants.Parsing;
using MonkeyPants.Reading;
using MonkeyPants.Results;

namespace MonkeyPants.Execution
{
    public class Scenario
    {
        private readonly string name;
        private readonly Assemblies assemblies;

        public Scenario(string name, Assemblies assemblies)
        {
            this.name = name;
            this.assemblies = assemblies;
        }

        public ScenarioResult Test(RawTest[] rawTests, IResultsWriter resultsWriter)
        {
            var results = new ScenarioResult(name);
            resultsWriter.Begin(results);

            UserCache cache = new UserCache();
            foreach (RawTest rawTest in rawTests)
            {
                var realTest = rawTest.CreateRealTest(assemblies, cache);
                results.Add(realTest.Test(resultsWriter));
            }
            resultsWriter.End(results);
            return results;
        }
    }
}