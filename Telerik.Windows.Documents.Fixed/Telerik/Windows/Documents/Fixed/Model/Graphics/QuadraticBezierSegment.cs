using System;
using System.Windows;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Graphics
{
	public class QuadraticBezierSegment : PathSegment
	{
		public Point Point1 { get; set; }

		public Point Point2 { get; set; }

		internal override Point LastPoint
		{
			get
			{
				return this.Point2;
			}
		}

		internal BezierSegment ToBezierSegment(Point previousPoint)
		{
			Point point = previousPoint.Plus(this.Point1.MultiplyBy(2.0)).MultiplyBy(0.3333333333333333);
			Point point2 = this.LastPoint.Plus(this.Point1.MultiplyBy(2.0)).MultiplyBy(0.3333333333333333);
			return new BezierSegment
			{
				Point1 = point,
				Point2 = point2,
				Point3 = this.LastPoint
			};
		}

		internal override Rect GetBounds(Point lastPoint)
		{
			Point point = lastPoint.Plus(this.Point1.MultiplyBy(-2.0)).Plus(this.Point2);
			Point point2 = lastPoint.MultiplyBy(-2.0).Plus(this.Point1.MultiplyBy(2.0));
			Point point3 = lastPoint;
			Interval interval = MathUtilities.FindLocalMinimumAndMaximum(QuadraticBezierSegment.ParametricInterval, point.X, point2.X, point3.X);
			Interval interval2 = MathUtilities.FindLocalMinimumAndMaximum(QuadraticBezierSegment.ParametricInterval, point.Y, point2.Y, point3.Y);
			return new Rect(new Point(interval.Start, interval2.Start), new Point(interval.End, interval2.End));
		}

		static readonly Interval ParametricInterval = new Interval(0.0, 1.0);
	}
}
