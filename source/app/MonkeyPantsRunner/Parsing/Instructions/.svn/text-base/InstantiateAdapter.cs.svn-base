using System;
using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing.Instructions
{
    public class InstantiateAdapter : Instruction
    {
        private readonly Type adapterType;

        public InstantiateAdapter(Type adapterType)
        {
            this.adapterType = adapterType;
        }

        public override void Execute(IResultsWriter resultsWriter)
        {
            object adapter = Assemblies.Instantiate(adapterType, DataCache[DataType.Source]);
            DataCache[DataType.Source] = adapter;
        }
    }
}