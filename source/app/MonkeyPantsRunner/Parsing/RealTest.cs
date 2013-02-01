using System;
using System.Collections.Generic;
using MonkeyPants.Execution;
using MonkeyPants.Output;
using MonkeyPants.Results;

namespace MonkeyPants.Parsing
{
    public class RealTest : IInstructionParent
    {
        public static readonly MissingRow MISSING_ROW = new MissingRow();

        private readonly string title;
        private readonly List<string> headers;
        private readonly List<RowResult> testResults;

        public DataCache DataCache { get; private set; }
        public UserCache UserCache { get; private set; }
        public List<IInstruction> ChildInstructions { get; private set; }

        public RealTest(string title, List<string> headers, UserCache userCache)
        {
            this.title = title;
            this.headers = headers;
            UserCache = userCache;
            DataCache = new DataCache();
            testResults = new List<RowResult>();
            ChildInstructions = new List<IInstruction>();
        }

        public void AddInstruction(IInstruction instruction)
        {
            instruction.Parent = this;
            ChildInstructions.Add(instruction);
        }

        public RowResult RowResult
        {
            get { throw new NotImplementedException(); }
        }

        public void AddRowResult(RowResult result, IResultsWriter resultsWriter)
        {
            resultsWriter.Begin(result);
            testResults.Add(result);
            resultsWriter.End(result);
        }

        public TestResult Test(IResultsWriter resultsWriter)
        {
            Execute(resultsWriter);
            return CreateResult();
        }

        public void Execute(IResultsWriter resultsWriter)
        {
            var result = CreateResult();
            resultsWriter.Begin(result);
            ChildInstructions.ForEach(instruction => instruction.Execute(resultsWriter));
            resultsWriter.End(result);
        }

        public IInstructionParent Parent
        {
            get { throw new NotSupportedException("RealTest is the root node, has no parent"); }
            set { throw new NotSupportedException("RealTest is the root node, has no parent"); }
        }

        private TestResult CreateResult()
        {
            return new TestResult(title, headers, testResults);
        }
    }
}