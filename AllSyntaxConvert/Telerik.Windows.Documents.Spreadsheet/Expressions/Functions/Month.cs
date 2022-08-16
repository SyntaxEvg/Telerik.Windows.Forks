using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Month : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Month.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Month.Info;
			}
		}

		static Month()
		{
			string description = "Returns the month of a date represented by a serial number. The month is given as an integer, ranging from 1 (January) to 12 (December).";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Month_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("SerialNumber", "is the date of the month you are trying to find. Dates should be entered by using the DATE function, or as results of other formulas or functions.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_SerialNumber", "Spreadsheet_Functions_Month_SerialNumberInfo")
			};
			Month.Info = new FunctionInfo(Month.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			return NumberExpression.CreateValidNumberOrErrorExpression((double)dateTime.Value.Month);
		}

		public static readonly string FunctionName = "MONTH";

		static readonly FunctionInfo Info;
	}
}
