using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfLayersBuilder
	{
		public virtual void BuildLayersOverride(PdfLayerStack layerStack)
		{
			layerStack.AddLast(new HeaderFooterPdfLayer());
			layerStack.AddLast(new RowHeadingMarginPdfLayer());
			layerStack.AddLast(new ColumnHeadingMarginPdfLayer());
			layerStack.AddLast(new GridlinesPdfLayer());
			layerStack.AddLast(new CellFillPdfLayer());
			layerStack.AddLast(new CellValuesPdfLayer());
			layerStack.AddLast(new CellBordersPdfLayer());
			layerStack.AddLast(new ShapesPdfLayer());
			layerStack.AddLast(new GridlinesOutlinePdfLayer());
		}
	}
}
