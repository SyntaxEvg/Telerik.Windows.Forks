using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Core.Data;

namespace Telerik.Windows.Documents.Core.Shapes
{
	class PathGeometry
	{
		internal static PathGeometry CreateRectangle(Rect rect)
		{
			PathGeometry pathGeometry = new PathGeometry();
			PathFigure pathFigure = new PathFigure();
			pathFigure.StartPoint = new Point(rect.Left, rect.Top);
			pathFigure.IsClosed = true;
			pathFigure.IsFilled = true;
			pathFigure.Segments.Add(new LineSegment
			{
				Point = new Point(rect.Right, rect.Top)
			});
			pathFigure.Segments.Add(new LineSegment
			{
				Point = new Point(rect.Right, rect.Bottom)
			});
			pathFigure.Segments.Add(new LineSegment
			{
				Point = new Point(rect.Left, rect.Bottom)
			});
			pathGeometry.Figures.Add(pathFigure);
			return pathGeometry;
		}

		static void Compare(Point point, ref double minX, ref double maxX, ref double minY, ref double maxY)
		{
			minX = System.Math.Min(point.X, minX);
			maxX = Math.Max(point.X, maxX);
			minY = System.Math.Min(point.Y, minY);
			maxY = Math.Max(point.Y, maxY);
		}

		public PathGeometry()
		{
			this.Figures = new List<PathFigure>();
			this.TransformMatrix = Matrix.Identity;
		}

		public List<PathFigure> Figures { get; set; }

		public FillRule FillRule { get; set; }

		public PathGeometry Clone()
		{
			PathGeometry pathGeometry = new PathGeometry();
			pathGeometry.FillRule = this.FillRule;
			pathGeometry.TransformMatrix = this.TransformMatrix;
			foreach (PathFigure pathFigure in this.Figures)
			{
				pathGeometry.Figures.Add(pathFigure.Clone());
			}
			return pathGeometry;
		}

		public Matrix TransformMatrix { get; set; }

		public bool IsEmpty
		{
			get
			{
				return this.Figures == null || this.Figures.Count == 0;
			}
		}

		public Rect GetBoundingRect()
		{
			double positiveInfinity = double.PositiveInfinity;
			double positiveInfinity2 = double.PositiveInfinity;
			double negativeInfinity = double.NegativeInfinity;
			double negativeInfinity2 = double.NegativeInfinity;
			foreach (PathFigure pathFigure in this.Figures)
			{
				PathGeometry.Compare(pathFigure.StartPoint, ref positiveInfinity, ref negativeInfinity, ref positiveInfinity2, ref negativeInfinity2);
				foreach (PathSegment pathSegment in pathFigure.Segments)
				{
					if (pathSegment is LineSegment)
					{
						PathGeometry.Compare(((LineSegment)pathSegment).Point, ref positiveInfinity, ref negativeInfinity, ref positiveInfinity2, ref negativeInfinity2);
					}
					else if (pathSegment is BezierSegment)
					{
						BezierSegment bezierSegment = (BezierSegment)pathSegment;
						PathGeometry.Compare(bezierSegment.Point1, ref positiveInfinity, ref negativeInfinity, ref positiveInfinity2, ref negativeInfinity2);
						PathGeometry.Compare(bezierSegment.Point2, ref positiveInfinity, ref negativeInfinity, ref positiveInfinity2, ref negativeInfinity2);
						PathGeometry.Compare(bezierSegment.Point3, ref positiveInfinity, ref negativeInfinity, ref positiveInfinity2, ref negativeInfinity2);
					}
				}
			}
			return new Rect(new Point(positiveInfinity, positiveInfinity2), new Point(negativeInfinity, negativeInfinity2));
		}
	}
}
