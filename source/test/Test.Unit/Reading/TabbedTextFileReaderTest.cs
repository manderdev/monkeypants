using MonkeyPants.Reading;
using NUnit.Framework;
using Test.Unit.SampleInput;

namespace Test.Unit.Reading
{
	[TestFixture]
	public class TabbedTextFileReaderTest
	{
		[Test]
		public void ShouldDoSomething()
		{
            RawTest[] tests = new TabbedTextFileReader().Read(Files.TabDelimited.TabDelimitedMessWithWords);
			Assert.AreEqual(1, tests.Length);
		}
	}
}