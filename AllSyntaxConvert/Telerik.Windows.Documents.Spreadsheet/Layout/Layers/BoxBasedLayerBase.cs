using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	abstract class BoxBasedLayerBase<T> : WorksheetLayerBase where T : LayoutBox
	{
		protected override void UpdateRenderOverride(WorksheetRenderUpdateContext worksheetUpdateContext)
		{
			foreach (KeyValuePair<ViewportPaneType, IEnumerable<CellLayoutBox>> keyValuePair in worksheetUpdateContext.VisibleCellLayoutBoxes)
			{
				ViewportPaneType key = keyValuePair.Key;
				foreach (T box in this.GetVisibleBoxes(key, worksheetUpdateContext))
				{
					IRenderable renderable = this.CreateRenderableElementForBox(key, box, worksheetUpdateContext);
					if (renderable != null)
					{
						base.ContainerManager.Add(renderable, key);
					}
				}
			}
		}

		protected abstract IEnumerable<T> GetVisibleBoxes(ViewportPaneType viewportPaneType, WorksheetRenderUpdateContext updateContext);

		protected abstract IRenderable CreateRenderableElementForBox(ViewportPaneType viewportPaneType, T box, WorksheetRenderUpdateContext updateContext);
	}
}
