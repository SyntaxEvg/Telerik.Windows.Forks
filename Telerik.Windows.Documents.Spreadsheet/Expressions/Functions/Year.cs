using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Year : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Year.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Year.Info;
			}
		}

		static Year()
		{
			string description = "Returns the year corresponding to a date. The year is returned as an integer in the range 1900-9999.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Year_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("SerialNumber", "is the date of the year you want to find. Dates should be entered by using the DATE function, or as results of other formulas or functions.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_SerialNumber", "Spreadsheet_Functions_Year_SerialNumberInfo")
			};
			Year.Info = new FunctionInfo(Year.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
			return NumberExpression.CreateValidNumberOrErrorExpression((double)dateTime.Value.Year);
		}

		public static readonly string FunctionName = "YEAR";

		static readonly FunctionInfo Info;
	}
}
