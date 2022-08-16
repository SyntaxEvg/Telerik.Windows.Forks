using System;
using System.Collections;
using System.Windows;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class DistanceExstensions
	{
		public static Point DistanceToRectanglePoint(Point pt, Rect rectangle)
		{
			return new Point(DistanceExstensions.DistToRectSelect(pt.X, rectangle.Left, rectangle.Right), DistanceExstensions.DistToRectSelect(pt.Y, rectangle.Top, rectangle.Bottom));
		}

		public static double DistToRectSelect(double pointX, double rectX1, double rectX2)
		{
			double num;
			double upper;
			DistanceExstensions.Closer(pointX, rectX1, rectX2, out num, out upper);
			if (!Utils.BetweenOrEqual(pointX, num, upper))
			{
				return num;
			}
			return pointX;
		}

		public static double DistanceToRectangle(Point pt, Rect rc)
		{
			Point b = DistanceExstensions.DistanceToRectanglePoint(pt, rc);
			return pt.Distance(b);
		}

		public static double DistanceToBezierCurve(Point point, IList bezierPoints)
		{
			double num = 1000000.0;
			int num2 = bezierPoints.Count / 3;
			for (int i = 0; i < num2; i++)
			{
				double x = ((Point)bezierPoints[i * 3]).X;
				double y = ((Point)bezierPoints[i * 3]).Y;
				double x2 = ((Point)bezierPoints[i * 3 + 1]).X;
				double y2 = ((Point)bezierPoints[i * 3 + 1]).Y;
				double x3 = ((Point)bezierPoints[i * 3 + 2]).X;
				double y3 = ((Point)bezierPoints[i * 3 + 2]).Y;
				double x4 = ((Point)bezierPoints[i * 3 + 3]).X;
				double y4 = ((Point)bezierPoints[i * 3 + 3]).Y;
				double num3 = (x4 - x + 3.0 * (x2 - x3)) / 8.0;
				double num4 = (y4 - y + 3.0 * (y2 - y3)) / 8.0;
				double num5 = (x4 + x - x2 - x3) * 3.0 / 8.0;
				double num6 = (y4 + y - y2 - y3) * 3.0 / 8.0;
				double num7 = (x4 - x) / 2.0 - num3;
				double num8 = (y4 - y) / 2.0 - num4;
				double num9 = (x4 + x) / 2.0 - num5;
				double num10 = (y4 + y) / 2.0 - num6;
				double x5 = point.X;
				double y5 = point.Y;
				double num11 = 0.0;
				double num12 = 0.0;
				double num13 = 0.0;
				double num14 = -1.0;
				double num15 = 0.0;
				for (double num16 = -1.0; num16 < 1.0; num16 += 0.2222222222222222)
				{
					Utils.GetBezierCoefficients(ref num9, ref num7, ref num5, ref num3, ref num10, ref num8, ref num6, ref num4, ref num16, ref num11, ref num12, ref x5, ref y5);
					if (Math.Abs(num11) < 1E-08)
					{
						num14 = num16;
						num15 = num12;
						num13 = num11;
						break;
					}
					if (Math.Abs(num16 + 1.0) < 1E-08)
					{
						num14 = num16;
						num15 = num12;
						num13 = num11;
					}
					if (num11 < num13)
					{
						num14 = num16;
						num15 = num12;
						num13 = num11;
					}
				}
				if (Math.Abs(num13) > 1E-08)
				{
					double num16 = num14 + 0.2222222222222222;
					if (num16 > 1.0)
					{
						num16 = 0.7777777777777778;
					}
					for (int j = 0; j < 20; j++)
					{
						Utils.GetBezierCoefficients(ref num9, ref num7, ref num5, ref num3, ref num10, ref num8, ref num6, ref num4, ref num16, ref num11, ref num12, ref x5, ref y5);
						if (Math.Abs(num11) < 1E-08 || Math.Abs(num12) < 1E-08)
						{
							break;
						}
						double num17 = num16;
						double num18 = num12;
						double num19 = num18 - num15;
						if (Math.Abs(num19) > 1E-08)
						{
							num16 = (num18 * num14 - num15 * num17) / num19;
						}
						else
						{
							num16 = (num14 + num17) / 2.0;
						}
						if (num16 > 1.0)
						{
							num16 = 1.0;
						}
						else if (num16 < -1.0)
						{
							num16 = -1.0;
						}
						if (Math.Abs(num16 - num17) < 1E-08)
						{
							break;
						}
						num14 = num17;
						num15 = num18;
					}
				}
				if (num > Math.Sqrt(num11))
				{
					num = Math.Sqrt(num11);
				}
				if (num.IsEqualTo(0.0))
				{
					return 0.0;
				}
			}
			return num;
		}

		public static double DistanceToPolyline(Point point, IList polyline)
		{
			int num = 0;
			return DistanceExstensions.DistanceToPolyline(point, polyline, ref num);
		}

		public static double DistanceToPolyline(Point point, IList polyline, ref int closestSegmentToPoint)
		{
			double num = double.MaxValue;
			closestSegmentToPoint = 0;
			for (int i = 0; i < polyline.Count - 1; i++)
			{
				Point a = (Point)polyline[i];
				Point b = (Point)polyline[i + 1];
				double num2 = DistanceExstensions.DistanceToLine(point, a, b);
				if (num2 < num)
				{
					num = num2;
					closestSegmentToPoint = i;
				}
			}
			return num;
		}

		public static double DistanceToLineSegment(Point point, IList polyline, double delta)
		{
			double result = double.NaN;
			for (int i = 0; i < polyline.Count - 1; i++)
			{
				Point point2 = (Point)polyline[i];
				Point point3 = (Point)polyline[i + 1];
				double num = DistanceExstensions.DistanceToLine(point, point2, point3);
				bool flag = (point2.X.IsEqualTo(point3.X) ? point.IsYBetween(point2, point3) : point.IsXBetween(point2, point3));
				if ((num < delta && flag) || point2.ContainsInNeighborhood(point, delta))
				{
					result = num;
					break;
				}
			}
			return result;
		}

		public static double DistanceToLine(Point p, Point a, Point b)
		{
			return p.Distance(PointExtensions.ProjectPointOnLine(p, a, b));
		}

		public static double DistanceToLineSquared(Point p, Point a, Point b)
		{
			if (a == b)
			{
				return DistanceExstensions.DistanceSquared(p, a);
			}
			double num = b.X - a.X;
			double num2 = b.Y - a.Y;
			double num3 = (p.Y - a.Y) * num - (p.X - a.X) * num2;
			return num3 * num3 / (num * num + num2 * num2);
		}

		public static double DistanceToSegmentSquared(Point p, Point a, Point b)
		{
			if (a == b)
			{
				return DistanceExstensions.DistanceSquared(p, a);
			}
			double num = b.X - a.X;
			double num2 = b.Y - a.Y;
			double num3 = (p.X - a.X) * num + (p.Y - a.Y) * num2;
			if (num3 < 0.0)
			{
				return DistanceExstensions.DistanceSquared(a, p);
			}
			num3 = (b.X - p.X) * num + (b.Y - p.Y) * num2;
			if (num3 < 0.0)
			{
				return DistanceExstensions.DistanceSquared(b, p);
			}
			return DistanceExstensions.DistanceToLineSquared(p, a, b);
		}

		public static double Distance(this Point p)
		{
			return p.Distance(new Point(0.0, 0.0));
		}

		public static double Distance(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}

		public static double Distance(this Point a, Point b)
		{
			return Math.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
		}

		public static double DistanceSquared(Point a, Point b)
		{
			return (a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y);
		}

		public static double Closer(double value, double choice1, double choice2)
		{
			double result;
			double num;
			DistanceExstensions.Closer(value, choice1, choice2, out result, out num);
			return result;
		}

		public static void Closer(double value, double choice1, double choice2, out double nearestValue, out double otherValue)
		{
			Utils.Sort(ref choice1, ref choice2);
			if (value >= choice1 && (value > choice2 || value - choice1 >= choice2 - value))
			{
				nearestValue = choice2;
				otherValue = choice1;
				return;
			}
			nearestValue = choice1;
			otherValue = choice2;
		}

		public static Point Closer(Point point, Point p1, Point p2)
		{
			if (!PointExtensions.AreDistanceOrdered(point, p1, p2))
			{
				return p2;
			}
			return p1;
		}
	}
}
