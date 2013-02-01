using System.Collections;
using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing.Instructions
{
    public class NextActualRow : Instruction
    {
        public override void Execute(IResultsWriter resultsWriter)
        {
            IEnumerator enumerator = (IEnumerator) DataCache[DataType.Enumerator];
            DataCache[DataType.Enumerated] = enumerator.MoveNext() ? enumerator.Current : RealTest.MISSING_ROW;
        }
    }
}