using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class TBillPrice : NumbersInFunction
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
				return TBillPrice.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return TBillPrice.Info;
			}
		}

		static TBillPrice()
		{
			string description = "Returns the price per $100 face value for a Treasury bill.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_TBillPrice_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Settlement", "is the Treasury bill's settlement date. The security settlement date is the date after the issue date when the Treasury bill is traded to the buyer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Settlement", "Spreadsheet_Functions_TBill_SettlementInfo"),
				new ArgumentInfo("Maturity", "is the Treasury bill's maturity date. The maturity date is the date when the Treasury bill expires.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Maturity", "Spreadsheet_Functions_TBill_MaturityInfo"),
				new ArgumentInfo("Discount", "is the Treasury bill's discount rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Discount", "Spreadsheet_Functions_TBill_DiscountInfo")
			};
			TBillPrice.Info = new FunctionInfo(TBillPrice.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(context.Arguments[0]);
			DateTime? dateTime2 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[1]);
			double num = context.Arguments[2];
			if (dateTime == null || dateTime2 == null || dateTime2 <= dateTime || num <= 0.0)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.TBILLPRICE(dateTime.Value, dateTime2.Value, num);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "TBILLPRICE";

		static readonly FunctionInfo Info;
	}
}
