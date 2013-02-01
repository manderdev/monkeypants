using MonkeyPants.Results;

namespace MonkeyPants.Output
{
    public interface IResultsWriter
    {
        void Open();
        void Close();

        void Begin(SuiteResult suite);
        void End(SuiteResult suite);

        void Begin(ScenarioResult scenario);
        void End(ScenarioResult scenario);

        void Begin(TestSetupResult test);
        void End(TestSetupResult test);

        void Begin(TestResult test);
        void End(TestResult test);

        void Begin(RowResult row);
        void End(RowResult row);
        
        void Begin(CellResult cell);
        void End(CellResult cell);
    }
}