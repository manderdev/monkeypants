using System;
using MonkeyPants.Reading;
using MonkeyPants.Reading.Tables;
using NUnit.Framework;
using Test.Unit.SampleInput;

namespace Test.Unit.Reading
{
	[TestFixture]
	public class RawTestTest
	{
		private CsvTextFileReader reader;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			reader = new CsvTextFileReader();
		}

		[Test]
		public void ShouldProduceSingleProperlyFormattedTest()
		{
            RawTest[] rawTests = reader.Read(Files.Csv.Mathematics);
			AssertTestIsProperlyFormatted(rawTests[0]);
		}

		[Test]
		public void ShouldProduceTwoProperlyFormattedTests()
		{
            RawTest[] rawTests = reader.Read(Files.Csv.SearchPeopleAndResults);
			AssertTestIsProperlyFormatted(rawTests[0]);
			AssertTestIsProperlyFormatted(rawTests[1]);
		}

		private static void AssertTestIsProperlyFormatted(RawTest rawTest)
		{
			Assert.IsFalse(string.IsNullOrEmpty(rawTest.Title));
			Assert.AreNotEqual(0, rawTest.Headers.Count);
			rawTest.Headers.ForEach(header => Assert.IsFalse(string.IsNullOrEmpty(header)));
			Assert.AreNotEqual(0, rawTest.DataRows);
			rawTest.DataRows.ForEach(dataRow=> AssertDataRowFilled(dataRow, rawTest.Headers.Count));
		}

		private static void AssertDataRowFilled(Row dataRow, int length)
		{
			Assert.AreEqual(length, dataRow.CellCount);
			dataRow.Cells.ForEach(cell => Assert.IsFalse(string.IsNullOrEmpty(cell.Value)));
		}
	}
}