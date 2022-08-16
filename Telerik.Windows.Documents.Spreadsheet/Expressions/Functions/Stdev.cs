using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Stdev : NumbersInFunction
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
				return Stdev.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Stdev.Info;
			}
		}

		static Stdev()
		{
			string description = "Estimates standard deviation based on a sample. The standard deviation is a measure of how widely values are dispersed from the average value (the mean).";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Stdev_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 number arguments corresponding to a sample of a population. You can also use a single array or a reference to an array instead of arguments separated by commas.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Stdev_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 number arguments corresponding to a sample of a population. You can also use a single array or a reference to an array instead of arguments separated by commas.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Stdev_NumberInfo")
			};
			Stdev.Info = new FunctionInfo(Stdev.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			if (context.Arguments.Length < 2)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number = MathUtility.StandardDeviation(context.Arguments, false);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "STDEV";

		static readonly FunctionInfo Info;
	}
}
