using System;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	class PathKeyComparer : IComparer<ushort[]>, IEqualityComparer<ushort[]>
	{
		public static PathKeyComparer Comparer
		{
			get
			{
				return PathKeyComparer._Comparer;
			}
		}

		public int Compare(ushort[] x, ushort[] y)
		{
			int num = x.Length;
			int num2 = y.Length;
			int num3 = ((num < num2) ? num : num2);
			int num4 = 0;
			while (num4 < num3 && x[num4] == y[num4])
			{
				num4++;
			}
			if (num4 >= num3)
			{
				if (num < num2)
				{
					return -1;
				}
				if (num <= num2)
				{
					return 0;
				}
				return 1;
			}
			else
			{
				if (x[num4] >= y[num4])
				{
					return 1;
				}
				return -1;
			}
		}

		protected bool CompareEqualLength(ushort[] x, ushort[] y)
		{
			int num = x.Length;
			for (int i = 0; i < num; i++)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		public bool Equals(ushort[] x, ushort[] y)
		{
			int num = x.Length;
			if (num != y.Length)
			{
				return false;
			}
			while (num-- > 0)
			{
				if (x[num] != y[num])
				{
					return false;
				}
			}
			return true;
		}

		public int GetHashCode(ushort[] obj)
		{
			int num = -2128831035;
			for (int i = 0; i < obj.Length; i++)
			{
				num = (num ^ (int)obj[i]) * 16777619;
			}
			return ((((num + (num << 13)) ^ (num >> 7)) + (num << 3)) ^ (num >> 17)) + (num << 5);
		}

		static readonly PathKeyComparer _Comparer = new PathKeyComparer();
	}
}
