using System;
using System.Collections.Generic;
using System.Reflection;
using MonkeyPants.Execution;
using MonkeyPants.Parsing;
using MonkeyPants.Parsing.Instructions;
using MonkeyPants.Reading.Tables;

namespace MonkeyPants.Reading
{
    public class RawTest
    {
        public List<string> Comments { get; private set; }
        public string Title { get; private set; }
		public List<string> Args { get; private set; }
		public List<string> Headers { get; private set; }
		public List<Row> DataRows { get; private set; }

        public RawTest(
            string title, List<string> comments, List<string> args, 
            List<string> headers, List<Row> dataRows)
        {
            Title = title;
            Comments = comments;
            Args = args;
            Headers = headers;
            DataRows = dataRows;
        }

        public override int GetHashCode()
    	{
    		unchecked
    		{
    			int result = (Title != null ? Title.GetHashCode() : 0);
    			result = (result*397) ^ (Headers != null ? Headers.GetHashCode() : 0);
    			result = (result*397) ^ (DataRows != null ? DataRows.GetHashCode() : 0);
    			return result;
    		}
    	}

        public void Validate()
		{
			AssertTitleExists(Title);            
			AssertHeadersExist(Headers);
			AssertDataRowsMatchHeaders(Headers, DataRows);
		}

		private void AssertTitleExists(string title)
		{
			if (string.IsNullOrEmpty(title))
			{
				throw new MalformedTestException("Cannot create test - no title");
			}
		}

		private void AssertHeadersExist(ICollection<string> headers)
		{
			foreach (string header in headers)
			{
				if (string.IsNullOrEmpty(header))
				{
					throw new MalformedTestException("Cannot create test - null or empty header. Title: " + Title);
				}
			}
		}

		private void AssertDataRowsMatchHeaders(ICollection<string> headers, List<Row> dataRows)
		{
			int headerLength = headers.Count;
            dataRows.ForEach(dataRow => AssertCorrectDataLength(dataRow, headerLength));
		}

		private void AssertCorrectDataLength(Row dataRow, int headerCellCount)
		{
		    int dataCellCount = dataRow.CellCount;
		    if (dataCellCount != headerCellCount)
			{
				throw new MalformedTestException(
                    string.Format("Cannot initialize test - mismatch in number of headers and data values. [headers {0}, cells {1}]. Title: {2}", headerCellCount, dataCellCount, Title));
			}
		}

        public bool Equals(RawTest other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(other.Title, Title) && Equals(other.Headers, Headers) && Equals(other.DataRows, DataRows);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != typeof(RawTest)) return false;
            return Equals((RawTest)obj);
        }

        public RealTest CreateRealTest(Assemblies assemblies, UserCache cache)
        {
            RealTestFactory testFactory = new RealTestFactory(assemblies, cache);
            return testFactory.CreateRealTestFrom(this);
        }

        public enum TestStyle { Input, Output }

        public class RealTestFactory
        {
            private const BindingFlags BINDING_FLAGS = BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance;
            private const BindingFlags PARSER_BINDING_FLAGS = BindingFlags.FlattenHierarchy | BindingFlags.Public | BindingFlags.Static;
            private static readonly Type[] PARSER_SINGLE_STRING_ARG = new[] { typeof(string) };
            private static readonly Type VOID_TYPE = typeof(void);

            public const string CACHE_INDICATOR = "=>";
            private const string ACTION_INDICATOR = "?";
            private static readonly Type GENERIC_ENUMERABLE_TYPE = typeof(IEnumerable<>);

            private readonly Assemblies assemblies;
            private readonly UserCache userCache;

            private TestStyle testStyle;
            private Type enumeratedType;

            private const string FIXTURE_ALIAS_SUFFIX = "Fixture";

            public RealTestFactory(Assemblies assemblies, UserCache userCache)
            {
                this.assemblies = assemblies;
                this.userCache = userCache;
            }

            public RealTest CreateRealTestFrom(RawTest rawTest)
            {
                var test = new RealTest(rawTest.Title, rawTest.Headers, userCache);

                // title is one of: (1) Class.Name (2) cache entry=> (3) cache entry=>Adapter.Class.Name
                Type targetType = AddInstructionsFromTitle(test, rawTest.Title);

                testStyle = DetermineTestType(targetType, rawTest.Headers);
                if (IsOutputTest)
                {
                    ManageActualRowEnumeration(test, targetType);
                }

                AddAutoExecutionWhereApplicable(rawTest, targetType);

                // instruction to repeat over every row of data
                IInstructionParent repeatOverRows = AddRepeatOverRows(test, rawTest.DataRows);

                // instructions repeated for each row of data
                AddRepeatedInstructions(repeatOverRows, rawTest, targetType);

                return test;
            }

