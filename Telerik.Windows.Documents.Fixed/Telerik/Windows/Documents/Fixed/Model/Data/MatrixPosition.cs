using System;
using System.Windows.Media;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Data
{
	public class MatrixPosition : IPosition
	{
		public MatrixPosition()
		{
			this.Matrix = default(Matrix);
		}

		public MatrixPosition(IPosition other)
		{
			this.Matrix = other.Matrix;
		}

		public MatrixPosition(Matrix matrix)
		{
			this.Matrix = matrix;
		}

		public static MatrixPosition Default
		{
			get
			{
				return new MatrixPosition();
			}
		}

		public Matrix Matrix { get; set; }

		public void Scale(double scaleX, double scaleY)
		{
			this.Matrix = this.Matrix.ScaleMatrix(scaleX, scaleY);
		}

		public void ScaleAt(double scaleX, double scaleY, double centerX, double centerY)
		{
			this.Matrix = this.Matrix.ScaleMatrixAt(scaleX, scaleY, centerX, centerY);
		}

		public void Rotate(double angle)
		{
			this.Matrix = this.Matrix.RotateMatrix(angle);
		}

		public void RotateAt(double angle, double centerX, double centerY)
		{
			this.Matrix = this.Matrix.RotateMatrixAt(angle, centerX, centerY);
		}

		public void Translate(double offsetX, double offsetY)
		{
			this.Matrix = this.Matrix.TranslateMatrix(offsetX, offsetY);
		}

		public void Clear()
		{
			this.Matrix = Matrix.Identity;
		}

		public IPosition Clone()
		{
			return new MatrixPosition(this.Matrix);
		}
	}
}
