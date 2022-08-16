using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	public class Range
	{
		public int Start
		{
			get
			{
				return this.start;
			}
		}

		public int End
		{
			get
			{
				return this.end;
			}
		}

		public long Length
		{
			get
			{
				return (long)(this.end - this.start + 1);
			}
		}

		public Range(int start, int end)
		{
			Guard.ThrowExceptionIfLessThan<int>(start, end, "end");
			this.start = start;
			this.end = end;
		}

		public Range Expand(int index)
		{
			return new Range(Math.Min(index, this.Start), Math.Max(index, this.End));
		}

		public static Range CreateOrExpand(Range range, int index)
		{
			if (range == null)
			{
				return new Range(index, index);
			}
			return range.Expand(index);
		}

		public static Range MaxOrNull(Range firstRange, Range secondRange)
		{
			if (firstRange == null)
			{
				return secondRange;
			}
			if (secondRange == null)
			{
				return firstRange;
			}
			return Range.Max(firstRange, secondRange);
		}

		public static Range Max(Range firstRange, Range secondRange)
		{
			Guard.ThrowExceptionIfNull<Range>(firstRange, "firstRange");
			Guard.ThrowExceptionIfNull<Range>(secondRange, "secondRange");
			return new Range(Math.Min(firstRange.Start, secondRange.Start), Math.Max(firstRange.End, secondRange.end));
		}

		public override string ToString()
		{
			return string.Format("[{0}, {1}]", this.Start, this.End);
		}

		readonly int start;

		readonly int end;
	}
}
