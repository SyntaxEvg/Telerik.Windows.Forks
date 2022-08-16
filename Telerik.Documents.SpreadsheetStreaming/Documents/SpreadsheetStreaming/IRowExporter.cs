using System;

namespace Telerik.Documents.SpreadsheetStreaming
{
	public interface IRowExporter : IDisposable
	{
		ICellExporter CreateCellExporter();

		void SkipCells(int count);

		void SetOutlineLevel(int value);

		void SetHeightInPixels(double value);

		void SetHeightInPoints(double value);

		void SetHidden(bool value);
	}
}
