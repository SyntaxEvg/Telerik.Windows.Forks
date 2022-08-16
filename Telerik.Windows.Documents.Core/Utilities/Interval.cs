using System;
using System.Globalization;

namespace Telerik.Windows.Documents.Utilities
{
	class Interval
	{
		public Interval(double start, double end)
		{
			Guard.ThrowExceptionIfTrue(start > end, "start cannot be bigger than end!");
			this.start = start;
			this.end = end;
		}

		public double Start
		{
			get
			{
				return this.start;
			}
		}

		public double End
		{
			get
			{
				return this.end;
			}
		}

		public override bool Equals(object obj)
		{
			Interval interval = obj as Interval;
			return interval != null && this.Start.Equals(interval.Start) && this.End.Equals(interval.End);
		}

		public override int GetHashCode()
		{
			return ObjectExtensions.CombineHashCodes(this.Start.GetHashCode(), this.End.GetHashCode());
		}

		public override string ToString()
		{
			return string.Format("[{0}{1}{2}]", this.Start, CultureInfo.CurrentCulture.TextInfo.ListSeparator, this.End);
		}

		readonly double start;

		readonly double end;
	}
}
