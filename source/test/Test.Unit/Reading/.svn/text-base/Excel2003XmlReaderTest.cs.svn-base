using System.Collections.Generic;
using MonkeyPants.Reading;
using MonkeyPants.Reading.Tables;
using NUnit.Framework;
using Test.Unit.SampleInput;

namespace Test.Unit.Reading
{
    [TestFixture]
    public class Excel2003XmlReaderTest
    {
        [Test]
        public void ShouldProduceDecentRawTest()
        {
            RawTest[] tests = new Excel2003XmlReader().Read(Files.Excel2003.MessWithWordsAndMathematics);
            Assert.AreEqual(3, tests.Length);
            
            RawTest wordsTestWithNs = tests[0];
            Assert.AreEqual("Test.Unit.SampleFixtures.MessWithWords", wordsTestWithNs.Title);
            AssertComments(wordsTestWithNs.Comments, "This is a comment", "This is another comment");
            AssertHeaders(wordsTestWithNs.Headers, "First Word", "Second Word", "Append?", "Character Count?");
            AssertRowCount(4, wordsTestWithNs.DataRows);
            AssertCellCount(4, wordsTestWithNs.DataRows);

            RawTest wordsTestNoNs = tests[1];
            Assert.AreEqual("MessWithWords", wordsTestNoNs.Title);
            AssertComments(wordsTestNoNs.Comments, "Should also work with namespace missing from fixture name");
            AssertHeaders(wordsTestNoNs.Headers, "First Word", "Second Word", "Append?", "Character Count?");
            AssertRowCount(4, wordsTestNoNs.DataRows);
            AssertCellCount(4, wordsTestNoNs.DataRows);

            RawTest mathTest = tests[2];
            Assert.AreEqual("Test.Unit.SampleFixtures.Mathematics", mathTest.Title);
            AssertComments(mathTest.Comments, "This comment is a comment");
            AssertHeaders(mathTest.Headers, "Value1", "Value2", "Add?", "Multiply?", "Subtract?", "Divide?");
            AssertRowCount(2, mathTest.DataRows);
            AssertCellCount(6, mathTest.DataRows);
        }

        private void AssertComments(List<string> actual, params string[] expected)
        {
            AssertContent(actual, expected, "Incorrect comment count");
        }

        private void AssertHeaders(List<string> actual, params string[] expected)
        {
            AssertContent(actual, expected, "Incorrect header count");
        }

        /// <summary>
        /// Actual and expected are flipped from normal to reflect the calling methods, which flip them to support params[]
        /// </summary>
        private void AssertContent(List<string> actual, string[] expected, string message)
        {
            Assert.AreEqual(expected.Length, actual.Count, message);
            for (int i = 0; i < expected.Length; i++)
            {
                Assert.AreEqual(expected[i], actual[i]);
            }
        }

        private void AssertRowCount(int expected, List<Row> rows)
        {
            Assert.AreEqual(expected, rows.Count);
        }

        private void AssertCellCount(int expected, List<Row> rows)
        {
            rows.ForEach(dataRow => Assert.IsTrue(dataRow.CellCount == expected));
        }
    }
}
