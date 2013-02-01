using System.Reflection;
using MonkeyPants.Execution;

namespace MonkeyPants.Parsing.Instructions
{
    public class GetMethod : ResultInstruction
    {
        private readonly MethodInfo methodInfo;

        public GetMethod(MethodInfo methodInfo, int column) : base(column)
        {
            this.methodInfo = methodInfo;
        }

        protected override object ExecuteWithResult(string expectedValue)
        {
            object target = DataCache[DataType.Enumerated] ?? DataCache[DataType.Source];
            return target == RealTest.MISSING_ROW ? RealTest.MISSING_ROW : methodInfo.Invoke(target, null);
        }
    }
}