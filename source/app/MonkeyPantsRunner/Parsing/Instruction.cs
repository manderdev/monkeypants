using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing
{
    public abstract class Instruction : IInstruction
    {
        public abstract void Execute(IResultsWriter resultsWriter);

        public IInstructionParent Parent { get; set; }

        public DataCache DataCache
        {
            get { return Parent.DataCache; }
        }

        public UserCache UserCache
        {
            get { return Parent.UserCache; }
        }
    }
}