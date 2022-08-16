using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout
{
	static class SheetViewportFactory
	{
		internal static SheetViewport CreateSheetViewport(RadWorksheetLayout worksheetLayout, SheetViewport oldSheetViewport, Size scaleFactor, Size finalSize, SizeI visibleSize, CellIndex frozenCellIndex, Point offset, bool horizontalScrollModeIsItemBased, bool verticalScrollModeIsItemBased)
		{
			ViewportPane viewportPane = oldSheetViewport[ViewportPaneType.Fixed];
			Point fixedTopLeft = new Point(viewportPane.Rect.Left, viewportPane.Rect.Top);
			Point scrollableTopLeft = offset;
			if (horizontalScrollModeIsItemBased || verticalScrollModeIsItemBased)
			{
				int num;
				int num2;
				worksheetLayout.GetCellIndexFromPoint(scrollableTopLeft.X, scrollableTopLeft.Y, false, false, out num, out num2, true, true);
				num = Math.Max(num, frozenCellIndex.RowIndex);
				num2 = Math.Max(num2, frozenCellIndex.ColumnIndex);
				CellLayoutBox cellLayoutBox = worksheetLayout.GetCellLayoutBox(num, num2);
				if (horizontalScrollModeIsItemBased)
				{
					scrollableTopLeft = new Point(cellLayoutBox.BoundingRectangle.Left, scrollableTopLeft.Y);
				}
				if (verticalScrollModeIsItemBased)
				{
					scrollableTopLeft = new Point(scrollableTopLeft.X, cellLayoutBox.BoundingRectangle.Top);
				}
			}
			Point maxBottomRight = worksheetLayout.GetMaxBottomRight(visibleSize);
			return new SheetViewport(worksheetLayout, finalSize, scaleFactor, fixedTopLeft, scrollableTopLeft, frozenCellIndex, maxBottomRight);
		}

		internal static SheetViewport CreateSheetViewport(RadWorksheetLayout worksheetLayout, Size displaySize, Point offset, Size scaleFactor)
		{
			SizeI visibleSize = SpreadsheetDefaultValues.VisibleSize;
			CellIndex frozenCellIndex = new CellIndex(0, 0);
			Point fixedTopLeft = new Point(0.0, 0.0);
			Point maxBottomRight = worksheetLayout.GetMaxBottomRight(visibleSize);
			return new SheetViewport(worksheetLayout, displaySize, scaleFactor, fixedTopLeft, offset, frozenCellIndex, maxBottomRight);
		}

		internal static SheetViewport CreateSheetViewport(RadWorksheetLayout worksheetLayout, Size finalSize, SizeI visibleSize, WorksheetViewState viewstate)
		{
			double rowTop = worksheetLayout.GetRowTop(viewstate.TopLeftCellIndex.RowIndex);
			double columnLeft = worksheetLayout.GetColumnLeft(viewstate.TopLeftCellIndex.ColumnIndex);
			Point fixedTopLeft = new Point(columnLeft, rowTop);
			double rowTop2 = worksheetLayout.GetRowTop(viewstate.Pane.TopLeftCellIndex.RowIndex);
			double columnLeft2 = worksheetLayout.GetColumnLeft(viewstate.Pane.TopLeftCellIndex.ColumnIndex);
			Point scrollableTopLeft = new Point(columnLeft2, rowTop2);
			CellIndex frozenCellIndex = viewstate.GetFrozenCellIndex();
			Point maxBottomRight = worksheetLayout.GetMaxBottomRight(visibleSize);
			return new SheetViewport(worksheetLayout, finalSize, viewstate.ScaleFactor, fixedTopLeft, scrollableTopLeft, frozenCellIndex, maxBottomRight);
		}

		internal static bool GetFrozenViewport(RadWorksheetLayout worksheetLayout, SheetViewport oldSheetViewport, SizeI visibleSize, CellIndex frozenCellIndex, out SheetViewport result)
		{
			result = oldSheetViewport;
			CellIndex fromIndex = oldSheetViewport[ViewportPaneType.Scrollable].VisibleRange.FromIndex;
			frozenCellIndex = SheetViewportFactory.ValidateFrozenCellIndex(oldSheetViewport, frozenCellIndex);
			int rowIndex = frozenCellIndex.RowIndex;
			int columnIndex = frozenCellIndex.ColumnIndex;
			if (frozenCellIndex.RowIndex == fromIndex.RowIndex)
			{
				rowIndex = 0;
			}
			if (frozenCellIndex.ColumnIndex == fromIndex.ColumnIndex)
			{
				columnIndex = 0;
			}
			if (frozenCellIndex.RowIndex == 0 && frozenCellIndex.ColumnIndex == 0)
			{
				return false;
			}
			frozenCellIndex = new CellIndex(rowIndex, columnIndex);
			Rect rect = oldSheetViewport[ViewportPaneType.Scrollable].Rect;
			Point fixedTopLeft = new Point(rect.Left, rect.Top);
			Rect boundingRectangle = worksheetLayout.GetCellLayoutBox(frozenCellIndex).BoundingRectangle;
			Point scrollableTopLeft = new Point(boundingRectangle.Left, boundingRectangle.Top);
			Point maxBottomRight = worksheetLayout.GetMaxBottomRight(visibleSize);
			result = new SheetViewport(worksheetLayout, oldSheetViewport.DisplaySize, oldSheetViewport.ScaleFactor, fixedTopLeft, scrollableTopLeft, frozenCellIndex, maxBottomRight);
			return true;
		}

		internal static SheetViewport GetUnfrozenViewport(RadWorksheetLayout worksheetLayout, SheetViewport oldSheetViewport)
		{
			Point topLeftPoint = oldSheetViewport.GetTopLeftPoint();
			return SheetViewportFactory.CreateSheetViewport(worksheetLayout, oldSheetViewport.DisplaySize, topLeftPoint, oldSheetViewport.ScaleFactor);
		}

		static CellIndex ValidateFrozenCellIndex(SheetViewport sheetViewport, CellIndex cellIndex)
		{
			CellIndex result = cellIndex;
			if ((sheetViewport.Contains(cellIndex) && sheetViewport[ViewportPaneType.Scrollable].VisibleRange.FromIndex != cellIndex) || (sheetViewport.ContainsRowIndex(cellIndex.RowIndex) && cellIndex.ColumnIndex == 0) || (sheetViewport.ContainsColumnIndex(cellIndex.ColumnIndex) && cellIndex.RowIndex == 0))
			{
				return result;
			}
			if (!sheetViewport.Contains(cellIndex) || sheetViewport[ViewportPaneType.Scrollable].VisibleRange.FromIndex == cellIndex)
			{
				CellRange visibleRange = sheetViewport[ViewportPaneType.Fixed].VisibleRange;
				CellRange visibleRange2 = sheetViewport[ViewportPaneType.Scrollable].VisibleRange;
				CellIndex cellIndex2 = (sheetViewport[ViewportPaneType.Fixed].IsEmpty ? visibleRange2.FromIndex : visibleRange.FromIndex);
				int num = visibleRange.RowCount + visibleRange2.RowCount;
				int num2 = visibleRange.ColumnCount + visibleRange2.ColumnCount;
				result = new CellIndex(cellIndex2.RowIndex + num / 2, cellIndex2.ColumnIndex + num2 / 2);
			}
			return result;
		}
	}
}
