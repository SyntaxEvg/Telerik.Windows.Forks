using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Fixed.Model.Graphics;

namespace Telerik.Windows.Documents.Fixed.Model.Collections
{
	public sealed class PathSegmentCollection : List<PathSegment>
	{
		public LineSegment AddLineSegment()
		{
			LineSegment lineSegment = new LineSegment();
			base.Add(lineSegment);
			return lineSegment;
		}

		public LineSegment AddLineSegment(Point point)
		{
			LineSegment lineSegment = this.AddLineSegment();
			lineSegment.Point = point;
			return lineSegment;
		}

		public QuadraticBezierSegment AddQuadraticBezierSegment()
		{
			QuadraticBezierSegment quadraticBezierSegment = new QuadraticBezierSegment();
			base.Add(quadraticBezierSegment);
			return quadraticBezierSegment;
		}

		public QuadraticBezierSegment AddQuadraticBezierSegment(Point point1, Point point2)
		{
			QuadraticBezierSegment quadraticBezierSegment = this.AddQuadraticBezierSegment();
			quadraticBezierSegment.Point1 = point1;
			quadraticBezierSegment.Point2 = point2;
			return quadraticBezierSegment;
		}

		public BezierSegment AddBezierSegment()
		{
			BezierSegment bezierSegment = new BezierSegment();
			base.Add(bezierSegment);
			return bezierSegment;
		}

		public BezierSegment AddBezierSegment(Point point1, Point point2, Point point3)
		{
			BezierSegment bezierSegment = this.AddBezierSegment();
			bezierSegment.Point1 = point1;
			bezierSegment.Point2 = point2;
			bezierSegment.Point3 = point3;
			return bezierSegment;
		}

		public ArcSegment AddArcSegment()
		{
			ArcSegment arcSegment = new ArcSegment();
			base.Add(arcSegment);
			return arcSegment;
		}

		public ArcSegment AddArcSegment(Point point, double radiusX, double radiusY)
		{
			ArcSegment arcSegment = this.AddArcSegment();
			arcSegment.Point = point;
			arcSegment.RadiusX = radiusX;
			arcSegment.RadiusY = radiusY;
			return arcSegment;
		}
	}
}
