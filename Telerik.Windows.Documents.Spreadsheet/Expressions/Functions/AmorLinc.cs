using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Maths;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class AmorLinc : NumbersInFunction
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
				return AmorLinc.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return AmorLinc.Info;
			}
		}

		static AmorLinc()
		{
			string description = "Returns the depreciation for each accounting period. This function is provided for the French accounting system. If an asset is purchased in the middle of the accounting period, the prorated depreciation is taken into account.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_AmorLinc_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Cost", "is the cost of the asset.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Cost", "Spreadsheet_Functions_AmorLinc_CostInfo"),
				new ArgumentInfo("DatePurchased", "is the date of the purchase of the asset.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_DatePurchased", "Spreadsheet_Functions_AmorLinc_DatePurchasedInfo"),
				new ArgumentInfo("FirstPeriod", "is the date of the end of the first period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_FirstPeriod", "Spreadsheet_Functions_AmorLinc_FirstperiodInfo"),
				new ArgumentInfo("Salvage", "is the salvage value at the end of the life of the asset.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Salvage", "Spreadsheet_Functions_AmorLinc_SalvageInfo"),
				new ArgumentInfo("Period", "is the period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Period", "Spreadsheet_Functions_AmorLinc_PeriodInfo"),
				new ArgumentInfo("Rate", "is the rate of depreciation.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_AmorLinc_RateInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Basis", "is the year basis to be used.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Basis", "Spreadsheet_Functions_AmorLinc_BasisInfo")
			};
			AmorLinc.Info = new FunctionInfo(AmorLinc.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			DateTime? dateTime = FormatHelper.ConvertDoubleToDateTime(context.Arguments[1]);
			DateTime? dateTime2 = FormatHelper.ConvertDoubleToDateTime(context.Arguments[2]);
			double num2 = context.Arguments[3];
			double num3 = context.Arguments[4];
			double num4 = context.Arguments[5];
			int num5 = 0;
			if (context.Arguments.Length == 7)
			{
				num5 = (int)context.Arguments[6];
			}
			if (dateTime == null || dateTime2 == null || dateTime >= dateTime2 || num < 0.0 || num2 < 0.0 || num2 >= num || num3 < 0.0 || num4 <= 0.0 || num5 < 0 || num5 > 4 || num5 == 2)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.AMORLINC(num, dateTime.Value, dateTime2.Value, num2, num3, num4, num5);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "AMORLINC";

		static readonly FunctionInfo Info;
	}
}
