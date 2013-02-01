using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing.Instructions
{
    public class RetrieveCachedSource : Instruction
    {
        private readonly string cacheKey;

        public RetrieveCachedSource(string cacheKey)
        {
            this.cacheKey = cacheKey;
        }

        public override void Execute(IResultsWriter resultsWriter)
        {
            DataCache[DataType.Source] = UserCache[cacheKey];
        }
    }
}