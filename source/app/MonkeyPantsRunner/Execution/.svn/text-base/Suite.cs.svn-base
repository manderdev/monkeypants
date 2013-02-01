using System;
using System.Collections.Generic;
using MonkeyPants.Output;
using MonkeyPants.Results;

namespace MonkeyPants.Execution
{
    public class Suite
    {
        private readonly string suiteName;

        public Suite(string suiteName)
        {
            this.suiteName = suiteName;
        }

        //broken - this should get something instead of scenarios[] cuz scenarios test rawTests
        public SuiteResult Test(Assemblies assemblies, Scenario[] scenarios, IResultsWriter resultsWriter)
        {
            var scenarioResults = new List<ScenarioResult>(scenarios.Length);

            Array.ForEach(scenarios, scenario => scenarioResults.Add(scenario.Test(null, resultsWriter)));
            return new SuiteResult(suiteName, scenarioResults);
        }
    }
}