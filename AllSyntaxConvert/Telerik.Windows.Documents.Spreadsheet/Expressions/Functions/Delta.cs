using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Delta : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NonBoolNumberFunctionConversion;
			}
		}

		public override string Name
		{
			get
			{
				return Delta.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Delta.Info;
			}
		}

		static Delta()
		{
			string description = "Tests whether two values are equal. Returns 1 if number1 = number2; returns 0 otherwise. Use this function to filter a set of values. For example, by summing several DELTA functions you calculate the count of equal pairs. This function is also known as the Kronecker Delta function.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Delta_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number1", "The first number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number1", "Spreadsheet_Functions_Delta_Number1Info")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number2", "The second number. If omitted, number2 is assumed to be zero.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number2", "Spreadsheet_Functions_Delta_Number2Info")
			};
			Delta.Info = new FunctionInfo(Delta.FunctionName, FunctionCategory.Engineering, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double number = (double)((context.Arguments.Length == 1) ? EngineeringFunctions.DELTA(context.Arguments[0]) : EngineeringFunctions.DELTA(context.Arguments[0], context.Arguments[1]));
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "DELTA";

		static readonly FunctionInfo Info;
	}
}
