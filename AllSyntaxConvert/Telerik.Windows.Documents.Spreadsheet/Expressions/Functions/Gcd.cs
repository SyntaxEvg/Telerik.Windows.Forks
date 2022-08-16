using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Gcd : NumbersInFunction
	{
		public override ArgumentConversionRules ArgumentConversionRules
		{
			get
			{
				return ArgumentConversionRules.NonBoolNaryFunctionConversion;
			}
		}

		public override string Name
		{
			get
			{
				return Gcd.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Gcd.Info;
			}
		}

		static Gcd()
		{
			string description = "Returns the greatest common divisor of two or more integers. The greatest common divisor is the largest integer that divides both number1 and number2 without a remainder.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Gcd_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1, number2, ...     are 1 to 29 values. If any value is not an integer, it is truncated.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Gcd_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1, number2, ...     are 1 to 29 values. If any value is not an integer, it is truncated.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Gcd_NumberInfo")
			};
			Gcd.Info = new FunctionInfo(Gcd.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				if (context.Arguments[i] < 0.0)
				{
					return ErrorExpressions.NumberError;
				}
			}
			double number = (double)MathUtility.GreatestCommonDivisor(context.Arguments);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "GCD";

		static readonly FunctionInfo Info;
	}
}