            private void AddAutoExecutionWhereApplicable(RawTest rawTest, Type targetType)
            {
                //todo: duplication
                List<string> trimmedHeaders = rawTest.Headers.ConvertAll(input => NormalizeHeader(input));
                //todo: lowercase and +? action format assumptions; also string literal
                if (IsAutoExecuteFixture(targetType) && !trimmedHeaders.Contains("autoexecute?"))
                {
                    rawTest.Headers.Add("autoexecute?");
                    if (rawTest.DataRows.Count == 0) { rawTest.DataRows.Add(new Row()); }
                    rawTest.DataRows.ForEach(row1 => row1.AddCell(new Cell("True")));
                }
            }

            private void ManageActualRowEnumeration(IInstructionParent test, Type targetType)
            {
                // output tests tend to produce multiple rows of data; but if the target is not enumerable, make it a single entry array.
                Type type = GetPotentialEnumeratedTypeFrom(targetType);

                if (type != null)
                {
                    enumeratedType = type;
                }
                else
                {
                    test.AddInstruction(new WrapUnenumerableSource());
                }

                test.AddInstruction(new CreateOutputEnumerator());
                test.AddInstruction(new CountActualRows());
            }

            private static TestStyle DetermineTestType(Type targetType, List<string> headers)
            {
                return (IsAutoExecuteFixture(targetType) || headers.Exists(IsAction)) ? TestStyle.Input : TestStyle.Output;
            }

            private static bool IsAutoExecuteFixture(Type targetType)
            {
                return typeof (IAutoExecuteFixture).IsAssignableFrom(targetType);
            }

            private Type AddInstructionsFromTitle(IInstructionParent test, string title)
            {
                string[] titleValues = SplitAndTrimTitle(title);
                string sourceName = titleValues[0];
                bool isCachedSource = titleValues.Length == 2;

                Type targetType;
                // source may be a class or a cached value.
                if (isCachedSource)
                {
                    targetType = GetCachedSourceType(sourceName);
                    test.AddInstruction(new RetrieveCachedSource(sourceName));

                    // Adapters potentially exist only for cached values. 
                    string adapterClassName = titleValues[1];
                    if (!string.IsNullOrEmpty(adapterClassName))
                    {
                        targetType = assemblies.ResolveType(adapterClassName);
                        test.AddInstruction(new InstantiateAdapter(targetType));
                    }
                }
                else
                {
                    targetType = assemblies.ResolveType(sourceName, sourceName + FIXTURE_ALIAS_SUFFIX);
                    test.AddInstruction(new InstantiateSource(targetType));
                }
                return targetType;
            }

            private IInstructionParent AddRepeatOverRows(IInstructionParent parent, ICollection<Row> dataRows)
            {
                IInstructionParent dataRepeater = new RepeatOverRows(dataRows.Count, IsOutputTest);
                parent.AddInstruction(dataRepeater);
                return dataRepeater;
            }

            private void AddRepeatedInstructions(IInstructionParent parent, RawTest rawTest, Type targetType)
            {               
                parent.AddInstruction(new NextDataRow(rawTest.DataRows));

                List<string> trimmedHeaders = rawTest.Headers.ConvertAll(header => NormalizeHeader(header));
                List<IInstruction> instructions = (testStyle == TestStyle.Input)
                                                      ? CreateInputInstructions(trimmedHeaders, targetType)
                                                      : CreateOutputInstructions(trimmedHeaders, targetType);
                instructions.ForEach(parent.AddInstruction);
            }

            private string NormalizeHeader(string header)
            {
                return header.Replace(" ", string.Empty).ToLower();
            }

