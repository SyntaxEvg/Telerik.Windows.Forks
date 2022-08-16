using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Utilities
{
	static class AlgebraExtensions
	{
		public static bool IsZero(this double a, double epsilon = 1E-08)
		{
			return Math.Abs(a) < epsilon;
		}

		public static bool IsEqualTo(this double a, double b, double epsilon = 1E-08)
		{
			return Math.Abs(a - b) < epsilon;
		}

		public static bool IsGreaterThanOrEqualTo(this double a, double b, double epsilon = 1E-08)
		{
			return a > b || a.IsEqualTo(b, epsilon);
		}

		public static bool IsLessThanOrEqualTo(this double a, double b, double epsilon = 1E-08)
		{
			return a < b || a.IsEqualTo(b, epsilon);
		}

		public static Point Plus(this Point first, Point second)
		{
			return new Point(first.X + second.X, first.Y + second.Y);
		}

		public static Point Minus(this Point first, Point second)
		{
			return new Point(first.X - second.X, first.Y - second.Y);
		}

		public static Point MultiplyBy(this Point a, double number)
		{
			return new Point(a.X * number, a.Y * number);
		}

		public static double MultiplyBy(this Point a, Point other)
		{
			return a.X * other.X + a.Y * other.Y;
		}

		public static double Length(this Point a)
		{
			return Math.Sqrt(a.MultiplyBy(a));
		}

		public static double CalculateDeterminant(this Matrix m)
		{
			return m.M11 * m.M22 - m.M12 * m.M21;
		}

		public static bool HasInverseMatrix(this Matrix m)
		{
			double a = m.CalculateDeterminant();
			return !a.IsZero(1E-08);
		}

		public static bool IsZero(this Point a, double epsilon = 1E-08)
		{
			return a.Length() < epsilon;
		}

		public static Point UnitVector(this Point a)
		{
			if (a.IsZero(1E-08))
			{
				throw new ArgumentException("Cannot calculate unit vector of a point with zero length!");
			}
			return a.MultiplyBy(1.0 / a.Length());
		}

		public static Matrix MultiplyBy(this Matrix m1, Matrix m2)
		{
			return new Matrix(m1.M11 * m2.M11 + m1.M12 * m2.M21, m1.M11 * m2.M12 + m1.M12 * m2.M22, m1.M21 * m2.M11 + m1.M22 * m2.M21, m1.M21 * m2.M12 + m1.M22 * m2.M22, m1.OffsetX * m2.M11 + m1.OffsetY * m2.M21 + m2.OffsetX, m1.OffsetX * m2.M12 + m1.OffsetY * m2.M22 + m2.OffsetY);
		}

		public static Matrix TranslateMatrix(this Matrix m, double deltaX, double deltaY)
		{
			return m.MultiplyBy(new Matrix(1.0, 0.0, 0.0, 1.0, deltaX, deltaY));
		}

		public static Rect Transform(this Matrix matrix, Rect rect)
		{
			return new Rect(matrix.Transform(new Point(rect.Left, rect.Top)), matrix.Transform(new Point(rect.Right, rect.Bottom)));
		}

		public static Matrix ScaleMatrix(this Matrix m, double scaleX, double scaleY)
		{
			return m.ScaleMatrixAt(scaleX, scaleY, 0.0, 0.0);
		}

		public static Matrix ScaleMatrixAt(this Matrix m, double scaleX, double scaleY, double centerX, double centerY)
		{
			return m.MultiplyBy(new Matrix(scaleX, 0.0, 0.0, scaleY, 0.0, 0.0).GetTransformationAt(centerX, centerY));
		}

		public static Matrix RotateMatrix(this Matrix m, double angleInDegrees)
		{
			return m.RotateMatrixAt(angleInDegrees, 0.0, 0.0);
		}

		public static Matrix RotateMatrixAt(this Matrix m, double angleInDegrees, double centerX, double centerY)
		{
			double num = angleInDegrees * 3.141592653589793 / 180.0;
			double num2 = Math.Sin(num);
			double num3 = Math.Cos(num);
			return m.MultiplyBy(new Matrix(num3, num2, -num2, num3, 0.0, 0.0).GetTransformationAt(centerX, centerY));
		}

		public static Matrix InverseMatrix(this Matrix m)
		{
			double num = m.CalculateDeterminant();
			double num2 = 1.0 / num;
			return new Matrix(m.M22 * num2, -m.M12 * num2, -m.M21 * num2, m.M11 * num2, (m.M21 * m.OffsetY - m.OffsetX * m.M22) * num2, (m.OffsetX * m.M12 - m.M11 * m.OffsetY) * num2);
		}

		public static Point Center(this Rect rect)
		{
			return new Point((rect.Right + rect.Left) / 2.0, (rect.Bottom + rect.Top) / 2.0);
		}

		static Matrix GetTransformationAt(this Matrix zeroCenteredTransform, double centerX, double centerY)
		{
			double offsetX = zeroCenteredTransform.OffsetX + (1.0 - zeroCenteredTransform.M11) * centerX - zeroCenteredTransform.M21 * centerY;
			double offsetY = zeroCenteredTransform.OffsetY + (1.0 - zeroCenteredTransform.M22) * centerY - zeroCenteredTransform.M12 * centerX;
			return new Matrix(zeroCenteredTransform.M11, zeroCenteredTransform.M12, zeroCenteredTransform.M21, zeroCenteredTransform.M22, offsetX, offsetY);
		}

		public const double Epsilon = 1E-08;
	}
}
