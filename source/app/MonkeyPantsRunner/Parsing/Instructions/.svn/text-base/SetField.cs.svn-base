using System.Reflection;
using MonkeyPants.Execution;

namespace MonkeyPants.Parsing.Instructions
{
    public class SetField : ResultInstruction
    {
        private readonly FieldInfo fieldInfo;
        private readonly DataType dataCacheTargetKey;

        public SetField(FieldInfo fieldInfo, DataType dataCacheTargetKey, int column, MethodInfo parserMethod)
            : base(column, parserMethod)
        {
            this.fieldInfo = fieldInfo;
            this.dataCacheTargetKey = dataCacheTargetKey;
        }

        protected override object ExecuteWithResult(string expectedValue)
        {
            object target = DataCache[dataCacheTargetKey];
            fieldInfo.SetValue(target, Parse(expectedValue));
            return expectedValue;
        }
    }
}