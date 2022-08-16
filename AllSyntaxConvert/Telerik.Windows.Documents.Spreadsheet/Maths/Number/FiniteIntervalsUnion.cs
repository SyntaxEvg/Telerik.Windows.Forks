using System;
using System.Collections.Generic;
using System.Text;

namespace Telerik.Windows.Documents.Spreadsheet.Maths.Number
{
	class FiniteIntervalsUnion<T> where T : IComparable
	{
		public FiniteIntervalsUnion()
		{
			this.sortedIntervals = new List<FiniteInterval<T>>();
		}

		public IEnumerable<T> IntervalEndPointValues
		{
			get
			{
				foreach (FiniteInterval<T> interval in this.sortedIntervals)
				{
					yield return interval.Start;
					yield return interval.End;
				}
				yield break;
			}
		}

		public IEnumerable<FiniteInterval<T>> Intervals
		{
			get
			{
				foreach (FiniteInterval<T> interval in this.sortedIntervals)
				{
					yield return interval;
				}
				yield break;
			}
		}

		public int IntervalsCount
		{
			get
			{
				return this.sortedIntervals.Count;
			}
		}

		public bool IsEmpty
		{
			get
			{
				return this.sortedIntervals.Count == 0;
			}
		}

		public void Clear()
		{
			this.sortedIntervals.Clear();
		}

		public FiniteInterval<T> GetContainingInterval(T point)
		{
			int num;
			return this.TryGetContainingIntervalOrSmallerIntervalIndex(point, out num);
		}

		public void AddInterval(T start, T end, bool isStartOpened = false, bool isEndOpened = false)
		{
			if (start.CompareTo(end) > 0)
			{
				throw new ArgumentException("Interval start have value less than or equal to interval end value!");
			}
			if (start.CompareTo(end) == 0)
			{
				if (isStartOpened && isEndOpened)
				{
					return;
				}
				isStartOpened = false;
				isEndOpened = false;
			}
			int num;
			T start2;
			bool isStartOpened2;
			this.CalculateIntervalsStartChanges(start, isStartOpened, out num, out start2, out isStartOpened2);
			int num2;
			T end2;
			bool isEndOpened2;
			this.CalculateIntervalsEndChanges(end, isEndOpened, out num2, out end2, out isEndOpened2);
			if (num <= num2)
			{
				int count = num2 - num + 1;
				this.sortedIntervals.RemoveRange(num, count);
			}
			FiniteInterval<T> item = new FiniteInterval<T>(start2, end2, isStartOpened2, isEndOpened2);
			this.sortedIntervals.Insert(num, item);
		}

		void CalculateIntervalsStartChanges(T startPoint, bool isStartOpened, out int startIndexToRemove, out T insertionIntervalStartPoint, out bool insertionStartIsOpened)
		{
			int num;
			FiniteInterval<T> finiteInterval = this.TryGetContainingIntervalOrSmallerIntervalIndex(startPoint, out num);
			startIndexToRemove = num + 1;
			insertionIntervalStartPoint = startPoint;
			insertionStartIsOpened = isStartOpened;
			if (finiteInterval == null)
			{
				if (num > -1)
				{
					FiniteInterval<T> finiteInterval2 = this.sortedIntervals[num];
					bool flag;
					if (!isStartOpened)
					{
						T end = finiteInterval2.End;
						flag = end.CompareTo(startPoint) == 0;
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					if (flag2)
					{
						startIndexToRemove = num;
						insertionIntervalStartPoint = finiteInterval2.Start;
						insertionStartIsOpened = finiteInterval2.IsStartOpened;
						return;
					}
				}
			}
			else
			{
				startIndexToRemove = num;
				insertionIntervalStartPoint = finiteInterval.Start;
				insertionStartIsOpened = finiteInterval.IsStartOpened;
			}
		}

		void CalculateIntervalsEndChanges(T endPoint, bool isEndOpened, out int endIndexToRemove, out T insertionIntervalEndPoint, out bool insertionEndIsOpened)
		{
			int num;
			FiniteInterval<T> finiteInterval = this.TryGetContainingIntervalOrSmallerIntervalIndex(endPoint, out num);
			endIndexToRemove = num;
			insertionIntervalEndPoint = endPoint;
			insertionEndIsOpened = isEndOpened;
			if (finiteInterval == null)
			{
				int num2 = num + 1;
				if (num2 < this.sortedIntervals.Count)
				{
					FiniteInterval<T> finiteInterval2 = this.sortedIntervals[num2];
					bool flag;
					if (!isEndOpened)
					{
						T start = finiteInterval2.Start;
						flag = start.CompareTo(endPoint) == 0;
					}
					else
					{
						flag = false;
					}
					bool flag2 = flag;
					if (flag2)
					{
						endIndexToRemove = num2;
						insertionIntervalEndPoint = finiteInterval2.End;
						insertionEndIsOpened = finiteInterval2.IsEndOpened;
						return;
					}
				}
			}
			else
			{
				endIndexToRemove = num;
				insertionIntervalEndPoint = finiteInterval.End;
				insertionEndIsOpened = finiteInterval.IsEndOpened;
			}
		}

		FiniteInterval<T> TryGetContainingIntervalOrSmallerIntervalIndex(T point, out int smallerOrEqualIntervalIndex)
		{
			smallerOrEqualIntervalIndex = -1;
			if (this.sortedIntervals.Count == 0)
			{
				return null;
			}
			int i = 0;
			int num = this.sortedIntervals.Count - 1;
			while (i < num)
			{
				int num2 = (i + num) / 2;
				int num3 = this.sortedIntervals[num2].CompareTo(point);
				if (num3 > 0)
				{
					num = num2 - 1;
					smallerOrEqualIntervalIndex = num;
				}
				else
				{
					if (num3 >= 0)
					{
						smallerOrEqualIntervalIndex = num2;
						return this.sortedIntervals[num2];
					}
					i = num2 + 1;
				}
			}
			int num4 = i;
			int num5 = this.sortedIntervals[num4].CompareTo(point);
			smallerOrEqualIntervalIndex = ((num5 > 0) ? (num4 - 1) : num4);
			if (num5 != 0)
			{
				return null;
			}
			return this.sortedIntervals[num4];
		}

		public override string ToString()
		{
			if (this.IsEmpty)
			{
				return "Ø";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.sortedIntervals[0].ToString());
			for (int i = 1; i < this.sortedIntervals.Count; i++)
			{
				stringBuilder.AppendFormat(" {0} {1}", "∪", this.sortedIntervals[i]);
			}
			return stringBuilder.ToString();
		}

		public override bool Equals(object obj)
		{
			FiniteIntervalsUnion<T> finiteIntervalsUnion = obj as FiniteIntervalsUnion<T>;
			if (finiteIntervalsUnion == null)
			{
				return false;
			}
			int count = this.sortedIntervals.Count;
			if (!finiteIntervalsUnion.sortedIntervals.Count.Equals(count))
			{
				return false;
			}
			for (int i = 0; i < count; i++)
			{
				if (!this.sortedIntervals[i].Equals(finiteIntervalsUnion.sortedIntervals[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override int GetHashCode()
		{
			if (this.IsEmpty)
			{
				return 0;
			}
			int num = this.sortedIntervals[0].GetHashCode();
			for (int i = 1; i < this.sortedIntervals.Count; i++)
			{
				num ^= this.sortedIntervals[i].GetHashCode();
			}
			return num;
		}

		const string EmptySetSymbol = "Ø";

		const string UnionSymbol = "∪";

		readonly List<FiniteInterval<T>> sortedIntervals;
	}
}
