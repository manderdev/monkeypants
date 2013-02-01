using System.Diagnostics;
using MonkeyPants.Parsing.Instructions;

namespace MonkeyPants
{
    // todo: allow case insensitivity for fixture names in test files (e.g. debug, Debug, DEBUG -- should all work).
    public class DebugFixture : IAutoExecuteFixture
    {
        public bool AutoExecute()
        {
            Debugger.Launch();
            return true;
        }
    }
}