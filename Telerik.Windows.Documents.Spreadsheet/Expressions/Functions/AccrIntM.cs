using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class AccrIntM : NumbersInFunction
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
				return AccrIntM.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return AccrIntM.Info;
			}
		}

		static AccrIntM()
		{
			string description = "Returns the accrued interest for a security that pays interest at maturity.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_AccrIntM_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Issue", "is the security's issue date, expressed as a serial date number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Issue", "Spreadsheet_Functions_AccrIntM_IssueInfo"),
				new ArgumentInfo("Settlement", "is the security's maturity date, expressed as a serial date number.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Settlement", "Spreadsheet_Functions_AccrIntM_SettlementInfo"),
				new ArgumentInfo("Rate", "is the security's annual coupon rate.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_AccrIntM_RateInfo"),
				new ArgumentInfo("Par", "is the security's par value.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Par", "Spreadsheet_Functions_AccrIntM_ParInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Basis", "is the type of day count basis to use.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Basis", "Spreadsheet_Functions_AccrIntM_BasisInfo")
			};
			AccrIntM.Info = new FunctionInfo(AccrIntM.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
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
				number = FinancialFunctions.ACCRINTM(dateTime.Value, dateTime2.Value, num, num2, num3);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "ACCRINTM";

		static readonly FunctionInfo Info;
	}
}
