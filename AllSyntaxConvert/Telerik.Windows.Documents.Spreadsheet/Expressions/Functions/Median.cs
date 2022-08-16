using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Median : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NaryIgnoreIndirectNumberFunctionConversion;
			}
		}

		public override string Name
		{
			get
			{
				return Median.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Median.Info;
			}
		}

		static Median()
		{
			string description = "Returns the median of the given numbers. The median is the number in the middle of a set of numbers.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Median_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 numbers for which you want the median.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Median_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 numbers for which you want the median.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Median_NumberInfo")
			};
			Median.Info = new FunctionInfo(Median.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			if (context.Arguments.Length < 1)
			{
				return ErrorExpressions.NumberError;
			}
			Array.Sort<double>(context.Arguments);
			int num = context.Arguments.Length / 2;
			double number;
			if (context.Arguments.Length % 2 == 1)
			{
				number = context.Arguments[num];
			}
			else
			{
				number = (context.Arguments[num - 1] + context.Arguments[num]) / 2.0;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "MEDIAN";

		static readonly FunctionInfo Info;
	}
}
