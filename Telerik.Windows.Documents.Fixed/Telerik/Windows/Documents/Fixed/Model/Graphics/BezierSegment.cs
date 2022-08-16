using System;
using System.Windows;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public class BezierSegment : PathSegment
	{
		public Point Point1 { get; set; }

		public Point Point2 { get; set; }

		public Point Point3 { get; set; }

		internal override Point LastPoint
		{
			get
			{
				return this.Point3;
			}
		}

		internal override Rect GetBounds(Point lastPoint)
		{
			Point point = lastPoint.MultiplyBy(-1.0).Plus(this.Point1.MultiplyBy(3.0)).Plus(this.Point2.MultiplyBy(-3.0))
				.Plus(this.Point3);
			Point point2 = lastPoint.MultiplyBy(3.0).Plus(this.Point1.MultiplyBy(-6.0)).Plus(this.Point2.MultiplyBy(3.0));
			Point point3 = lastPoint.MultiplyBy(-3.0).Plus(this.Point1.MultiplyBy(3.0));
			Point point4 = lastPoint;
			Interval interval = MathUtilities.FindLocalMinimumAndMaximum(BezierSegment.ParametricInterval, point.X, point2.X, point3.X, point4.X);
			Interval interval2 = MathUtilities.FindLocalMinimumAndMaximum(BezierSegment.ParametricInterval, point.Y, point2.Y, point3.Y, point4.Y);
			return new Rect(new Point(interval.Start, interval2.Start), new Point(interval.End, interval2.End));
		}

		static readonly Interval ParametricInterval = new Interval(0.0, 1.0);
	}
}
