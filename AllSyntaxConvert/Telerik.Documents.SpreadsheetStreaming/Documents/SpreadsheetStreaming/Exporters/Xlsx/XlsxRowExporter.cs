using System;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.ImportExport.Model.Elements.Worksheet;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	sealed class XlsxRowExporter : EntityBase, IRowExporter, IDisposable
	{
		internal XlsxRowExporter(SheetDataElement sheetData, StylesRepository stylesElement, int rowIndex)
		{
			Guard.ThrowExceptionIfGreaterThan<int>(DefaultValues.RowCount - 1, rowIndex, "rowIndex");
			this.stylesElement = stylesElement;
			this.columnIndex = 0;
			this.rowIndex = rowIndex;
			this.rowElementWriter = sheetData.CreateRowElementWriter();
			this.rowElementWriter.RowIndex = new int?(this.rowIndex + 1);
		}

		public void SkipCells(int count)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, count, "count");
			if (this.hasAliveCell)
			{
				throw new InvalidOperationException("Only one ICellExporter is allowed at a time. Please dispose any previously created ICellExporter instances.");
			}
			this.columnIndex += count;
		}

		public ICellExporter CreateCellExporter()
		{
			if (this.hasAliveCell)
			{
				throw new InvalidOperationException("Only one ICellExporter is allowed at a time. Please dispose any previously created ICellExporter instances.");
			}
			this.rowElementWriter.EnsureWritingStarted();
			this.hasAliveCell = true;
			return new XlsxCellExporter(this, this.rowElementWriter, this.stylesElement, this.rowIndex, this.columnIndex++);
		}

		public void SetOutlineLevel(int value)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, value, "value");
			this.rowElementWriter.OutlineLevel = value;
		}

		public void SetHeightInPixels(double value)
		{
			Guard.ThrowExceptionIfLessThan<double>(0.0, value, "value");
			double heightInPoints = UnitHelper.DipToPoint(value);
			this.SetHeightInPoints(heightInPoints);
		}

		public void SetHeightInPoints(double value)
		{
			Guard.ThrowExceptionIfOutOfRange<double>(0.0, 409.0, value, "value");
			this.rowElementWriter.CustomHeight = true;
			this.rowElementWriter.RowHeight = value;
		}

		public void SetHidden(bool value)
		{
			this.rowElementWriter.Hidden = value;
		}

		internal void NotifyCellDisposing()
		{
			this.hasAliveCell = false;
		}

		internal sealed override void CompleteWriteOverride()
		{
			this.rowElementWriter.EnsureWritingStarted();
			this.rowElementWriter.EnsureWritingEnded();
		}

		readonly StylesRepository stylesElement;

		readonly RowElement rowElementWriter;

		readonly int rowIndex;

		int columnIndex;

		bool hasAliveCell;
	}
}
