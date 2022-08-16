using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Lcm : NumbersInFunction
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
				return Lcm.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Lcm.Info;
			}
		}

		static Lcm()
		{
			string description = "Returns the least common multiple of integers. The least common multiple is the smallest positive integer that is a multiple of all integer arguments number1, number2, and so on. Use LCM to add fractions with different denominators.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Lcm_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1, number2, ...     are 1 to 29 values for which you want the least common multiple. If value is not an integer, it is truncated.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Lcm_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1, number2, ...     are 1 to 29 values for which you want the least common multiple. If value is not an integer, it is truncated.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Lcm_NumberInfo")
			};
			Lcm.Info = new FunctionInfo(Lcm.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, false, descriptionLocalizationKey);
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
			double number = (double)MathUtility.LeastCommonMultiple(context.Arguments);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "LCM";

		static readonly FunctionInfo Info;
	}
}
