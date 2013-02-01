using System.Collections;
using System.Collections.Generic;
using MonkeyPants.Execution;
using MonkeyPants.Output;
using MonkeyPants.Reading.Tables;

namespace MonkeyPants.Parsing.Instructions
{
    public class NextDataRow : Instruction
    {
        private readonly IEnumerator enumerator;

        public NextDataRow(List<Row> dataRows)
        {
            enumerator = dataRows.GetEnumerator();
        }

        public override void Execute(IResultsWriter resultsWriter)
        {
            DataCache[DataType.DataRow] = enumerator.MoveNext() ? enumerator.Current : RealTest.MISSING_ROW;
        }
    }
}