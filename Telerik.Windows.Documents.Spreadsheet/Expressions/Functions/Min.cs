using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Min : NumbersInFunction
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
				return Min.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Min.Info;
			}
		}

		static Min()
		{
			string description = "Returns the smallest number in a set of values. Ignores logical values and text.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Min_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 255 numbers, empty cells, logical values, or text numbers for which you want the minimum.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Min_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "number1, number2,... are 1 to 255 numbers, empty cells, logical values, or text numbers for which you want the minimum.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_Min_NumberInfo")
			};
			Min.Info = new FunctionInfo(Min.FunctionName, FunctionCategory.Statistical, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
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

		public static readonly string FunctionName = "MIN";

		static readonly FunctionInfo Info;
	}
}
