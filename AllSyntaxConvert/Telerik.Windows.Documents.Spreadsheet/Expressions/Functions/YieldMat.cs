using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class YieldMat : NumbersInFunction
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
				return YieldMat.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return YieldMat.Info;
			}
		}

		static YieldMat()
		{
			string description = "Returns the annual yield of a security that pays interest at maturity.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_YieldMat_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Settlement", "is the security's settlement date. The security settlement date is the date after the issue date when the security is traded to the buyer.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Settlement", "Spreadsheet_Functions_Yield_SettlementInfo"),
				new ArgumentInfo("Maturity", "is the security's maturity date. The maturity date is the date when the security expires.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Maturity", "Spreadsheet_Functions_Yield_MaturityInfo"),
				new ArgumentInfo("Issue", "is the ssecurity's issue date, expressed as a serial date number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Issue", "Spreadsheet_Functions_YieldMat_IssueInfo"),
				new ArgumentInfo("Rate", "is the annual coupon rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_YieldMat_RateInfo"),
				new ArgumentInfo("Pr", "is the type of day count basis to use", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Pr", "Spreadsheet_Functions_YieldMat_PrInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Basis", "is the type of day count basis to use.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Basis", "Spreadsheet_Functions_Yield_MaturityInfo")
			};
			YieldMat.Info = new FunctionInfo(YieldMat.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(context.Arguments[0]);
			DateTime? dateTime2 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[1]);
			DateTime? dateTime3 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[2]);
			double num = context.Arguments[3];
			double num2 = context.Arguments[4];
			int num3 = 0;
			if (context.Arguments.Length == 6)
			{
				num3 = (int)context.Arguments[5];
			}
			if (dateTime == null || dateTime2 == null || dateTime3 == null || dateTime3 >= dateTime || dateTime >= dateTime2 || num <= 0.0 || num2 <= 0.0 || num3 < 0 || num3 > 4)
			{
				return ErrorExpressions.NumberError;
			}
			double number = 5.0;
			try
			{
				number = FinancialFunctions.YIELDMAT(dateTime.Value, dateTime2.Value, dateTime3.Value, num, num2, num3);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "YIELDMAT";

		static readonly FunctionInfo Info;
	}
}
