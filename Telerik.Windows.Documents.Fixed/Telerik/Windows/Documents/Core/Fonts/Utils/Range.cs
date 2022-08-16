using System;

namespace Telerik.Windows.Documents.Core.Fonts.Utils
{
	class Range
	{
		public Range()
		{
			this.RangeStart = (this.RangeEnd = 0);
		}

		public Range(int start, int end)
		{
			this.RangeStart = start;
			this.RangeEnd = end;
		}

		public int RangeStart { get; set; }

		public int RangeEnd { get; set; }

		public bool IsInRange(int value)
		{
			return this.RangeStart <= value && value <= this.RangeEnd;
		}
	}
}
