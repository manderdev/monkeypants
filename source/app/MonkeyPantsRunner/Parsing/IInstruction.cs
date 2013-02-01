using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing
{
    public interface IInstruction
    {
        IInstructionParent Parent { set; }
        void Execute(IResultsWriter resultsWriter);
        DataCache DataCache { get; }
        UserCache UserCache { get; }
    }
}