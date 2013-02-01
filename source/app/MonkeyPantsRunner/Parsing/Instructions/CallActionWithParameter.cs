using System.Reflection;
using MonkeyPants.Execution;

namespace MonkeyPants.Parsing.Instructions
{
    public class CallActionWithParameter : ResultInstruction
    {
        private readonly MethodInfo methodInfo;
        private readonly object[] actionParameterArray = new object[1];

        public CallActionWithParameter(MethodInfo methodInfo, int column) : base(column)
        {
            this.methodInfo = methodInfo;			
        }

        protected override object ExecuteWithResult(string expectedValue)
        {
            actionParameterArray[0] = DataCache[DataType.ActionParameter];
            object targetSource = DataCache[DataType.Source];
            return methodInfo.Invoke(targetSource, actionParameterArray);
        }
    }
}