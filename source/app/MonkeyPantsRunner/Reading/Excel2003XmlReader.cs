using System;
using System.IO;
using MonkeyPants.Reading.Interpreters;
using MonkeyPants.Reading.Tables;

namespace MonkeyPants.Reading
{
    public class Excel2003XmlReader : ITestFileReader
    {
        public RawTest[] Read(string filePathAndName)
        {
            // Can't just say File.OpenRead(...) otherwise we throw a File.IO exception if the file 
            // is open somewhere else (e.g. in Excel).
            FileStream stream = new FileStream(filePathAndName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            using (StreamReader reader = new StreamReader(stream))
            {
                var xmlReader = XmlReaderAdapter.Create(reader);
                Table table = new Excel2003XmlReaderInstance(xmlReader).ReadTable();

                var tableConverter = new ExcelTableInterpreter();
                return tableConverter.TranslateScenario(table);
            }
        }

        private class Excel2003XmlReaderInstance
        {
            private readonly XmlReaderAdapter reader;

            public Excel2003XmlReaderInstance(XmlReaderAdapter xmlReader)
            {
                reader = xmlReader;
            }

            public Table ReadTable()
            {
                reader.MoveToNextElement("Table");

                var table = new Table();
                while (true)
                {
                    reader.MoveToNextElement();

                    if (reader.IsStartElement("Row"))
                    {
                        ReadRow(table);
                    }

                    if (reader.IsEndElement("Table"))
                    {
                        return table;
                    }
                }
            }

            void ReadRow(Table table)
            {
                reader.AssertIsStartElement("Row");

                bool isEmptyRow = reader.IsEmptyElement;
                if (isEmptyRow)
                {
                    table.AddEmptyRow();
                }

                CorrectForRowIndex(table);

                if (! isEmptyRow)
                {
                    var row = new Row();
                    table.AddRow(row);
                    ReadRowCells(row);
                }
            }

            private void ReadRowCells(Row row)
            {
                while (true)
                {
                    reader.MoveToNextElement();

                    if (reader.IsStartElement("Cell"))
                    {
                        ReadCell(row);
                    }

                    if (reader.IsEndElement("Row"))
                    {
                        return;
                    }
                }
            }

            void ReadCell(Row row)
            {
                reader.AssertIsStartElement("Cell");

                bool isEmptyCell = reader.IsEmptyElement;
                if (isEmptyCell)
                    row.AddEmptyCell();

                CorrectForCellIndex(row);

                if (!isEmptyCell)
                    ReadCellData(row);
            }

            private void ReadCellData(Row row)
            {
                bool dataRead = false;
                while (true)
                {
                    reader.MoveToNextElement();
                    if (reader.IsStartElement("Data"))
                    {
                        if (dataRead) throw new ApplicationException("Multiple Data elements within a Cell element");

                        row.AddCell(new Cell(ReadData()));
                        dataRead = true;
                    }

                    if (reader.IsEndElement("Cell"))
                    {
                        if (!dataRead) row.AddEmptyCell();
                        return;
                    }
                }
            }

            private string ReadData()
            {
                reader.AssertIsStartElement("Data");
                if (reader.MoveToAttribute("ss:Type"))
                {
                    string typeName = reader.ReadContentAsString();
                    reader.MoveToElement();

                    // todo: this list is likely incomplete. consider datetime
                    // Certain types format other than as shown in excel. Boolean, for example, becomes 1 or 0 while showing TRUE or FALSE.
                    // If the type formats as displayed, though, we'll fall through here and just treat it as a string.
                    if ("Boolean".Equals(typeName))
                    {
                        return reader.ReadElementContentAsBoolean().ToString();
                    }
                }
                // default to string
                return reader.ReadElementContentAsString();
            }
            
            private int GetMissingCount(int count)
            {
                return reader.ReadContentAsInt() - 1 - count;
            }

            private void CorrectForRowIndex(Table table)
            {
                while (reader.MoveToNextAttribute())
                {
                    if (IsIndexAttribute())
                    {
                        table.AddEmptyRows(GetMissingCount(table.RowCount));
                    }
                }
            }

            private void CorrectForCellIndex(Row row)
            {
                while (reader.MoveToNextAttribute())
                {
                    if (IsIndexAttribute())
                    {
                        row.AddEmptyCells(GetMissingCount(row.CellCount));
                    }
                }
            }

            private bool IsIndexAttribute()
            {
                return reader.IsAttribute("ss:Index");
            }
        }
    }
}
