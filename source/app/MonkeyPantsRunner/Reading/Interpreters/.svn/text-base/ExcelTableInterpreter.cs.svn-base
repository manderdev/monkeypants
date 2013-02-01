using System.Collections.Generic;
using MonkeyPants.Reading.Tables;

namespace MonkeyPants.Reading.Interpreters
{
    /// <summary>
    /// Expects: 
    /// - comments are on their own rows and in first column
    /// - comments come before titles
    /// - titles in second columns, empty rows separating tests
    /// - args (if any) on title line in third and following columns
    /// - headers follow title row
    /// - data rows follow title row
    /// - tests in scenario are separated by at least blank line
    /// - blank lines between comments or between comments and titles are ignored
    /// </summary>
    public class ExcelTableInterpreter : TableInterpreter
    {
        // comments are in column 0, tests in column 1 - this provides an unassailable separation 
        // between comments and tests/data and, in spreadsheets, is simple to compose (would more 
        // intrusive in tab/csv format though)
        private const int TEST_COLUMN_OFFSET = 1;

        public ExcelTableInterpreter() : base(TEST_COLUMN_OFFSET, IsComment, IsDivider)
        {
        }

        private static bool IsComment(Row row)
        {
            List<Cell> cells = row.Cells;
            return cells.Count > 0 && !cells[0].IsEmpty();
        }

        private static bool IsDivider(Row row)
        {
            return row.IsEmpty();
        }
    }
}