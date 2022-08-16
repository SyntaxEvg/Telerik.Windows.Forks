using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class DateValue : StringsInFunction
	{
		public override string Name
		{
			get
			{
				return DateValue.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return DateValue.Info;
			}
		}

		static DateValue()
		{
			string description = "Returns the serial number of the date represented by date_text. Use DATEVALUE to convert a date represented by text to a serial number.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_DateValue_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("DateText", "is text that represents a date in a Microsoft Excel date format. For example, \"1/30/2008\" or \"30-Jan-2008\" are text strings within quotation marks that represent dates.", ArgumentType.Text, true, "Spreadsheet_Functions_Args_DateText", "Spreadsheet_Functions_Datevalue_DateTextInfo")
			};
			DateValue.Info = new FunctionInfo(DateValue.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<string> context)
		{
			double num;
			if (!DateValue.TryParseToDate(context.Arguments[0], out num))
			{
				return ErrorExpressions.ValueError;
			}
			num = MathUtility.RoundDown(num, 0);
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		internal static bool TryParseToDate(string value, out double dateSerialNumber)
		{
			string empty = string.Empty;
			return NumberValueParser.TryParse(value, null, null, out empty, out dateSerialNumber) && new CellValueFormat(empty).FormatStringInfo.FormatType == FormatStringType.DateTime;
		}

		public static readonly string FunctionName = "DATEVALUE";

		static readonly FunctionInfo Info;
	}
}
