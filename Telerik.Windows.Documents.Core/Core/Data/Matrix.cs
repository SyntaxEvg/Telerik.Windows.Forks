using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Core.Data
{
	struct Matrix
	{
		public Matrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
		{
			this = default(Matrix);
			this.M11 = m11;
			this.M12 = m12;
			this.M21 = m21;
			this.M22 = m22;
			this.OffsetX = offsetX;
			this.OffsetY = offsetY;
		}

		public Matrix(Matrix matrix)
		{
			this = new Matrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
		}

		public static Matrix Identity
		{
			get
			{
				return new Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
			}
		}

		public double Determinant
		{
			get
			{
				return this.M11 * this.M22 - this.M12 * this.M21;
			}
		}

		public double M11 { get; set; }

		public double M12 { get; set; }

		public double M21 { get; set; }

		public double M22 { get; set; }

		public double OffsetX { get; set; }

		public double OffsetY { get; set; }

		public static Matrix operator *(Matrix matrix1, Matrix matrix2)
		{
			return new Matrix(matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21, matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22, matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21, matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22, matrix1.OffsetX * matrix2.M11 + matrix1.OffsetY * matrix2.M21 + matrix2.OffsetX, matrix1.OffsetX * matrix2.M12 + matrix1.OffsetY * matrix2.M22 + matrix2.OffsetY);
		}

		public static bool operator ==(Matrix a, Matrix b)
		{
			return a.M11 == b.M11 && a.M21 == b.M21 && a.M12 == b.M12 && a.M22 == b.M22 && a.OffsetX == b.OffsetX && a.OffsetY == b.OffsetY;
		}

		public static bool operator !=(Matrix a, Matrix b)
		{
			return !(a == b);
		}

		public bool IsIdentity()
		{
			return this == Matrix.Identity;
		}

		public Matrix Translate(double offsetX, double offsetY)
		{
			this.OffsetX += offsetX;
			this.OffsetY += offsetY;
			return this;
		}

		public Matrix Scale(double scaleX, double scaleY, double centerX = 0.0, double centerY = 0.0)
		{
			this = new Matrix(scaleX, 0.0, 0.0, scaleY, centerX, centerY) * this;
			return this;
		}

		public Matrix ScaleAppend(double scaleX, double scaleY, double centerX = 0.0, double centerY = 0.0)
		{
			this *= new Matrix(scaleX, 0.0, 0.0, scaleY, centerX, centerY);
			return this;
		}

		public Matrix Rotate(double angle, double centerX = 0.0, double centerY = 0.0)
		{
			Matrix matrix = default(Matrix);
			angle = 3.141592653589793 * angle / 180.0;
			double num = Math.Sin(angle);
			double num2 = Math.Cos(angle);
			double offsetX = centerX * (1.0 - num2) + centerY * num;
			double offsetY = centerY * (1.0 - num2) - centerX * num;
			matrix.SetMatrix(num2, num, -num, num2, offsetX, offsetY);
			this = matrix * this;
			return this;
		}

		public bool Equals(Matrix value)
		{
			return this.M11 == value.M11 && this.M12 == value.M12 && this.M21 == value.M21 && this.M22 == value.M22 && this.OffsetX == value.OffsetX && this.OffsetY == value.OffsetY;
		}

		public Point Transform(Point point)
		{
			double x = point.X;
			double y = point.Y;
			double x2 = x * this.M11 + y * this.M21 + this.OffsetX;
			double y2 = x * this.M12 + y * this.M22 + this.OffsetY;
			return new Point(x2, y2);
		}

		public Rect Transform(Rect rect)
		{
			Point point = new Point(rect.Top, rect.Left);
			Point point2 = new Point(rect.Bottom, rect.Right);
			return new Rect(this.Transform(point), this.Transform(point2));
		}

		internal Matrix ToMatrix()
		{
			return new Matrix(this.M11, this.M12, this.M21, this.M22, this.OffsetX, this.OffsetY);
		}

		public override int GetHashCode()
		{
			int num = 17;
			num = num * 23 + this.M11.GetHashCode();
			num = num * 23 + this.M12.GetHashCode();
			num = num * 23 + this.M21.GetHashCode();
			num = num * 23 + this.M22.GetHashCode();
			num = num * 23 + this.OffsetX.GetHashCode();
			return num * 23 + this.OffsetY.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			return obj != null && obj is Matrix && this.Equals((Matrix)obj);
		}

		public override string ToString()
		{
			return string.Format("{0} {1} 0 | {2} {3} 0 | {4} {5} 1", new object[] { this.M11, this.M12, this.M21, this.M22, this.OffsetX, this.OffsetY });
		}

		void SetMatrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
		{
			this.M11 = m11;
			this.M12 = m12;
			this.M21 = m21;
			this.M22 = m22;
			this.OffsetX = offsetX;
			this.OffsetY = offsetY;
		}
	}
}
