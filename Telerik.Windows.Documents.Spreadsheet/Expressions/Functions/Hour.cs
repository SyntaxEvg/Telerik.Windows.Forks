using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Hour : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Hour.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Hour.Info;
			}
		}

		static Hour()
		{
			string description = "Returns the hour of a time value. The hour is given as an integer, ranging from 0 (12:00 A.M.) to 23 (11:00 P.M.).";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Hour_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("SerialNumber", "is the time that contains the hour you want to find. Times may be entered as text strings within quotation marks (for example, \"6:45 PM\"), as decimal numbers (for example, 0.78125, which represents 6:45 PM), or as results of other formulas or functions (for example, TIMEVALUE(\"6:45 PM\")).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_SerialNumber", "Spreadsheet_Functions_Hour_SerialNumberInfo")
			};
			Hour.Info = new FunctionInfo(Hour.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			return NumberExpression.CreateValidNumberOrErrorExpression((double)dateTime.Value.Hour);
		}

		public static readonly string FunctionName = "HOUR";

		static readonly FunctionInfo Info;
	}
}
