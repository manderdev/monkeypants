using MonkeyPants.Output;
using MonkeyPants.Reading;

namespace MonkeyPants.Execution
{
    public class TestRunner
    {
        private readonly Assemblies assemblies;

        public TestRunner(Assemblies assemblies)
        {
            this.assemblies = assemblies;
        }

        //broken - this should get something instead of scenarios[]
        public void Test(string suiteName, Scenario[] scenarios, IResultsWriter resultsWriter)
        {
            var suite = new Suite(suiteName);
            suite.Test(assemblies, scenarios, resultsWriter);            
        }

        public void Test(string scenarioName, RawTest[] rawTests, IResultsWriter resultsWriter)
        {
            var scenario = new Scenario(scenarioName, assemblies);
            scenario.Test(rawTests, resultsWriter);
        }
    }
}