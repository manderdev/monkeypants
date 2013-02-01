using System.Reflection;
using MonkeyPants.Execution;

namespace MonkeyPants.Parsing.Instructions
{
    public class GetField : ResultInstruction
    {
        private readonly FieldInfo fieldInfo;

        public GetField(FieldInfo fieldInfo, int column) : base(column)
        {
            this.fieldInfo = fieldInfo;
        }

        protected override object ExecuteWithResult(string expectedValue)
        {
            object target = DataCache[DataType.Enumerated] ?? DataCache[DataType.Source];
            return target == RealTest.MISSING_ROW ? RealTest.MISSING_ROW : fieldInfo.GetValue(target);
        }
    }
}