using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class CumIPmt : NumbersInFunction
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
				return CumIPmt.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return CumIPmt.Info;
			}
		}

		static CumIPmt()
		{
			string description = "Returns the cumulative interest paid on a loan between start_period and end_period.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_CumIPmt_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the interest rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Cum_RateInfo"),
				new ArgumentInfo("Nper", "is the total number of payment periods.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Nper", "Spreadsheet_Functions_Cum_NperInfo"),
				new ArgumentInfo("Pv", "is the present value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pv", "Spreadsheet_Functions_Cum_PvInfo"),
				new ArgumentInfo("StartPeriod", "is the first period in the calculation. Payment periods are numbered beginning with 1.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_StartPeriod", "Spreadsheet_Functions_Cum_StartPeriodInfo"),
				new ArgumentInfo("EndPeriod", "is the last period in the calculation.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_EndPeriod", "Spreadsheet_Functions_Cum_EndPeriodInfo"),
				new ArgumentInfo("Type", "is the timing of the payment.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Type", "Spreadsheet_Functions_Cum_TypeInfo")
			};
			CumIPmt.Info = new FunctionInfo(CumIPmt.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			double num3 = context.Arguments[2];
			double num4 = context.Arguments[3];
			double num5 = context.Arguments[4];
			int num6 = (int)context.Arguments[5];
			if (num <= 0.0 || num2 <= 0.0 || num3 <= 0.0 || num4 < 1.0 || num5 < 1.0 || num4 > num5 || (num6 != 0 && num6 != 1))
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.CUMIPMT(num, num2, num3, num4, num5, num6);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "CUMIPMT";

		static readonly FunctionInfo Info;
	}
}
