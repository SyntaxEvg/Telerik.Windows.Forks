using System;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class Today : FunctionBase
	{
		public override string Name
		{
			get
			{
				return Today.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return Today.Info;
			}
		}

		static Today()
		{
			string description = "Returns the serial number of the current date. If the cell format was General before the function was entered, the result is formatted as a date.";
			string descriptionLocalizationKey = "Spreadsheet_Functions_Today_Info";
			string formatString = FormatHelper.DefaultSpreadsheetCulture.CultureInfo.DateTimeFormat.ShortDatePattern.ToLower();
			Today.Info = new FunctionInfo(Today.FunctionName, FunctionCategory.DateTime, description, new CellValueFormat(formatString), descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			double number = FormatHelper.ConvertDateTimeToDouble(DateTime.Now.Date);
			return NumberExpression.CreateValidNumberOrErrorExpression(number);
		}

		public static readonly string FunctionName = "TODAY";

		static readonly FunctionInfo Info;
	}
}
