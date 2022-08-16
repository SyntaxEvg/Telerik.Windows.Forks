using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class ComparisonOperatorConverter : IStringConverter<ComparisonOperator>
	{
		static ComparisonOperatorConverter()
		{
			ComparisonOperatorConverter.AddId("equal", ComparisonOperator.EqualsTo);
			ComparisonOperatorConverter.AddId("lessThan", ComparisonOperator.LessThan);
			ComparisonOperatorConverter.AddId("lessThanOrEqual", ComparisonOperator.LessThanOrEqualsTo);
			ComparisonOperatorConverter.AddId("notEqual", ComparisonOperator.NotEqualsTo);
			ComparisonOperatorConverter.AddId("greaterThanOrEqual", ComparisonOperator.GreaterThanOrEqualsTo);
			ComparisonOperatorConverter.AddId("greaterThan", ComparisonOperator.GreaterThan);
			ComparisonOperatorConverter.AddId("between", ComparisonOperator.Between);
			ComparisonOperatorConverter.AddId("notBetween", ComparisonOperator.NotBetween);
			ComparisonOperatorConverter.defaultId = ComparisonOperatorConverter.comparisonOperatorToId[ComparisonOperator.EqualsTo];
		}

		public ComparisonOperator ConvertFromString(string value)
		{
			ComparisonOperator result;
			if (ComparisonOperatorConverter.operatorIdToComparisonOperator.TryGetValue(value, out result))
			{
				return result;
			}
			return ComparisonOperator.EqualsTo;
		}

		public string ConvertToString(ComparisonOperator value)
		{
			string result;
			if (ComparisonOperatorConverter.comparisonOperatorToId.TryGetValue(value, out result))
			{
				return result;
			}
			return ComparisonOperatorConverter.defaultId;
		}

		static void AddId(string id, ComparisonOperator comparisonOperator)
		{
			ComparisonOperatorConverter.operatorIdToComparisonOperator[id] = comparisonOperator;
			if (!ComparisonOperatorConverter.comparisonOperatorToId.ContainsKey(comparisonOperator))
			{
				ComparisonOperatorConverter.comparisonOperatorToId[comparisonOperator] = id;
			}
		}

		public const ComparisonOperator DefaultExportComparisonOperator = ComparisonOperator.EqualsTo;

		static readonly Dictionary<ComparisonOperator, string> comparisonOperatorToId = new Dictionary<ComparisonOperator, string>();

		static readonly Dictionary<string, ComparisonOperator> operatorIdToComparisonOperator = new Dictionary<string, ComparisonOperator>();

		static readonly string defaultId;
	}
}
