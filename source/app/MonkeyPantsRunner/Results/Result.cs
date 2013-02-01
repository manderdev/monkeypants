namespace MonkeyPants.Results
{
	public abstract class Result
	{		
        public int TotalCount { get; protected set; }
		public int PassCount { get; protected set; }
		public int FailureCount { get; protected set; }
		public int ErrorCount { get; protected set; }

		public bool Passed
		{
			get { return FailureCount == 0 && ErrorCount == 0; }
		}

		public bool Failed
		{
			get { return FailureCount > 0 && ErrorCount == 0; }
		}

		public bool Errored
		{
			get { return ErrorCount > 0; }
		}

		protected virtual void Count(Result childResult)
		{
			TotalCount++;
		    if (childResult.Passed)
		    {
		        PassCount++;
		    }
		    else if (childResult.Failed)
		    {
		        FailureCount++;
		    }
		    else
		    {
		        ErrorCount++;
		    }
		}
	}
}