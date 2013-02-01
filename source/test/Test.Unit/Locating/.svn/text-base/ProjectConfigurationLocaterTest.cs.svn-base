using System.IO;
using MonkeyPants.Locating;
using MonkeyPants;
using NUnit.Framework;
using Test.Unit.SampleInput;

namespace Test.Unit.Locating
{
    [TestFixture]
    public class ProjectConfigurationLocaterTest
    {
        [Test]
        public void ShouldLocateExistingProjectConfig()
        {
            var locater = new ProjectConfigurationLocater(Files.Excel2003.MessWithWordsAndMathematics, "*MonkeyPants.config");
            string configPath = locater.Locate();
            Assert.AreEqual(Path.GetFullPath(Files.Config.GoodConfig), configPath);
        }

        [Test, ExpectedException(typeof(MonkeyPantsApplicationException), @"File matching '*notThere.config' not found. Searched upward starting at SampleInput\Excel\2003Xmland reaching C:\")]
        public void ShouldFailForNonExistentFile()
        {
            var locater = new ProjectConfigurationLocater(Files.Excel2003.MessWithWordsAndMathematics, "*notThere.config");
            string configPath = locater.Locate();
            Assert.AreEqual(Path.GetFullPath(Files.Config.GoodConfig), configPath);
        }

        [Test, ExpectedException(typeof(MonkeyPantsApplicationException), @"Found multiple configuration files in SampleInput\Text\Csv matching *.csv")]
        public void ShouldFailForMultipleFiles()
        {
            // cheating a bit and using csv instead of config files. Prinicple is the same, though. Should fail if more than one thing matches. This
            // of course assumes there are multiple .csv files. But there are. So we're good.
            var locater = new ProjectConfigurationLocater(Files.Csv.Mathematics, "*.csv");
            string configPath = locater.Locate();
            Assert.AreEqual(Path.GetFullPath(Files.Config.GoodConfig), configPath);
        }
    }
}
