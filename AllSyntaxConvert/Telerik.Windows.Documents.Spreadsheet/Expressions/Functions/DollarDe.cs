using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class DollarDe : NumbersInFunction
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
				return DollarDe.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return DollarDe.Info;
			}
		}

		static DollarDe()
		{
			string description = "Converts a dollar price expressed as an integer part and a fraction part, such as 1.02, into a dollar price expressed as a decimal number. Fractional dollar numbers are sometimes used for security prices. The fraction part of the value is divided by an integer that you specify. For example, if you want your price to be expressed to a precision of 1/16 of a dollar, you divide the fraction part by 16. In this case, 1.02 represents $1.125 ($1 + 2/16 = $1.125).";
			string descriptionLocalizationKey = "Spreadsheet_Functions_DollarDe_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("FractionalDollar", "is the number expressed as an integer part and a fraction part, separated by a decimal symbol.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_FractionalDollar", "Spreadsheet_Functions_DollarDe_FractionalDollarInfo"),
				new ArgumentInfo("Fraction", "is the integer to use in the denominator of the fraction.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fraction", "Spreadsheet_Functions_Dollar_FractionInfo")
			};
			DollarDe.Info = new FunctionInfo(DollarDe.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[1];
			if (num < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			if (num >= 0.0 && num < 1.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number;
			try
			{
				number = FinancialFunctions.DOLLARDE(context.Arguments[0], context.Arguments[1]);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "DOLLARDE";

		static readonly FunctionInfo Info;
	}
}
