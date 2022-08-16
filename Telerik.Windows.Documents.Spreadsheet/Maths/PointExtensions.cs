using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class PointExtensions
	{
		public static Point Add(Point point, Vector vector)
		{
			return new Point(point.X + vector.X, point.Y + vector.Y);
		}

		public static Point Add(this Point point, Point p2)
		{
			return new Point(point.X + p2.X, point.Y + p2.Y);
		}

		internal static Point RotatePoint(Point pt, double rotation)
		{
			Point result = pt;
			if (rotation.IsEqualTo(0.0))
			{
				return result;
			}
			if (rotation.IsEqualTo(90.0))
			{
				result.X = 100.0 - pt.Y;
				result.Y = pt.X;
			}
			else if (rotation.IsEqualTo(180.0))
			{
				result.X = 100.0 - pt.X;
				result.Y = 100.0 - pt.Y;
			}
			else
			{
				result.X = pt.Y;
				result.Y = 100.0 - pt.X;
			}
			return result;
		}

		public static void Swap(ref Point p, ref Point q)
		{
			Point point = p;
			p = q;
			q = point;
		}

		public static IEnumerable<Point> Offset(this IEnumerable<Point> points, Vector offsetVector)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			points.ForEach(delegate(Point p)
			{
				p += offsetVector;
			});
			return points;
		}

		public static Point Snap(this Point point, int snapX, int snapY)
		{
			double num = point.X % (double)snapX;
			double num2 = point.Y % (double)snapY;
			double x = point.X - num + (double)((num > (double)snapX / 2.0) ? snapX : 0);
			double y = point.Y - num2 + (double)((num2 > (double)snapY / 2.0) ? snapY : 0);
			return new Point(x, y);
		}

		public static Point BarycentricPercentageFromPoint(Point realPoint, Rect rectangle)
		{
			Point result = new Point(50.0, 50.0);
			double num = rectangle.Right - rectangle.Left;
			double num2 = rectangle.Bottom - rectangle.Top;
			if (num.IsNotEqualTo(0.0) && num2.IsNotEqualTo(0.0))
			{
				result.X = (realPoint.X - rectangle.Left) * 100.0 / num;
				result.Y = (realPoint.Y - rectangle.Top) * 100.0 / num2;
			}
			return result;
		}

		public static Point PointFromBarycentricPercentage(Point percentage, Size size)
		{
			return new Point(percentage.X / 100.0 * size.Width, percentage.Y / 100.0 * size.Height);
		}

		public static Point PointFromBarycentricPercentage(Point percentage, Rect rectangle)
		{
			return new Point(rectangle.Left + percentage.X / 100.0 * rectangle.Width, rectangle.Top + percentage.Y / 100.0 * rectangle.Height);
		}

		public static bool IsXBetween(this Point point, Point firstPoint, Point secondPoint)
		{
			return (firstPoint.X <= point.X && point.X <= secondPoint.X) || (secondPoint.X <= point.X && point.X <= firstPoint.X);
		}

		public static bool IsYBetween(this Point point, Point firstPoint, Point secondPoint)
		{
			return (firstPoint.Y <= point.Y && point.Y <= secondPoint.Y) || (secondPoint.Y <= point.Y && point.Y <= firstPoint.Y);
		}

		public static Point Rotate(this Point point, Point anchorPoint, double angle)
		{
			RectExtensions.RotateTransform.Angle = angle;
			RectExtensions.RotateTransform.CenterX = anchorPoint.X;
			RectExtensions.RotateTransform.CenterY = anchorPoint.Y;
			return RectExtensions.RotateTransform.Transform(point);
		}

		public static bool ContainsInNeighborhood(this Point originPoint, Point point, double delta)
		{
			Rect rect = new Rect(originPoint.X - delta, originPoint.Y - delta, 2.0 * delta, 2.0 * delta);
			return rect.Contains(point);
		}

		public static Point? ToPoint(string s)
		{
			if (string.IsNullOrEmpty(s))
			{
				return null;
			}
			string[] array = s.Split(new char[] { ';' });
			if (array.Length < 2)
			{
				return null;
			}
			double x;
			double y;
			if (double.TryParse(array[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x) && double.TryParse(array[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y))
			{
				return new Point?(new Point(x, y));
			}
			return null;
		}

		public static Point RotatePoint(Point point, Point pivot, double angle)
		{
			return PointExtensions.RotateTransform(pivot, angle).Transform(point);
		}

		public static void RotatePointsAt(Point[] points, Point pivot, double angle)
		{
			RotateTransform rotateTransform = PointExtensions.RotateTransform(pivot, angle);
			int num = points.Length;
			for (int i = 0; i < num; i++)
			{
				points[i] = rotateTransform.Transform(points[i]);
			}
		}

		public static string ToInvariantString(this Point p)
		{
			return string.Format("{0};{1}", p.X.ToString(CultureInfo.InvariantCulture), p.Y.ToString(CultureInfo.InvariantCulture));
		}

		public static Point MirrorPoint(Point p, Point a, Point b)
		{
			Point point = GeometryIntersections.FindLinesIntersection(a, b, p, p + a.Subtract(b).MirrorHorizontally(), true);
			return point + point.Subtract(p);
		}

		public static bool AreDistanceOrdered(Point p, Point u, Point v)
		{
			return DistanceExstensions.DistanceSquared(p, u) < DistanceExstensions.DistanceSquared(p, v);
		}

		public static Point Minus(this Point p1, Point p2)
		{
			return new Point(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Point MirrorPoint(Point point, Point center)
		{
			Point point2 = new Point(center.X - point.X, center.Y - point.Y);
			return new Point(point2.X + center.X, point2.Y + center.Y);
		}

		public static Point MiddlePoint(Point p1, Point p2)
		{
			return new Point((p1.X + p2.X) / 2.0, (p1.Y + p2.Y) / 2.0);
		}

		public static Point GetTopLeftPoint(IEnumerable<Point> points)
		{
			return new Point(points.Min((Point p) => p.X), points.Min((Point p) => p.Y));
		}

		public static Point GetBottomRightPoint(IEnumerable<Point> points)
		{
			return new Point(points.Max((Point p) => p.X), points.Max((Point p) => p.Y));
		}

		public static Point Substract(this Point p1, Point p2)
		{
			return new Point(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static Vector Subtract(this Point p1, Point p2)
		{
			return new Vector(p1.X - p2.X, p1.Y - p2.Y);
		}

		public static bool SameSide(Point linePoint1, Point linePoint2, Point p1, Point p2)
		{
			return PointExtensions.IsCounterClockWise(linePoint1, linePoint2, p1) == PointExtensions.IsCounterClockWise(linePoint1, linePoint2, p2);
		}

		public static int IsCounterClockWise(Point p0, Point p1, Point p2)
		{
			if ((p1.X - p0.X) * (p2.Y - p0.Y) <= (p1.Y - p0.Y) * (p2.X - p0.X))
			{
				return -1;
			}
			return 1;
		}

		public static double Dot(Vector v1, Vector v2)
		{
			return v1.X * v2.X + v1.Y * v2.Y;
		}

		public static double Dot(Point p1, Point p2)
		{
			return p1.X * p2.X + p1.Y * p2.Y;
		}

		public static double Determinant(Point p1, Point p2)
		{
			return p1.X * p2.Y - p1.Y * p2.X;
		}

		public static Point DistancePoint(Point p, Point a, Point b)
		{
			if (a == b)
			{
				return a;
			}
			double num = b.X - a.X;
			double num2 = b.Y - a.Y;
			double num3 = (p.X - a.X) * num + (p.Y - a.Y) * num2;
			if (num3 < 0.0)
			{
				return a;
			}
			num3 = (b.X - p.X) * num + (b.Y - p.Y) * num2;
			if (num3 < 0.0)
			{
				return b;
			}
			Vector vector = new Vector(a.X - b.X, a.Y - b.Y).MirrorHorizontally();
			Point d = new Point(p.X + vector.X, p.Y + vector.Y);
			return GeometryIntersections.FindLinesIntersection(a, b, p, d, true);
		}

		public static Point ProjectPointOnLine(Point p, Point a, Point b)
		{
			Vector vector = VectorExtensions.Perpendicular(new Vector(a.X - b.X, a.Y - b.Y));
			Point d = new Point(p.X + vector.X, p.Y + vector.Y);
			Point p2 = GeometryIntersections.FindLinesIntersection(a, b, p, d, true);
			return DistanceExstensions.Closer(p, a, double.IsNaN(p2.X) ? b : DistanceExstensions.Closer(p, p2, b));
		}

		public static RotateTransform RotateTransform(Point center, double angle)
		{
			return new RotateTransform
			{
				Angle = angle,
				CenterX = center.X,
				CenterY = center.Y
			};
		}

		public static Point Lerp(Point p, Point q, double fraction)
		{
			return new Point(Utils.Lerp(p.X, q.X, fraction), Utils.Lerp(p.Y, q.Y, fraction));
		}

		public static Vector Normal(Point p1, Point p2)
		{
			return new Vector(p1.Y - p2.Y, p2.X - p1.X).Normalized();
		}

		public static Point InvertPoint(this Point point)
		{
			return new Point(-point.X, -point.Y);
		}

		public static Point NearestPoint(this Point point, IEnumerable<Point> points)
		{
			if (points == null)
			{
				throw new ArgumentNullException("points");
			}
			Point result = point;
			double num = double.MaxValue;
			foreach (Point point2 in points)
			{
				double num2 = point2.Distance(point);
				if (num2 < num)
				{
					num = num2;
					result = point2;
				}
			}
			return result;
		}
	}
}
