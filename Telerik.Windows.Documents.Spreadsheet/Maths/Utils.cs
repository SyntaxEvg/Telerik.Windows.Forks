using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class Utils
	{
		public static bool BetweenOrEqual(double value, double lower, double upper)
		{
			Utils.Sort(ref lower, ref upper);
			return Utils.BetweenOrEqualSorted(value, lower, upper);
		}

		public static IEnumerable<Point> PolylineToBezier(Collection<Point> points)
		{
			Collection<Point> collection = new Collection<Point>();
			collection.Add(points[0]);
			for (int i = 0; i < points.Count - 2; i++)
			{
				Point item = points[i + 1];
				collection.Add(item);
				collection.Add(item);
				if (i != points.Count - 3)
				{
					Point point = points[i + 2];
					Point item2 = new Point((item.X + point.X) / 2.0, (item.Y + point.Y) / 2.0);
					collection.Add(item2);
				}
				else
				{
					collection.Add(points[i + 2]);
				}
			}
			if (collection.Count == 1)
			{
				collection = new Collection<Point>();
				Point item3 = points[0];
				Point item = points[points.Count - 1];
				Point item2 = new Point((item3.X + item.X) / 2.0, (item3.Y + item.Y) / 2.0);
				collection.Add(item3);
				collection.Add(item2);
				collection.Add(item2);
				collection.Add(item);
			}
			return collection;
		}

		public static void Sort(ref double a, ref double b)
		{
			if (b >= a)
			{
				return;
			}
			double num = a;
			a = b;
			b = num;
		}

		public static void CartesianToPolar(Point rootPoint, Point otherPoint, ref double angle, ref double rho)
		{
			if (rootPoint == otherPoint)
			{
				angle = 0.0;
				rho = 0.0;
				return;
			}
			double num = otherPoint.X - rootPoint.X;
			double num2 = otherPoint.Y - rootPoint.Y;
			rho = DistanceExstensions.Distance(num, num2);
			angle = Math.Atan(-num2 / num) * 180.0 / 3.141592653589793;
			if (num < 0.0)
			{
				angle += 180.0;
			}
		}

		public static Point PolarToCartesian(Point coordCenter, double angle, double rho)
		{
			double num = angle.ToRadians();
			return new Point(coordCenter.X + Math.Cos(num) * rho, coordCenter.Y - Math.Sin(num) * rho);
		}

		public static DoubleCollection Clone(this IEnumerable<double> doubles)
		{
			DoubleCollection doubleCollection = new DoubleCollection();
			doubles.ToList<double>().ForEach(new Action<double>(doubleCollection.Add));
			return doubleCollection;
		}

		public static double Constrain(this double value, double min, double max)
		{
			return Math.Max(min, Math.Min(value, max));
		}

		public static bool AreIntersectingIntervals(int firstStart, int firstEnd, int secondStart, int secondEnd)
		{
			bool flag = secondStart <= firstStart && firstStart <= secondEnd;
			flag |= secondStart <= firstEnd && firstEnd <= secondEnd;
			flag |= firstStart <= secondStart && secondStart <= firstEnd;
			return flag | (firstStart <= secondEnd && secondEnd <= firstEnd);
		}

		public static Tuple<int, int> GetIntersection(int firstStart, int firstEnd, int secondStart, int secondEnd)
		{
			if (!Utils.AreIntersectingIntervals(firstStart, firstEnd, secondStart, secondEnd))
			{
				return null;
			}
			int item = Math.Max(firstStart, secondStart);
			int item2 = System.Math.Min(firstEnd, secondEnd);
			return new Tuple<int, int>(item, item2);
		}

		public static void GetProjections(Point point, Rect rectangle, Point[] projections)
		{
			int num = 0;
			int num2 = 1;
			int num3 = 2;
			projections[3] = point;
			projections[num3] = point;
			projections[num2] = point;
			projections[num] = point;
			projections[0].Y = rectangle.Top;
			projections[1].Y = rectangle.Bottom;
			projections[2].X = rectangle.Right;
			projections[3].X = rectangle.Left;
		}

		public static Point GetBezierPoint(Collection<Point> points, int segment, double value)
		{
			if (segment >= 715827882 || segment * 3 > points.Count - 3)
			{
				throw new ArgumentOutOfRangeException("segment");
			}
			double x = points[segment * 3].X;
			double y = points[segment * 3].Y;
			double x2 = points[segment * 3 + 1].X;
			double y2 = points[segment * 3 + 1].Y;
			double x3 = points[segment * 3 + 2].X;
			double y3 = points[segment * 3 + 2].Y;
			double x4 = points[segment * 3 + 3].X;
			double y4 = points[segment * 3 + 3].Y;
			double num = (1.0 - value) * (1.0 - value) * (1.0 - value);
			double num2 = 3.0 * value * (1.0 - value) * (1.0 - value);
			double num3 = 3.0 * value * value * (1.0 - value);
			double num4 = value * value * value;
			double x5 = num * x + num2 * x2 + num3 * x3 + num4 * x4;
			double y5 = num * y + num2 * y2 + num3 * y3 + num4 * y4;
			return new Point(x5, y5);
		}

		public static double GetPythagorEqualSide(double hypotenuse)
		{
			return Math.Sqrt(Math.Pow(hypotenuse, 2.0) / 2.0);
		}

		public static double GetPythagorHypotenuse(double sideA, double sideB)
		{
			return Math.Sqrt(Math.Pow(sideA, 2.0) + Math.Pow(sideB, 2.0));
		}

		public static bool HasValidArea(this Size size)
		{
			return size.Width > 0.0 && size.Height > 0.0 && !double.IsInfinity(size.Width) && !double.IsInfinity(size.Height);
		}

		public static double Hypotenuse(double x, double y)
		{
			return Math.Sqrt(x * x + y * y);
		}

		public static bool IsPointInRectangle(Point pt, Rect rc)
		{
			return rc.Contains(pt);
		}

		public static bool IsInRightOpenInterval(this double v, double min, double max)
		{
			return min <= v && v < max;
		}

		public static bool IsInClosedInterval(this double v, double min, double max)
		{
			return min <= v && v <= max;
		}

		public static Matrix Invert(this Matrix m)
		{
			return m.InverseMatrix();
		}

		public static Point Limit(this Point p, Rect rectangle)
		{
			return new Point(Math.Min(Math.Max(p.X, rectangle.Left), rectangle.Right), Math.Min(Math.Max(p.Y, rectangle.Top), rectangle.Bottom));
		}

		public static double Lerp(double x, double y, double fraction)
		{
			return x * (1.0 - fraction) + y * fraction;
		}

		public static Matrix Multiply(this Matrix m1, Matrix m2)
		{
			return m1.MultiplyBy(m2);
		}

		public static double NormalizeAngle(double angle)
		{
			while (angle > 6.283185307179586)
			{
				angle -= 6.283185307179586;
			}
			while (angle < 0.0)
			{
				angle += 6.283185307179586;
			}
			return angle;
		}

		public static void OffsetPointCollection(Collection<Point> points, Collection<Point> originalPoints, Vector offset)
		{
			if (points.Count != originalPoints.Count)
			{
				return;
			}
			for (int i = 0; i < points.Count; i++)
			{
				points[i] = originalPoints[i] + offset;
			}
		}

		public static Point Push(Point start, Vector unitVector, double distance)
		{
			return new Point(start.X + unitVector.X * distance, start.Y + unitVector.Y * distance);
		}

		public static double SafeDivide(this double value1, double value2, double fallback)
		{
			if (!value2.IsVerySmall())
			{
				return value1 / value2;
			}
			return fallback;
		}

		public static Rect Shrink(Rect outerBounds, double left = 0.0, double top = 0.0, double right = 0.0, double bottom = 0.0)
		{
			return new Rect(outerBounds.Left + left, outerBounds.Top + top, outerBounds.Width - left - right, outerBounds.Height - top - bottom);
		}

		public static int StairValue(double value, double lower, double upper)
		{
			if (value < lower)
			{
				return -1;
			}
			if (value >= upper)
			{
				return 1;
			}
			return 0;
		}

		public static Vector StairValue(Point p, Rect rectangle)
		{
			return new Vector((double)Utils.StairValue(p.X, rectangle.Left, rectangle.Right), (double)Utils.StairValue(p.Y, rectangle.Top, rectangle.Bottom));
		}

		public static void Swap(ref double a, ref double b)
		{
			double num = a;
			a = b;
			b = num;
		}

		public static double Snap(this double value, int snappingValue)
		{
			return value - value % (double)snappingValue;
		}

		public static Rect Transform(this GeneralTransform tr, Rect r)
		{
			return new Rect(tr.Transform(r.TopLeft()), tr.Transform(r.BottomRight()));
		}

		public static Rect Transform(this Matrix m, Rect r)
		{
			return new Rect(m.Transform(r.TopLeft()), m.Transform(r.BottomRight()));
		}

		public static Rect TransformPercentToSize(Rect rect, Size size)
		{
			return new Rect(PointExtensions.PointFromBarycentricPercentage(rect.TopLeft(), size), PointExtensions.PointFromBarycentricPercentage(rect.BottomRight(), size));
		}

		public static Matrix ToMatrix(this Transform transform)
		{
			MatrixTransform matrixTransform = transform as MatrixTransform;
			if (matrixTransform != null)
			{
				return matrixTransform.Matrix;
			}
			TransformGroup transformGroup = transform as TransformGroup;
			if (transformGroup != null)
			{
				return transformGroup.Value;
			}
			return new TransformGroup
			{
				Children = { transform }
			}.Value;
		}

		public static string ToInvariantString(this Size size)
		{
			string arg = (double.IsNaN(size.Width) ? "Auto" : size.Width.ToString(CultureInfo.InvariantCulture));
			string arg2 = (double.IsNaN(size.Height) ? "Auto" : size.Height.ToString(CultureInfo.InvariantCulture));
			return string.Format("{0};{1}", arg, arg2);
		}

		public static string ToInvariantString(this double value)
		{
			if (!double.IsNaN(value))
			{
				return value.ToString(CultureInfo.InvariantCulture);
			}
			return "Auto";
		}

		public static string ToInvariantString(this int value)
		{
			return value.ToString(CultureInfo.InvariantCulture);
		}

		public static Size? ToSize(string s)
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
			double naN;
			if (array[0].ToLower() == "auto")
			{
				naN = double.NaN;
			}
			else if (!double.TryParse(array[0], NumberStyles.Float, CultureInfo.InvariantCulture, out naN))
			{
				return null;
			}
			double naN2;
			if (array[1].ToLower() == "auto")
			{
				naN2 = double.NaN;
			}
			else if (!double.TryParse(array[1], NumberStyles.Float, CultureInfo.InvariantCulture, out naN2))
			{
				return null;
			}
			return new Size?(new Size(naN, naN2));
		}

		public static T ToEnum<T>(object value)
		{
			if (value == null)
			{
				return default(T);
			}
			return (T)((object)Enum.Parse(typeof(T), value.ToString(), true));
		}

		internal static Matrix ToMatix(this GeneralTransform tr)
		{
			MatrixTransform matrixTransform = tr as MatrixTransform;
			if (matrixTransform == null)
			{
				return Matrix.Identity;
			}
			return matrixTransform.Matrix;
		}

		internal static Point RectanglePointFromPercent(Point pointPercent, Rect rc)
		{
			return new Point(rc.Left + pointPercent.X / 100.0 * rc.Width, rc.Top + pointPercent.Y / 100.0 * rc.Height);
		}

		internal static double ReplaceZero(double suspect, double defaultValue)
		{
			if (!suspect.IsEqualTo(0.0))
			{
				return suspect;
			}
			return defaultValue;
		}

		internal static List<object> Clone(this IEnumerable<object> list)
		{
			List<object> list2 = new List<object>();
			list2.AddRange(list);
			return list2;
		}

		internal static Point PercentagePoint(Point pt, Rect rc)
		{
			Point result = new Point(50.0, 50.0);
			double num = rc.Right - rc.Left;
			double num2 = rc.Bottom - rc.Top;
			if (num.IsNotEqualTo(0.0) && num2.IsNotEqualTo(0.0))
			{
				result.X = (pt.X - rc.Left) * 100.0 / num;
				result.Y = (pt.Y - rc.Top) * 100.0 / num2;
			}
			return result;
		}

		internal static Size Scale(this Size size, double scale)
		{
			return new Size(size.Width * scale, size.Height * scale);
		}

		public static void GetBezierCoefficients(ref double a0, ref double a1, ref double a2, ref double a3, ref double b0, ref double b1, ref double b2, ref double b3, ref double u, ref double s, ref double z, ref double x4, ref double y4)
		{
			double num = a0 + u * (a1 + u * (a2 + u * a3));
			double num2 = num - x4;
			double num3 = b1 + u * (2.0 * b2 + 3.0 * u * b3);
			double num4 = b0 + u * (b1 + u * (b2 + u * b3));
			double num5 = num4 - y4;
			double num6 = a1 + u * (2.0 * a2 + 3.0 * u * a3);
			s = num2 * num2 + num5 * num5;
			z = num6 * num2 + num3 * num5;
		}

		public static bool BetweenOrEqualSorted(double n, double boundary1, double boundary2)
		{
			return boundary1 <= n && n <= boundary2;
		}

		static char GetPointSeparator(IFormatProvider provider)
		{
			char c = ',';
			NumberFormatInfo instance = NumberFormatInfo.GetInstance(provider);
			if (instance.NumberDecimalSeparator.Length > 0 && c == instance.NumberDecimalSeparator[0])
			{
				c = ';';
			}
			return c;
		}

		public static bool ArraysEqual<T>(T[] a1, T[] a2)
		{
			if (object.ReferenceEquals(a1, a2))
			{
				return true;
			}
			if (a1 == null || a2 == null)
			{
				return false;
			}
			if (a1.Length != a2.Length)
			{
				return false;
			}
			EqualityComparer<T> comparer = EqualityComparer<T>.Default;
			return !a1.Where((T t, int i) => !comparer.Equals(t, a2[i])).Any<T>();
		}

		public static bool ArraysEqual<T>(T[,] a1, T[,] a2)
		{
			if (object.ReferenceEquals(a1, a2))
			{
				return true;
			}
			if (a1 == null || a2 == null)
			{
				return false;
			}
			if (a1.Length != a2.Length)
			{
				return false;
			}
			for (int i = 0; i < 2; i++)
			{
				if (a1.GetLength(i) != a2.GetLength(i))
				{
					return false;
				}
			}
			EqualityComparer<T> @default = EqualityComparer<T>.Default;
			for (int j = 0; j < a1.GetLength(0); j++)
			{
				for (int k = 0; k < a1.GetLength(1); k++)
				{
					if (!@default.Equals(a1[j, k], a2[j, k]))
					{
						return false;
					}
				}
			}
			return true;
		}
	}
}
