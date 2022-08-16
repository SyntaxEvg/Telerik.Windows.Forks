using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class GridlinesPdfLayer : PdfLayerBase
	{
		public GridlinesPdfLayer()
		{
			this.lineRenderer = new PdfLineRenderer();
			this.gridlinesLayer = new GridlinesLayer(this.lineRenderer);
		}

		public override string Name
		{
			get
			{
				return "Gridlines";
			}
		}

		public override void UpdateRenderOverride(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			this.lineRenderer.Editor = editor;
			this.gridlinesLayer.UpdateRender(updateContext);
		}

		readonly PdfLineRenderer lineRenderer;

		readonly GridlinesLayer gridlinesLayer;
	}
}
