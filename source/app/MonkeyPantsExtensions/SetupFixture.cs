using MonkeyPants.Execution;
using MonkeyPants.Parsing.Instructions;

namespace MonkeyPants
{
    public class SetupFixture : IAutoExecuteFixture
    {
        public string Fixture { get; set; }

        public bool AutoExecute()
        {
            Session.Current.RunSetup(Fixture);
            return true;
        }
    }

    // note: backwards compatibility with AGA
    public class RunSetupFixture : SetupFixture{}
}
