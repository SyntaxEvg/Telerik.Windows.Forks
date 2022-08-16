using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class GridlinesOutlinePdfLayer : PdfLayerBase
	{
		public GridlinesOutlinePdfLayer()
		{
			this.lineRenderer = new PdfLineRenderer();
			this.gridlinesOutlinesLayer = new GridlinesOutlinesLayer(this.lineRenderer);
		}

		public override string Name
		{
			get
			{
				return "GridlinesOutline";
			}
		}

		public override void UpdateRenderOverride(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			this.lineRenderer.Editor = editor;
			this.gridlinesOutlinesLayer.UpdateRender(updateContext);
		}

		readonly PdfLineRenderer lineRenderer;

		readonly GridlinesOutlinesLayer gridlinesOutlinesLayer;
	}
}
