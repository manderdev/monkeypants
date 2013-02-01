using System.IO;

namespace MonkeyPants.Reading
{
	public class TextFileReader : ITestFileReader
	{
	    private readonly DelimitedTextReader textReader;

	    public TextFileReader(char columnDelimiter)
	    {
	        textReader = new DelimitedTextReader(columnDelimiter);
        }

	    public RawTest[] Read(string filePathAndName)
	    {
			using (StreamReader reader = File.OpenText(filePathAndName))
			{
				string input = reader.ReadToEnd();
				return textReader.Read(input);
			}
		}
	}
}