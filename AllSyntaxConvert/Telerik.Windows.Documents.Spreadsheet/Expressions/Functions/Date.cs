using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Date : NumbersInFunction
	{
		public override string Name
		{
			get
			{
				return Date.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Date.Info;
			}
		}

		static Date()
		{
			string description = "Returns the sequential serial number that represents a particular date. If the cell format was General before the function was entered, the result is formatted as a date.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Date_Info";
			IEnumerable<ArgumentInfo> requiredArgumentsInfos = new ArgumentInfo[]
			{
				new ArgumentInfo("Year", "can be one to four digits. Microsoft Excel interprets the year argument according to the date system you are using. By default, Excel for Windows uses the 1900 date system; Excel for the Macintosh uses the 1904 date system.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Year", "Spreadsheet_Functions_Date_YearInfo"),
				new ArgumentInfo("Month", "is a positive or negative integer representing the month of the year from 1 to 12 (January to December).", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Month", "Spreadsheet_Functions_Date_MonthInfo"),
				new ArgumentInfo("Day", "is a positive or negative integer representing the day of the month from 1 to 31.", ArgumentType.Number, true, "Spreadsheet_Functions_Args_Day", "Spreadsheet_Functions_Date_DayInfo")
			};
			string formatString = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.DateTimeFormat.ShortDatePattern.ToLower();
			Date.Info = new FunctionInfo(Date.FunctionName, FunctionCategory.DateTime, description, requiredArgumentsInfos, new CellValueFormat(formatString), false, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<double> context)
		{
			double year = context.Arguments[0];
			double month = context.Arguments[1];
			double day = context.Arguments[2];
			double num;
			if (MathUtility.GetDateSerialNumber(year, month, day, out num) && num >= 0.0)
			{
				return NumberExpression.CreateValidNumberOrErrorExpression(num);
			}
			return ErrorExpressions.NumberError;
		}

		public static readonly string FunctionName = "DATE";

		static readonly FunctionInfo Info;
	}
}
