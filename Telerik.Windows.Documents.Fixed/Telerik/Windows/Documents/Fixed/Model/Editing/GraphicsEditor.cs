using System;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Graphics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Editing
{
	class GraphicsEditor : FixedContentElementEditorBase
	{
		public GraphicsEditor(FixedContentEditorBase editor)
			: base(editor)
		{
		}

		public void DrawLine(Point point1, Point point2)
		{
			PathGeometry pathGeometry = new PathGeometry();
			PathFigure pathFigure = pathGeometry.Figures.AddPathFigure();
			pathFigure.StartPoint = point1;
			pathFigure.Segments.AddLineSegment().Point = point2;
			this.DrawPathInternal(pathGeometry);
		}

		public void DrawRectangle(Rect rectangle)
		{
			this.DrawPathInternal(new RectangleGeometry
			{
				Rect = rectangle
			});
		}

		public void DrawEllipse(Point center, double radiusX, double radiusY)
		{
			double x = center.X - radiusX;
			double x2 = center.X - radiusX * GraphicsEditor.controlPointRatio;
			double x3 = center.X;
			double x4 = center.X + radiusX * GraphicsEditor.controlPointRatio;
			double x5 = center.X + radiusX;
			double y = center.Y - radiusY;
			double y2 = center.Y - radiusY * GraphicsEditor.controlPointRatio;
			double y3 = center.Y;
			double y4 = center.Y + radiusY * GraphicsEditor.controlPointRatio;
			double y5 = center.Y + radiusY;
			PathGeometry pathGeometry = new PathGeometry();
			PathFigure pathFigure = pathGeometry.Figures.AddPathFigure();
			pathFigure.StartPoint = new Point(x3, y);
			pathFigure.IsClosed = true;
			pathFigure.Segments.AddBezierSegment(new Point(x4, y), new Point(x5, y2), new Point(x5, y3));
			pathFigure.Segments.AddBezierSegment(new Point(x5, y4), new Point(x4, y5), new Point(x3, y5));
			pathFigure.Segments.AddBezierSegment(new Point(x2, y5), new Point(x, y4), new Point(x, y3));
			pathFigure.Segments.AddBezierSegment(new Point(x, y2), new Point(x2, y), new Point(x3, y));
			this.DrawPathInternal(pathGeometry);
		}

		public void DrawPath(GeometryBase geometry)
		{
			Guard.ThrowExceptionIfNull<GeometryBase>(geometry, "geometry");
			this.DrawPathInternal(geometry);
		}

		void DrawPathInternal(GeometryBase geometry)
		{
			Guard.ThrowExceptionIfNull<GeometryBase>(geometry, "geometry");
			Path path = new Path();
			path.Geometry = geometry;
			path.Fill = base.Editor.GraphicProperties.FillColor;
			path.Stroke = base.Editor.GraphicProperties.StrokeColor;
			path.StrokeThickness = base.Editor.GraphicProperties.StrokeThickness;
			path.MiterLimit = base.Editor.GraphicProperties.MiterLimit;
			path.StrokeDashArray = base.Editor.GraphicProperties.StrokeDashArray;
			path.StrokeDashOffset = base.Editor.GraphicProperties.StrokeDashOffset;
			path.StrokeLineCap = base.Editor.GraphicProperties.StrokeLineCap;
			path.StrokeLineJoin = base.Editor.GraphicProperties.StrokeLineJoin;
			path.IsFilled = base.Editor.GraphicProperties.IsFilled;
			path.IsStroked = base.Editor.GraphicProperties.IsStroked;
			base.Editor.Append(path);
		}

		static readonly double controlPointRatio = (Math.Sqrt(2.0) - 1.0) * 4.0 / 3.0;
	}
}
