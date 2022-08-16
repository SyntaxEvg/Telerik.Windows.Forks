using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class CellBordersPdfLayer : PdfLayerBase
	{
		public CellBordersPdfLayer()
		{
			this.lineRenderer = new PdfLineRenderer();
			this.cellBordersLayer = new CellBordersLayer(this.lineRenderer, true);
		}

		public override string Name
		{
			get
			{
				return "CellBorders";
			}
		}

		public override void UpdateRenderOverride(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			this.lineRenderer.Editor = editor;
			this.cellBordersLayer.UpdateRender(updateContext);
		}

		readonly PdfLineRenderer lineRenderer;

		readonly CellBordersLayer cellBordersLayer;
	}
}
