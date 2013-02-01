using System.Collections.Generic;
using MonkeyPants.Output;

namespace MonkeyPants.Results
{
    public class ScenarioResult : Result
    {
        internal List<TestResult> TestResults { get; private set; }
    	internal string ScenarioName { get; private set; }

        public ScenarioResult(string scenarioName)
        {
        	ScenarioName = scenarioName;
            TestResults = new List<TestResult>();
        }

		public void Add(TestResult testResult)
		{
			TestResults.Add(testResult);
			Count(testResult);
		}

		public override string ToString()
		{
            // primarily for debugging readability
			return SimpleTextResultsWriter.ReportOn(this);
		}
    }
}