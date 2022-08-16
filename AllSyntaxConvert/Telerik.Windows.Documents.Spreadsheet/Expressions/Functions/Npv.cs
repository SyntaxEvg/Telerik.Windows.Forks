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
	public class Npv : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Npv.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Npv.Info;
			}
		}

		static Npv()
		{
			string description = "Calculates the net present value of an investment by using a discount rate and a series of future payments (negative values) and income (positive values).";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Npv_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Rate", "is the rate of discount over the length of one period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Rate", "Spreadsheet_Functions_Npv_RateInfo"),
				new ArgumentInfo("Value1", "must be equally spaced in time and occur at the end of each period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Value1", "Spreadsheet_Functions_Npv_ValueInfo")
			};
			IEnumerable<ArgumentInfo> optionalArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Value2", "must be equally spaced in time and occur at the end of each period.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Value2", "Spreadsheet_Functions_Npv_ValueInfo")
			};
			string formatString = CurrencyFormatStringBuilder.BuildFormatStrings(new CurrencyInfo(FormatHelper.DefaultSpreadsheetCulture.CultureInfo.NumberFormat.CurrencySymbol, null), 2).Last<string>();
			Npv.Info = new FunctionInfo(Npv.FunctionName, FunctionCategory.Financial, description, requiredArgumentsInfos, optionalArgumentsInfos, new CellValueFormat(formatString), 254, false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double rate = context.Arguments[0];
			double[] array = new double[context.Arguments.Length - 1];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = context.Arguments[i + 1];
			}
			double number;
			try
			{
				number = FinancialFunctions.NPV(rate, array);
			}
			catch (Exception)
			{
				return ErrorExpressions.NumberError;
			}
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "NPV";

		static readonly FunctionInfo Info;
	}
}
