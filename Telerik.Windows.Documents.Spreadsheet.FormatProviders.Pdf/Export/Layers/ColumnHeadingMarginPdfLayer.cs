using System;
using Telerik.Windows.Documents.Fixed.Model.Editing;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class ColumnHeadingMarginPdfLayer : PdfLayerBase
	{
		public ColumnHeadingMarginPdfLayer()
		{
			this.columnHeadingRenderer = new PdfColumnHeadingRenderer();
			this.columnHeadingMarginLayer = new ColumnHeadingMarginLayer(this.columnHeadingRenderer);
		}

		public override string Name
		{
			get
			{
				return "ColumnHeadingMargin";
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
			this.columnHeadingRenderer.Editor = editor;
			this.columnHeadingMarginLayer.UpdateRender(updateContext);
		}

		readonly PdfColumnHeadingRenderer columnHeadingRenderer;

		readonly ColumnHeadingMarginLayer columnHeadingMarginLayer;
	}
}
