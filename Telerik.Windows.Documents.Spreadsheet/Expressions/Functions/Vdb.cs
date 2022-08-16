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
	public class Vdb : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Vdb.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Vdb.Info;
			}
		}

		static Vdb()
		{
			string description = "Returns the depreciation for each accounting period. This function is provided for the French accounting system. If an asset is purchased in the middle of the accounting period, the prorated depreciation is taken into account. The function is similar to AMORLINC, except that a depreciation coefficient is applied in the calculation depending on the life of the assets.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Vdb_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Cost", "is the initial cost of the asset.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Cost", "Spreadsheet_Functions_Vdb_CostInfo"),
				new ArgumentInfo("Salvage", "is the value at the end of the depreciation (sometimes called the salvage value of the asset). This value can be 0.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Salvage", "Spreadsheet_Functions_Vdb_SalvageInfo"),
				new ArgumentInfo("Life", "is the number of periods over which the asset is depreciated (sometimes called the useful life of the asset)..", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Life", "Spreadsheet_Functions_Vdb_LifeInfo"),
				new ArgumentInfo("StartPeriod", "is the starting period for which you want to calculate the depreciation. Start_period must use the same units as life.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_StartPeriod", "Spreadsheet_Functions_Vdb_StartPeriodInfo"),
				new ArgumentInfo("EndPeriod", "is the ending period for which you want to calculate the depreciation. End_period must use the same units as life.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_EndPeriod", "Spreadsheet_Functions_Vdb_EndPeriodInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Factor", "is the rate at which the balance declines. If factor is omitted, it is assumed to be 2 (the double-declining balance method). Change factor if you do not want to use the double-declining balance method. For a description of the double-declining balance method, see DDB.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Factor", "Spreadsheet_Functions_Vdb_FactorInfo"),
				new ArgumentInfo("NoSwitch", "A logical value specifying whether to switch to straight-line depreciation when depreciation is greater than the declining balance calculation.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_NoSwitch", "Spreadsheet_Functions_Vdb_NoSwitchInfo")
			};
			string formatString = CurrencyFormatStringBuilder.BuildFormatStrings(new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null), 2).Last<string>();
			Vdb.Info = new FunctionInfo(Vdb.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, new CellValueFormat(formatString), 1, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double cost = context.Arguments[0];
			double salvage = context.Arguments[1];
			double life = context.Arguments[2];
			double startPeriod = context.Arguments[3];
			double endPeriod = context.Arguments[4];
			double number;
			try
			{
				if (context.Arguments.Length == 5)
				{
					number = FinancialFunctions.VDB(cost, salvage, life, startPeriod, endPeriod);
				}
				else if (context.Arguments.Length == 6)
				{
					number = FinancialFunctions.VDB(cost, salvage, life, startPeriod, endPeriod, context.Arguments[5]);
				}
				else
				{
					int num = (int)context.Arguments[6];
					if (num != 0 && num != 1)
					{
						return ErrorExpressions.NumberError;
					}
					number = FinancialFunctions.VDB(cost, salvage, life, startPeriod, endPeriod, context.Arguments[5], num);
				}
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "VDB";

		static readonly FunctionInfo Info;
	}
}
