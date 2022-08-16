using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Multinomial : NumbersInFunction
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
				return Multinomial.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Multinomial.Info;
			}
		}

		static Multinomial()
		{
			string description = "Returns the ratio of the factorial of a sum of values to the product of factorials.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Multinomial_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1,number2, ...     are 1 to 29 values for which you want the multinomial.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Multinomial_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1,number2, ...     are 1 to 29 values for which you want the multinomial.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Multinomial_NumberInfo")
			};
			Multinomial.Info = new FunctionInfo(Multinomial.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = 1.0;
			int num2 = 0;
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				if (context.Arguments[i] < 0.0)
				{
					return ErrorExpressions.NumberError;
				}
				num *= MathUtility.Factorial(context.Arguments[i]);
				num2 += (int)context.Arguments[i];
			}
			double num3 = MathUtility.Factorial((double)num2);
			double number = num3 / num;
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "MULTINOMIAL";

		static readonly FunctionInfo Info;
	}
}
