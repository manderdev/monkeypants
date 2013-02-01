using MonkeyPants.Execution;
using MonkeyPants.Output;
using MonkeyPants.Output.Channels;
using MonkeyPants.Reading;
using NUnit.Framework;
using Test.Unit.SampleInput;

namespace Test.Unit.Execution
{
	[TestFixture]
	public class TestRunnerTest
	{
		private TestRunner testRunner;
		private CsvTextFileReader csvReader;

		[TestFixtureSetUp]
		public void TestFixtureSetup()
		{
			testRunner = new TestRunner(new Assemblies(Files.Assemblies.TestUnitList));
			csvReader = new CsvTextFileReader();
		}

		[Test]
		public void ShouldPassMathematicsScenario()
		{
            string results = Test("Math Test", Files.Csv.MessWithWords);
			Assert.IsTrue(results.Contains("PASS"));
		}

		[Test]
		public void ShouldFailMathScenario()
		{
            string results = Test("Math With Failure", Files.Csv.MathematicsWithFailures);
			Assert.IsTrue(results.Contains("FAILURE: 1 ran: 0 passed; 1 failed; 0 had errors"));
		}

		[Test]
		public void ShouldErrorMathScenario()
		{
			string results = Test("Math With Error", Files.Csv.MathematicsWithErrors);
			Assert.IsTrue(results.Contains("ERROR: 1 ran: 0 passed; 0 failed; 1 had errors"));
		}

		[Test]
		public void ShouldPassSearchWithGoodResults()
		{
            string results = Test("Search With Good Results", Files.Csv.SearchPeopleAndResults);
			Assert.IsTrue(results.Contains("PASS"));
		}

		[Test]
		public void ShouldFailSearchWithNotEnoughActualResults()
		{
            string results = Test("Search With Missing Actual", Files.Csv.SearchPeopleAndNotEnoughResults);
			Assert.IsTrue(results.Contains("FAILURE: 2 ran: 1 passed; 1 failed; 0 had errors"));
		}

		[Test]
		public void ShouldFailSearchWithTooManyActualResults()
		{
            string results = Test("Search With Too Many Actuals", Files.Csv.SearchPeopleAndTooManyResults);
			Assert.IsTrue(results.Contains("FAILURE: 2 ran: 1 passed; 1 failed; 0 had errors"));
		}

		[Test]
		public void ShouldHandleBlanksAndMultipleActions()
		{
            string results = Test("Append Blanks", Files.Csv.MessWithWords);
			Assert.IsTrue(results.Contains("PASS"));
		}

		[Test]
		public void ShouldHandleEmptyResultSets()
		{
            string results = Test("Search With No Results", Files.Csv.SearchPeopleAndNoResults);
			Assert.IsTrue(results.Contains("PASS"));
		}

		private string Test(string scenarioName, string fileName)
		{
			RawTest[] rawTests = csvReader.Read(fileName);
		    StringOutputChannel resultsChannel = new StringOutputChannel();            
		    IResultsWriter resultsWriter = new SimpleTextResultsWriter(resultsChannel);
			testRunner.Test(scenarioName, rawTests, resultsWriter);
			return resultsChannel.Content;
		}
	}
}