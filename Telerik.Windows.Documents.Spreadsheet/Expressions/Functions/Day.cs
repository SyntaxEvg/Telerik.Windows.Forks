using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Day : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Day.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Day.Info;
			}
		}

		static Day()
		{
			string description = "Returns the day of a date, represented by a serial number. The day is given as an integer ranging from 1 to 31.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Day_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("SerialNumber", "is the date of the day you are trying to find. Dates should be entered by using the DATE function, or as results of other formulas or functions.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_SerialNumber", "Spreadsheet_Functions_Day_SerialNumberInfo")
			};
			Day.Info = new FunctionInfo(Day.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			return NumberExpression.CreateValidNumberOrErrorExpression((double)dateTime.Value.Day);
		}

		public static readonly string FunctionName = "DAY";

		static readonly FunctionInfo Info;
	}
}
