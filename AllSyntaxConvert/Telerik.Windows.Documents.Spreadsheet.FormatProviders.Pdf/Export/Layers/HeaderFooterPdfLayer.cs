using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class HeaderFooterPdfLayer : PdfLayerBase
	{
		public HeaderFooterPdfLayer()
		{
			this.sectionRenderer = new PdfHeaderFooterSectionRenderer();
			this.headerFooterLayer = new HeaderFooterLayer(this.sectionRenderer);
		}

		public override string Name
		{
			get
			{
				return "HeaderFooter";
			}
		}

		public override bool ClipToSheetViewport
		{
			get
			{
				return false;
			}
		}

		public override void UpdateRenderOverride(PdfPrintWorksheetRenderUpdateContext updateContext, FixedContentEditor editor)
		{
			this.sectionRenderer.Editor = editor;
			this.headerFooterLayer.UpdateRender(updateContext);
		}

		readonly PdfHeaderFooterSectionRenderer sectionRenderer;

		readonly HeaderFooterLayer headerFooterLayer;
	}
}
