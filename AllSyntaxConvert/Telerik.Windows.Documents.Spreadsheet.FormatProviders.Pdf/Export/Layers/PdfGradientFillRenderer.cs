using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Layout.Layers;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Pdf.Export.Layers
{
	class PdfGradientFillRenderer : PdfRenderer<GradientFillRenderable>
	{
		public override void RenderOverride(GradientFillRenderable renderable, ViewportPaneType paneType)
		{
			base.Editor.GraphicProperties.StrokeThickness = 0.0;
			base.Editor.GraphicProperties.IsStroked = false;
			Size containerSize = new Size(renderable.BoundingRectangle.Width, renderable.BoundingRectangle.Height);
			IEnumerable<LinearGradientBox> enumerable = GradientsRenderHelper.GenerateGradientBoxes(renderable.GradientFill, renderable.ColorScheme, containerSize);
			base.Editor.Position.Translate(renderable.BoundingRectangle.Left, renderable.BoundingRectangle.Top);
			foreach (LinearGradientBox linearGradientBox in enumerable)
			{
				base.Editor.Position.Translate(linearGradientBox.BoundingBox.Left, linearGradientBox.BoundingBox.Top);
				Matrix matrix = default(Matrix).ScaleMatrix(linearGradientBox.BoundingBox.Width, linearGradientBox.BoundingBox.Height).MultiplyBy(base.Editor.Position.Matrix);
				base.Editor.GraphicProperties.FillColor = linearGradientBox.Fill.ToPdfGradient(matrix);
				Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry geometry = this.CalculatePath(linearGradientBox);
				base.Editor.DrawPath(geometry);
				base.Editor.Position.Translate(-linearGradientBox.BoundingBox.Left, -linearGradientBox.BoundingBox.Top);
			}
		}

		Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry CalculatePath(LinearGradientBox gradientBox)
		{
			Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry pathGeometry = new Telerik.Windows.Documents.Fixed.Model.Graphics.PathGeometry();
			foreach (System.Windows.Media.PathFigure pathFigure in gradientBox.Clip.Figures)
			{
				Telerik.Windows.Documents.Fixed.Model.Graphics.PathFigure pathFigure2 = pathGeometry.Figures.AddPathFigure();
				pathFigure2.IsClosed = pathFigure.IsClosed;
				pathFigure2.StartPoint = pathFigure.StartPoint;
				foreach (System.Windows.Media.PathSegment pathSegment in pathFigure.Segments)
				{
					System.Windows.Media.LineSegment lineSegment = pathSegment as System.Windows.Media.LineSegment;
					if (lineSegment != null)
					{
						pathFigure2.Segments.AddLineSegment().Point = lineSegment.Point;
					}
				}
			}
			return pathGeometry;
		}
	}
}
