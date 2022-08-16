using System;
using System.Globalization;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	struct RMatrix : ICloneable
	{
		public RMatrix(int rowCount, int colCount)
		{
			this.rowCount = rowCount;
			this.colCount = colCount;
			this.matrix = new double[rowCount, colCount];
			for (int i = 0; i < rowCount; i++)
			{
				for (int j = 0; j < colCount; j++)
				{
					this.matrix[i, j] = 0.0;
				}
			}
		}

		public RMatrix(double[,] matrix)
		{
			this.rowCount = matrix.GetLength(0);
			this.colCount = matrix.GetLength(1);
			this.matrix = matrix;
		}

		public RMatrix(RMatrix m)
		{
			this.rowCount = m.GetnRows;
			this.colCount = m.GetnCols;
			this.matrix = m.matrix;
		}

		public RMatrix IdentityMatrix()
		{
			RMatrix result = new RMatrix(this.rowCount, this.colCount);
			for (int i = 0; i < this.rowCount; i++)
			{
				for (int j = 0; j < this.colCount; j++)
				{
					if (i == j)
					{
						result[i, j] = 1.0;
					}
				}
			}
			return result;
		}

		public double this[int m, int n]
		{
			get
			{
				if (m < 0 || m > this.rowCount)
				{
					throw new Exception("m-th row is out of range!");
				}
				if (n < 0 || n > this.colCount)
				{
					throw new Exception("n-th col is out of range!");
				}
				return this.matrix[m, n];
			}
			set
			{
				this.matrix[m, n] = value;
			}
		}

		public int GetnRows
		{
			get
			{
				return this.rowCount;
			}
		}

		public int GetnCols
		{
			get
			{
				return this.colCount;
			}
		}

		public RMatrix Clone()
		{
			return new RMatrix(this.matrix)
			{
				matrix = (double[,])this.matrix.Clone()
			};
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public override string ToString()
		{
			string text = "(";
			for (int i = 0; i < this.rowCount; i++)
			{
				string text2 = "";
				for (int j = 0; j < this.colCount - 1; j++)
				{
					text2 = text2 + this.matrix[i, j].ToString(CultureInfo.InvariantCulture) + ", ";
				}
				text2 += this.matrix[i, this.colCount - 1].ToString(CultureInfo.InvariantCulture);
				if (i != this.rowCount - 1 && i == 0)
				{
					text = text + text2 + "\n";
				}
				else if (i != this.rowCount - 1 && i != 0)
				{
					text = text + " " + text2 + "\n";
				}
				else
				{
					text = text + " " + text2 + ")";
				}
			}
			return text;
		}

		public override bool Equals(object obj)
		{
			return obj is RMatrix && this.Equals((RMatrix)obj);
		}

		public bool Equals(RMatrix m)
		{
			return Utils.ArraysEqual<double>(this.matrix, m.matrix);
		}

		public override int GetHashCode()
		{
			return this.matrix.GetHashCode();
		}

		public static bool operator ==(RMatrix m1, RMatrix m2)
		{
			return m1.Equals(m2);
		}

		public static bool operator !=(RMatrix m1, RMatrix m2)
		{
			return !m1.Equals(m2);
		}

		public static RMatrix operator +(RMatrix m)
		{
			return m;
		}

		public static RMatrix operator +(RMatrix m1, RMatrix m2)
		{
			if (!RMatrix.CompareDimension(m1, m2))
			{
				throw new Exception("The dimensions of two matrices must be the same!");
			}
			RMatrix result = new RMatrix(m1.GetnRows, m1.GetnCols);
			for (int i = 0; i < m1.GetnRows; i++)
			{
				for (int j = 0; j < m1.GetnCols; j++)
				{
					result[i, j] = m1[i, j] + m2[i, j];
				}
			}
			return result;
		}

		public static RMatrix operator -(RMatrix m)
		{
			for (int i = 0; i < m.GetnRows; i++)
			{
				for (int j = 0; j < m.GetnCols; j++)
				{
					m[i, j] = -m[i, j];
				}
			}
			return m;
		}

		public static RMatrix operator -(RMatrix m1, RMatrix m2)
		{
			if (!RMatrix.CompareDimension(m1, m2))
			{
				throw new Exception("The dimensions of two matrices must be the same!");
			}
			RMatrix result = new RMatrix(m1.GetnRows, m1.GetnCols);
			for (int i = 0; i < m1.GetnRows; i++)
			{
				for (int j = 0; j < m1.GetnCols; j++)
				{
					result[i, j] = m1[i, j] - m2[i, j];
				}
			}
			return result;
		}

		public static RMatrix operator *(RMatrix m, double d)
		{
			RMatrix result = new RMatrix(m.GetnRows, m.GetnCols);
			for (int i = 0; i < m.GetnRows; i++)
			{
				for (int j = 0; j < m.GetnCols; j++)
				{
					result[i, j] = m[i, j] * d;
				}
			}
			return result;
		}

		public static RMatrix operator *(double d, RMatrix m)
		{
			RMatrix result = new RMatrix(m.GetnRows, m.GetnCols);
			for (int i = 0; i < m.GetnRows; i++)
			{
				for (int j = 0; j < m.GetnCols; j++)
				{
					result[i, j] = m[i, j] * d;
				}
			}
			return result;
		}

		public static RMatrix operator /(RMatrix m, double d)
		{
			RMatrix result = new RMatrix(m.GetnRows, m.GetnCols);
			for (int i = 0; i < m.GetnRows; i++)
			{
				for (int j = 0; j < m.GetnCols; j++)
				{
					result[i, j] = m[i, j] / d;
				}
			}
			return result;
		}

		public static RMatrix operator /(double d, RMatrix m)
		{
			RMatrix result = new RMatrix(m.GetnRows, m.GetnCols);
			for (int i = 0; i < m.GetnRows; i++)
			{
				for (int j = 0; j < m.GetnCols; j++)
				{
					result[i, j] = m[i, j] / d;
				}
			}
			return result;
		}

		public static RMatrix operator *(RMatrix m1, RMatrix m2)
		{
			if (m1.GetnCols != m2.GetnRows)
			{
				throw new Exception("The numbers of columns of the first matrix must be equal to the number of  rows of the second matrix!");
			}
			RMatrix result = new RMatrix(m1.GetnRows, m2.GetnCols);
			for (int i = 0; i < m1.GetnRows; i++)
			{
				for (int j = 0; j < m2.GetnCols; j++)
				{
					double num = result[i, j];
					for (int k = 0; k < result.GetnRows; k++)
					{
						num += m1[i, k] * m2[k, j];
					}
					result[i, j] = num;
				}
			}
			return result;
		}

		public RMatrix GetTranspose()
		{
			RMatrix result = this;
			result.Transpose();
			return result;
		}

		public void Transpose()
		{
			RMatrix rmatrix = new RMatrix(this.colCount, this.rowCount);
			for (int i = 0; i < this.rowCount; i++)
			{
				for (int j = 0; j < this.colCount; j++)
				{
					rmatrix[j, i] = this.matrix[i, j];
				}
			}
			this = rmatrix;
		}

		public double GetTrace()
		{
			double num = 0.0;
			for (int i = 0; i < this.rowCount; i++)
			{
				if (i < this.colCount)
				{
					num += this.matrix[i, i];
				}
			}
			return num;
		}

		public bool IsSquared()
		{
			return this.rowCount == this.colCount;
		}

		public static bool CompareDimension(RMatrix m1, RMatrix m2)
		{
			return m1.GetnRows == m2.GetnRows && m1.GetnCols == m2.GetnCols;
		}

		public RVector GetRowVector(int m)
		{
			if (m < 0 || m > this.rowCount)
			{
				throw new Exception("m-th row is out of range!");
			}
			RVector result = new RVector(this.colCount);
			for (int i = 0; i < this.colCount; i++)
			{
				result[i] = this.matrix[m, i];
			}
			return result;
		}

		public RVector GetColVector(int n)
		{
			if (n < 0 || n > this.colCount)
			{
				throw new Exception("n-th col is out of range!");
			}
			RVector result = new RVector(this.rowCount);
			for (int i = 0; i < this.rowCount; i++)
			{
				result[i] = this.matrix[i, n];
			}
			return result;
		}

		public RMatrix ReplaceRow(RVector vec, int m)
		{
			if (m < 0 || m > this.rowCount)
			{
				throw new Exception("m-th row is out of range!");
			}
			if (vec.GetVectorSize != this.colCount)
			{
				throw new Exception("Vector ndim is out of range!");
			}
			for (int i = 0; i < this.colCount; i++)
			{
				this.matrix[m, i] = vec[i];
			}
			return new RMatrix(this.matrix);
		}

		public RMatrix ReplaceCol(RVector vec, int n)
		{
			if (n < 0 || n > this.colCount)
			{
				throw new Exception("n-th col is out of range!");
			}
			if (vec.GetVectorSize != this.rowCount)
			{
				throw new Exception("Vector ndim is out of range!");
			}
			for (int i = 0; i < this.rowCount; i++)
			{
				this.matrix[i, n] = vec[i];
			}
			return new RMatrix(this.matrix);
		}

		public RMatrix SwapRMatrixow(int m, int n)
		{
			for (int i = 0; i < this.colCount; i++)
			{
				double num = this.matrix[m, i];
				this.matrix[m, i] = this.matrix[n, i];
				this.matrix[n, i] = num;
			}
			return new RMatrix(this.matrix);
		}

		public RMatrix SwapMatrixColumn(int m, int n)
		{
			for (int i = 0; i < this.rowCount; i++)
			{
				double num = this.matrix[i, m];
				this.matrix[i, m] = this.matrix[i, n];
				this.matrix[i, n] = num;
			}
			return new RMatrix(this.matrix);
		}

		public static RVector Transform(RMatrix mat, RVector vec)
		{
			RVector result = new RVector(vec.GetVectorSize);
			if (!mat.IsSquared())
			{
				throw new Exception("The matrix must be squared!");
			}
			if (mat.GetnCols != vec.GetVectorSize)
			{
				throw new Exception("The ndim of the vector must be equal to the number of cols of the matrix!");
			}
			for (int i = 0; i < mat.GetnRows; i++)
			{
				result[i] = 0.0;
				for (int j = 0; j < mat.GetnCols; j++)
				{
					ref RVector ptr = ref result;
					int i2;
					result[i2 = i] = ptr[i2] + mat[i, j] * vec[j];
				}
			}
			return result;
		}

		public static RVector Transform(RVector vec, RMatrix mat)
		{
			RVector result = new RVector(vec.GetVectorSize);
			if (!mat.IsSquared())
			{
				throw new Exception("The matrix must be squared!");
			}
			if (mat.GetnRows != vec.GetVectorSize)
			{
				throw new Exception("The ndim of the vector must be equal to the number of rows of the matrix!");
			}
			for (int i = 0; i < mat.GetnRows; i++)
			{
				result[i] = 0.0;
				for (int j = 0; j < mat.GetnCols; j++)
				{
					ref RVector ptr = ref result;
					int i2;
					result[i2 = i] = ptr[i2] + vec[j] * mat[j, i];
				}
			}
			return result;
		}

		public static RMatrix Transform(RVector v1, RVector v2)
		{
			if (v1.GetVectorSize != v2.GetVectorSize)
			{
				throw new Exception("The vectors must have the same ndim!");
			}
			RMatrix result = new RMatrix(v1.GetVectorSize, v1.GetVectorSize);
			for (int i = 0; i < v1.GetVectorSize; i++)
			{
				for (int j = 0; j < v1.GetVectorSize; j++)
				{
					result[j, i] = v1[i] * v2[j];
				}
			}
			return result;
		}

		public static double Determinant(RMatrix mat)
		{
			double num = 0.0;
			if (!mat.IsSquared())
			{
				throw new Exception("The matrix must be squared!");
			}
			if (mat.GetnRows == 1)
			{
				num = mat[0, 0];
			}
			else
			{
				for (int i = 0; i < mat.GetnRows; i++)
				{
					num += Math.Pow(1.0, (double)i) * mat[0, i] * RMatrix.Determinant(RMatrix.Minor(mat, 0, i));
				}
			}
			return num;
		}

		public static RMatrix Minor(RMatrix mat, int row, int col)
		{
			RMatrix result = new RMatrix(mat.GetnRows - 1, mat.GetnCols - 1);
			int num = 0;
			for (int i = 0; i < mat.GetnRows; i++)
			{
				if (i != row)
				{
					int num2 = 0;
					for (int j = 0; j < mat.GetnCols; j++)
					{
						if (j != col)
						{
							result[num, num2] = mat[i, j];
							num2++;
						}
					}
					num++;
				}
			}
			return result;
		}

		public static RMatrix Adjoint(RMatrix mat)
		{
			if (!mat.IsSquared())
			{
				throw new Exception("The matrix must be squared!");
			}
			RMatrix rmatrix = new RMatrix(mat.GetnRows, mat.GetnCols);
			for (int i = 0; i < mat.GetnRows; i++)
			{
				for (int j = 0; j < mat.GetnCols; j++)
				{
					rmatrix[i, j] = Math.Pow(1.0, (double)(i + j)) * RMatrix.Determinant(RMatrix.Minor(mat, i, j));
				}
			}
			return rmatrix.GetTranspose();
		}

		public static RMatrix Inverse(RMatrix mat)
		{
			if (Math.Abs(RMatrix.Determinant(mat)) < 1E-08)
			{
				throw new Exception("Cannot inverse a matrix with a zero determinant!");
			}
			return RMatrix.Adjoint(mat) / RMatrix.Determinant(mat);
		}

		readonly int rowCount;

		readonly int colCount;

		double[,] matrix;
	}
}
