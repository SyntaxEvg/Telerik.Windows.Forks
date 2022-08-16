using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Average : NumbersInFunction
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
				return Average.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Average.Info;
			}
		}

		static Average()
		{
			string description = "Returns the average (arithmetic mean) of its arguments, which can be numbers or names, arrays, or references that contain numbers.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Average_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are the numberic arguments for which you want the average.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Average_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are the numberic arguments for which you want the average.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Average_NumberInfo")
			};
			Average.Info = new FunctionInfo(Average.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			if (context.Arguments.Length == 0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number = MathUtility.Average(context.Arguments);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "AVERAGE";

		static readonly FunctionInfo Info;
	}
}
