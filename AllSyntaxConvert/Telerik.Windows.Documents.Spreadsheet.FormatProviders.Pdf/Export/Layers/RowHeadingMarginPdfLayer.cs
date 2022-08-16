using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class RowHeadingMarginPdfLayer : PdfLayerBase
	{
		public RowHeadingMarginPdfLayer()
		{
			this.rowHeadingRenderer = new PdfRowHeadingRenderer();
			this.rowHeadingMarginLayer = new RowHeadingMarginLayer(this.rowHeadingRenderer);
		}

		public override string Name
		{
			get
			{
				return "RowHeadingMargin";
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
			this.rowHeadingRenderer.Editor = editor;
			this.rowHeadingMarginLayer.UpdateRender(updateContext);
		}

		readonly PdfRowHeadingRenderer rowHeadingRenderer;

		readonly RowHeadingMarginLayer rowHeadingMarginLayer;
	}
}
