using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Pmt : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Pmt.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Pmt.Info;
			}
		}

		static Pmt()
		{
			string description = "Calculates the payment for a loan based on constant payments and a constant interest rate.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Pmt_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the interest rate per period. For example, use 6%/4 for quarterly payments at 6% APR.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Pmt_RateInfo"),
				new ArgumentInfo("Nper", "is the total number of payments for the loan.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Nper", "Spreadsheet_Functions_Pmt_NperInfo"),
				new ArgumentInfo("Pv", "is the present value, or the lump-sum amount that a series of future payments is worth right now.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Pmt_PvInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Fv", "is the future value, or a cash balance you want to attain after the last payment is made. If fv is omitted, it is assumed to be 0 (the future value of a loan, for example, is 0).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fv", "Spreadsheet_Functions_Pmt_FvInfo"),
				new ArgumentInfo("Type", "is the number 0 or 1 and indicates when payments are due.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Type", "Spreadsheet_Functions_Pmt_TypeInfo")
			};
			Pmt.Info = new FunctionInfo(Pmt.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double rate = context.Arguments[0];
			double nPer = context.Arguments[1];
			double pv = context.Arguments[2];
			double fv = 0.0;
			if (context.Arguments.Length > 3)
			{
				fv = context.Arguments[3];
			}
			int due = 0;
			if (context.Arguments.Length > 4)
			{
				due = (int)context.Arguments[4];
			}
			double number;
			try
			{
				number = FinancialFunctions.PMT(rate, nPer, pv, fv, due);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "PMT";

		static readonly FunctionInfo Info;
	}
}
