namespace Test.Unit.SampleFixtures
{
	public class MessWithWordsFixture
	{
		public bool ContainsLetterI { get; private set; }

		public string Append(MessWithWordsData data)
		{
			string result = data.FirstWord + data.SecondWord;
			ContainsLetterI = result.ToLower().Contains("i");
			return result;
		}

		public string CharacterCount(MessWithWordsData data)
		{
			return (data.FirstWord.Length + data.SecondWord.Length).ToString();
		}
	}

	public class MessWithWordsData
	{
	    public MessWithWordsData()
	    {
	        FirstWord = string.Empty;
	        SecondWord = string.Empty;
	    }

	    public string FirstWord { get; set; }
		public string SecondWord { get; set; }
	}
}