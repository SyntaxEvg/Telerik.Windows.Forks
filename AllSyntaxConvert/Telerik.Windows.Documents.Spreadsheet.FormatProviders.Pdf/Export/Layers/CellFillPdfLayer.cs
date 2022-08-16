using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class CellFillPdfLayer : PdfLayerBase
	{
		public CellFillPdfLayer()
		{
			this.patternFillRenderer = new PdfPatternFillRenderer();
			this.gradientFillRenderer = new PdfGradientFillRenderer();
			this.cellFillLayer = new CellFillLayer(this.patternFillRenderer, this.gradientFillRenderer);
		}

		public override string Name
		{
			get
			{
				return "CellFill";
			}
		}

		public override void UpdateRenderOverride(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			this.patternFillRenderer.Editor = editor;
			this.gradientFillRenderer.Editor = editor;
			this.cellFillLayer.UpdateRender(updateContext);
		}

		readonly PdfPatternFillRenderer patternFillRenderer;

		readonly PdfGradientFillRenderer gradientFillRenderer;

		readonly CellFillLayer cellFillLayer;
	}
}
