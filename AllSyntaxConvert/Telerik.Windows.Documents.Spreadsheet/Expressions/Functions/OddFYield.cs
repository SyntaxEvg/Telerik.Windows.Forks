using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class OddFYield : NumbersInFunction
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
				return OddFYield.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return OddFYield.Info;
			}
		}

		static OddFYield()
		{
			string description = "Returns the yield of a security that has an odd (short or long) first period.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_OddFYield_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Settlement", "is the security's settlement date. The security settlement date is the date after the issue date when the security is traded to the buyer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Settlement", "Spreadsheet_Functions_OddFL_SettlementInfo"),
				new ArgumentInfo("Maturity", "is the security's maturity date. The maturity date is the date when the security expires.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Maturity", "Spreadsheet_Functions_OddFL_MaturityInfo"),
				new ArgumentInfo("Issue", "is the security's issue date.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Issue", "Spreadsheet_Functions_OddF_IssueInfo"),
				new ArgumentInfo("FirstCoupon", "is the security's first coupon date.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_FirstCoupon", "Spreadsheet_Functions_OddF_FirstCouponInfo"),
				new ArgumentInfo("Rate", "is the security's interest rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_OddFL_RateInfo"),
				new ArgumentInfo("Pr", "is the security's price.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pr", "Spreadsheet_Functions_OddYield_PrInfo"),
				new ArgumentInfo("Redemption", "is the security's redemption value per $100 face value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Redemption", "Spreadsheet_Functions_OddFL_RedemptionInfo"),
				new ArgumentInfo("Frequency", "is the number of coupon payments per year. For annual payments, frequency = 1; for semiannual, frequency = 2; for quarterly, frequency = 4.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Frequency", "Spreadsheet_Functions_OddFL_FrequencyInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Basis", "is the type of day count basis to use.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Basis", "Spreadsheet_Functions_OddFL_BasisInfo")
			};
			OddFYield.Info = new FunctionInfo(OddFYield.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(context.Arguments[0]);
			DateTime? dateTime2 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[1]);
			DateTime? dateTime3 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[2]);
			DateTime? dateTime4 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[3]);
			double num = context.Arguments[4];
			double num2 = context.Arguments[5];
			double num3 = context.Arguments[6];
			int frequency = (int)context.Arguments[7];
			int num4 = 0;
			if (context.Arguments.Length == 9)
			{
				num4 = (int)context.Arguments[8];
			}
			if (dateTime == null || dateTime2 == null || dateTime3 == null || dateTime4 == null || dateTime2 <= dateTime4 || dateTime4 <= dateTime || dateTime <= dateTime3 || num < 0.0 || num2 <= 0.0 || num3 <= 0.0 || num4 < 0 || num4 > 4)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.ODDFYIELD(dateTime.Value, dateTime2.Value, dateTime3.Value, dateTime4.Value, num, num2, num3, frequency, num4);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ODDFYIELD";

		static readonly FunctionInfo Info;
	}
}
