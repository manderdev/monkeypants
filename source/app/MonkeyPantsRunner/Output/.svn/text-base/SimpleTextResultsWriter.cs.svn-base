using System;
using MonkeyPants.Output.Channels;
using MonkeyPants.Results;

namespace MonkeyPants.Output
{
    public class SimpleTextResultsWriter : IResultsWriter
    {
        private readonly IOutputChannel channel;

        private int scenarioCount = 1;
        private int currentScenario;

        public SimpleTextResultsWriter(IOutputChannel channel)
        {
            this.channel = channel;    
        }

        public void Begin(SuiteResult suiteResult)
        {
            scenarioCount = suiteResult.ScenarioResults.Count;
            string s = Format("suite: {0}", ReportOn(suiteResult));
            channel.AppendLine(s);
        }

        // todo: timer, summary
        public void End(SuiteResult suite)
        {
            // do nothing
        }

        public void Begin(ScenarioResult scenario)
        {
            currentScenario++;
            channel.Append(Format("scenario: {0} ({1} of {2}): ", scenario.ScenarioName, currentScenario, scenarioCount));
        }

        public void End(ScenarioResult scenario)
        {
            channel.AppendLine(": ");
            channel.Append("   ");

            if (scenario.Passed)
            {
                channel.AppendLine(ReportOn(scenario));                
            }
            else if (scenario.Failed)
            {
                channel.AppendFailureLine(ReportOn(scenario));
            }
            else if (scenario.Errored)
            {
                channel.AppendErrorLine(ReportOn(scenario));
            }
        }

        public void Begin(TestSetupResult test)
        {
            channel.AppendLine("SETUP: " + test);
        }

        public void End(TestSetupResult test)
        {
            channel.AppendLine("END SETUP: " + test);
        }

        public void Begin(TestResult test)
        {
            // do nothing
        }

        // todo: it doesn't look like testresult is being set to fail/error ever
        public void End(TestResult test)
        {
            if (test.Passed)
            {
                channel.Append(".");                
            }
            else if (test.Failed)
            {
                channel.AppendFailure("F");                
            }
            else if (test.Errored)
            {
                channel.AppendError("E");                                
            }
        }

        public void Begin(RowResult row)
        {
            // do nothing
        }

        public void End(RowResult row)
        {
            // do nothing
        }

        public void Begin(CellResult cell)
        {
            // do nothing
        }

        public void End(CellResult cell)
        {
            if (cell.Passed)
            {
                channel.Append(".");
            }
            else if (cell.Failed)
            {
                channel.AppendFailure("F");
            }
            else if (cell.Errored)
            {
                channel.AppendError("E");
            }
        }

        public void Open()
        {
            // we don't need to do anything with the stream here, so empty delegate
            channel.Open(delegate { });
        }

        public void Close()
        {
            channel.Close();            
        }

        public static string ReportOn(SuiteResult result)
        {
            return result.SuiteName;
        }

        public static string ReportOn(ScenarioResult result)
        {
            if (result.Passed) return "PASS";

            string summary = string.Format("{0} ran: {1} passed; {2} failed; {3} had errors",
                result.TotalCount, result.PassCount, result.FailureCount, result.ErrorCount);
            return result.Failed ? "FAILURE: " + summary : "ERROR: " + summary;
        }

        public static string ReportOn(TestResult result)
        {
            return result.Passed ? "." : result.Errored ? "E" : "F";
        }

        public static string ReportOn(RowResult result)
        {
            return "Row " + (result.Passed ? "Passed" : result.Failed ? "Failed" : "Errored");
        }

        public static string ReportOn(CellResult result)
        {
            return string.Format("Exp >{0}< Act >{1}< Exc >{2}<", result.ExpectedResult, result.ActualResult,
                                 result.Exception == null ? string.Empty : result.Exception.Message);
        }

        private static string Format(string format, params object[] args)
        {
            return String.Format(format, args);
        }
    }
}