using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Second : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Second.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Second.Info;
			}
		}

		static Second()
		{
			string description = "Returns the seconds of a time value. The second is given as an integer in the range 0 (zero) to 59.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Second_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("SerialNumber", "is the time that contains the minute you want to find. Times may be entered as text strings within quotation marks (for example, \"6:45 PM\"), as decimal numbers (for example, 0.78125, which represents 6:45 PM), or as results of other formulas or functions (for example, TIMEVALUE(\"6:45 PM\")).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_SerialNumber", "Spreadsheet_Functions_Second_SerialNumberInfo")
			};
			Second.Info = new FunctionInfo(Second.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			return NumberExpression.CreateValidNumberOrErrorExpression((double)dateTime.Value.Second);
		}

		public static readonly string FunctionName = "SECOND";

		static readonly FunctionInfo Info;
	}
}
