using System.Collections.Generic;
using MonkeyPants.Results;

namespace MonkeyPants.Output
{
    public class MulticastResultsWriter : IResultsWriter
    {
        readonly List<IResultsWriter> writers = new List<IResultsWriter>();
        
        public void Add(IResultsWriter writer)
        {
            writers.Add(writer);
        }

        public void Begin(SuiteResult suite)
        {
            writers.ForEach(writer => writer.Begin(suite));
        }

        public void End(SuiteResult suite)
        {
            writers.ForEach(writer => writer.End(suite));
        }

        public void Begin(ScenarioResult scenario)
        {
            writers.ForEach(writer => writer.Begin(scenario));
        }

        public void End(ScenarioResult scenario)
        {
            writers.ForEach(writer => writer.End(scenario));
        }

        public void Begin(TestSetupResult setup)
        {
        }

        public void End(TestSetupResult setup)
        {
        }

        public void Begin(TestResult test)
        {
            writers.ForEach(writer => writer.Begin(test));
        }

        public void End(TestResult test)
        {
            writers.ForEach(writer => writer.End(test));
        }

        public void Begin(RowResult row)
        {
            writers.ForEach(writer => writer.Begin(row));
        }

        public void End(RowResult row)
        {
            writers.ForEach(writer => writer.End(row));
        }

        public void Begin(CellResult cell)
        {
            writers.ForEach(writer => writer.Begin(cell));
        }

        public void End(CellResult cell)
        {
            writers.ForEach(writer => writer.End(cell));
        }

        public void Close()
        {
            writers.ForEach(writer => writer.Close());
        }

        public void Open()
        {
            writers.ForEach(writer => writer.Open());
        }
    }
}