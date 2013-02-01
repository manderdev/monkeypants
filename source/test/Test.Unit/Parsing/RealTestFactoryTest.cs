using System.Collections.Generic;
using MonkeyPants.Execution;
using MonkeyPants.Parsing;
using MonkeyPants.Parsing.Instructions;
using MonkeyPants.Reading;
using MonkeyPants.Reading.Tables;
using NUnit.Framework;
using Test.Unit.SampleFixtures;
using Test.Unit.SampleInput;

namespace Test.Unit.Parsing
{
    [TestFixture]
    public class RealTestFactoryTest
    {
        private RawTest.RealTestFactory testFactory;
        private UserCache userCache;
        private CsvTextFileReader csvReader;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            csvReader = new CsvTextFileReader();
        }

        [SetUp]
        public void Setup()
        {
            userCache = new UserCache();
            testFactory = new RawTest.RealTestFactory(new Assemblies(Files.Assemblies.TestUnitList), userCache);
        }

        private static RawTest CreateRawTest(string title)
        {
            return CreateRawTest(title, new List<string> {"h1"});
        }

        private static RawTest CreateRawTest(string title, List<string> headers)
        {
            return CreateRawTest(title, headers, new List<Row>());
        }

        private static RawTest CreateRawTest(string title, List<string> headers, List<Row> data)
        {
            return new RawTest(title, new List<string>(), new List<string>(), headers, data);
        }

        [Test]
        public void ShouldConstructInputTestWithParameterOnActionMethod()
        {
            RawTest raw = csvReader.Read(Files.Csv.SearchPeopleWithActionParameter)[0];
            var test = testFactory.CreateRealTestFrom(raw);
            Assert.AreEqual(2, test.ChildInstructions.Count);
            Assert.IsInstanceOfType(typeof (InstantiateSource), test.ChildInstructions[0]);
            Assert.IsInstanceOfType(typeof (RepeatOverRows), test.ChildInstructions[1]);
            var instructionSet = (RepeatOverRows) test.ChildInstructions[1];
            List<IInstruction> instructions = instructionSet.ChildInstructions;
            Assert.AreEqual(5, instructions.Count);
            Assert.IsInstanceOfType(typeof (NextDataRow), instructions[0]);
            Assert.IsInstanceOfType(typeof (InstantiateActionParameter), instructions[1]);
            Assert.IsInstanceOfType(typeof (SetMethod), instructions[2]);
            Assert.IsInstanceOfType(typeof (SetMethod), instructions[3]);
            Assert.IsInstanceOfType(typeof (CallActionWithParameter), instructions[4]);
        }

        [Test]
        public void ShouldCreateInputTestFromClassAndProperties()
        {
            RawTest raw = csvReader.Read(Files.Csv.SearchPeopleNoActionParameter)[0];
            var test = testFactory.CreateRealTestFrom(raw);
            Assert.AreEqual(2, test.ChildInstructions.Count);
            Assert.IsInstanceOfType(typeof (InstantiateSource), test.ChildInstructions[0]);
            Assert.IsInstanceOfType(typeof (RepeatOverRows), test.ChildInstructions[1]);
            var repeatInstruction = (RepeatOverRows) test.ChildInstructions[1];
            List<IInstruction> instructions = repeatInstruction.ChildInstructions;
            Assert.IsInstanceOfType(typeof (NextDataRow), instructions[0]);
            Assert.IsInstanceOfType(typeof (SetMethod), instructions[1]);
            Assert.IsInstanceOfType(typeof (SetMethod), instructions[2]);
        }

        [Test]
        public void ShouldCreateOutputTestFromCache()
        {
            RawTest raw = csvReader.Read(Files.Csv.SearchResults)[0];
            userCache["People Results"] = SearchPeopleFixture.PretendToRetrievePeople(new SearchPeopleParameters
                                                                                   {
                                                                                       FirstName = "Buzz",
                                                                                       LastName = "Lightyear"
                                                                                   });
            var test = testFactory.CreateRealTestFrom(raw);
            Assert.AreEqual(4, test.ChildInstructions.Count);
            Assert.IsInstanceOfType(typeof (RetrieveCachedSource), test.ChildInstructions[0]);
            Assert.IsInstanceOfType(typeof (CreateOutputEnumerator), test.ChildInstructions[1]);
            Assert.IsInstanceOfType(typeof (CountActualRows), test.ChildInstructions[2]);
            Assert.IsInstanceOfType(typeof (RepeatOverRows), test.ChildInstructions[3]);
            var instructionSet = (RepeatOverRows) test.ChildInstructions[3];
            List<IInstruction> instructions = instructionSet.ChildInstructions;
            Assert.AreEqual(5, instructions.Count);
            Assert.IsInstanceOfType(typeof (NextDataRow), instructions[0]);
            Assert.IsInstanceOfType(typeof (NextActualRow), instructions[1]);
            Assert.IsInstanceOfType(typeof (GetMethod), instructions[2]);
            Assert.IsInstanceOfType(typeof (GetMethod), instructions[3]);
            Assert.IsInstanceOfType(typeof (GetMethod), instructions[4]);
        }

        [Test, ExpectedException(typeof (MalformedTestException))]
        public void ShouldNotCreateTestFromBizarreTitle()
        {
            RawTest rawTestWithBizarreTitle = CreateRawTest("a=>b=>c");
            testFactory.CreateRealTestFrom(rawTestWithBizarreTitle);
        }

        [Test, ExpectedException(typeof (MalformedTestException))]
        public void ShouldNotCreateTestFromBlankTitle()
        {
            RawTest rawTestWithEmptyTitle = CreateRawTest(" ");
            testFactory.CreateRealTestFrom(rawTestWithEmptyTitle);
        }

        [Test, ExpectedException(typeof (MalformedTestException))]
        public void ShouldNotCreateTestWhenTitleHasNoSource()
        {
            RawTest rawTestWithNoSourceTitle = CreateRawTest("=>c");
            testFactory.CreateRealTestFrom(rawTestWithNoSourceTitle);
        }
    }
}