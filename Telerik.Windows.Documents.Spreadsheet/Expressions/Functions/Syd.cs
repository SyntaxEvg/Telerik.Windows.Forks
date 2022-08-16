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
	public class Syd : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Syd.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Syd.Info;
			}
		}

		static Syd()
		{
			string description = "Returns the sum-of-years' digits depreciation of an asset for a specified period.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Syd_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Cost", "is the initial cost of the asset.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Cost", "Spreadsheet_Functions_Syd_CostInfo"),
				new ArgumentInfo("Salvage", "is the value at the end of the depreciation (sometimes called the salvage value of the asset).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Salvage", "Spreadsheet_Functions_Syd_SalvageInfo"),
				new ArgumentInfo("Life", "is the number of periods over which the asset is depreciated (sometimes called the useful life of the asset).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Life", "Spreadsheet_Functions_Syd_LifeInfo"),
				new ArgumentInfo("Per", "is the period and must use the same units as life.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Per", "Spreadsheet_Functions_Syd_PerInfo")
			};
			string formatString = CurrencyFormatStringBuilder.BuildFormatStrings(new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null), 2).Last<string>();
			Syd.Info = new FunctionInfo(Syd.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, new CellValueFormat(formatString), false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double cost = context.Arguments[0];
			double salvage = context.Arguments[1];
			double life = context.Arguments[2];
			double period = context.Arguments[3];
			double number;
			try
			{
				number = FinancialFunctions.SYD(cost, salvage, life, period);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "SYD";

		static readonly FunctionInfo Info;
	}
}
