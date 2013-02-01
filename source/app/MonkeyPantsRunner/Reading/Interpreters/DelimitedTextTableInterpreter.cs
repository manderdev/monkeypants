using MonkeyPants.Reading.Tables;

namespace MonkeyPants.Reading.Interpreters
{
    public class DelimitedTextTableInterpreter : TableInterpreter
    {
        private const int TEST_COLUMN_OFFSET = 0;

        public DelimitedTextTableInterpreter() : base(TEST_COLUMN_OFFSET, IsComment, IsDivider)
        {
        }
        
        private static bool IsComment(Row currentRow)
        {
            if (currentRow.Cells.Count == 0) return false;
            string value = currentRow.Cells[0].Value;
            return value != null && value.StartsWith("*");            
        }

        private static bool IsDivider(Row row)
        {
            return row.IsEmpty();
        }
    }
}