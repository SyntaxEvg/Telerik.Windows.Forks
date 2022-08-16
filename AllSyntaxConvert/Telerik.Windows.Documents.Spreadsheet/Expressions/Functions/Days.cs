using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Days : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Days.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Days.Info;
			}
		}

		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return Days.argumentConversionRules;
			}
		}

		static Days()
		{
			string description = "Returns the number of days between the two dates.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Days_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("End_date", "Start_date and end_date are the two dates between which you want to know the number of days.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_EndDate", "Spreadsheet_Functions_Days_StartEndDateInfo"),
				new ArgumentInfo("Start_date", "Start_date and end_date are the two dates between which you want to know the number of days.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_StartDate", "Spreadsheet_Functions_Days_StartEndDateInfo")
			};
			Days.Info = new FunctionInfo(Days.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			if (num < 0.0 || num2 < 0.0 || num > FormatHelper.MaxDoubleValueTranslatableToDateTime || num2 > FormatHelper.MaxDoubleValueTranslatableToDateTime)
			{
				return ErrorExpressions.NumberError;
			}
			int num3 = (int)num - (int)num2;
			return NumberExpression.CreateValidNumberOrErrorExpression((double)num3);
		}

		public static readonly string FunctionName = "DAYS";

		static readonly FunctionInfo Info;

		static readonly ArgumentConversionRules argumentConversionRules = new ArgumentConversionRules(ArgumentConversionRules.NumberFunctionConversion, true);
	}
}
