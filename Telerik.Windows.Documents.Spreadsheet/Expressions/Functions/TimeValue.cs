using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class TimeValue : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return TimeValue.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return TimeValue.Info;
			}
		}

		static TimeValue()
		{
			string description = "Returns the decimal number of the time represented by a text string. The decimal number is a value ranging from 0 (zero) to 0.99999999, representing the times from 0:00:00 (12:00:00 AM) to 23:59:59 (11:59:59 P.M.).";
			string descriptionLocalizationKey = "Spreadsheet_Functions_TimeValue_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("TimeText", "is a text string that represents a time in any one of the Microsoft Excel time formats; for example, \"6:45 PM\" and \"18:45\" text strings within quotation marks that represent time.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_TimeText", "Spreadsheet_Functions_TimeValue_TimeTextInfo")
			};
			TimeValue.Info = new FunctionInfo(TimeValue.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			string empty = string.Empty;
			double num;
			if (!NumberValueParser.TryParse(context.Arguments[0], null, null, out empty, out num) || !(new CellValueFormat(empty).FormatStringInfo.FormatType == FormatStringType.DateTime))
			{
				return ErrorExpressions.ValueError;
			}
			num %= 1.0;
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "TIMEVALUE";

		static readonly FunctionInfo Info;
	}
}
