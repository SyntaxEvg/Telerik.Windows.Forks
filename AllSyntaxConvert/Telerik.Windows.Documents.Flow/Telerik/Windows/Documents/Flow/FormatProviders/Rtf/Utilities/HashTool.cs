using System;
using System.Collections;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Utilities
{
	static class HashTool
	{
		public static int AddHashCode(int hash, object obj)
		{
			int num = ((obj != null) ? obj.GetHashCode() : 0);
			if (hash != 0)
			{
				num += hash * 31;
			}
			return num;
		}

		public static int AddHashCode(int hash, int objHash)
		{
			int num = objHash;
			if (hash != 0)
			{
				num += hash * 31;
			}
			return num;
		}

		public static int ComputeHashCode(IEnumerable enumerable)
		{
			int num = 1;
			if (enumerable == null)
			{
				throw new ArgumentNullException("enumerable");
			}
			foreach (object obj in enumerable)
			{
				num = num * 31 + ((obj != null) ? obj.GetHashCode() : 0);
			}
			return num;
		}
	}
}
