using System.Reflection;
using MonkeyPants.Execution;

namespace MonkeyPants.Parsing.Instructions
{
    public class SetMethod : ResultInstruction
    {
        private readonly MethodInfo methodInfo;
        private readonly DataType dataCacheTargetKey;
        private readonly object[] methodArgsArray = new object[1];

        public SetMethod(MethodInfo methodInfo, DataType dataCacheTargetKey, int column, MethodInfo parserMethod)
            : base(column, parserMethod)
        {
            this.methodInfo = methodInfo;
            this.dataCacheTargetKey = dataCacheTargetKey;
        }

        protected override object ExecuteWithResult(string expectedValue)
        {
            object target = DataCache[dataCacheTargetKey];
            methodArgsArray[0] = Parse(expectedValue);
            methodInfo.Invoke(target, methodArgsArray);
            return expectedValue;
        }
    }
}