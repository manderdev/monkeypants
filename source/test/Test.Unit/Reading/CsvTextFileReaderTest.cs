using System.Collections.Generic;
using MonkeyPants.Reading;
using NUnit.Framework;
using Test.Unit.SampleInput;

namespace Test.Unit.Reading
{
	[TestFixture]
	public class CsvTextFileReaderTest
	{
        private CsvTextFileReader csvFileReader;

        [SetUp]
        public void Setup()
        {
            csvFileReader = new CsvTextFileReader();
        }

        [Test]
        public void ShouldProduceSingleTestFromSimpleInput()
        {
            RawTest[] rawTests = csvFileReader.Read(Files.Csv.Mathematics);
            Assert.AreEqual(1, rawTests.Length);
        }

        [Test]
        public void ShouldProduceSingleTestFromMessyInput()
        {
            RawTest[] rawTests = csvFileReader.Read(Files.Csv.MathematicsMessyButGood);
            Assert.AreEqual(1, rawTests.Length);
        }

        [Test]
        public void ShouldProduceTwoTestsFromInputWithTwoTablesSeparatedByANewline()
        {
            RawTest[] rawTests = csvFileReader.Read(Files.Csv.SearchPeopleAndResults);
            Assert.AreEqual(2, rawTests.Length);
        }

		[Test]
		public void ShouldProduceDecentRawTest()
		{
            RawTest[] tests = csvFileReader.Read(Files.Csv.MessWithWords);
			Assert.AreEqual(1, tests.Length);
			RawTest test = tests[0];
			Assert.AreEqual("Test.Unit.SampleFixtures.MessWithWords", test.Title);
			Assert.AreEqual(4, test.Headers.Count);
			List<string> headers = test.Headers;
			Assert.AreEqual("First Word", headers[0]);
			Assert.AreEqual("Second Word", headers[1]);
			Assert.AreEqual("Append?", headers[2]);
			Assert.AreEqual("Character Count?", headers[3]);
			Assert.AreEqual(4, test.DataRows.Count);
			test.DataRows.ForEach(dataRow => Assert.IsTrue(dataRow.CellCount == 4));
		}

	}
}