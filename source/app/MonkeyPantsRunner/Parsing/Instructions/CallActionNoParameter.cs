using System.Reflection;
using MonkeyPants.Execution;

namespace MonkeyPants.Parsing.Instructions
{
    public class CallActionNoParameter : ResultInstruction
    {
        private readonly MethodInfo methodInfo;

        public CallActionNoParameter(MethodInfo methodInfo, int column) : base(column)
        {
            this.methodInfo = methodInfo;
        }

        protected override object ExecuteWithResult(string expectedValue)
        {
            object targetSource = DataCache[DataType.Source];
            return methodInfo.Invoke(targetSource, null);
        }
    }
}