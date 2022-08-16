using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class NPer : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return NPer.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return NPer.Info;
			}
		}

		static NPer()
		{
			string description = "Returns the number of periods for an investment based on periodic, constant payments and a constant interest rate.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Nper_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the interest rate per period. For example, use 6%/4 for quarterly payments at 6% APR.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Nper_RateInfo"),
				new ArgumentInfo("Pmt", "is the payment made each period; it cannot change over the life of the annuity. Typically, pmt contains principal and interest but no other fees or taxes.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pmt", "Spreadsheet_Functions_Nper_PmtInfo"),
				new ArgumentInfo("Pv", "is the present value, or the lump-sum amount that a series of future payments is worth right now.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Nper_PvInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Fv", "is the future value, or a cash balance you want to attain after the last payment is made. If fv is omitted, it is assumed to be 0 (the future value of a loan, for example, is 0).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Fv", "Spreadsheet_Functions_Nper_FvInfo"),
				new ArgumentInfo("Type", "is the number 0 or 1 and indicates when payments are due.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Type", "Spreadsheet_Functions_Nper_TypeInfo")
			};
			NPer.Info = new FunctionInfo(NPer.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double rate = context.Arguments[0];
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
			if (num < 0 || num > 1)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.NPER(rate, pmt, pv, fv, num);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "NPER";

		static readonly FunctionInfo Info;
	}
}
