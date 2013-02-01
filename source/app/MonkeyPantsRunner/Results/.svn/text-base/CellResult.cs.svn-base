using System;
using MonkeyPants.Output;

namespace MonkeyPants.Results
{
    public class CellResult : Result
	{
		public string ExpectedResult { get; private set; }
		public string ActualResult { get; private set; }
		public Exception Exception { get; private set; }

		public CellResult(string expectedResult, string actualResult)
		{
			ExpectedResult = expectedResult;
			ActualResult = actualResult;
			Count(this);
		}

		public CellResult(string expectedResult, Exception exception)
		{
			ExpectedResult = expectedResult;
			Exception = exception;
			Count(this);
		}

		protected override sealed void Count(Result unused)
		{
			TotalCount = 1;
			if (Exception != null)
			{
				ErrorCount = 1;
			}
			else if (ExpectedResult != null && ExpectedResult.Equals(ActualResult))
			{
				PassCount = 1;
			}
			else
			{
				FailureCount = 1;
			}
		}

		public override string ToString()
		{
            // primarily for debugging readability
			return SimpleTextResultsWriter.ReportOn(this);
		}
	}
}
