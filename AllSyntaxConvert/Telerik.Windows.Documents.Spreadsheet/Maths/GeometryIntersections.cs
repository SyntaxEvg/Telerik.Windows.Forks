using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Windows;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class GeometryIntersections
	{
		public static bool AreIntersecting(Rect rectangle, Rect rect)
		{
			rectangle.Intersect(rect);
			return !rectangle.IsEmpty;
		}

		public static bool AreIntersecting(Rect rectangle, Point center, double radius)
		{
			double num = rectangle.Left - center.X;
			double num2 = rectangle.Right - center.X;
			double num3 = rectangle.Top - center.Y;
			double num4 = rectangle.Bottom - center.Y;
			if (num2 < 0.0)
			{
				if (num3 > 0.0)
				{
					return num2 * num2 + num3 * num3 < radius * radius;
				}
				if (num4 < 0.0)
				{
					return num2 * num2 + num4 * num4 < radius * radius;
				}
				return Math.Abs(num2) < radius;
			}
			else if (num > 0.0)
			{
				if (num3 > 0.0)
				{
					return num * num + num3 * num3 < radius * radius;
				}
				if (num4 < 0.0)
				{
					return num * num + num4 * num4 < radius * radius;
				}
				return Math.Abs(num) < radius;
			}
			else
			{
				if (num3 > 0.0)
				{
					return Math.Abs(num3) < radius;
				}
				return num4 >= 0.0 || Math.Abs(num4) < radius;
			}
		}

		public static bool IsPointInEllipse(Point point, Rect rectangle)
		{
			Rect rect = rectangle;
			double num = (rect.Right - rect.Left) / 2.0;
			double num2 = (rect.Bottom - rect.Top) / 2.0;
			double num3 = point.X - (rect.Left + rect.Right) / 2.0;
			double num4 = point.Y - (rect.Top + rect.Bottom) / 2.0;
			return num3 * num3 / (num * num) + num4 * num4 / (num2 * num2) <= 1.0;
		}

		public static bool IsPointInRectangle(Point point, Size size)
		{
			return Utils.BetweenOrEqualSorted(point.X, 0.0, size.Width) && Utils.BetweenOrEqualSorted(point.Y, 0.0, size.Height);
		}

		public static bool IntersectsCircle(this Rect rectangle, Point center, double radius)
		{
			return GeometryIntersections.AreIntersecting(rectangle, center, radius);
		}

		public static bool AreLinesIntersecting(Point a, Point b, Point c, Point d, ref Point intersectionPoint)
		{
			double num = (b.X - a.X) * (d.Y - c.Y) - (b.Y - a.Y) * (d.X - c.X);
			if (num.IsEqualTo(0.0))
			{
				return false;
			}
			double num2 = (a.Y - c.Y) * (d.X - c.X) - (a.X - c.X) * (d.Y - c.Y);
			double num3 = (a.Y - c.Y) * (b.X - a.X) - (a.X - c.X) * (b.Y - a.Y);
			double num4 = num2 / num;
			double num5 = num3 / num;
			if (num4 < 0.0 || num4 > 1.0 || num5 < 0.0 || num5 > 1.0)
			{
				return false;
			}
			intersectionPoint = new Point(a.X + num4 * (b.X - a.X), a.Y + num4 * (b.Y - a.Y));
			return true;
		}

		public static bool IntersectsLineSegment(this Rect rect, Point a, Point b, ref Point intersectionPoint)
		{
			return GeometryIntersections.AreLinesIntersecting(a, b, rect.TopLeft(), rect.TopRight(), ref intersectionPoint) || GeometryIntersections.AreLinesIntersecting(a, b, rect.TopRight(), rect.BottomRight(), ref intersectionPoint) || GeometryIntersections.AreLinesIntersecting(a, b, rect.BottomRight(), rect.BottomLeft(), ref intersectionPoint) || GeometryIntersections.AreLinesIntersecting(a, b, rect.BottomLeft(), rect.TopLeft(), ref intersectionPoint);
		}

		public static bool IntersectsLine(this Rect rect, IList polyline)
		{
			bool flag = false;
			for (int i = 0; i < polyline.Count - 1; i++)
			{
				Point a = (Point)polyline[i];
				Point b = (Point)polyline[i + 1];
				Point point = default(Point);
				flag = rect.IntersectsLineSegment(a, b, ref point);
				if (flag)
				{
					break;
				}
			}
			return flag;
		}

		public static bool IntersectsWith(this Rect r1, Rect r2)
		{
			r1.Intersect(r2);
			return !r1.IsEmpty;
		}

		public static bool SegmentIntersect(Point s1, Point s2, Point l1, Point l2, ref Point p)
		{
			p = GeometryIntersections.FindLinesIntersection(s1, s2, l1, l2, false);
			double num = (p.X - s1.X) * (p.X - s2.X);
			double num2 = (p.Y - s1.Y) * (p.Y - s2.Y);
			if (num > 1E-08 || num2 > 1E-08)
			{
				return false;
			}
			double num3 = (p.X - l1.X) * (p.X - l2.X);
			double num4 = (p.Y - l1.Y) * (p.Y - l2.Y);
			return num3 <= 1E-08 && num4 <= 1E-08;
		}

		public static Point FindLinesIntersection(Point a, Point b, Point c, Point d, bool acceptNaN = false)
		{
			Point result = (acceptNaN ? new Point(double.NaN, double.NaN) : new Point(double.MinValue, double.MinValue));
			if (a.X.IsEqualTo(b.X) && c.X.IsEqualTo(d.X))
			{
				return result;
			}
			if (a.X.IsEqualTo(b.X))
			{
				result.X = a.X;
				result.Y = (c.Y - d.Y) / (c.X - d.X) * result.X + (c.X * d.Y - d.X * c.Y) / (c.X - d.X);
				return result;
			}
			if (c.X.IsEqualTo(d.X))
			{
				result.X = c.X;
				result.Y = (a.Y - b.Y) / (a.X - b.X) * result.X + (a.X * b.Y - b.X * a.Y) / (a.X - b.X);
				return result;
			}
			double num = (a.Y - b.Y) / (a.X - b.X);
			double num2 = (a.X * b.Y - b.X * a.Y) / (a.X - b.X);
			double num3 = (c.Y - d.Y) / (c.X - d.X);
			double num4 = (c.X * d.Y - d.X * c.Y) / (c.X - d.X);
			if (num.IsNotEqualTo(num3) || acceptNaN)
			{
				result.X = (num4 - num2) / (num - num3);
				result.Y = num * (num4 - num2) / (num - num3) + num2;
			}
			return result;
		}

		public static Point IntersectionPoint(Rect rectangle, Point pt1, Point pt2)
		{
			Rect rect = new Rect(pt1, pt2);
			Point result = default(Point);
			double x = pt1.X;
			double y = pt1.Y;
			double x2 = pt2.X;
			double y2 = pt2.Y;
			if (Math.Abs(x - x2) > 1E-08)
			{
				double num = (rectangle.Left + rectangle.Right) / 2.0;
				double num2 = (rectangle.Top + rectangle.Bottom) / 2.0;
				double num3 = (rectangle.Right - rectangle.Left) / 2.0;
				double num4 = (rectangle.Bottom - rectangle.Top) / 2.0;
				double num5 = (y - y2) / (x - x2);
				double num6 = (x * y2 - x2 * y) / (x - x2);
				double num7 = num4 * num4 + num5 * num5 * num3 * num3;
				double num8 = 2.0 * num5 * (num6 - num2) * num3 * num3 - 2.0 * num * num4 * num4;
				double num9 = num4 * num4 * num * num + num3 * num3 * (num6 - num2) * (num6 - num2) - num3 * num3 * num4 * num4;
				double num10 = Math.Sqrt(num8 * num8 - 4.0 * num7 * num9);
				double num11 = (-num8 + num10) / (2.0 * num7);
				double num12 = (-num8 - num10) / (2.0 * num7);
				double y3 = num5 * num11 + num6;
				double y4 = num5 * num12 + num6;
				result.X = num11;
				result.Y = y3;
				if (result.X >= rect.Left && result.X <= rect.Right && result.Y >= rect.Top && result.Y <= rect.Bottom)
				{
					return result;
				}
				result.X = num12;
				result.Y = y4;
				if (result.X >= rect.Left && result.X <= rect.Right && result.Y >= rect.Top && result.Y <= rect.Bottom)
				{
					return result;
				}
			}
			else
			{
				double num13 = (rectangle.Left + rectangle.Right) / 2.0;
				double num14 = (rectangle.Top + rectangle.Bottom) / 2.0;
				double num15 = (rectangle.Right - rectangle.Left) / 2.0;
				double num16 = (rectangle.Bottom - rectangle.Top) / 2.0;
				double num17 = x;
				double y5 = num14 - Math.Sqrt(1.0 - (num17 - num13) * (num17 - num13) / (num15 * num15) * num16 * num16);
				double y6 = num14 + Math.Sqrt(1.0 - (num17 - num13) * (num17 - num13) / (num15 * num15) * num16 * num16);
				result.X = num17;
				result.Y = y5;
				if (result.X >= rect.Left && result.X <= rect.Right && result.Y >= rect.Top && result.Y <= rect.Bottom)
				{
					return result;
				}
				result.X = num17;
				result.Y = y6;
				if (result.X >= rect.Left && result.X <= rect.Right && result.Y >= rect.Top && result.Y <= rect.Bottom)
				{
					return result;
				}
			}
			return result;
		}

		public static bool IntersectionPointOnEllipse(Collection<Point> points, Point org, Point end, ref Point result)
		{
			Point point = default(Point);
			double num = double.PositiveInfinity;
			for (int i = 0; i < points.Count; i++)
			{
				if (GeometryIntersections.SegmentIntersect(points[i], points[(i + 1) % points.Count], org, end, ref point))
				{
					double num2 = DistanceExstensions.DistanceSquared(point, end);
					if (num2 < num)
					{
						num = num2;
						result = point;
					}
				}
			}
			return !double.IsPositiveInfinity(num);
		}

		public static void IntersectionPointOnRectangle(Rect rectangle, Point pt1, Point pt2, ref Point intersectionPoint)
		{
			Rect rect = RectExtensions.FromLtrd(pt1.X, pt1.Y, pt2.X, pt2.Y);
			double x = pt1.X;
			double y = pt1.Y;
			double x2 = pt2.X;
			double y2 = pt2.Y;
			if (x == x2)
			{
				intersectionPoint.X = x;
				intersectionPoint.Y = rectangle.Top;
				if (intersectionPoint.X >= rectangle.Left && intersectionPoint.X <= rectangle.Right && intersectionPoint.Y >= rect.Top && intersectionPoint.Y <= rect.Bottom)
				{
					return;
				}
				intersectionPoint.Y = rectangle.Bottom;
				if (intersectionPoint.X >= rectangle.Left && intersectionPoint.X <= rectangle.Right && intersectionPoint.Y >= rect.Top && intersectionPoint.Y <= rect.Bottom)
				{
					return;
				}
			}
			else if (y == y2)
			{
				intersectionPoint.Y = y;
				intersectionPoint.X = rectangle.Left;
				if (intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y <= rectangle.Bottom && intersectionPoint.X >= rect.Left && intersectionPoint.X <= rect.Right)
				{
					return;
				}
				intersectionPoint.X = rectangle.Right;
				if (intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y <= rectangle.Bottom && intersectionPoint.X >= rect.Left && intersectionPoint.X <= rect.Right)
				{
					return;
				}
			}
			else
			{
				double num = (y - y2) / (x - x2);
				double num2 = (x * y2 - x2 * y) / (x - x2);
				intersectionPoint.Y = rectangle.Top;
				intersectionPoint.X = (intersectionPoint.Y - num2) / num;
				if (intersectionPoint.X >= rectangle.Left && intersectionPoint.X <= rectangle.Right && intersectionPoint.Y <= rectangle.Bottom && intersectionPoint.Y >= rect.Top && intersectionPoint.Y <= rect.Bottom)
				{
					return;
				}
				intersectionPoint.Y = rectangle.Bottom;
				intersectionPoint.X = (intersectionPoint.Y - num2) / num;
				if (intersectionPoint.X >= rectangle.Left && intersectionPoint.X <= rectangle.Right && intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y >= rect.Top && intersectionPoint.Y <= rect.Bottom)
				{
					return;
				}
				intersectionPoint.X = rectangle.Left;
				intersectionPoint.Y = num * intersectionPoint.X + num2;
				if (intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y <= rectangle.Bottom && intersectionPoint.X <= rectangle.Right && intersectionPoint.X >= rect.Left && intersectionPoint.X <= rect.Right)
				{
					return;
				}
				intersectionPoint.X = rectangle.Right;
				intersectionPoint.Y = num * intersectionPoint.X + num2;
				if (intersectionPoint.Y >= rectangle.Top && intersectionPoint.Y <= rectangle.Bottom && intersectionPoint.X >= rectangle.Left && intersectionPoint.X >= rect.Left)
				{
					double x3 = intersectionPoint.X;
					double right = rect.Right;
				}
			}
		}
	}
}
