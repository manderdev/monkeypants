using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing.Instructions
{
    public class WrapUnenumerableSource : Instruction
    {
        public override void Execute(IResultsWriter resultsWriter)
        {
            DataCache[DataType.Source] = new[] { DataCache[DataType.Source] };
        }
    }
}