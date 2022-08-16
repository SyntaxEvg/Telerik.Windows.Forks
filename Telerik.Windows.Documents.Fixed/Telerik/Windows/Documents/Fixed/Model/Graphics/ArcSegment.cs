using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Fixed.Utilities;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public class ArcSegment : PathSegment
	{
		public ArcSegment()
		{
			this.IsLargeArc = false;
			this.SweepDirection = SweepDirection.Clockwise;
		}

		public Point Point { get; set; }

		public double RadiusX { get; set; }

		public double RadiusY { get; set; }

		public bool IsLargeArc { get; set; }

		public SweepDirection SweepDirection { get; set; }

		public double RotationAngle { get; set; }

		internal override Point LastPoint
		{
			get
			{
				return this.Point;
			}
		}

		internal IEnumerable<BezierSegment> ToBezierSegments(Point previousPoint)
		{
			if (!previousPoint.Minus(this.LastPoint).IsZero(1E-08))
			{
				SweepDirection sweepDirection = ((this.SweepDirection == SweepDirection.Clockwise) ? System.Windows.Media.SweepDirection.Clockwise : System.Windows.Media.SweepDirection.Counterclockwise);
				IEnumerable<Tuple<Point, Point, Point, Point>> bezierSegments = ArcHelper.GetEllipticArcCubicBezierApproximation(previousPoint, this.LastPoint, new Size(this.RadiusX, this.RadiusY), this.IsLargeArc, sweepDirection, this.RotationAngle);
				foreach (Tuple<Point, Point, Point, Point> bezierSegment in bezierSegments)
				{
					yield return new BezierSegment
					{
						Point1 = bezierSegment.Item2,
						Point2 = bezierSegment.Item3,
						Point3 = bezierSegment.Item4
					};
				}
			}
			yield break;
		}

		internal override Rect GetBounds(Point lastPoint)
		{
			Rect rect = new Rect(lastPoint, this.Point);
			double x = rect.X;
			double x2 = rect.X + rect.Width;
			double y = rect.Y;
			double y2 = rect.Y + rect.Height;
			Point lastPoint2 = lastPoint;
			foreach (BezierSegment bezierSegment in this.ToBezierSegments(lastPoint))
			{
				PathGeometry.AppendBounds(bezierSegment.GetBounds(lastPoint2), ref x, ref x2, ref y, ref y2);
				lastPoint2 = bezierSegment.LastPoint;
			}
			return new Rect(new Point(x, y), new Point(x2, y2));
		}
	}
}
