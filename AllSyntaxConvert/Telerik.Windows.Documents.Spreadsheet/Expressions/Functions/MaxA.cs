using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class MaxA : NumbersInFunction
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
				return MaxA.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return MaxA.Info;
			}
		}

		static MaxA()
		{
			string description = "Returns the largest value in a list of arguments. Arguments can be the following: numbers; names, arrays, or references that contain numbers; text representations of numbers; or logical values, such as TRUE and FALSE, in a reference.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_MaxA_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 values for which you want to find the largest value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_MaxA_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 values for which you want to find the largest value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_MaxA_NumberInfo")
			};
			MaxA.Info = new FunctionInfo(MaxA.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = double.MinValue;
			if (context.Arguments.Length < 1)
			{
				num = 0.0;
			}
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				num = Math.Max(num, context.Arguments[i]);
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "MAXA";

		static readonly FunctionInfo Info;
	}
}
