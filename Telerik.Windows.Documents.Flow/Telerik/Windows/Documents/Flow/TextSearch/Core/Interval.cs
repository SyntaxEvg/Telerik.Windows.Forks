using System;
using System.Diagnostics;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.TextSearch.Core
{
	[DebuggerDisplay("[{Start} - {End}]")]
	class Interval<T> : IComparable<T> where T : IComparable<T>
	{
		public Interval(T start, T end)
		{
			Guard.ThrowExceptionIfTrue(start.CompareTo(end) == 1, "Interval's end cannot be less than its start.");
			this.start = start;
			this.end = end;
		}

		public T Start
		{
			get
			{
				return this.start;
			}
		}

		public T End
		{
			get
			{
				return this.end;
			}
		}

		public bool Contains(T value)
		{
			T t = this.Start;
			if (t.CompareTo(value) <= 0)
			{
				T t2 = this.End;
				return t2.CompareTo(value) == 1;
			}
			return false;
		}

		public int CompareTo(T other)
		{
			T t = this.Start;
			return t.CompareTo(other);
		}

		readonly T start;

		readonly T end;
	}
}
