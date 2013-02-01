using System.Collections.Generic;
using MonkeyPants.Output;

namespace MonkeyPants.Results
{
	public class RowResult : Result
	{
		private readonly List<CellResult> cellResults = new List<CellResult>();

	    public void Add(CellResult cellResult)
		{
			cellResults.Add(cellResult);
			Count(cellResult);
		}

		public override string ToString()
		{
            // primarily for debugging readability
			return SimpleTextResultsWriter.ReportOn(this);
		}
	}
}