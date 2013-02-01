using System;
using System.IO;
using MonkeyPants.Reading.Interpreters;
using MonkeyPants.Reading.Tables;

namespace MonkeyPants.Reading
{
	public class DelimitedTextReader
    {
	    private readonly char delimiter;

	    public DelimitedTextReader(char delimiter)
	    {
	        this.delimiter = delimiter;
	    }

	    public RawTest[] Read(string text)
        {
			StringReader reader = new StringReader(text);
		    Table table = ReadTable(reader);
		    return Interpret(table);
        }

	    private Table ReadTable(TextReader reader)
        {
            Table table = new Table();
            string line;
            while ((line = reader.ReadLine()) != null)
            {
                Row row = ReadRow(line);
                table.AddRow(row);
            }
	        return table;
        }

	    private Row ReadRow(string line)
	    {
	        string[] strings = line.Split(delimiter);
	        Cell[] cells = Array.ConvertAll(strings, value => new Cell(value));
	        return new Row(cells);
	    }

	    private RawTest[] Interpret(Table table)
	    {
	        DelimitedTextTableInterpreter tableInterpreter = new DelimitedTextTableInterpreter();
	        return tableInterpreter.TranslateScenario(table);
	    }
    }
}