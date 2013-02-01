using System;
using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing.Instructions
{
    public class InstantiateSource : Instruction
    {
        private readonly Type sourceType;

        public InstantiateSource(Type sourceType)
        {
            this.sourceType = sourceType;
        }

        public override void Execute(IResultsWriter resultsWriter)
        {
            DataCache[DataType.Source] = Assemblies.Instantiate(sourceType);
        }
    }
}