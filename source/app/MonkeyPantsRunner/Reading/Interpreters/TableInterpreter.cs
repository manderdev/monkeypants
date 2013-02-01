using System;
using System.Collections.Generic;
using MonkeyPants.Reading.Tables;

namespace MonkeyPants.Reading.Interpreters
{
    public abstract class TableInterpreter
    {
        private readonly int testColumnOffset;
        private readonly Predicate<Row> isComment;
        private readonly Predicate<Row> isDivider;

        protected TableInterpreter(
            int testColumnOffset, Predicate<Row> commentIdentifier, Predicate<Row> tableDivider)
        {
            this.testColumnOffset = testColumnOffset;
            isComment = commentIdentifier;
            isDivider = tableDivider;
        }

        public RawTest[] TranslateScenario(Table table)
        {
            // compact comments before splitting table to prevent comments becoming their own tests
            RemoveIntercommentLines(table);
            List<Table> tables = table.SplitOn(isDivider);
            return tables.ConvertAll(t => TranslateIndividual(t)).ToArray();
        }

        private RawTest TranslateIndividual(Table table)
        {
            int fixtureIndex = 0;
            int headersIndex = 1;

            List<string> comments = new List<string>();
            string title = string.Empty;
            List<string> args = new List<string>();
            List<string> headers = new List<string>();
            List<Row> dataRows = new List<Row>();

            List<Row> rows = table.Rows;
            for (int i = 0; i < rows.Count; i++)
            {
                Row currentRow = rows[i];
                if (isComment(currentRow))
                {
                    // note: comments are really only expected at the front of the test table
                    comments.Add(currentRow.Cells[0].Value.Trim());
                    fixtureIndex++;
                    headersIndex++;
                }
                else
                {
                    List<Cell> cells = currentRow.Cells;

                    AdjustForTestOffset(cells);

                    if (i == fixtureIndex)
                    {
                        // todo: bulletproof the indexing and stuff, provide nice exceptions with nice details
                        title = cells[0].Value;
                        args.AddRange(GetArgs(cells));
                    }
                    else if (i == headersIndex)
                    {
                        headers.AddRange(cells.ConvertAll(input => input.Value));
                    }
                    else
                    {
                        dataRows.Add(new Row(cells));
                    }
                }
            }

            RawTest rawTest = new RawTest(title, comments, args, headers, dataRows);
            rawTest.Validate();
            return rawTest;
        }

        /// <summary>
        /// adjust for test offset (e.g. if comments are in first column, as in excel)
        /// </summary>
        private void AdjustForTestOffset(List<Cell> cells)
        {
            if (testColumnOffset > 0)
            {
                cells.RemoveRange(0, testColumnOffset);
            }
        }

        private static List<string> GetArgs(List<Cell> titleLineContents)
        {
            if (titleLineContents.Count < 2) return new List<string>();
            return titleLineContents.GetRange(1, titleLineContents.Count - 1).
                ConvertAll(input => input.Value).FindAll(s => s != null);
        }

        // todo: inefficient
        public void RemoveIntercommentLines(Table table)
        {
            List<Row> rowsToRemove = new List<Row>();
            List<Row> rows = table.Rows;
            bool inTest = false;
            foreach (var row in rows)
            {
                if (isComment(row))
                {
                    // keep line
                }
                else if (row.IsEmpty())
                {
                    if (inTest)
                    {
                        // it's a divider - keep it
                        // end of test
                        inTest = false;
                    }
                    else
                    {
                        // empty row, not in test is empty row between comments. ditch it.
                        rowsToRemove.Add(row);
                    }
                }
                else
                {
                    inTest = true;
                }
            }
            rowsToRemove.ForEach(row => table.Rows.Remove(row));
        }
    }
}