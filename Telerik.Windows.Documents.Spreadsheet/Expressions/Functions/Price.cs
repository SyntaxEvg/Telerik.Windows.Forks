using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Price : NumbersInFunction
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
				return Price.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Price.Info;
			}
		}

		static Price()
		{
			string description = "Returns the price per $100 face value of a security that pays periodic interest.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Price_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Settlement", "is the security's settlement date. The security settlement date is the date after the issue date when the security is traded to the buyer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Settlement", "Spreadsheet_Functions_Price_SettlementInfo"),
				new ArgumentInfo("Maturity", "is the security's maturity date. The maturity date is the date when the security expires.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Maturity", "Spreadsheet_Functions_Price_MaturityInfo"),
				new ArgumentInfo("Rate", "is the annual coupon rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Price_RateInfo"),
				new ArgumentInfo("Yld", "is the security's annual yield.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Yld", "Spreadsheet_Functions_Price_YldInfo"),
				new ArgumentInfo("Redemption", "is the security's redemption value per $100 face value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Redemption", "Spreadsheet_Functions_Price_RedemptionInfo"),
				new ArgumentInfo("Frequency", "is the number of coupon payments per year. For annual payments, frequency = 1; for semiannual, frequency = 2; for quarterly, frequency = 4.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Frequency", "Spreadsheet_Functions_Price_FrequencyInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Basis", "is the type of day count basis to use.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Basis", "Spreadsheet_Functions_Price_BasisInfo")
			};
			Price.Info = new FunctionInfo(Price.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(context.Arguments[0]);
			DateTime? dateTime2 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[1]);
			double num = context.Arguments[2];
			double num2 = context.Arguments[3];
			double num3 = context.Arguments[4];
			int num4 = (int)context.Arguments[5];
			int num5 = 0;
			if (context.Arguments.Length == 7)
			{
				num5 = (int)context.Arguments[6];
			}
			if (dateTime == null || dateTime2 == null || (dateTime >= dateTime2 || num < 0.0 || num2 < 0.0 || num3 <= 0.0 || (num4 != 1 && num4 != 2 && num4 != 4)) || num5 < 0 || num5 > 4)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.PRICE(dateTime.Value, dateTime2.Value, num, num2, num3, num4, num5);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "PRICE";

		static readonly FunctionInfo Info;
	}
}