            private static List<IInstruction> CreateInputInstructions(IList<string> headers, Type sourceType)
            {
                var instructions = new List<IInstruction>();

                Type actualTargetType = sourceType;

                // input tests have action method(s), and they might expect a parameter that needs instantiation.
                MethodInfo[] actionMethodInfos = GetActionMethodInfos(sourceType, headers);
                Type actionParameterType = GetActionMethodParameterType(actionMethodInfos);
                bool hasActionParameter = actionParameterType != null;
                if (hasActionParameter)
                {
                    instructions.Add(new InstantiateActionParameter(actionParameterType));
                    actualTargetType = actionParameterType;
                }

                for (int column = 0; column < headers.Count; column++)
                {
                    IInstruction instruction = actionMethodInfos[column] != null
                                                   ? CreateActionInstruction(actionMethodInfos, column, hasActionParameter)
                                                   : CreateInputInstruction(headers, column, actualTargetType, hasActionParameter);
                    instructions.Add(instruction);
                }

                return instructions;
            }

            private List<IInstruction> CreateOutputInstructions(List<string> headers, Type targetType)
            {
                var instructions = new List<IInstruction>();

                if (enumeratedType != null)
                {
                    instructions.Add(new NextActualRow());
                    targetType = enumeratedType;
                }

                int column = 0;
                headers.ForEach(header => instructions.Add(CreateOutputInstruction(header, column++, targetType)));

                return instructions;
            }

            private static Type GetActionMethodParameterType(MethodInfo[] actionMethodInfos)
            {
                Type[] actionParameterTypes = Array.ConvertAll(actionMethodInfos, actionMethodInfo => GetActionMethodParameterType(actionMethodInfo));

                Type resultParameterType = null;
                foreach (Type type in actionParameterTypes)
                {
                    if (type != null)
                    {
                        // todo there can be only one parameter type for all actions. This seems somewhat arbitrary. Why can't there be more than one?
                        if (resultParameterType != null && !resultParameterType.Equals(type))
                            throw new MalformedTestException("Multiple Action methods found with disparate parameters");

                        resultParameterType = type;
                    }
                }

                return resultParameterType;
            }

            private static Type GetActionMethodParameterType(MethodBase actionMethodInfo)
            {
                if (!(actionMethodInfo != null))
                {
                    return null;
                }

                ParameterInfo[] actionMethodParameters = actionMethodInfo.GetParameters();
                switch (actionMethodParameters.Length)
                {
                    case 0:
                        return null;
                    case 1:
                        return actionMethodParameters[0].ParameterType;
                    default:
                        throw new MalformedTestException(string.Format("Action method '{0}' found with more than one parameter", actionMethodInfo.Name));
                }
            }

            private static MethodInfo[] GetActionMethodInfos(Type targetType, IList<string> headers)
            {
                var actionMethodInfos = new MethodInfo[headers.Count];
                for (int column = 0; column < headers.Count; column++)
                {
                    string header = headers[column];
                    if (IsAction(header))
                    {
                        actionMethodInfos[column] = CreateActionMethodInfo(header, targetType);
                    }
                }

                return actionMethodInfos;
            }

            private static string[] SplitAndTrimTitle(string title)
            {
                string[] values = title.Split(new[] { CACHE_INDICATOR }, StringSplitOptions.None);
                Array.ForEach(values, value => value.Trim());
                if (values.Length == 0 || values.Length > 2)
                {
                    throw new MalformedTestException(
                        string.Format("RawTest title is invalid: '{0}'", title));
                }
                return values;
            }

            private Type GetCachedSourceType(string userCacheKey)
            {
                object cachedObject = userCache[userCacheKey];
                if (!(cachedObject != null))
                {
                    throw new MalformedTestException(string.Format("Cached object '{0}' not found", userCacheKey));
                }

                return cachedObject.GetType();
            }

            private static Type GetPotentialEnumeratedTypeFrom(Type type)
            {
                Type genericType = Array.Find(type.GetInterfaces(), IsGenericEnumerable);
                return genericType != null ? genericType.GetGenericArguments()[0] : null;
            }

            private static bool IsGenericEnumerable(Type type)
            {
                return type.IsGenericType && type.GetGenericTypeDefinition().Equals(GENERIC_ENUMERABLE_TYPE);
            }

            private static IInstruction CreateOutputInstruction(string header, int column, Type targetType)
            {
                FieldInfo fieldInfo = targetType.GetField(header, BINDING_FLAGS);
                if (IsValidField(fieldInfo))
                {
                    return new GetField(fieldInfo, column);
                }

                PropertyInfo propertyInfo = targetType.GetProperty(header, BINDING_FLAGS);
                if (IsValidOutputProperty(propertyInfo))
                {
                    return new GetMethod(propertyInfo.GetGetMethod(), column);
                }

                MethodInfo methodInfo = targetType.GetMethod(header, BINDING_FLAGS);
                if (IsValidOutputMethod(methodInfo))
                {
                    return new GetMethod(methodInfo, column);
                }

                throw new MalformedTestException(string.Format("Input property/method/field '{0}' not found", header));
            }

