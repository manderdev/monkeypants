using System.Collections.Generic;
using MonkeyPants.Output;

namespace MonkeyPants.Results
{
    public class SuiteResult : Result
    {
		public string SuiteName { get; private set; }
        internal List<ScenarioResult> ScenarioResults { get; private set; }

        public SuiteResult(string suiteName, List<ScenarioResult> scenarioResults)
        {
			SuiteName = suiteName;
            ScenarioResults = scenarioResults;
			ScenarioResults.ForEach(Count);
        }

		public override string ToString()
		{
            // primarily for debugging readability
			return SimpleTextResultsWriter.ReportOn(this);
		}
	}
}