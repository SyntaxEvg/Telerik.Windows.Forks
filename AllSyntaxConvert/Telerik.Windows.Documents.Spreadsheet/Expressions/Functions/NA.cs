using System;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions.Functions
{
	public class NA : FunctionBase
	{
		public override string Name
		{
			get
			{
				return NA.FunctionName;
			}
		}

		public override FunctionInfo FunctionInfo
		{
			get
			{
				return NA.Info;
			}
		}

		static NA()
		{
			string description = "Returns the error value #N/A. #N/A is the error value that means \"no value is available.\" Use NA to mark empty cells. By entering #N/A in cells where you are missing information, you can avoid the problem of unintentionally including empty cells in your calculations. (When a formula refers to a cell containing #N/A, the formula returns the #N/A error value.)";
			string descriptionLocalizationKey = "Spreadsheet_Functions_NA_Info";
			NA.Info = new FunctionInfo(NA.FunctionName, FunctionCategory.Information, description, descriptionLocalizationKey);
		}

		protected override RadExpression EvaluateOverride(FunctionEvaluationContext<RadExpression> context)
		{
			return ErrorExpressions.NotAvailableError;
		}

		public static readonly string FunctionName = "NA";

		static readonly FunctionInfo Info;
	}
}