            private static MethodInfo CreateActionMethodInfo(string header, Type targetType)
            {
                string trimmedHeader = header.Substring(0, header.Length - ACTION_INDICATOR.Length);
                MethodInfo methodInfo = targetType.GetMethod(trimmedHeader, BINDING_FLAGS);
                if (IsValidMethod(methodInfo))
                {
                    return methodInfo;
                }

                throw new MalformedTestException(string.Format("Action method '{0}' not found", trimmedHeader));
            }

            private static IInstruction CreateActionInstruction(MethodInfo[] actionMethodInfos, int column, bool hasActionParameter)
            {
                if (hasActionParameter)
                {
                    return new CallActionWithParameter(actionMethodInfos[column], column);
                }

                return new CallActionNoParameter(actionMethodInfos[column], column);
            }

            private static IInstruction CreateInputInstruction(IList<string> headers, int column, Type targetType, bool hasActionParameter)
            {
                string header = headers[column];

                DataType dataCacheTargetKey = hasActionParameter ? DataType.ActionParameter : DataType.Source;
                FieldInfo fieldInfo = targetType.GetField(header, BINDING_FLAGS);
                if (IsValidField(fieldInfo))
                {
                    return CreateSetField(fieldInfo, column, dataCacheTargetKey);
                }

                PropertyInfo propertyInfo = targetType.GetProperty(header, BINDING_FLAGS);
                if (IsValidInputProperty(propertyInfo))
                {
                    return CreateSetMethod(propertyInfo.GetSetMethod(), dataCacheTargetKey, column);
                }

                MethodInfo methodInfo = targetType.GetMethod(header, BINDING_FLAGS);
                if (IsValidInputMethod(methodInfo))
                {
                    return CreateSetMethod(methodInfo, dataCacheTargetKey, column);
                }

                throw new MalformedTestException(string.Format("Input property/method/field '{0}' not found", header));
            }

            private static IInstruction CreateSetField(FieldInfo fieldInfo, int column, DataType dataCacheTargetKey)
            {
                MethodInfo parserMethod =
                    fieldInfo.FieldType.GetMethod("Parse", PARSER_BINDING_FLAGS, null, PARSER_SINGLE_STRING_ARG, null);
                return new SetField(fieldInfo, dataCacheTargetKey, column, parserMethod);
            }

            private static IInstruction CreateSetMethod(MethodInfo methodInfo, DataType dataCacheTargetKey, int column)
            {
                Type singleArg = methodInfo.GetParameters()[0].ParameterType;
                MethodInfo parserMethod = singleArg.GetMethod("Parse", PARSER_BINDING_FLAGS, null, PARSER_SINGLE_STRING_ARG, null);
                return new SetMethod(methodInfo, dataCacheTargetKey, column, parserMethod);
            }

            private static bool IsValidMethod(MethodBase methodInfo)
            {
                return methodInfo != null && !methodInfo.IsAbstract;
            }

            private static bool IsValidInputMethod(MethodBase methodInfo)
            {
                return IsValidMethod(methodInfo) && methodInfo.GetParameters().Length == 1;
            }

            private static bool IsValidOutputMethod(MethodInfo methodInfo)
            {
                return IsValidMethod(methodInfo) && methodInfo.GetParameters().Length == 0
                       && !methodInfo.ReturnType.Equals(VOID_TYPE);
            }

            private static bool IsValidField(FieldInfo fieldInfo)
            {
                return fieldInfo != null && !fieldInfo.IsLiteral;
            }

            private static bool IsValidInputProperty(PropertyInfo propertyInfo)
            {
                return propertyInfo != null && propertyInfo.CanWrite;
            }

            private static bool IsValidOutputProperty(PropertyInfo propertyInfo)
            {
                return propertyInfo != null && propertyInfo.CanRead;
            }

            private static bool IsAction(string header)
            {
                return header.EndsWith(ACTION_INDICATOR);
            }

            private bool IsOutputTest
            {
                get { return testStyle == TestStyle.Output; }
            }
        }
    }
}