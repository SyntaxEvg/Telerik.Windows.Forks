using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class DollarFr : NumbersInFunction
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
				return DollarFr.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return DollarFr.Info;
			}
		}

		static DollarFr()
		{
			string description = "Converts decimal numbers to fractional dollar numbers, such as securities prices.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_DollarFr_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("DecimalDollar", "is a decimal number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_DecimalDollar", "Spreadsheet_Functions_DollarFr_DecimalDollarInfo"),
				new ArgumentInfo("Fraction", "is the integer to use in the denominator of the fraction.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fraction", "Spreadsheet_Functions_Dollar_FractionInfo")
			};
			DollarFr.Info = new FunctionInfo(DollarFr.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[1];
			if (num < 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			if (num == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number;
			try
			{
				number = FinancialFunctions.DOLLARFR(context.Arguments[0], context.Arguments[1]);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "DOLLARFR";

		static readonly FunctionInfo Info;
	}
}
