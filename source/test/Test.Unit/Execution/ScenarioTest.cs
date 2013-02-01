using MonkeyPants.Execution;
using MonkeyPants.Output;
using MonkeyPants.Output.Channels;
using MonkeyPants.Reading;
using MonkeyPants.Results;
using NUnit.Framework;
using Test.Unit.SampleInput;

namespace Test.Unit.Execution
{
	[TestFixture]
	public class ScenarioTest
	{
		private CsvTextFileReader csvReader;

		[TestFixtureSetUp]
		public void TestFixtureSetUp()
		{
			csvReader = new CsvTextFileReader();
		}

		[Test]
		public void ShouldCreateAnActionTesterAndSuccessfullyTestIt()
		{
            ScenarioResult result = ExecuteScenarioFor("Action Test", Files.Csv.SearchPeopleNoActionParameter);
			Assert.AreEqual(result.TotalCount, result.PassCount);
		}

		[Test]
		public void ShouldCreateAScenarioForBothInputAndOutputAndTestingShouldPass()
		{
            ScenarioResult result = ExecuteScenarioFor("I/O Test", Files.Csv.SearchPeopleNoActionParameterAndResults);
			Assert.AreEqual(result.TotalCount, result.PassCount);
		}

		[Test]
		public void ShouldCreateAScenarioForMathematics()
		{
            ScenarioResult result = ExecuteScenarioFor("Math Test", Files.Csv.Mathematics);
			Assert.AreEqual(result.TotalCount, result.PassCount);
		}

		[Test]
		public void ShouldExecuteScenarioWithActionParameters()
		{
            ScenarioResult result = ExecuteScenarioFor("Action Parameter Test", Files.Csv.SearchPeopleWithActionParameter);
			Assert.AreEqual(result.TotalCount, result.PassCount);
		}

		[Test]
		public void ShouldExecuteScenarioForMathematicsWithOneFailingRow()
		{
            ScenarioResult result = ExecuteScenarioFor("Failing Math Test", Files.Csv.MathematicsWithFailures);
			Assert.AreEqual(1, result.TotalCount);
			Assert.AreEqual(1, result.FailureCount);
		}

		[Test]
		public void ShouldExecuteScenarioForMathematicsWithOneErrorRow()
		{
            ScenarioResult result = ExecuteScenarioFor("Failing Math Test", Files.Csv.MathematicsWithErrors);
			Assert.AreEqual(1, result.TotalCount);
			Assert.AreEqual(1, result.ErrorCount);
		}

		private ScenarioResult ExecuteScenarioFor(string name, string fileName)
		{
			RawTest[] rawTests = csvReader.Read(fileName);
			var acceptanceScenario = new Scenario(name, new Assemblies(Files.Assemblies.TestUnitList));
            return acceptanceScenario.Test(rawTests, new SimpleTextResultsWriter(new StringOutputChannel()));
		}
	}

    public class NullResultsWriter : MulticastResultsWriter
    {
    }
}
