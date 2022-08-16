using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings.Builders;
using Telerik.Windows.Documents.Spreadsheet.Maths;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Db : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Db.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Db.Info;
			}
		}

		static Db()
		{
			string description = "Returns the depreciation of an asset for a specified period using the fixed-declining balance method.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Db_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Cost", "is the initial cost of the asset.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Cost", "Spreadsheet_Functions_Db_CostInfo"),
				new ArgumentInfo("Salvage", "is the value at the end of the depreciation (sometimes called the salvage value of the asset).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Salvage", "Spreadsheet_Functions_Db_SalvageInfo"),
				new ArgumentInfo("Life", "is the number of periods over which the asset is being depreciated (sometimes called the useful life of the asset).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Life", "Spreadsheet_Functions_Db_LifeInfo"),
				new ArgumentInfo("Period", "is the period for which you want to calculate the depreciation. Period must use the same units as life.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Period", "Spreadsheet_Functions_Db_PeriodInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Month", "is the number of months in the first year. If month is omitted, it is assumed to be 12.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Month", "Spreadsheet_Functions_Db_MonthInfo")
			};
			string formatString = CurrencyFormatStringBuilder.BuildFormatStrings(new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null), 2).Last<string>();
			Db.Info = new FunctionInfo(Db.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, new CellValueFormat(formatString), 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double num = context.Arguments[0];
			double num2 = context.Arguments[1];
			double num3 = context.Arguments[2];
			double num4 = context.Arguments[3];
			int num5 = 12;
			if (context.Arguments.Length == 5)
			{
				num5 = (int)context.Arguments[4];
			}
			if (num < 0.0 || num2 < 0.0 || num3 <= 0.0 || num4 <= 0.0 || num4 > num3 || num5 <= 0 || num5 > 12)
			{
				return ErrorExpressions.NumberError;
			}
			double number;
			try
			{
				number = FinancialFunctions.DB(num, num2, num3, num4, (double)num5);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "DB";

		static readonly FunctionInfo Info;
	}
}
