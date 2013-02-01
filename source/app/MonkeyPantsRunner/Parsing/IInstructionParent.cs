using MonkeyPants.Output;
using MonkeyPants.Results;

namespace MonkeyPants.Parsing
{
    public interface IInstructionParent : IInstruction
    {
        void AddInstruction(IInstruction instruction);
        RowResult RowResult { get; }

        void AddRowResult(RowResult result, IResultsWriter resultsWriter);
    }
}