using System;
using System.Collections.Generic;
using Telerik.Documents.SpreadsheetStreaming.Core;
using Telerik.Documents.SpreadsheetStreaming.Model;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	sealed class XlsxColumnExporter : EntityBase, IColumnExporter, IDisposable
	{
		internal XlsxColumnExporter(Queue<ColumnRange> columnRangesQueue, int columnIndex, Action notifyDisposeAction)
		{
			this.notifyDisposeAction = notifyDisposeAction;
			columnIndex++;
			this.columnRange = new ColumnRange(columnIndex);
			this.columnRangesQueue = columnRangesQueue;
		}

		public void SetWidthInPixels(double width)
		{
			Guard.ThrowExceptionIfLessThan<double>(0.0, width, "width");
			double value = UnitHelper.PixelWidthToExcelColumnWidth(width);
			this.columnRange.CustomWidth = new bool?(true);
			this.columnRange.Width = new double?(value);
		}

		public void SetWidthInCharacters(double count)
		{
			Guard.ThrowExceptionIfOutOfRange<double>(0.0, 255.0, count, "count");
			double num = UnitHelper.CharactersCountToPixelWidth(count);
			num += DefaultValues.LeftCellMargin + DefaultValues.RightCellMargin;
			this.SetWidthInPixels(num);
		}

		public void SetOutlineLevel(int value)
		{
			Guard.ThrowExceptionIfLessThan<int>(0, value, "value");
			this.columnRange.OutlineLevel = new int?(value);
		}

		public void SetHidden(bool value)
		{
			this.columnRange.Hidden = new bool?(value);
		}

		internal sealed override void CompleteWriteOverride()
		{
			if (this.columnRangesQueue.Count > 0)
			{
				ColumnRange columnRange = this.columnRangesQueue.Peek();
				if (columnRange.LastColumnNumber + 1 == this.columnRange.LastColumnNumber && columnRange.HasEqualColumnProperties(this.columnRange))
				{
					columnRange.MergeWith(this.columnRange);
					return;
				}
			}
			this.columnRangesQueue.Enqueue(this.columnRange);
		}

		internal override void DisposeOverride()
		{
			this.notifyDisposeAction();
			base.DisposeOverride();
		}

		readonly ColumnRange columnRange;

		readonly Queue<ColumnRange> columnRangesQueue;

		readonly Action notifyDisposeAction;
	}
}
