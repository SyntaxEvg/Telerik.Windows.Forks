using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public interface IDataValidationRule
	{
		bool ShowInputMessage { get; }

		string InputMessageTitle { get; }

		string InputMessageContent { get; }

		bool ShowErrorMessage { get; }

		ErrorStyle ErrorStyle { get; }

		string ErrorAlertTitle { get; }

		string ErrorAlertContent { get; }

		bool Evaluate(Worksheet worksheet, int rowIndex, int columnIndex, ICellValue cellValue);
	}
}
