using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Rate : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Rate.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Rate.Info;
			}
		}

		static Rate()
		{
			string description = "Returns the interest rate per period of an annuity. RATE is calculated by iteration and can have zero or more solutions. If the successive results of RATE do not converge to within 0.0000001 after 20 iterations, RATE returns the #NUM! error value.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Rate_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Nper", "is the total number of payment periods in an annuity.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Nper", "Spreadsheet_Functions_Rate_NperInfo"),
				new ArgumentInfo("Pmt", "is the payment made each period and cannot change over the life of the annuity. Typically, pmt includes principal and interest but no other fees or taxes. If pmt is omitted, you must include the fv argument.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pmt", "Spreadsheet_Functions_Rate_PmtInfo"),
				new ArgumentInfo("Pv", "is the total amount that a series of future payments is worth now.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Rate_PvInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Fv", "is future value, or a cash balance you want to attain after the last payment is made. If fv is omitted, it is assumed to be 0 (the future value of a loan, for example, is 0). For example, if you want to save $50,000 to pay for a special project in 18 years, then $50,000 is the future value. You could then make a conservative guess at an interest rate and determine how much you must save each month. If fv is omitted, you must include the pmt argument.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fv", "Spreadsheet_Functions_Rate_FvInfo"),
				new ArgumentInfo("Type", "is the number 0 or 1 and indicates when payments are due.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Type", "Spreadsheet_Functions_Rate_TypeInfo"),
				new ArgumentInfo("Guess", "is your guess for what the rate will be.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Guess", "Spreadsheet_Functions_Rate_GuessInfo")
			};
			string formatString = string.Format("0{0}00%", FormatHelper.DefaultSpreadsheetCulture.NumberDecimalSeparator);
			Rate.Info = new FunctionInfo(Rate.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, new CellValueFormat(formatString), 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double nPer = context.Arguments[0];
			double pmt = context.Arguments[1];
			double pv = context.Arguments[2];
			double fv = 0.0;
			if (context.Arguments.Length > 3)
			{
				fv = context.Arguments[3];
			}
			int num = 0;
			if (context.Arguments.Length > 4)
			{
				num = (int)context.Arguments[4];
			}
			if (num != 0 && num != 1)
			{
				return ErrorExpressions.NumberError;
			}
			double guess = 0.1;
			if (context.Arguments.Length > 5)
			{
				guess = context.Arguments[5];
			}
			double number;
			try
			{
				number = FinancialFunctions.RATE(nPer, pmt, pv, fv, num, guess);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "RATE";

		static readonly FunctionInfo Info;
	}
}
