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
	public class Sln : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Sln.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Sln.Info;
			}
		}

		static Sln()
		{
			string description = "Returns the straight-line depreciation of an asset for one period.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Sln_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Cost", "is the initial cost of the asset.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Cost", "Spreadsheet_Functions_Sln_CostInfo"),
				new ArgumentInfo("Salvage", "is the value at the end of the depreciation (sometimes called the salvage value of the asset).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Salvage", "Spreadsheet_Functions_Sln_SalvageInfo"),
				new ArgumentInfo("Life", "is the number of periods over which the asset is depreciated (sometimes called the useful life of the asset).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Life", "Spreadsheet_Functions_Sln_LifeInfo")
			};
			string formatString = CurrencyFormatStringBuilder.BuildFormatStrings(new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null), 2).Last<string>();
			Sln.Info = new FunctionInfo(Sln.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, new CellValueFormat(formatString), false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double cost = context.Arguments[0];
			double salvage = context.Arguments[1];
			double num = context.Arguments[2];
			if (num == 0.0)
			{
				return ErrorExpressions.DivisionByZero;
			}
			double number;
			try
			{
				number = FinancialFunctions.SLN(cost, salvage, num);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "SLN";

		static readonly FunctionInfo Info;
	}
}
