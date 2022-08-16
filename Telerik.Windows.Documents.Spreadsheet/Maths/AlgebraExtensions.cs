using System;
using System.Windows;
using System.Windows.Media;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class AlgebraExtensions
	{
		public static RVector GaussJacobi(RMatrix A, RVector b, int MaxIterations = 10, double tolerance = 0.0001)
		{
			int getVectorSize = b.GetVectorSize;
			RVector rvector = new RVector(getVectorSize);
			for (int i = 0; i < MaxIterations; i++)
			{
				RVector v = rvector.Clone();
				for (int j = 0; j < getVectorSize; j++)
				{
					double num = b[j];
					double num2 = A[j, j];
					if (Math.Abs(num2) < 1E-08)
					{
						throw new Exception("Diagonal element went into epsilon.");
					}
					for (int k = 0; k < getVectorSize; k++)
					{
						if (k != j)
						{
							num -= A[j, k] * v[k];
						}
					}
					rvector[j] = num / num2;
				}
				if ((rvector - v).GetNorm() < tolerance)
				{
					return rvector;
				}
			}
			return rvector;
		}

		public static RVector GaussJordan(RMatrix A, RVector b)
		{
			AlgebraExtensions.Triangulate(A, b);
			int getVectorSize = b.GetVectorSize;
			RVector rvector = new RVector(getVectorSize);
			for (int i = getVectorSize - 1; i >= 0; i--)
			{
				double num = A[i, i];
				if (Math.Abs(num) < 1E-08)
				{
					throw new Exception("Diagonal element is too small!");
				}
				rvector[i] = (b[i] - RVector.DotProduct(A.GetRowVector(i), rvector)) / num;
			}
			return rvector;
		}

		public static RVector GaussSeidel(RMatrix A, RVector b, int maxIterations = 10, double tolerance = 0.0001)
		{
			int getVectorSize = b.GetVectorSize;
			RVector rvector = new RVector(getVectorSize);
			for (int i = 0; i < maxIterations; i++)
			{
				RVector v = rvector.Clone();
				for (int j = 0; j < getVectorSize; j++)
				{
					double num = b[j];
					double num2 = A[j, j];
					if (Math.Abs(num2) < 1E-08)
					{
						throw new Exception("Diagonal element is too small!");
					}
					for (int k = 0; k < j; k++)
					{
						num -= A[j, k] * rvector[k];
					}
					for (int l = j + 1; l < getVectorSize; l++)
					{
						num -= A[j, l] * v[l];
					}
					rvector[j] = num / num2;
				}
				if ((rvector - v).GetNorm() < tolerance)
				{
					return rvector;
				}
			}
			return rvector;
		}

		public static double LUCrout(RMatrix A, RVector b)
		{
			AlgebraExtensions.LUDecompose(A);
			return AlgebraExtensions.LUSubstitute(A, b);
		}

		public static RMatrix LUInverse(RMatrix matrix)
		{
			int getnRows = matrix.GetnRows;
			RMatrix rmatrix = matrix.IdentityMatrix();
			AlgebraExtensions.LUDecompose(matrix);
			RVector rowVector = new RVector(getnRows);
			for (int i = 0; i < getnRows; i++)
			{
				rowVector = rmatrix.GetRowVector(i);
				AlgebraExtensions.LUSubstitute(matrix, rowVector);
				rmatrix.ReplaceRow(rowVector, i);
			}
			return rmatrix.GetTranspose();
		}

		public static RMatrix LUDecompose(RMatrix matrix)
		{
			int getnRows = matrix.GetnRows;
			for (int i = 0; i < getnRows; i++)
			{
				for (int j = 0; j < getnRows; j++)
				{
					double num = matrix[i, j];
					for (int k = 0; k < Math.Min(i, j); k++)
					{
						num -= matrix[i, k] * matrix[k, j];
					}
					if (j > i)
					{
						double num2 = matrix[i, i];
						if (Math.Abs(num) < 1E-08)
						{
							throw new Exception("Diagonal element is too small!");
						}
						num /= num2;
					}
					matrix[i, j] = num;
				}
			}
			return matrix;
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

		public static bool IsZero(this Point a)
		{
			return a.Length() < 1E-08;
		}

		public static Point UnitVector(this Point a)
		{
			if (a.IsZero())
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
			double num = m.M11 * m.M22 - m.M12 * m.M21;
			double num2 = 1.0 / num;
			return new Matrix(m.M22 * num2, -m.M12 * num2, -m.M21 * num2, m.M11 * num2, (m.M21 * m.OffsetY - m.OffsetX * m.M22) * num2, (m.OffsetX * m.M12 - m.M11 * m.OffsetY) * num2);
		}

		static Matrix GetTransformationAt(this Matrix zeroCenteredTransform, double centerX, double centerY)
		{
			double offsetX = zeroCenteredTransform.OffsetX + (1.0 - zeroCenteredTransform.M11) * centerX - zeroCenteredTransform.M21 * centerY;
			double offsetY = zeroCenteredTransform.OffsetY + (1.0 - zeroCenteredTransform.M22) * centerY - zeroCenteredTransform.M12 * centerX;
			return new Matrix(zeroCenteredTransform.M11, zeroCenteredTransform.M12, zeroCenteredTransform.M21, zeroCenteredTransform.M22, offsetX, offsetY);
		}

		static double LUSubstitute(RMatrix matrix, RVector vec)
		{
			int getVectorSize = vec.GetVectorSize;
			double num = 1.0;
			for (int i = 0; i < getVectorSize; i++)
			{
				double num2 = vec[i];
				for (int j = 0; j < i; j++)
				{
					num2 -= matrix[i, j] * vec[j];
				}
				double num3 = matrix[i, i];
				if (Math.Abs(num2) < 1E-08)
				{
					throw new Exception("Diagonal element is too small!");
				}
				num2 /= num3;
				vec[i] = num2;
				num *= matrix[i, i];
			}
			for (int k = getVectorSize - 1; k >= 0; k--)
			{
				double num4 = vec[k];
				for (int l = k + 1; l < getVectorSize; l++)
				{
					num4 -= matrix[k, l] * vec[l];
				}
				vec[k] = num4;
			}
			return num;
		}

		static void Triangulate(RMatrix A, RVector b)
		{
			int getnRows = A.GetnRows;
			for (int i = 0; i < getnRows - 1; i++)
			{
				double num = AlgebraExtensions.pivotGaussJordan(A, b, i);
				if (Math.Abs(num) < 1E-08)
				{
					throw new Exception("Diagonal element is too small!");
				}
				for (int j = i + 1; j < getnRows; j++)
				{
					double num2 = A[j, i] / num;
					for (int k = i + 1; k < getnRows; k++)
					{
						ref RMatrix ptr = ref A;
						int m;
						int n;
						A[m = j, n = k] = ptr[m, n] - num2 * A[i, k];
					}
					ref RVector ptr2 = ref b;
					int i2;
					b[i2 = j] = ptr2[i2] - num2 * b[i];
				}
			}
		}

		static double pivotGaussJordan(RMatrix A, RVector b, int q)
		{
			int getVectorSize = b.GetVectorSize;
			int num = q;
			double num2 = 0.0;
			for (int i = q; i < getVectorSize; i++)
			{
				double num3 = Math.Abs(A[i, q]);
				if (num3 > num2)
				{
					num2 = num3;
					num = i;
				}
			}
			if (num > q)
			{
				A.SwapRMatrixow(q, num);
				b.SwapVectorEntries(q, num);
			}
			return A[q, q];
		}
	}
}
