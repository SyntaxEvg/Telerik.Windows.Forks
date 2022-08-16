using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class GridlinesLayer : CellBordersLayerBase
	{
		public GridlinesLayer(IRenderer<LineRenderable> lineRenderer)
			: base(lineRenderer, false, false, GridlinesLayer.ShowBorder, "Gridlines")
		{
		}

		public const bool ShouldShowTopLeftMostBorders = false;

		public const bool SupportDiagonals = false;

		public static readonly Func<CellBorder, bool> ShowBorder = (CellBorder border) => border == CellBorder.Default;
	}
}
