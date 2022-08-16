using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public interface IWorksheetExporter : IDisposable
	{
		IWorksheetViewExporter CreateWorksheetViewExporter();

		IRowExporter CreateRowExporter();

		IColumnExporter CreateColumnExporter();

		void SkipRows(int count);

		void SkipColumns(int count);

		void MergeCells(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex);
	}
}
