using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	class Range<TIndex, TValue> : IComparable<Range<TIndex, TValue>>, IEquatable<Range<TIndex, TValue>> where TIndex : IComparable<TIndex>
	{
		public TValue Value { get; set; }

		public TIndex Start
		{
			get
			{
				return this.start;
			}
			internal set
			{
				this.start = value;
			}
		}

		public TIndex End
		{
			get
			{
				return this.end;
			}
			internal set
			{
				this.end = value;
			}
		}

		public bool IsDefault
		{
			get
			{
				return this.isDefault;
			}
		}

		public Range(TIndex start, TIndex end, bool isDefault, TValue value = default(TValue))
		{
			Guard.ThrowExceptionIfLessThan<TIndex>(start, end, "end");
			this.Start = start;
			this.End = end;
			this.Value = value;
			this.isDefault = isDefault;
		}

		public bool Contains(TIndex position)
		{
			return position.CompareTo(this.Start) >= 0 && position.CompareTo(this.End) <= 0;
		}

		public bool Contains(Range<TIndex, TValue> range)
		{
			Guard.ThrowExceptionIfNull<Range<TIndex, TValue>>(range, "range");
			TIndex tindex = range.Start;
			if (tindex.CompareTo(this.Start) >= 0)
			{
				TIndex tindex2 = range.End;
				return tindex2.CompareTo(this.End) <= 0;
			}
			return false;
		}

		public bool IntersectsWith(Range<TIndex, TValue> range)
		{
			Guard.ThrowExceptionIfNull<Range<TIndex, TValue>>(range, "range");
			TIndex tindex = range.Start;
			if (tindex.CompareTo(this.End) <= 0)
			{
				TIndex tindex2 = range.End;
				return tindex2.CompareTo(this.Start) >= 0;
			}
			return false;
		}

		public bool Equals(Range<TIndex, TValue> other)
		{
			if (other == null)
			{
				return false;
			}
			TIndex tindex = other.Start;
			if (tindex.Equals(this.Start))
			{
				TIndex tindex2 = other.End;
				return tindex2.Equals(this.End);
			}
			return false;
		}

		public override bool Equals(object obj)
		{
			Range<TIndex, TValue> other = obj as Range<TIndex, TValue>;
			return this.Equals(other);
		}

		public override int GetHashCode()
		{
			TIndex tindex = this.Start;
			int hashCode = tindex.GetHashCode();
			TIndex tindex2 = this.End;
			int hashCode2 = tindex2.GetHashCode();
			return TelerikHelper.CombineHashCodes(hashCode, hashCode2);
		}

		public override string ToString()
		{
			return string.Format("[{0}..{1}]", this.Start, this.End);
		}

		int IComparable<Range<TIndex, TValue>>.CompareTo(Range<TIndex, TValue> other)
		{
			return this.start.CompareTo(other.start);
		}

		internal Range<TIndex, TValue> CreateCopy()
		{
			return new Range<TIndex, TValue>(this.Start, this.End, this.IsDefault, this.Value);
		}

		TIndex start;

		TIndex end;

		readonly bool isDefault;
	}
}
