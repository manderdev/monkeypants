using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Test.Unit.SampleInput
{
    public static class Files
    {
        public static class Config
        {
            public const string GoodConfig = "SampleInput/sampleMonkeyPants.config";            
        }

        public static class Csv
        {
            public const string MessWithWords = "SampleInput/Text/Csv/MessWithWords.csv";

            public const string Mathematics = "SampleInput/Text/Csv/Mathematics.csv";
            public const string MathematicsMessyButGood = "SampleInput/Text/Csv/MathematicsMessyButGood.csv";
            public const string MathematicsWithFailures = "SampleInput/Text/Csv/MathematicsWithFailures.csv";
            public const string MathematicsWithErrors = "SampleInput/Text/Csv/MathematicsWithErrors.csv";

            public const string SearchPeopleAndNoResults = "SampleInput/Text/Csv/SearchPeopleAndNoResults.csv";
            public const string SearchPeopleAndResults = "SampleInput/Text/Csv/SearchPeopleAndResults.csv";
            public const string SearchPeopleAndNotEnoughResults = "SampleInput/Text/Csv/SearchPeopleAndNotEnoughResults.csv";
            public const string SearchPeopleAndTooManyResults = "SampleInput/Text/Csv/SearchPeopleAndTooManyResults.csv";
            public const string SearchPeopleWithActionParameter = "SampleInput/Text/Csv/SearchPeopleWithActionParameter.csv";
            public const string SearchPeopleNoActionParameter = "SampleInput/Text/Csv/SearchPeopleNoActionParameter.csv";
            public const string SearchPeopleNoActionParameterAndResults = "SampleInput/Text/Csv/SearchPeopleNoActionParameterAndResults.csv";
            public const string SearchResults = "SampleInput/Text/Csv/SearchResults.csv";
        }

        public static class Excel2003
        {
            public const string MessWithWordsAndMathematics = "SampleInput/Excel/2003Xml/MessWithWordsAndMathematics.xml";            
        }

        public static class TabDelimited
        {
            public const string TabDelimitedMessWithWords = "SampleInput/Text/TabDelimited/MessWithWords.txt";
        }

        public static class Assemblies
        {
            // todo: a test to verify all these are here and named as such would help troubleshoot
            public static List<Assembly> TestUnitList
            {
                // note: MonkeyPants.exe contains default, built-in fixtures. They are still configurable, though, in case someone wants to re-implement some other way without altering MonkeyPants source
                get { return new List<Assembly> {Assembly.LoadFile(Path.GetFullPath("Test.Unit.dll")), Assembly.LoadFile(Path.GetFullPath("MonkeyPants.exe"))}; }
            }
        }
    }
}
