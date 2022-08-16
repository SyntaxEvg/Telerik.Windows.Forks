using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Maths
{
	class Range<TData> : IEnumerable<TData>, IEnumerable
	{
		public Range(TData start, TData end, Func<TData, TData> nextElement, Comparison<TData> compare)
		{
			this.start = start;
			this.end = end;
			this.compare = compare;
			if (compare(start, end) == 0)
			{
				this.sequence = new TData[] { start };
				return;
			}
			if (compare(start, end) < 0)
			{
				this.sequence = Sequence.CreateSequence<TData>(nextElement, start, (TData v) => compare(nextElement(v), end) > 0);
				return;
			}
			this.sequence = Sequence.CreateSequence<TData>(nextElement, start, (TData v) => compare(nextElement(v), end) < 0);
		}

		public Range(TData start, TData end, Func<TData, TData> nextElement)
			: this(start, end, nextElement, new Comparison<TData>(Range<TData>.Compare))
		{
		}

		public TData End
		{
			get
			{
				return this.end;
			}
		}

		public TData Start
		{
			get
			{
				return this.start;
			}
		}

		public bool Contains(TData value)
		{
			return this.compare(value, this.start) >= 0 && this.compare(this.end, value) >= 0;
		}

		IEnumerator<TData> IEnumerable<T>.GetEnumerator()
		{
			return this.sequence.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		static int Compare(TData item1, TData item2)
		{
			return Comparer<TData>.Default.Compare(item1, item2);
		}

		readonly Comparison<TData> compare;

		readonly TData end;

		readonly IEnumerable<TData> sequence;

		readonly TData start;
	}
}
