using System;
using MonkeyPants.Execution;
using MonkeyPants.Output;

namespace MonkeyPants.Parsing.Instructions
{
    public class InstantiateActionParameter : Instruction
    {
        private readonly Type actionParameterType;

        public InstantiateActionParameter(Type actionParameterType)
        {
            this.actionParameterType = actionParameterType;
        }

        public override void Execute(IResultsWriter resultsWriter)
        {
            DataCache[DataType.ActionParameter] = Assemblies.Instantiate(actionParameterType);
        }
    }
}