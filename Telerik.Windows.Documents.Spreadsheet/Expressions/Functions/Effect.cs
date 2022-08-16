using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Effect : NumbersInFunction
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
				return Effect.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Effect.Info;
			}
		}

		static Effect()
		{
			string description = "Returns the effective annual interest rate, given the nominal annual interest rate and the number of compounding periods per year.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Effect_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("NominalRate", "is the nominal interest rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_NominalRate", "Spreadsheet_Functions_Effect_NominalRateInfo"),
				new ArgumentInfo("Npery", "is the number of compounding periods per year.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Npery", "Spreadsheet_Functions_Effect_NperyInfo")
			};
			Effect.Info = new FunctionInfo(Effect.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
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
				number = FinancialFunctions.EFFECT(context.Arguments[0], context.Arguments[1]);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "EFFECT";

		static readonly FunctionInfo Info;
	}
}
