using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class ColumnBasedLayerBase : BoxBasedLayerBase<ColumnLayoutBox>
	{
		protected override IEnumerable<ColumnLayoutBox> GetVisibleBoxes(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext)
		{
			return ColumnBasedLayerBase.GetVisibleBoxesInternal(viewportPaneType, updateContext);
		}

		internal static IEnumerable<ColumnLayoutBox> GetVisibleBoxesInternal(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext)
		{
			ViewportPane viewportPane = updateContext.SheetViewport[viewportPaneType];
			CellRange cellRange = viewportPane.VisibleRange;
			if (viewportPane.IsEmpty)
			{
				return Enumerable.Empty<ColumnLayoutBox>();
			}
			return from x in updateContext.VisibleColumnBoxes
				where x.ColumnIndex >= cellRange.FromIndex.ColumnIndex && x.ColumnIndex <= cellRange.ToIndex.ColumnIndex
				select x;
		}
	}
}
