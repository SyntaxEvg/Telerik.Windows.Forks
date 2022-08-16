using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class SumSq : NumbersInFunction
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
				return SumSq.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return SumSq.Info;
			}
		}

		static SumSq()
		{
			string description = "Returns the sum of the squares of the arguments.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_SumSq_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1, number2, ... are 1 to 30 arguments for which you want the sum of the squares. You can also use a single array or a reference to an array instead of arguments separated by commas.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_SumSq_NumberInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "Number1, number2, ... are 1 to 30 arguments for which you want the sum of the squares. You can also use a single array or a reference to an array instead of arguments separated by commas.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_SumSq_NumberInfo")
			};
			SumSq.Info = new FunctionInfo(SumSq.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, optionalArgumentsInfos, 254, true, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = 0.0;
			for (int i = 0; i < context.Arguments.Length; i++)
			{
				num += Math.Pow(context.Arguments[i], 2.0);
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(num);
		}

		public static readonly string FunctionName = "SUMSQ";

		static readonly FunctionInfo Info;
	}
}
