using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Max : NumbersInFunction
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
				return Max.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Max.Info;
			}
		}

		static Max()
		{
			string description = "Returns the largest value in a set of values. Ignores logical values and text.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Max_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are empty cells, logical values, or text numbers for which you want the maximum.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Max_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are empty cells, logical values, or text numbers for which you want the maximum.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Max_NumberInfo")
			};
			Max.Info = new FunctionInfo(Max.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
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

		public static readonly string FunctionName = "MAX";

		static readonly FunctionInfo Info;
	}
}
