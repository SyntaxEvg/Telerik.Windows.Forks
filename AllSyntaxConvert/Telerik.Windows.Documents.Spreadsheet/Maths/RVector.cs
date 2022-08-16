using System;
using System.Linq;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	struct RVector : ICloneable
	{
		public RVector(int dimension)
		{
			this.dimension = dimension;
			this.vector = new double[dimension];
			for (int i = 0; i < dimension; i++)
			{
				this.vector[i] = 0.0;
			}
		}

		public RVector(double[] vector)
		{
			this.dimension = vector.Length;
			this.vector = vector;
		}

		public double this[int i]
		{
			get
			{
				if (i < 0 || i > this.dimension)
				{
					throw new ArgumentOutOfRangeException("i");
				}
				return this.vector[i];
			}
			set
			{
				this.vector[i] = value;
			}
		}

		public int GetVectorSize
		{
			get
			{
				return this.dimension;
			}
		}

		public RVector Clone()
		{
			return new RVector(this.vector)
			{
				vector = (double[])this.vector.Clone()
			};
		}

		object ICloneable.Clone()
		{
			return this.Clone();
		}

		public RVector SwapVectorEntries(int m, int n)
		{
			double num = this.vector[m];
			this.vector[m] = this.vector[n];
			this.vector[n] = num;
			return new RVector(this.vector);
		}

		public override string ToString()
		{
			if (this.vector.Length <= 5)
			{
				return "(" + string.Join<double>(",", this.vector) + ")";
			}
			return "(" + string.Join<double>(",", this.vector.Take(5)) + "...)";
		}

		public override bool Equals(object obj)
		{
			return obj is RVector && this.Equals((RVector)obj);
		}

		public bool Equals(RVector v)
		{
			return this.vector == v.vector;
		}

		public override int GetHashCode()
		{
			return this.vector.GetHashCode();
		}

		public static bool operator ==(RVector v1, RVector v2)
		{
			return v1.Equals(v2);
		}

		public static bool operator !=(RVector v1, RVector v2)
		{
			return !v1.Equals(v2);
		}

		public static RVector operator +(RVector v)
		{
			return v;
		}

		public static RVector operator +(RVector v1, RVector v2)
		{
			RVector result = new RVector(v1.dimension);
			for (int i = 0; i < v1.dimension; i++)
			{
				result[i] = v1[i] + v2[i];
			}
			return result;
		}

		public static RVector operator -(RVector v)
		{
			double[] array = new double[v.dimension];
			for (int i = 0; i < v.dimension; i++)
			{
				array[i] = -v[i];
			}
			return new RVector(array);
		}

		public static RVector operator -(RVector v1, RVector v2)
		{
			RVector result = new RVector(v1.dimension);
			for (int i = 0; i < v1.dimension; i++)
			{
				result[i] = v1[i] - v2[i];
			}
			return result;
		}

		public static RVector operator *(RVector v, double d)
		{
			RVector result = new RVector(v.dimension);
			for (int i = 0; i < v.dimension; i++)
			{
				result[i] = v[i] * d;
			}
			return result;
		}

		public static RVector operator *(double d, RVector v)
		{
			RVector result = new RVector(v.dimension);
			for (int i = 0; i < v.dimension; i++)
			{
				result[i] = d * v[i];
			}
			return result;
		}

		public static RVector operator /(RVector v, double d)
		{
			RVector result = new RVector(v.dimension);
			for (int i = 0; i < v.dimension; i++)
			{
				result[i] = v[i] / d;
			}
			return result;
		}

		public static RVector operator /(double d, RVector v)
		{
			RVector result = new RVector(v.dimension);
			for (int i = 0; i < v.dimension; i++)
			{
				result[i] = v[i] / d;
			}
			return result;
		}

		public static double DotProduct(RVector v1, RVector v2)
		{
			double num = 0.0;
			for (int i = 0; i < v1.dimension; i++)
			{
				num += v1[i] * v2[i];
			}
			return num;
		}

		public double GetNorm()
		{
			double num = 0.0;
			for (int i = 0; i < this.dimension; i++)
			{
				num += this.vector[i] * this.vector[i];
			}
			return Math.Sqrt(num);
		}

		public double GetNormSquare()
		{
			double num = 0.0;
			for (int i = 0; i < this.dimension; i++)
			{
				num += this.vector[i] * this.vector[i];
			}
			return num;
		}

		public void Normalize()
		{
			double norm = this.GetNorm();
			if (Math.Abs(norm) < 1E-08)
			{
				throw new Exception("Tried to normalize a vector with norm of zero!");
			}
			for (int i = 0; i < this.dimension; i++)
			{
				this.vector[i] /= norm;
			}
		}

		public RVector GetUnitVector()
		{
			RVector result = new RVector(this.vector);
			result.Normalize();
			return result;
		}

		public static RVector CrossProduct(RVector v1, RVector v2)
		{
			if (v1.dimension != 3)
			{
				throw new Exception("Vector v1 must be 3 dimensional!");
			}
			if (v2.dimension != 3)
			{
				throw new Exception("Vector v2 must be 3 dimensional!");
			}
			RVector result = new RVector(3);
			result[0] = v1[1] * v2[2] - v1[2] * v2[1];
			result[1] = v1[2] * v2[0] - v1[0] * v2[2];
			result[2] = v1[0] * v2[1] - v1[1] * v2[0];
			return result;
		}

		readonly int dimension;

		double[] vector;
	}
}
