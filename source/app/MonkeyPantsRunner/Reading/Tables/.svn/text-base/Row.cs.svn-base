using System.Collections.Generic;
using System.Text;

namespace MonkeyPants.Reading.Tables
{
    public class Row
    {
        public static Row Empty
        {
            get { return new Row(); }
        }

        public List<Cell> Cells { get; private set; }

        public Row()
        {
            Cells = new List<Cell>();
        }

        public Row(IEnumerable<Cell> cells) : this()
        {
            Cells.AddRange(cells);
        }

        public int CellCount
        {
            get { return Cells.Count; }
        }

        public void AddEmptyCell()
        {
            Cells.Add(Cell.Empty);
        }

        public void AddCell(Cell cell)
        {
            Cells.Add(cell);
        }

        public void AddEmptyCells(int cellCount)
        {
            for (int i=0; i< cellCount; i++)
            {
                AddEmptyCell();
            }
        }

        public bool IsEmpty()
        {
            return Cells.Count == 0 || Cells.TrueForAll(cell => cell.IsEmpty());
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var cell in Cells)
            {
                sb.AppendFormat("{0} {1}", cell, " | ");
            }
            return sb.ToString();
        }

        // note: true only for excel comments. other comments may have different indicators
        public bool IsComment()
        {
            return !Cells[0].IsEmpty();
        }
    }
}