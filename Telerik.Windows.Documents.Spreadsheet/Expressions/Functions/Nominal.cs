using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Nominal : NumbersInFunction
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
				return Nominal.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Nominal.Info;
			}
		}

		static Nominal()
		{
			string description = "Returns the nominal annual interest rate, given the effective rate and the number of compounding periods per year.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Nominal_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("EffectRate", "is the effective interest rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_EffectRate", "Spreadsheet_Functions_Nominal_EffectRateInfo"),
				new ArgumentInfo("Npery", "is the number of compounding periods per year.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Npery", "Spreadsheet_Functions_Nominal_NperyInfo")
			};
			Nominal.Info = new FunctionInfo(Nominal.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			if (num <= 0.0 || num2 < 1.0)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.NOMINAL(num, num2);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "NOMINAL";

		static readonly FunctionInfo Info;
	}
}
