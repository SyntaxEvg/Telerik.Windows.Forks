using System;

namespace Telerik.Windows.Documents.Spreadsheet.Maths.Number
{
	class FiniteInterval<T> where T : IComparable
	{
		public T Start { get; set; }

		public T End { get; set; }

		public bool IsStartOpened { get; set; }

		public bool IsEndOpened { get; set; }

		public FiniteInterval(T start, T end, bool isStartOpened = false, bool isEndOpened = false)
		{
			if (start.CompareTo(end) > 0)
			{
				throw new ArgumentException("Interval start value must be less than or equal to interval end value!");
			}
			this.Start = start;
			this.IsStartOpened = isStartOpened;
			this.End = end;
			this.IsEndOpened = isEndOpened;
		}

		public bool Contains(T value)
		{
			return this.CompareTo(value) == 0;
		}

		public int CompareTo(T value)
		{
			bool flag = (this.IsStartOpened ? (value.CompareTo(this.Start) <= 0) : (value.CompareTo(this.Start) < 0));
			if (flag)
			{
				return 1;
			}
			bool flag2 = (this.IsEndOpened ? (value.CompareTo(this.End) >= 0) : (value.CompareTo(this.End) > 0));
			if (flag2)
			{
				return -1;
			}
			return 0;
		}

		public override string ToString()
		{
			Func<bool, bool, string> func = delegate(bool isOpened, bool isLeft)
			{
				if (isOpened)
				{
					if (!isLeft)
					{
						return ")";
					}
					return "(";
				}
				else
				{
					if (!isLeft)
					{
						return "]";
					}
					return "[";
				}
			};
			return string.Format("{0}{1};{2}{3}", new object[]
			{
				func(this.IsStartOpened, true),
				this.Start,
				this.End,
				func(this.IsEndOpened, false)
			});
		}

		public override bool Equals(object obj)
		{
			FiniteInterval<T> finiteInterval = obj as FiniteInterval<T>;
			if (finiteInterval == null)
			{
				return false;
			}
			T start = this.Start;
			if (start.Equals(finiteInterval.Start))
			{
				T end = this.End;
				if (end.Equals(finiteInterval.End) && this.IsStartOpened.Equals(finiteInterval.IsStartOpened))
				{
					return this.IsEndOpened.Equals(finiteInterval.IsEndOpened);
				}
			}
			return false;
		}

		public override int GetHashCode()
		{
			T start = this.Start;
			int hashCode = start.GetHashCode();
			T end = this.End;
			return hashCode ^ end.GetHashCode() ^ this.IsStartOpened.GetHashCode() ^ this.IsEndOpened.GetHashCode();
		}
	}
}
