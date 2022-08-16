using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class RowBasedLayerBase : BoxBasedLayerBase<RowLayoutBox>
	{
		protected override IEnumerable<RowLayoutBox> GetVisibleBoxes(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext)
		{
			return RowBasedLayerBase.GetVisibleBoxesInternal(viewportPaneType, updateContext);
		}

		internal static IEnumerable<RowLayoutBox> GetVisibleBoxesInternal(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext)
		{
			ViewportPane viewportPane = updateContext.SheetViewport[viewportPaneType];
			CellRange cellRange = viewportPane.VisibleRange;
			if (viewportPane.IsEmpty)
			{
				return Enumerable.Empty<RowLayoutBox>();
			}
			return from x in updateContext.VisibleRowBoxes
				where x.RowIndex >= cellRange.FromIndex.RowIndex && x.RowIndex <= cellRange.ToIndex.RowIndex
				select x;
		}
	}
}
