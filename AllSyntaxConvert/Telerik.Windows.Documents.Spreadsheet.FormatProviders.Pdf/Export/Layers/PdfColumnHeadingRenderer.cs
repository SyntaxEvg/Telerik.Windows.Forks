using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfColumnHeadingRenderer : PdfRowColumnHeadingRenderer<ColumnLayoutBox>
	{
		protected override Point CalculateTopLeft(RowColumnHeadingRenderable<ColumnLayoutBox> renderable)
		{
			return new Point(renderable.Rectangle.X * renderable.ScaleFactor.Width, -renderable.Rectangle.Height * renderable.ScaleFactor.Height);
		}
	}
}
