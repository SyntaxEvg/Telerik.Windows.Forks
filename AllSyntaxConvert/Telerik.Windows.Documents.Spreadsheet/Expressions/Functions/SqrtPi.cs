using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class SqrtPi : NumbersInFunction
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
				return SqrtPi.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return SqrtPi.Info;
			}
		}

		static SqrtPi()
		{
			string description = "Returns the square root of (number * pi).";
			string descriptionLocalizationKey = "Spreadsheet_Functions_SqrtPi_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Number", "is the number by which pi is multiplied.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Number", "Spreadsheet_Functions_SqrtPi_NumberInfo")
			};
			SqrtPi.Info = new FunctionInfo(SqrtPi.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			if (num < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			double number = Math.Sqrt(num * 3.141592653589793);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "SQRTPI";

		static readonly FunctionInfo Info;
	}
}
