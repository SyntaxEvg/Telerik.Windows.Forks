using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class AverageA : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.DefaultValueNumberFunctionConversion;
			}
		}

		public override string Name
		{
			get
			{
				return AverageA.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return AverageA.Info;
			}
		}

		static AverageA()
		{
			string description = "Calculates the average (arithmetic mean) of the values in the list of arguments. Logical values and text representations of numbers that you type directly into the list of arguments are counted.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_AverageA_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 cells, ranges of cells, or values for which you want the average.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_AverageA_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 cells, ranges of cells, or values for which you want the average.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_AverageA_NumberInfo")
			};
			AverageA.Info = new FunctionInfo(AverageA.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			if (context.Arguments.Length < 1)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number = MathUtility.Average(context.Arguments);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "AVERAGEA";

		static readonly FunctionInfo Info;
	}
}
