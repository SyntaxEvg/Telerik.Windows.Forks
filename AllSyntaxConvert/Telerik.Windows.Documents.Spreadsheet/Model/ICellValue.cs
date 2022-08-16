using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public interface ICellValue
	{
		string RawValue { get; }

		CellValueType ValueType { get; }

		CellValueType ResultValueType { get; }

		string GetValueAsString(CellValueFormat format);

		string GetResultValueAsString(CellValueFormat format);
	}
}
