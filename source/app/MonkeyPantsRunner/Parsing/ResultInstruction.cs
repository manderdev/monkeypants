using System;
using System.Reflection;
using MonkeyPants.Execution;
using MonkeyPants.Output;
using MonkeyPants.Reading;
using MonkeyPants.Reading.Tables;
using MonkeyPants.Results;

namespace MonkeyPants.Parsing
{
    public abstract class ResultInstruction : Instruction
    {
        private readonly MethodInfo parserMethod;
        private readonly int column;

        private readonly object[] parserArgs = new object[1];

        protected ResultInstruction(int column) : this(column, null)
        {}

        protected ResultInstruction(int column, MethodInfo parserMethod)
        {
            this.column = column;
            this.parserMethod = parserMethod;
        }

        protected abstract object ExecuteWithResult(string expectedValue);

        protected object Parse(string expectedValue)
        {
            if (parserMethod == null)
                return expectedValue;

            parserArgs[0] = expectedValue;
            return parserMethod.Invoke(null, parserArgs);
        }

        public override void Execute(IResultsWriter resultsWriter)
        {
            CellResult cellResult = Evaluate();
            resultsWriter.Begin(cellResult);
            Parent.RowResult.Add(cellResult);
            resultsWriter.End(cellResult);
        }

        private CellResult Evaluate()
        {
            object expectedRow = DataCache[DataType.DataRow];
            bool isMissingExpectedRow = (expectedRow == RealTest.MISSING_ROW);
            string expectedString = isMissingExpectedRow ? null : ((Row) expectedRow).Cells[column].Value;

            try
            {
                object actualResult = ExecuteWithResult(expectedString);
                if (actualResult == RealTest.MISSING_ROW)
                {
                    return new CellResult(expectedString, (string) null);
                }
                else if (isMissingExpectedRow)
                {
                    return new CellResult(null, actualResult.ToString());
                }
                else if (IsToBeCached(expectedString))
                {
                    UserCache[GetCacheKeyFrom(expectedString)] = actualResult;
                    return new CellResult(expectedString, expectedString);
                }
                else
                {
                    return new CellResult(expectedString, actualResult.ToString());
                }
            }
            catch (Exception exception)
            {
                // todo: guaranteed inner? what if there's no such method as fixture asks for?
                return new CellResult(expectedString, exception.InnerException);
            }
        }

        private static string GetCacheKeyFrom(string expectedValue)
        {
            return expectedValue.Substring(RawTest.RealTestFactory.CACHE_INDICATOR.Length);
        }

        private static bool IsToBeCached(string expectedValue)
        {
            return expectedValue.StartsWith(RawTest.RealTestFactory.CACHE_INDICATOR);
        }
    }
}