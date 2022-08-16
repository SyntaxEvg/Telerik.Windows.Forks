using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Ppmt : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Ppmt.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Ppmt.Info;
			}
		}

		static Ppmt()
		{
			string description = "Returns the payment on the principal for a given period for an investment based on periodic, constant payments and a constant interest rate.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Ppmt_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the interest rate per period. For example, use 6%/4 for quarterly payments at 6% APR.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Pmt_RateInfo"),
				new ArgumentInfo("Per", "is the period and must be in the range 1 to nper.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Per", "Spreadsheet_Functions_Ppmt_PerInfo"),
				new ArgumentInfo("Nper", "is the total number of payment periods in an annuity.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Nper", "Spreadsheet_Functions_Ppmt_NperInfo"),
				new ArgumentInfo("Pv", "is the present value, or the lump-sum amount that a series of future payments is worth right now.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Pmt_PvInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Fv", "is the future value, or a cash balance you want to attain after the last payment is made. If fv is omitted, it is assumed to be 0 (the future value of a loan, for example, is 0).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fv", "Spreadsheet_Functions_Pmt_FvInfo"),
				new ArgumentInfo("Type", "is the number 0 or 1 and indicates when payments are due.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Type", "Spreadsheet_Functions_Pmt_TypeInfo")
			};
			string formatString = CurrencyFormatStringBuilder.BuildFormatStrings(new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null), 2).Last<string>();
			Ppmt.Info = new FunctionInfo(Ppmt.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, new CellValueFormat(formatString), 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double rate = context.Arguments[0];
			double per = context.Arguments[1];
			double nPer = context.Arguments[2];
			double pv = context.Arguments[3];
			double fv = 0.0;
			if (context.Arguments.Length > 4)
			{
				fv = context.Arguments[4];
			}
			int due = 0;
			if (context.Arguments.Length > 5)
			{
				due = (((int)context.Arguments[5] == 0) ? 0 : 1);
			}
			double number;
			try
			{
				number = FinancialFunctions.PPMT(rate, per, nPer, pv, fv, due);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "PPMT";

		static readonly FunctionInfo Info;
	}
}
