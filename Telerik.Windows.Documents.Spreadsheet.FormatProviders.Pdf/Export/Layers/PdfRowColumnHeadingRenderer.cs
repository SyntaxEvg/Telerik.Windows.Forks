using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Data;
using Telerik.Windows.Documents.Fixed.Model.Editing.Flow;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	abstract class PdfRowColumnHeadingRenderer<T> : PdfRenderer<RowColumnHeadingRenderable<T>> where T : LayoutBox
	{
		public sealed override void RenderOverride(RowColumnHeadingRenderable<T> renderable, ViewportPaneType paneType)
		{
			Point point = this.CalculateTopLeft(renderable);
			Matrix matrix = default(Matrix).ScaleMatrix(renderable.ScaleFactor.Width, renderable.ScaleFactor.Height).TranslateMatrix(point.X, point.Y).MultiplyBy(base.Editor.Position.Matrix);
			base.Editor.Position = new MatrixPosition(matrix);
			Point point2 = new Point(0.0, 0.0);
			Point point3 = new Point(renderable.Rectangle.Width, 0.0);
			Point point4 = new Point(0.0, renderable.Rectangle.Height);
			Point point5 = new Point(renderable.Rectangle.Width, renderable.Rectangle.Height);
			base.Editor.DrawLine(point2, point3);
			base.Editor.DrawLine(point2, point4);
			base.Editor.DrawLine(point3, point5);
			base.Editor.DrawLine(point4, point5);
			base.Editor.PushTransformedClipping(new Rect(0.0, 0.0, renderable.Rectangle.Width, renderable.Rectangle.Height));
			base.Editor.DrawTextBlock(renderable.Text, renderable.Rectangle.Width, renderable.Rectangle.Height, Telerik.Windows.Documents.Fixed.Model.Editing.Flow.HorizontalAlignment.Center, Telerik.Windows.Documents.Fixed.Model.Editing.Flow.VerticalAlignment.Center);
			base.Editor.PopClipping();
		}

		protected abstract Point CalculateTopLeft(RowColumnHeadingRenderable<T> renderable);
	}
}
