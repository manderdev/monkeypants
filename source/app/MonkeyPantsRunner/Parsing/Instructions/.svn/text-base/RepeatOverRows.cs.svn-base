using System;
using System.Collections.Generic;
using MonkeyPants.Execution;
using MonkeyPants.Output;
using MonkeyPants.Results;

namespace MonkeyPants.Parsing.Instructions
{
    public class RepeatOverRows : Instruction, IInstructionParent
    {
        private readonly int dataRowCount;
        private readonly bool isOutputTest;

        public List<IInstruction> ChildInstructions { get; private set; }

        public RepeatOverRows(int dataRowCount, bool isOutputTest)
        {
            this.dataRowCount = dataRowCount;
            this.isOutputTest = isOutputTest;
            ChildInstructions = new List<IInstruction>();
        }

        public override void Execute(IResultsWriter resultsWriter)
        {
            int biggestRowCount = (isOutputTest) ? MostRowsDataOrActual() : dataRowCount;
            for (int row = 0; row < biggestRowCount; row++)
            {
                Parent.AddRowResult(ExecuteRowInstructions(resultsWriter), resultsWriter);
            }
        }

        private int MostRowsDataOrActual()
        {
            int actualRowCount = (int)DataCache[DataType.ActualRowCount];
            return Math.Max(dataRowCount, actualRowCount);
        }

        private RowResult ExecuteRowInstructions(IResultsWriter resultsWriter)
        {
            RowResult rowResult = new RowResult();
            AddRowResult(rowResult, resultsWriter);
            resultsWriter.Begin(rowResult);
            ChildInstructions.ForEach(instruction => instruction.Execute(resultsWriter));
            resultsWriter.End(rowResult);
            return RowResult;
        }

        public void AddInstruction(IInstruction instruction)
        {
            instruction.Parent = this;
            ChildInstructions.Add(instruction);
        }

        public RowResult RowResult { get; private set; }
	    
        public void AddRowResult(RowResult result, IResultsWriter resultsWriter)
        {
            RowResult = result;
        }
    }
}