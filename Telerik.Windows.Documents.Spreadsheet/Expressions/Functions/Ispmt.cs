using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Ispmt : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Ispmt.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Ispmt.Info;
			}
		}

		static Ispmt()
		{
			string description = "Calculates the interest paid during a specific period of an investment. This function is provided for compatibility with Lotus 1-2-3.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Ispmt_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the interest rate per period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Ispmt_RateInfo"),
				new ArgumentInfo("Per", "is the period for which you want to find the interest and must be in the range 1 to nper.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Per", "Spreadsheet_Functions_Ispmt_PerInfo"),
				new ArgumentInfo("Nper", "is the total number of payment periods in an annuity.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Nper", "Spreadsheet_Functions_Ispmt_NperInfo"),
				new ArgumentInfo("Pv", "is the present value, or the lump-sum amount that a series of future payments is worth right now. If pv is omitted, it is assumed to be 0 (zero), and you must include the pmt argument.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Ispmt_PvInfo")
			};
			Ispmt.Info = new FunctionInfo(Ispmt.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double rate = context.Arguments[0];
			double per = context.Arguments[1];
			double num = context.Arguments[2];
			double pv = context.Arguments[3];
			if (num == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number;
			try
			{
				number = FinancialFunctions.ISPMT(rate, per, num, pv);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ISPMT";

		static readonly FunctionInfo Info;
	}
}
