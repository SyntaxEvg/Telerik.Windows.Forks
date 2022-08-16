using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Core.Data
{
	struct Matrix
	{
		public Matrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
		{
			this = default(global::Telerik.Windows.Documents.Core.Data.Matrix);
			this.M11 = m11;
			this.M12 = m12;
			this.M21 = m21;
			this.M22 = m22;
			this.OffsetX = offsetX;
			this.OffsetY = offsetY;
		}

		public Matrix(global::System.Windows.Media.Matrix matrix)
		{
			this = new global::Telerik.Windows.Documents.Core.Data.Matrix(matrix.M11, matrix.M12, matrix.M21, matrix.M22, matrix.OffsetX, matrix.OffsetY);
		}

		public static global::Telerik.Windows.Documents.Core.Data.Matrix Identity
		{
			get
			{
				return new global::Telerik.Windows.Documents.Core.Data.Matrix(1.0, 0.0, 0.0, 1.0, 0.0, 0.0);
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

		public static global::Telerik.Windows.Documents.Core.Data.Matrix operator *(global::Telerik.Windows.Documents.Core.Data.Matrix matrix1, global::Telerik.Windows.Documents.Core.Data.Matrix matrix2)
		{
			return new global::Telerik.Windows.Documents.Core.Data.Matrix(matrix1.M11 * matrix2.M11 + matrix1.M12 * matrix2.M21, matrix1.M11 * matrix2.M12 + matrix1.M12 * matrix2.M22, matrix1.M21 * matrix2.M11 + matrix1.M22 * matrix2.M21, matrix1.M21 * matrix2.M12 + matrix1.M22 * matrix2.M22, matrix1.OffsetX * matrix2.M11 + matrix1.OffsetY * matrix2.M21 + matrix2.OffsetX, matrix1.OffsetX * matrix2.M12 + matrix1.OffsetY * matrix2.M22 + matrix2.OffsetY);
		}

		public static bool operator ==(global::Telerik.Windows.Documents.Core.Data.Matrix a, global::Telerik.Windows.Documents.Core.Data.Matrix b)
		{
			return a.M11 == b.M11 && a.M21 == b.M21 && a.M12 == b.M12 && a.M22 == b.M22 && a.OffsetX == b.OffsetX && a.OffsetY == b.OffsetY;
		}

		public static bool operator !=(global::Telerik.Windows.Documents.Core.Data.Matrix a, global::Telerik.Windows.Documents.Core.Data.Matrix b)
		{
			return !(a == b);
		}

		public bool IsIdentity()
		{
			return this == global::Telerik.Windows.Documents.Core.Data.Matrix.Identity;
		}

		public global::Telerik.Windows.Documents.Core.Data.Matrix Translate(double offsetX, double offsetY)
		{
			this.OffsetX += offsetX;
			this.OffsetY += offsetY;
			return this;
		}

		public global::Telerik.Windows.Documents.Core.Data.Matrix Scale(double scaleX, double scaleY, double centerX = 0.0, double centerY = 0.0)
		{
			this = new global::Telerik.Windows.Documents.Core.Data.Matrix(scaleX, 0.0, 0.0, scaleY, centerX, centerY) * this;
			return this;
		}

		public global::Telerik.Windows.Documents.Core.Data.Matrix ScaleAppend(double scaleX, double scaleY, double centerX = 0.0, double centerY = 0.0)
		{
			this *= new global::Telerik.Windows.Documents.Core.Data.Matrix(scaleX, 0.0, 0.0, scaleY, centerX, centerY);
			return this;
		}

		public global::Telerik.Windows.Documents.Core.Data.Matrix Rotate(double angle, double centerX = 0.0, double centerY = 0.0)
		{
			global::Telerik.Windows.Documents.Core.Data.Matrix matrix = default(global::Telerik.Windows.Documents.Core.Data.Matrix);
			angle = 3.141592653589793 * angle / 180.0;
			double num = global::System.Math.Sin(angle);
			double num2 = global::System.Math.Cos(angle);
			double offsetX = centerX * (1.0 - num2) + centerY * num;
			double offsetY = centerY * (1.0 - num2) - centerX * num;
			matrix.SetMatrix(num2, num, -num, num2, offsetX, offsetY);
			this = matrix * this;
			return this;
		}

		public bool Equals(global::Telerik.Windows.Documents.Core.Data.Matrix value)
		{
			return this.M11 == value.M11 && this.M12 == value.M12 && this.M21 == value.M21 && this.M22 == value.M22 && this.OffsetX == value.OffsetX && this.OffsetY == value.OffsetY;
		}

		public global::System.Windows.Point Transform(global::System.Windows.Point point)
		{
			double x = point.X;
			double y = point.Y;
			double x2 = x * this.M11 + y * this.M21 + this.OffsetX;
			double y2 = x * this.M12 + y * this.M22 + this.OffsetY;
			return new global::System.Windows.Point(x2, y2);
		}

		public global::System.Windows.Rect Transform(global::System.Windows.Rect rect)
		{
			global::System.Windows.Point point = new global::System.Windows.Point(rect.Top, rect.Left);
			global::System.Windows.Point point2 = new global::System.Windows.Point(rect.Bottom, rect.Right);
			return new global::System.Windows.Rect(this.Transform(point), this.Transform(point2));
		}

		internal global::System.Windows.Media.Matrix ToMatrix()
		{
			return new global::System.Windows.Media.Matrix(this.M11, this.M12, this.M21, this.M22, this.OffsetX, this.OffsetY);
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
			return obj != null && obj is global::Telerik.Windows.Documents.Core.Data.Matrix && this.Equals((global::Telerik.Windows.Documents.Core.Data.Matrix)obj);
		}

		public override string ToString()
		{
			return string.Format("{0} {1} 0 | {2} {3} 0 | {4} {5} 1", new object[] { this.M11, this.M12, this.M21, this.M22, this.OffsetX, this.OffsetY });
		}

		private void SetMatrix(double m11, double m12, double m21, double m22, double offsetX, double offsetY)
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
