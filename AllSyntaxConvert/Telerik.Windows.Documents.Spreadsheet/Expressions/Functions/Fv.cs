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
	public class Fv : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Fv.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Fv.Info;
			}
		}

		static Fv()
		{
			string description = "Returns the future value of an investment based on periodic, constant payments and a constant interest rate.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Fv_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the interest rate per period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Fv_RateInfo"),
				new ArgumentInfo("Nper", "is the total number of payment periods in an annuity.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Nper", "Spreadsheet_Functions_Fv_NperInfo"),
				new ArgumentInfo("Pmt", "is the payment made each period; it cannot change over the life of the annuity. Typically, pmt contains principal and interest but no other fees or taxes. If pmt is omitted, you must include the pv argument.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pmt", "Spreadsheet_Functions_Fv_PmtInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Pv", "is the present value, or the lump-sum amount that a series of future payments is worth right now. If pv is omitted, it is assumed to be 0 (zero), and you must include the pmt argument.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Fv_PvInfo"),
				new ArgumentInfo("Type", "is the indicates when payments are due. If type is omitted, it is assumed to be 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Type", "Spreadsheet_Functions_Fv_TypeInfo")
			};
			string formatString = CurrencyFormatStringBuilder.BuildFormatStrings(new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null), 2).Last<string>();
			Fv.Info = new FunctionInfo(Fv.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, new CellValueFormat(formatString), 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double rate = context.Arguments[0];
			double nPer = context.Arguments[1];
			double pmt = context.Arguments[2];
			int num = 0;
			if (context.Arguments.Length > 3)
			{
				num = (int)context.Arguments[3];
			}
			int num2 = 0;
			if (context.Arguments.Length == 5)
			{
				num2 = (int)context.Arguments[4];
			}
			if (num2 < 0 || num2 > 1)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.FV(rate, nPer, pmt, (double)num, num2);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "FV";

		static readonly FunctionInfo Info;
	}
}
