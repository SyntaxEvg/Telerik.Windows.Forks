// Decompiled with JetBrains decompiler
// Type: Telerik.Windows.Documents.Utilities.AlgebraExtensions
// Assembly: Telerik.Windows.Documents.Core, Version=2019.2.503.40, Culture=neutral, PublicKeyToken=5803cfa389c90ce7
// MVID: F77F8994-0B02-4C6D-8302-951D4E56A706
// Assembly location: C:\Users\user\Downloads\FromFileToFileCore\FromFileToFileCore\Export_Word_Excel_PDF_CSV_HTML-master\ExportDemo1\ConsoleApp1\bin\x64\Debug\Telerik.Windows.Documents.Core.dll

using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Utilities
{
    internal static class AlgebraExtensions
    {
        public const double Epsilon = 1E-08;

        public static bool IsZero(this double a, double epsilon = 1E-08) => Math.Abs(a) < epsilon;

        public static bool IsEqualTo(this double a, double b, double epsilon = 1E-08) => Math.Abs(a - b) < epsilon;

        public static bool IsGreaterThanOrEqualTo(this double a, double b, double epsilon = 1E-08) => a > b || a.IsEqualTo(b, epsilon);

        public static bool IsLessThanOrEqualTo(this double a, double b, double epsilon = 1E-08) => a < b || a.IsEqualTo(b, epsilon);

        public static Point Plus(this Point first, Point second) => new Point(first.X + second.X, first.Y + second.Y);

        public static Point Minus(this Point first, Point second) => new Point(first.X - second.X, first.Y - second.Y);

        public static Point MultiplyBy(this Point a, double number) => new Point(a.X * number, a.Y * number);

        public static double MultiplyBy(this Point a, Point other) => a.X * other.X + a.Y * other.Y;

        public static double Length(this Point a) => Math.Sqrt(a.MultiplyBy(a));

        public static double CalculateDeterminant(this Matrix m) => m.M11 * m.M22 - m.M12 * m.M21;

        public static bool HasInverseMatrix(this Matrix m) => !m.CalculateDeterminant().IsZero();

        public static bool IsZero(this Point a, double epsilon = 1E-08) => a.Length() < epsilon;

        public static Point UnitVector(this Point a) => !a.IsZero() ? a.MultiplyBy(1.0 / a.Length()) : throw new ArgumentException("Cannot calculate unit vector of a point with zero length!");

        public static Matrix MultiplyBy(this Matrix m1, Matrix m2) => new Matrix(m1.M11 * m2.M11 + m1.M12 * m2.M21, m1.M11 * m2.M12 + m1.M12 * m2.M22, m1.M21 * m2.M11 + m1.M22 * m2.M21, m1.M21 * m2.M12 + m1.M22 * m2.M22, m1.OffsetX * m2.M11 + m1.OffsetY * m2.M21 + m2.OffsetX, m1.OffsetX * m2.M12 + m1.OffsetY * m2.M22 + m2.OffsetY);

        public static Matrix TranslateMatrix(this Matrix m, double deltaX, double deltaY) => m.MultiplyBy(new Matrix(1.0, 0.0, 0.0, 1.0, deltaX, deltaY));

        public static Rect Transform(this Matrix matrix, Rect rect) => new Rect(matrix.Transform(new Point(rect.Left, rect.Top)), matrix.Transform(new Point(rect.Right, rect.Bottom)));

        public static Matrix ScaleMatrix(this Matrix m, double scaleX, double scaleY) => m.ScaleMatrixAt(scaleX, scaleY, 0.0, 0.0);

        public static Matrix ScaleMatrixAt(
          this Matrix m,
          double scaleX,
          double scaleY,
          double centerX,
          double centerY)
        {
            return m.MultiplyBy(new Matrix(scaleX, 0.0, 0.0, scaleY, 0.0, 0.0).GetTransformationAt(centerX, centerY));
        }

        public static Matrix RotateMatrix(this Matrix m, double angleInDegrees) => m.RotateMatrixAt(angleInDegrees, 0.0, 0.0);

        public static Matrix RotateMatrixAt(
          this Matrix m,
          double angleInDegrees,
          double centerX,
          double centerY)
        {
            double num1 = angleInDegrees * Math.PI / 180.0;
            double m12 = Math.Sin(num1);
            double num2 = Math.Cos(num1);
            return m.MultiplyBy(new Matrix(num2, m12, -m12, num2, 0.0, 0.0).GetTransformationAt(centerX, centerY));
        }

        public static Matrix InverseMatrix(this Matrix m)
        {
            double num = 1.0 / m.CalculateDeterminant();
            return new Matrix(m.M22 * num, -m.M12 * num, -m.M21 * num, m.M11 * num, (m.M21 * m.OffsetY - m.OffsetX * m.M22) * num, (m.OffsetX * m.M12 - m.M11 * m.OffsetY) * num);
        }

        public static Point Center(this Rect rect) => new Point((rect.Right + rect.Left) / 2.0, (rect.Bottom + rect.Top) / 2.0);

        private static Matrix GetTransformationAt(
          this Matrix zeroCenteredTransform,
          double centerX,
          double centerY)
        {
            double offsetX = zeroCenteredTransform.OffsetX + (1.0 - zeroCenteredTransform.M11) * centerX - zeroCenteredTransform.M21 * centerY;
            double offsetY = zeroCenteredTransform.OffsetY + (1.0 - zeroCenteredTransform.M22) * centerY - zeroCenteredTransform.M12 * centerX;
            return new Matrix(zeroCenteredTransform.M11, zeroCenteredTransform.M12, zeroCenteredTransform.M21, zeroCenteredTransform.M22, offsetX, offsetY);
        }
    }
}
