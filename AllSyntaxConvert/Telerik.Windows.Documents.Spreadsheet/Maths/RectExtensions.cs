using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class RectExtensions
	{
		public static RotateTransform RotateTransform
		{
			get
			{
				if (!RectExtensions.transform.Dispatcher.CheckAccess())
				{
					RectExtensions.transform = new RotateTransform();
				}
				return RectExtensions.transform;
			}
			set
			{
				RectExtensions.transform = value;
			}
		}

		public static bool IsInBoundsOf(this Rect rect, Rect hostingRect)
		{
			return hostingRect.Contains(new Point(rect.Left, rect.Top)) && hostingRect.Contains(new Point(rect.Right, rect.Bottom));
		}

		public static bool IsBiggerThan(this Rect rect, Rect targetRect)
		{
			return rect.Width * rect.Height > targetRect.Width * targetRect.Height;
		}

		public static Rect FromLtrd(double l, double t, double r, double b)
		{
			return new Rect(new Point(l, t), new Point(r, b));
		}

		public static Rect Rotate(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			RectExtensions.RotateTransform.Angle = angle;
			RectExtensions.RotateTransform.CenterX = rect.Left + offsetVector.X + rect.Width / 2.0;
			RectExtensions.RotateTransform.CenterY = rect.Top + offsetVector.Y + rect.Height / 2.0;
			return RectExtensions.RotateTransform.TransformBounds(rect);
		}

		public static Point TopLeft(this Rect rect)
		{
			return new Point(rect.Left, rect.Top);
		}

		public static Point TopRight(this Rect rect)
		{
			return new Point(rect.Right, rect.Top);
		}

		public static Point BottomRight(this Rect rect)
		{
			return new Point(rect.Right, rect.Bottom);
		}

		public static Point BottomLeft(this Rect rect)
		{
			return new Point(rect.Left, rect.Bottom);
		}

		public static Point CenterLeft(this Rect rect)
		{
			return new Point(rect.Left, rect.Top + rect.Height / 2.0);
		}

		public static Point CenterTop(this Rect rect)
		{
			return new Point(rect.Left + rect.Width / 2.0, rect.Top);
		}

		public static Point CenterRight(this Rect rect)
		{
			return new Point(rect.Right, rect.Top + rect.Height / 2.0);
		}

		public static Point CenterBottom(this Rect rect)
		{
			return new Point(rect.Left + rect.Width / 2.0, rect.Bottom);
		}

		public static Point Center(this Rect rectangle)
		{
			return PointExtensions.MiddlePoint(rectangle.TopLeft(), rectangle.BottomRight());
		}

		public static double CenterX(this Rect rect)
		{
			return rect.Left + rect.Width / 2.0;
		}

		public static double CenterY(this Rect rect)
		{
			return rect.Top + rect.Height / 2.0;
		}

		public static Point TopLeft(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = new Point(rect.Left, rect.Top);
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static Point TopRight(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = new Point(rect.Right, rect.Top);
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static Point BottomRight(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = new Point(rect.Right, rect.Bottom);
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static Point BottomLeft(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = new Point(rect.Left, rect.Bottom);
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static Point Center(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = PointExtensions.MiddlePoint(rect.TopLeft(), rect.BottomRight());
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static Point CenterLeft(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = new Point(rect.Left, rect.Top + rect.Height / 2.0);
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static Point CenterTop(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = new Point(rect.Left + rect.Width / 2.0, rect.Top);
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static Point CenterRight(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = new Point(rect.Right, rect.Top + rect.Height / 2.0);
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static Point CenterBottom(this Rect rect, double angle, Point offsetVector = default(Point))
		{
			Point pointToRotate = new Point(rect.Left + rect.Width / 2.0, rect.Bottom);
			return RectExtensions.GenerateTransform(pointToRotate, rect, angle, offsetVector);
		}

		public static bool Contains(this Rect rect, Point point, double angle)
		{
			Point point2 = point;
			if (Math.Abs(angle % 180.0 - 0.0) > 1E-08)
			{
				point2 = point.Rotate(rect.Center(), -angle);
			}
			return rect.Contains(point2);
		}

		public static bool IntersectsWith(this Rect rect, Rect otherRectangle, double angle)
		{
			if (Math.Abs(angle % 180.0 - 0.0) <= 1E-08)
			{
				return rect.IntersectsWith(otherRectangle);
			}
			Point anchorPoint = otherRectangle.Center();
			bool flag = false;
			Point point = otherRectangle.TopLeft().Rotate(anchorPoint, angle);
			Point point2 = otherRectangle.TopRight().Rotate(anchorPoint, angle);
			Point point3 = otherRectangle.BottomLeft().Rotate(anchorPoint, angle);
			Point point4 = otherRectangle.BottomRight().Rotate(anchorPoint, angle);
			flag |= rect.Contains(point);
			flag |= rect.Contains(point2);
			flag |= rect.Contains(point3);
			flag |= rect.Contains(point4);
			if (!flag)
			{
				Point point5 = default(Point);
				flag = rect.IntersectsLineSegment(point, point2, ref point5);
				flag |= rect.IntersectsLineSegment(point, point3, ref point5);
				flag |= rect.IntersectsLineSegment(point2, point4, ref point5);
				flag |= rect.IntersectsLineSegment(point3, point4, ref point5);
			}
			return flag;
		}

		public static Size ToSize(this Rect rect)
		{
			return new Size(rect.Width, rect.Height);
		}

		static Point GenerateTransform(Point pointToRotate, Rect rect, double angle, Point offsetVector)
		{
			RectExtensions.RotateTransform.Angle = angle;
			RectExtensions.RotateTransform.CenterX = rect.Left + rect.Width / 2.0 + offsetVector.X;
			RectExtensions.RotateTransform.CenterY = rect.Top + rect.Height / 2.0 + offsetVector.Y;
			return RectExtensions.RotateTransform.Transform(pointToRotate);
		}

		public static Rect ToRect(this Size size)
		{
			return new Rect(0.0, 0.0, size.Width, size.Height);
		}

		public static Rect NewRect(Point center, double size)
		{
			return new Rect(center.X - size / 2.0, center.Y - size / 2.0, size, size);
		}

		public static Rect NewRect(Point center, Size size)
		{
			return new Rect(center.X - size.Width / 2.0, center.Y - size.Height / 2.0, size.Width, size.Height);
		}

		public static Rect Offset(this Rect rect, Vector offsetVector)
		{
			return RectExtensions.Offset(rect, offsetVector.X, offsetVector.Y);
		}

		public static Rect Offset(Rect rect, double x, double y)
		{
			rect.X += x;
			rect.Y += y;
			return rect;
		}

		public static Rect Inflate(this Rect rect, double deltaX, double deltaY)
		{
			if (rect.Width + 2.0 * deltaX < 0.0)
			{
				deltaX = -rect.Width / 2.0;
			}
			if (rect.Height + 2.0 * deltaY < 0.0)
			{
				deltaY = -rect.Height / 2.0;
			}
			return new Rect(rect.X - deltaX, rect.Y - deltaY, rect.Width + 2.0 * deltaX, rect.Height + 2.0 * deltaY);
		}

		public static Rect Inflate(this Rect rect, Size size)
		{
			return rect.Inflate(size.Width, size.Height, size.Width, size.Height);
		}

		internal static bool Contains(this Rect r1, Rect r2)
		{
			return r1.Contains(r2.BottomLeft()) && r1.Contains(r2.BottomRight()) && r1.Contains(r2.TopRight()) && r1.Contains(r2.TopLeft());
		}

		internal static void SetLocation(ref Rect bounds, Point point)
		{
			bounds.X = point.X;
			bounds.Y = point.Y;
		}

		public static bool Contains(this Rect rectangle, Point p)
		{
			return rectangle.Contains(p);
		}

		public static Point MiddlePoint(Rect rect)
		{
			return new Point(rect.X + rect.Width / 2.0, rect.Y + rect.Height / 2.0);
		}

		public static Rect Union(Rect a, Rect b)
		{
			Rect result = a;
			result.Union(b);
			return result;
		}

		public static Rect Inflate(this Rect rect, double left, double top, double right, double bottom)
		{
			double num = rect.Width + left + right;
			double num2 = rect.Height + top + bottom;
			double num3 = rect.X - left;
			if (num < 0.0)
			{
				num3 += num / 2.0;
				num = 0.0;
			}
			double num4 = rect.Y - top;
			if (num2 < 0.0)
			{
				num4 += num2 / 2.0;
				num2 = 0.0;
			}
			return new Rect(num3, num4, num, num2);
		}

		static RotateTransform transform = new RotateTransform();
	}
}
