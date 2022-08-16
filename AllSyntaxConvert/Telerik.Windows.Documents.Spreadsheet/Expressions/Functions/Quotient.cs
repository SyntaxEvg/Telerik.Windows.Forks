using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Quotient : NumbersInFunction
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
				return Quotient.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Quotient.Info;
			}
		}

		static Quotient()
		{
			string description = "Returns the integer portion of a division. Use this function when you want to discard the remainder of a division.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Quotient_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Numerator", "is the dividend.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Numerator", "Spreadsheet_Functions_Quotient_NumeratorInfo"),
				new ArgumentInfo("Denominator", "is the divisor.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Denominator", "Spreadsheet_Functions_Quotient_DenominatorInfo")
			};
			Quotient.Info = new FunctionInfo(Quotient.FunctionName, FunctionCategory.MathTrig, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			if (num2 == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number = MathUtility.RoundDown(num / num2, 0);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "QUOTIENT";

		static readonly FunctionInfo Info;
	}
}
