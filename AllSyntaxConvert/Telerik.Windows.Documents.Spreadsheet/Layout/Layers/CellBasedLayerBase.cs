using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class CellBasedLayerBase : BoxBasedLayerBase<CellLayoutBox>
	{
		protected override IEnumerable<CellLayoutBox> GetVisibleBoxes(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext)
		{
			return CellBasedLayerBase.GetVisibleBoxesInternal(viewportPaneType, updateContext);
		}

		internal static IEnumerable<CellLayoutBox> GetVisibleBoxesInternal(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext)
		{
			foreach (CellLayoutBox cellBox in updateContext.VisibleCellLayoutBoxes[viewportPaneType])
			{
				if (cellBox.MergeState != CellMergeState.NonTopLeftCellInMergedRange)
				{
					yield return cellBox;
				}
			}
			yield break;
		}
	}
}
