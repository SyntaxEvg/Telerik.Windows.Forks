using System;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	static class ObjectExtensions
	{
		public static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		public static int CombineHashCodes(int h1, int h2, int h3)
		{
			return ObjectExtensions.CombineHashCodes(ObjectExtensions.CombineHashCodes(h1, h2), h3);
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return ObjectExtensions.CombineHashCodes(ObjectExtensions.CombineHashCodes(h1, h2), ObjectExtensions.CombineHashCodes(h3, h4));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return ObjectExtensions.CombineHashCodes(ObjectExtensions.CombineHashCodes(h1, h2, h3, h4), h5);
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return ObjectExtensions.CombineHashCodes(ObjectExtensions.CombineHashCodes(h1, h2, h3, h4), ObjectExtensions.CombineHashCodes(h5, h6));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return ObjectExtensions.CombineHashCodes(ObjectExtensions.CombineHashCodes(h1, h2, h3, h4), ObjectExtensions.CombineHashCodes(h5, h6, h7));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return ObjectExtensions.CombineHashCodes(ObjectExtensions.CombineHashCodes(h1, h2, h3, h4), ObjectExtensions.CombineHashCodes(h5, h6, h7, h8));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9)
		{
			return ObjectExtensions.CombineHashCodes(ObjectExtensions.CombineHashCodes(ObjectExtensions.CombineHashCodes(h1, h2, h3, h4), ObjectExtensions.CombineHashCodes(h5, h6, h7, h8)), h9);
		}

		public static int CombineHashCodes(int h1, int h2, params int[] h)
		{
			int num = ObjectExtensions.CombineHashCodes(h1, h2);
			foreach (int h3 in h)
			{
				num = ObjectExtensions.CombineHashCodes(num, h3);
			}
			return num;
		}

		public static int GetHashCodeOrZero(this object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		public static bool EqualsOfT<T>(T first, T second)
		{
			return (first == null && second == null) || (first != null && first.Equals(second));
		}

		public static bool EqualsDouble(this double d1, double d2)
		{
			return Math.Abs(d1 - d2) <= ((Math.Abs(d1) > Math.Abs(d2)) ? Math.Abs(d2) : Math.Abs(d1)) * 1E-08;
		}

		public static bool EqualsDouble(this double? d1, double? d2)
		{
			if (d1 != null && d2 != null)
			{
				return d1.Value.EqualsDouble(d2.Value);
			}
			return d1 == null && d2 == null;
		}
	}
}
