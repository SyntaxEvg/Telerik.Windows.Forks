using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class MinA : NumbersInFunction
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
				return MinA.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return MinA.Info;
			}
		}

		static MinA()
		{
			string description = "Returns the smallest value in the list of arguments. Arguments can be the following: numbers; names, arrays, or references that contain numbers; text representations of numbers; or logical values, such as TRUE and FALSE, in a reference.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_MinA_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 values for which you want to find the smallest value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_MinA_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 30 values for which you want to find the smallest value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_MinA_NumberInfo")
			};
			MinA.Info = new FunctionInfo(MinA.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = double.MaxValue;
			if (context.Arguments.Length < 1)
			{
				num = 0.0;
			}
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				num = System.Math.Min(num, context.Arguments[i]);
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "MINA";

		static readonly FunctionInfo Info;
	}
}
