using System;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Csv
{
	class CsvWorksheetViewExporter : IWorksheetViewExporter, IDisposable
	{
		public void SetFirstVisibleCell(int rowIndex, int columnIndex)
		{
		}

		public void SetFreezePanes(int rowsCount, int columnsCount)
		{
		}

		public void SetFreezePanes(int rowsCount, int columnsCount, int scrollablePaneFirstVisibleCellRowIndex, int scrollablePaneFirstVisibleCellColumnIndex)
		{
		}

		public void SetScaleFactor(double percent)
		{
		}

		public void SetShouldShowGridLines(bool value)
		{
		}

		public void SetShouldShowRowColumnHeaders(bool value)
		{
		}

		public void SetActiveSelectionCell(int rowIndex, int columnIndex)
		{
		}

		public void AddSelectionRange(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
		}

		public void Dispose()
		{
		}
	}
}
