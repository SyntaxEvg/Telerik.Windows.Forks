using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class IntRate : NumbersInFunction
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
				return IntRate.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return IntRate.Info;
			}
		}

		static IntRate()
		{
			string description = "Returns the interest rate for a fully invested security.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_IntRate_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Settlement", "is the security's settlement date. The security settlement date is the date after the issue date when the security is traded to the buyer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Settlement", "Spreadsheet_Functions_IntRate_SettlementInfo"),
				new ArgumentInfo("Maturity", "is the security's maturity date. The maturity date is the date when the security expires.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Maturity", "Spreadsheet_Functions_IntRate_MaturityInfo"),
				new ArgumentInfo("Investment", "is the amount invested in the security.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Investment", "Spreadsheet_Functions_IntRate_InvestmentInfo"),
				new ArgumentInfo("Redemption", "is the security's redemption value per $100 face value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Redemption", "Spreadsheet_Functions_IntRate_RedemptionInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Basis", "is the type of day count basis to use.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Basis", "Spreadsheet_Functions_IntRate_BasisInfo")
			};
			IntRate.Info = new FunctionInfo(IntRate.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(context.Arguments[0]);
			DateTime? dateTime2 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[1]);
			double num = context.Arguments[2];
			double num2 = context.Arguments[3];
			int num3 = 0;
			if (context.Arguments.Length == 5)
			{
				num3 = (int)context.Arguments[4];
			}
			if (dateTime == null || dateTime2 == null || dateTime >= dateTime2 || num <= 0.0 || num2 <= 0.0 || num3 < 0 || num3 > 4)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.INRATE(dateTime.Value, dateTime2.Value, num, num2, num3);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "INTRATE";

		static readonly FunctionInfo Info;
	}
}
