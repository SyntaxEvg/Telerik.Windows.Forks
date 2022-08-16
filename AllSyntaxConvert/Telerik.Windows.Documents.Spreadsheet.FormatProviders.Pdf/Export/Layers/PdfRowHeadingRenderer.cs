using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfRowHeadingRenderer : PdfRowColumnHeadingRenderer<RowLayoutBox>
	{
		protected override Point CalculateTopLeft(RowColumnHeadingRenderable<RowLayoutBox> renderable)
		{
			return new Point(-renderable.Rectangle.Width * renderable.ScaleFactor.Width, renderable.Rectangle.Y * renderable.ScaleFactor.Height);
		}
	}
}
