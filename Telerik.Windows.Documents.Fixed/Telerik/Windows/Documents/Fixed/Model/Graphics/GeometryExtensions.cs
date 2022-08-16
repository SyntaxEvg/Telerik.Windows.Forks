using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	static class GeometryExtensions
	{
		public static PathGeometry TransformRectangle(this Matrix matrix, Rect rectangle)
		{
			Point startPoint = matrix.Transform(new Point(rectangle.Left, rectangle.Top));
			Point point = matrix.Transform(new Point(rectangle.Right, rectangle.Top));
			Point point2 = matrix.Transform(new Point(rectangle.Right, rectangle.Bottom));
			Point point3 = matrix.Transform(new Point(rectangle.Left, rectangle.Bottom));
			PathGeometry pathGeometry = new PathGeometry();
			PathFigure pathFigure = pathGeometry.Figures.AddPathFigure();
			pathFigure.IsClosed = true;
			pathFigure.StartPoint = startPoint;
			pathFigure.Segments.AddLineSegment().Point = point;
			pathFigure.Segments.AddLineSegment().Point = point2;
			pathFigure.Segments.AddLineSegment().Point = point3;
			return pathGeometry;
		}
	}
}
