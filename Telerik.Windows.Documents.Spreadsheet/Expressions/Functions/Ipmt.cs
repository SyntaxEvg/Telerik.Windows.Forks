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
	public class Ipmt : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Ipmt.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Ipmt.Info;
			}
		}

		static Ipmt()
		{
			string description = "Returns the interest payment for a given period for an investment based on periodic, constant payments and a constant interest rate.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Ipmt_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the interest rate per period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Ipmt_RateInfo"),
				new ArgumentInfo("Per", "is the period for which you want to find the interest and must be in the range 1 to nper.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Per", "Spreadsheet_Functions_Ipmt_PerInfo"),
				new ArgumentInfo("Nper", "is the total number of payment periods in an annuity.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Nper", "Spreadsheet_Functions_Ipmt_NperInfo"),
				new ArgumentInfo("Pv", "is the present value, or the lump-sum amount that a series of future payments is worth right now. If pv is omitted, it is assumed to be 0 (zero), and you must include the pmt argument.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Ipmt_PvInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Fv", "is the future value, or a cash balance you want to attain after the last payment is made. If fv is omitted, it is assumed to be 0 (the future value of a loan, for example, is 0).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fv", "Spreadsheet_Functions_Ipmt_FvInfo"),
				new ArgumentInfo("Type", "is the indicates when payments are due. If type is omitted, it is assumed to be 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Type", "Spreadsheet_Functions_Ipmt_TypeInfo")
			};
			string formatString = CurrencyFormatStringBuilder.BuildFormatStrings(new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null), 2).Last<string>();
			Ipmt.Info = new FunctionInfo(Ipmt.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, new CellValueFormat(formatString), 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double rate = context.Arguments[0];
			double per = context.Arguments[1];
			double nPer = context.Arguments[2];
			double pv = context.Arguments[3];
			int num = 0;
			if (context.Arguments.Length > 4)
			{
				num = (int)context.Arguments[4];
			}
			int num2 = 0;
			if (context.Arguments.Length == 6)
			{
				num2 = (int)context.Arguments[5];
			}
			if (num2 < 0 || num2 > 1)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.IPMT(rate, per, nPer, pv, (double)num, num2);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "IPMT";

		static readonly FunctionInfo Info;
	}
}
