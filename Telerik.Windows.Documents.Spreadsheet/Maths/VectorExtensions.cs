using System;
using System.Windows;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	static class VectorExtensions
	{
		public static Vector Lerp(Vector u, Vector v, double fraction)
		{
			return new Vector(Utils.Lerp(u.X, v.X, fraction), Utils.Lerp(u.Y, v.Y, fraction));
		}

		public static Vector MirrorHorizontally(this Vector v)
		{
			return new Vector(v.Y, -v.X);
		}

		public static Vector MirrorVertically(this Vector v)
		{
			return new Vector(-v.Y, v.X);
		}

		public static Vector UnitVector(double degrees)
		{
			double num = Trigonometry.DegreesToRadians(degrees);
			return new Vector(Math.Cos(num), Math.Sin(num));
		}

		public static Vector Perpendicular(Vector v)
		{
			return new Vector(v.Y, -v.X);
		}

		public static Vector Normalized(this Vector vector)
		{
			Vector vector2 = new Vector(vector.X, vector.Y);
			double length = vector2.Length;
			if (length.IsVerySmall())
			{
				return new Vector(0.0, 1.0);
			}
			return vector2 / length;
		}
	}
}
