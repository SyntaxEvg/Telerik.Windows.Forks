using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public interface IWorksheetViewExporter : IDisposable
	{
		void SetFirstVisibleCell(int rowIndex, int columnIndex);

		void SetFreezePanes(int rowsCount, int columnsCount);

		void SetFreezePanes(int rowsCount, int columnsCount, int scrollablePaneFirstVisibleCellRowIndex, int scrollablePaneFirstVisibleCellColumnIndex);

		void SetScaleFactor(double percent);

		void SetShouldShowGridLines(bool value);

		void SetShouldShowRowColumnHeaders(bool value);

		void SetActiveSelectionCell(int rowIndex, int columnIndex);

		void AddSelectionRange(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex);
	}
}
