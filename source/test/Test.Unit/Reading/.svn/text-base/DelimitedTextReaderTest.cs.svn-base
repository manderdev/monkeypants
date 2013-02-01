using System;
using MonkeyPants.Execution;
using MonkeyPants.Reading;
using NUnit.Framework;

namespace Test.Unit.Reading
{
	[TestFixture]
	public class DelimitedTextReaderTest
	{
		private DelimitedTextReader textReader;

		[SetUp]
		public void Setup()
		{
			textReader = new DelimitedTextReader('\t');
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void ShouldThrowExceptionOnNullString()
		{
			textReader.Read(null);
		}

		[Test]
		public void ShouldProduceEmptyArrayFromEmptyString()
		{
			Assert.IsEmpty(textReader.Read(string.Empty));
		}

        [Test, Ignore("AutoExecute fixture currently makes this irrelevant, but it is relevent if NOT autoexecute, so should tighten to know the diff"), ExpectedException(typeof(MalformedTestException))]
		public void ShouldFailValidationOnNoHeaders()
		{
			textReader.Read("fixtureName");
		}

		[Test, ExpectedException(typeof(MalformedTestException))]
		public void ShouldFailValidationOnSomeBlankHeader()
		{
			textReader.Read("fixtureName\nheader1\t\theader3\ndata1\tdata2\tdata3");
		}

		[Test, ExpectedException(typeof(MalformedTestException))]
		public void ShouldFailValidationOnMismatchedHeadersAndData()
		{
			textReader.Read("fixtureName\nheader1\ndata1\tdata2");
		}

		[Test]
		public void ShouldPassValidationOnHeadersButNoData()
		{
			textReader.Read("fixtureName\nheader1");
		}
	}
}