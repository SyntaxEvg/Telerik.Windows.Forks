using System;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class CellBordersLayer : CellBordersLayerBase
	{
		public CellBordersLayer(IRenderer<LineRenderable> lineRenderer, bool shouldShowTopLeftMostBorders)
			: base(lineRenderer, shouldShowTopLeftMostBorders, true, CellBordersLayer.ShowBorder, "CellBorders")
		{
		}

		public const bool SupportDiagonals = true;

		public static readonly Func<CellBorder, bool> ShowBorder = (CellBorder border) => border != CellBorder.Default;
	}
}
