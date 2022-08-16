using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Minute : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Minute.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Minute.Info;
			}
		}

		static Minute()
		{
			string description = "Returns the minutes of a time value. The minute is given as an integer, ranging from 0 to 59.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Minute_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("SerialNumber", "is the time that contains the minute you want to find. Times may be entered as text strings within quotation marks (for example, \"6:45 PM\"), as decimal numbers (for example, 0.78125, which represents 6:45 PM), or as results of other formulas or functions (for example, TIMEVALUE(\"6:45 PM\")).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_SerialNumber", "Spreadsheet_Functions_Minute_SerialNumberInfo")
			};
			Minute.Info = new FunctionInfo(Minute.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(num);
			if (dateTime == null)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression((double)dateTime.Value.Minute);
		}

		public static readonly string FunctionName = "MINUTE";

		static readonly FunctionInfo Info;
	}
}
