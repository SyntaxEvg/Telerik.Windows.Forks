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

		internal global::System.Collections.Generic.IEnumerable<global::Telerik.Windows.Documents.Fixed.Model.Graphics.BezierSegment> ToBezierSegments(global::System.Windows.Point previousPoint)
		{
			if (!previousPoint.Minus(this.LastPoint).IsZero(1E-08))
			{
				global::System.Windows.Media.SweepDirection sweepDirection = ((this.SweepDirection == global::Telerik.Windows.Documents.Fixed.Model.Graphics.SweepDirection.Clockwise) ? global::System.Windows.Media.SweepDirection.Clockwise : global::System.Windows.Media.SweepDirection.Counterclockwise);
				global::System.Collections.Generic.IEnumerable<global::System.Tuple<global::System.Windows.Point, global::System.Windows.Point, global::System.Windows.Point, global::System.Windows.Point>> bezierSegments = global::Telerik.Windows.Documents.Fixed.Utilities.ArcHelper.GetEllipticArcCubicBezierApproximation(previousPoint, this.LastPoint, new global::System.Windows.Size(this.RadiusX, this.RadiusY), this.IsLargeArc, sweepDirection, this.RotationAngle);
				foreach (global::System.Tuple<global::System.Windows.Point, global::System.Windows.Point, global::System.Windows.Point, global::System.Windows.Point> bezierSegment in bezierSegments)
				{
					yield return new global::Telerik.Windows.Documents.Fixed.Model.Graphics.BezierSegment
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
