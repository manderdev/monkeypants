using System.Collections;
using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing.Instructions
{
    public class CreateOutputEnumerator : Instruction
    {
        public override void Execute(IResultsWriter resultsWriter)
        {
            IEnumerable enumerableTarget = (IEnumerable)DataCache[DataType.Source];
            DataCache[DataType.Enumerator] = enumerableTarget.GetEnumerator();
        }
    }
}