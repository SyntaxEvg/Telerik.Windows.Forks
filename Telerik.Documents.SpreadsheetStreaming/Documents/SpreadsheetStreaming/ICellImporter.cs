using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	interface ICellImporter
	{
		int RowIndex { get; }

		int ColumnIndex { get; }

		SpreadCellFormat Format { get; }

		string Value { get; }
	}
}
