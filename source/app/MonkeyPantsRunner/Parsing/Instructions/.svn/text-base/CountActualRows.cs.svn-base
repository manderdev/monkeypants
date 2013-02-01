using System.Collections;
using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing.Instructions
{
    public class CountActualRows : Instruction
    {
        public override void Execute(IResultsWriter resultsWriter)
        {
            IEnumerator enumerator = (IEnumerator) DataCache[DataType.Enumerator];

            int actualRowCount = 0;
            while (enumerator.MoveNext())
                actualRowCount++;

            DataCache[DataType.ActualRowCount] = actualRowCount;

            // somebody else needs that enumerator
            enumerator.Reset();
        }
    }
}