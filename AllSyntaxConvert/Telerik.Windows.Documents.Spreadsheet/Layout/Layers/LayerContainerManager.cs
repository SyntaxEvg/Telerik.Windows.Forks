using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class LayerContainerManager
	{
		public LayerContainerManager()
		{
			this.viewportPaneTypeToIRenderable = new Dictionary<ViewportPaneType, List<IRenderable>>();
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType key = (ViewportPaneType)obj;
				this.viewportPaneTypeToIRenderable[key] = new List<IRenderable>();
			}
		}

		public IEnumerable<IRenderable> GetElementsForViewportPane(ViewportPaneType viewportPaneType)
		{
			return this.viewportPaneTypeToIRenderable[viewportPaneType];
		}

		public void Add(IRenderable renderable, ViewportPaneType paneType)
		{
			this.viewportPaneTypeToIRenderable[paneType].Add(renderable);
		}

		public void Clear()
		{
			foreach (object obj in Enum.GetValues(typeof(ViewportPaneType)))
			{
				ViewportPaneType key = (ViewportPaneType)obj;
				this.viewportPaneTypeToIRenderable[key].Clear();
			}
		}

		readonly Dictionary<ViewportPaneType, List<IRenderable>> viewportPaneTypeToIRenderable;
	}
}
