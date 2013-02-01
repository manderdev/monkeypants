using System;
using System.Collections.Generic;
using System.Text;

namespace MonkeyPants.Reading.Tables
{
    public class Table
    {
        public List<Row> Rows { get; private set; }

        public Table()
        {
            Rows = new List<Row>();
        }

        public int RowCount
        {
            get { return Rows.Count; }
        }

        public void AddEmptyRow()
        {
            Rows.Add(Row.Empty);
        }

        public void AddRow(Row row)
        {
            Rows.Add(row);
        }

        public void AddEmptyRows(int rowCount)
        {
            for (int i=0; i<rowCount; i++)
                AddEmptyRow();
        }

        /// <summary>
        /// Split table into subtables based on predicate (blank rows, for example)
        /// </summary>
        public List<Table> SplitOn(Predicate<Row> divideOn)
        {
            var allTables = new List<Table>();
            Table currentTable = null;

            foreach (Row row in Rows)
            {
                // end current table?
                if (divideOn(row))
                {
                    // note: divider row is discarded here (so far no need to keep the divider, but easy enough to do)
                    currentTable = null;
                    continue;
                }

                // start new table?
                if (currentTable == null)
                {
                    currentTable = new Table();
                    allTables.Add(currentTable);
                }

                currentTable.AddRow(row);
            }
            return allTables;
        }

        // intended for debugging only. See IResultsWriter for test output.
        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var row in Rows)
            {
                sb.AppendLine(row.ToString());
            }
            return sb.ToString();
        }
    }
}