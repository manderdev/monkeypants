using System.Collections.Generic;
using MonkeyPants.Output;

namespace MonkeyPants.Results
{
    public class TestResult : Result
    {
        public TestResult(string title, List<string> headers, List<RowResult> rowResults)
        {
            TestName = title;
            Headers = headers;
            RowResults = rowResults;
            RowResults.ForEach(Count);
        }

        public string TestName { get; private set; }

        public List<string> Headers { get; private set; }

        private List<RowResult> RowResults { get; set; }

        public override string ToString()
		{
            // primarily for debugging readability
			return SimpleTextResultsWriter.ReportOn(this);
		}
	}
}