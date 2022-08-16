using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	class CompressedList<TValue> : ICompressedList<TValue>, ICompressedList, ITranslateableCollection, IEnumerable<Range<long, TValue>>, IEnumerable
	{
		public long FromIndex
		{
			get
			{
				return this.fromIndex;
			}
		}

		public long ToIndex
		{
			get
			{
				return this.toIndex;
			}
		}

		public long RangeLength
		{
			get
			{
				return this.toIndex - this.fromIndex + 1L;
			}
		}

		public CompressedList(long fromIndex, long toIndex, TValue defaultValue)
		{
			Guard.ThrowExceptionIfLessThan<long>(fromIndex, toIndex, "toIndex");
			this.defaultValue = new ValueBox<TValue>(defaultValue);
			this.fromIndex = fromIndex;
			this.toIndex = toIndex;
			this.rangesTree = new RangeTreeCollection<long, ValueBox<TValue>>();
			this.rangesTree.Add(new Range<long, ValueBox<TValue>>(fromIndex, toIndex, true, null));
		}

		public CompressedList(long fromIndex, long toIndex)
			: this(fromIndex, toIndex, default(TValue))
		{
		}

		internal CompressedList(CompressedList<TValue> original)
			: this(original.FromIndex, original.ToIndex, original.defaultValue.Value)
		{
		}

		internal CompressedList(ICompressedList<TValue> original)
			: this(original.FromIndex, original.ToIndex, original.GetDefaultValue())
		{
		}

		public void SetDefaultValue(TValue defaultValue)
		{
			this.defaultValue = new ValueBox<TValue>(defaultValue);
		}

		public TValue GetDefaultValue()
		{
			return this.defaultValue.Value;
		}

		ICompressedList ICompressedList.Offset(long delta)
		{
			return this.Offset(delta);
		}

		public CompressedList<TValue> Offset(long delta)
		{
			long num = this.FromIndex + delta;
			long num2 = this.ToIndex + delta;
			CompressedList<TValue> compressedList = new CompressedList<TValue>(num, num2, this.GetDefaultValue());
			((ITranslateableCollection)this).Copy(this.FromIndex, this.RangeLength, compressedList, num);
			return compressedList;
		}

		public void SetValue(long index, TValue value)
		{
			this.SetValue(index, index, value);
		}

		public void SetValue(long fromIndex, IEnumerable<TValue> values)
		{
			Guard.ThrowExceptionIfNull<IEnumerable<TValue>>(values, "values");
			TValue[] array = values.ToArray<TValue>();
			this.ValidatePosition(fromIndex);
			this.ValidatePosition(fromIndex + (long)array.Length - 1L);
			long num = 0L;
			for (long num2 = 0L; num2 < (long)array.Length; num2 += 1L)
			{
				if (!checked(TelerikHelper.EqualsOfT<TValue>(array[(int)((IntPtr)num)], array[(int)((IntPtr)num2)])))
				{
					this.SetValue(num + fromIndex, num2 + fromIndex, array[(int)(checked((IntPtr)num))]);
					num = num2;
				}
			}
			this.SetValue(num + fromIndex, (long)(array.Length - 1) + fromIndex, array[(int)(checked((IntPtr)num))]);
		}

		public void SetValue(long fromIndex, long toIndex, TValue value)
		{
			this.SetValueInternal(fromIndex, toIndex, new ValueBox<TValue>(value));
		}

		public void SetValue(CompressedList<TValue> compressedList)
		{
			foreach (Range<long, ValueBox<TValue>> range in compressedList.rangesTree)
			{
				this.SetValueInternal(range.Start, range.End, range.Value);
			}
		}

		void SetValueInternal(long index, ValueBox<TValue> value)
		{
			if (!TelerikHelper.EqualsOfT<ValueBox<TValue>>(this.GetValueInternal(index), value))
			{
				this.SetValueInternal(index, index, value);
			}
		}

		void SetValueInternal(long fromIndex, long toIndex, ValueBox<TValue> value)
		{
			Guard.ThrowExceptionIfLessThan<long>(fromIndex, toIndex, "toIndex");
			this.ValidatePosition(fromIndex);
			this.ValidatePosition(toIndex);
			bool isDefault = TelerikHelper.EqualsOfT<ValueBox<TValue>>(CompressedList<TValue>.DefaultValueMarker, value);
			List<Range<long, ValueBox<TValue>>> intersectingRanges = this.rangesTree.GetIntersectingRanges(new Range<long, ValueBox<TValue>>(Math.Max(0L, fromIndex - 1L), toIndex + 1L, isDefault, value));
			if (intersectingRanges.Count > 0 && intersectingRanges.First<Range<long, ValueBox<TValue>>>().End.CompareTo(fromIndex) < 0)
			{
				Range<long, ValueBox<TValue>> range = intersectingRanges.First<Range<long, ValueBox<TValue>>>();
				if ((value == null && range.Value == null) || (range.Value != null && range.Value.Equals(value)))
				{
					fromIndex = range.Start;
				}
				else
				{
					intersectingRanges.RemoveAt(0);
				}
			}
			if (intersectingRanges.Count > 0 && intersectingRanges.Last<Range<long, ValueBox<TValue>>>().Start.CompareTo(toIndex) > 0)
			{
				Range<long, ValueBox<TValue>> range2 = intersectingRanges.Last<Range<long, ValueBox<TValue>>>();
				if ((value == null && range2.Value == null) || (range2.Value != null && range2.Value.Equals(value)))
				{
					toIndex = range2.End;
				}
				else
				{
					intersectingRanges.RemoveAt(intersectingRanges.Count - 1);
				}
			}
			foreach (Range<long, ValueBox<TValue>> range3 in intersectingRanges)
			{
				this.rangesTree.Delete(range3);
				if (range3.Start.CompareTo(fromIndex) < 0)
				{
					if (!TelerikHelper.EqualsOfT<ValueBox<TValue>>(range3.Value, value))
					{
						Range<long, ValueBox<TValue>> value2 = new Range<long, ValueBox<TValue>>(range3.Start, fromIndex - 1L, range3.IsDefault, range3.Value);
						this.rangesTree.Add(value2);
					}
					else
					{
						fromIndex = range3.Start;
					}
				}
				if (range3.End.CompareTo(toIndex) > 0)
				{
					if (!TelerikHelper.EqualsOfT<ValueBox<TValue>>(range3.Value, value))
					{
						Range<long, ValueBox<TValue>> value3 = new Range<long, ValueBox<TValue>>(toIndex + 1L, range3.End, range3.IsDefault, range3.Value);
						this.rangesTree.Add(value3);
					}
					else
					{
						toIndex = range3.End;
					}
				}
			}
			isDefault = TelerikHelper.EqualsOfT<ValueBox<TValue>>(CompressedList<TValue>.DefaultValueMarker, value);
			Range<long, ValueBox<TValue>> value4 = new Range<long, ValueBox<TValue>>(fromIndex, toIndex, isDefault, value);
			this.rangesTree.Add(value4);
			this.ValidateStructure();
		}

		public TValue GetValue(long index)
		{
			ValueBox<TValue> valueInternal = this.GetValueInternal(index);
			if (TelerikHelper.EqualsOfT<ValueBox<TValue>>(CompressedList<TValue>.DefaultValueMarker, valueInternal))
			{
				return this.defaultValue.Value;
			}
			return valueInternal.Value;
		}

		public CompressedList<TValue> GetValue(long fromIndex, long toIndex)
		{
			Guard.ThrowExceptionIfLessThan<long>(fromIndex, toIndex, "toIndex");
			this.ValidatePosition(fromIndex);
			this.ValidatePosition(toIndex);
			CompressedList<TValue> compressedList = new CompressedList<TValue>(fromIndex, toIndex, this.defaultValue.Value);
			List<Range<long, ValueBox<TValue>>> intersectingRanges = this.rangesTree.GetIntersectingRanges(new Range<long, ValueBox<TValue>>(fromIndex, toIndex, true, new ValueBox<TValue>(default(TValue))));
			Range<long, ValueBox<TValue>> range = intersectingRanges.First<Range<long, ValueBox<TValue>>>();
			compressedList.SetValueInternal(TelerikHelper.Max<long>(fromIndex, range.Start), TelerikHelper.Min<long>(toIndex, range.End), range.Value);
			intersectingRanges.RemoveAt(0);
			if (intersectingRanges.Count > 0)
			{
				Range<long, ValueBox<TValue>> range2 = intersectingRanges.Last<Range<long, ValueBox<TValue>>>();
				compressedList.SetValueInternal(range2.Start, TelerikHelper.Min<long>(toIndex, range2.End), range2.Value);
				intersectingRanges.RemoveAt(intersectingRanges.Count - 1);
			}
			foreach (Range<long, ValueBox<TValue>> range3 in intersectingRanges)
			{
				compressedList.SetValueInternal(range3.Start, range3.End, range3.Value);
			}
			return compressedList;
		}

		ICompressedList<TValue> ICompressedList<TValue>.GetValue(long fromIndex, long toIndex)
		{
			return this.GetValue(fromIndex, toIndex);
		}

		public ValueBox<TValue> GetValueInternal(long index)
		{
			this.ValidatePosition(index);
			Range<long, ValueBox<TValue>> containingRange = this.rangesTree.GetContainingRange(index);
			return containingRange.Value;
		}

		public void ClearValue(long index)
		{
			this.ClearValue(index, index);
		}

		public void ClearValue(long fromIndex, long toIndex)
		{
			this.SetValueInternal(fromIndex, toIndex, CompressedList<TValue>.DefaultValueMarker);
		}

		public IEnumerable<Range<long, TValue>> GetNonDefaultRanges()
		{
			foreach (Range<long, ValueBox<TValue>> range in this.rangesTree)
			{
				if (!TelerikHelper.EqualsOfT<ValueBox<TValue>>(CompressedList<TValue>.DefaultValueMarker, range.Value))
				{
					yield return new Range<long, TValue>(range.Start, range.End, range.IsDefault, range.Value.Value);
				}
			}
			yield break;
		}

		public IEnumerable<LongRange> GetRanges(bool getDefaultRanges)
		{
			foreach (Range<long, ValueBox<TValue>> range in this.rangesTree)
			{
				bool isDefault = TelerikHelper.EqualsOfT<ValueBox<TValue>>(CompressedList<TValue>.DefaultValueMarker, range.Value);
				if ((!getDefaultRanges && !isDefault) || (getDefaultRanges && isDefault))
				{
					yield return new LongRange(range.Start, range.End);
				}
			}
			yield break;
		}

		public IEnumerator<Range<long, TValue>> GetEnumerator()
		{
			foreach (Range<long, ValueBox<TValue>> range in this.rangesTree)
			{
				if (TelerikHelper.EqualsOfT<ValueBox<TValue>>(CompressedList<TValue>.DefaultValueMarker, range.Value))
				{
					yield return new Range<long, TValue>(range.Start, range.End, true, this.defaultValue.Value);
				}
				else
				{
					yield return new Range<long, TValue>(range.Start, range.End, false, range.Value.Value);
				}
			}
			yield break;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<Range<long, TValue>>)this).GetEnumerator();
		}

		void ITranslateableCollection.Copy(long fromIndex, long toIndex)
		{
			ValueBox<TValue> valueInternal = this.GetValueInternal(fromIndex);
			this.SetValueInternal(toIndex, valueInternal);
		}

		void ITranslateableCollection.Copy(long fromIndex, long itemsCount, long toIndex)
		{
			((ITranslateableCollection)this).Copy(fromIndex, itemsCount, this, toIndex);
		}

		void ITranslateableCollection.Copy(long fromIndex, long itemsCount, ITranslateableCollection toCollection, long toIndex)
		{
			CompressedList<TValue> compressedList = (CompressedList<TValue>)toCollection;
			List<Range<long, ValueBox<TValue>>> intersectingRanges = this.rangesTree.GetIntersectingRanges(new Range<long, ValueBox<TValue>>(fromIndex, fromIndex + itemsCount - 1L, true, null));
			long num = toIndex - fromIndex;
			foreach (Range<long, ValueBox<TValue>> range in intersectingRanges)
			{
				long num2 = Math.Max(fromIndex, range.Start);
				long num3 = System.Math.Min(fromIndex + itemsCount - 1L, range.End);
				compressedList.SetValueInternal(num2 + num, num3 + num, range.Value);
			}
		}

		void ITranslateableCollection.Clear(long index)
		{
			this.ClearValue(index);
		}

		void ITranslateableCollection.Clear(long fromIndex, long toIndex)
		{
			this.ClearValue(fromIndex, toIndex);
		}

		void ITranslateableCollection.Translate(long fromIndex, long toIndex, long offset, bool useSameValueAsPrevious)
		{
			Guard.ThrowExceptionIfLessThan<long>(fromIndex, toIndex, "toIndex");
			this.ValidatePosition(fromIndex);
			this.ValidatePosition(toIndex);
			if (offset == 0L)
			{
				return;
			}
			Range<long, ValueBox<TValue>> rangeForTranslation = new Range<long, ValueBox<TValue>>(fromIndex, toIndex, true, null);
			List<Range<long, ValueBox<TValue>>> ranges = this.rangesTree.GetIntersectingRanges(new Range<long, ValueBox<TValue>>(fromIndex, toIndex, true, null));
			Range<long, ValueBox<TValue>> range6 = ranges.First<Range<long, ValueBox<TValue>>>();
			if (range6.Start < fromIndex)
			{
				Range<long, ValueBox<TValue>> value = new Range<long, ValueBox<TValue>>(range6.Start, fromIndex - 1L, range6.IsDefault, range6.Value);
				range6.Start = fromIndex;
				this.rangesTree.Add(value);
			}
			Range<long, ValueBox<TValue>> range2 = ranges.Last<Range<long, ValueBox<TValue>>>();
			if (range2.End > toIndex)
			{
				Range<long, ValueBox<TValue>> value2 = new Range<long, ValueBox<TValue>>(toIndex + 1L, range2.End, range2.IsDefault, range2.Value);
				range2.End = toIndex;
				this.rangesTree.Add(value2);
			}
			Action<Range<long, ValueBox<TValue>>> action = delegate(Range<long, ValueBox<TValue>> range)
			{
				Range<long, ValueBox<TValue>> range7 = new Range<long, ValueBox<TValue>>(range.Start + offset, range.End + offset, range.IsDefault, range.Value);
				if (!rangeForTranslation.IntersectsWith(range7))
				{
					this.rangesTree.Delete(range);
					ranges.Remove(range);
					return;
				}
				range.Start = Math.Max(fromIndex, range7.Start);
				range.End = System.Math.Min(range7.End, toIndex);
			};
			Range<long, ValueBox<TValue>>[] array = ranges.ToArray();
			if (offset > 0L)
			{
				for (int i = array.Length - 1; i >= 0; i--)
				{
					action(array[i]);
				}
			}
			else
			{
				for (int j = 0; j < array.Length; j++)
				{
					action(array[j]);
				}
			}
			Range<long, ValueBox<TValue>> containingRange = this.rangesTree.GetContainingRange(fromIndex - 1L);
			Range<long, ValueBox<TValue>> containingRange2 = this.rangesTree.GetContainingRange(toIndex + 1L);
			ValueBox<TValue> value3 = CompressedList<TValue>.DefaultValueMarker;
			bool isDefault = true;
			if (useSameValueAsPrevious && containingRange != null)
			{
				value3 = containingRange.Value;
				isDefault = false;
			}
			if (ranges.Count == 0)
			{
				Range<long, ValueBox<TValue>> range3 = new Range<long, ValueBox<TValue>>(fromIndex, toIndex, isDefault, value3);
				ranges.Add(range3);
				this.rangesTree.Add(range3);
			}
			else if (offset > 0L)
			{
				Range<long, ValueBox<TValue>> range3 = new Range<long, ValueBox<TValue>>(fromIndex, ranges[0].Start - 1L, isDefault, value3);
				if (TelerikHelper.EqualsOfT<ValueBox<TValue>>(ranges[0].Value, range3.Value))
				{
					ranges[0].Start = range3.Start;
				}
				else
				{
					ranges.Insert(0, range3);
					this.rangesTree.Add(range3);
				}
			}
			else
			{
				Range<long, ValueBox<TValue>> range3 = new Range<long, ValueBox<TValue>>(ranges.Last<Range<long, ValueBox<TValue>>>().End + 1L, toIndex, isDefault, value3);
				if (TelerikHelper.EqualsOfT<ValueBox<TValue>>(ranges.Last<Range<long, ValueBox<TValue>>>().Value, range3.Value))
				{
					ranges.Last<Range<long, ValueBox<TValue>>>().End = range3.End;
				}
				else
				{
					ranges.Add(range3);
					this.rangesTree.Add(range3);
				}
			}
			Range<long, ValueBox<TValue>> range4 = ranges.FirstOrDefault<Range<long, ValueBox<TValue>>>();
			Range<long, ValueBox<TValue>> range5 = ranges.LastOrDefault<Range<long, ValueBox<TValue>>>();
			if (range4 != null)
			{
				containingRange = this.rangesTree.GetContainingRange(range4.Start - 1L);
				if (containingRange != null && TelerikHelper.EqualsOfT<ValueBox<TValue>>(containingRange.Value, range4.Value))
				{
					this.rangesTree.Delete(containingRange);
					range4.Start = containingRange.Start;
				}
			}
			if (range5 != null)
			{
				containingRange2 = this.rangesTree.GetContainingRange(range5.End + 1L);
				if (containingRange2 != null && TelerikHelper.EqualsOfT<ValueBox<TValue>>(containingRange2.Value, range5.Value))
				{
					this.rangesTree.Delete(containingRange2);
					range5.End = containingRange2.End;
				}
			}
			this.ValidateStructure();
		}

		void ICompressedList.Sort(long fromIndex, long toIndex, int[] sortedIndexes)
		{
			this.Sort(fromIndex, toIndex, sortedIndexes, (long newIndex, ValueBox<TValue> value) => value);
		}

		void ICompressedList<TValue>.Sort(long fromIndex, long toIndex, int[] sortedIndexes, Func<long, ValueBox<TValue>, ValueBox<TValue>> translateValue)
		{
			this.Sort(fromIndex, toIndex, sortedIndexes, translateValue);
		}

		ICompressedList ICompressedList.GetValue(long fromIndex, long toIndex)
		{
			return this.GetValue(fromIndex, toIndex);
		}

		bool ICompressedList.ContainsNonDefaultValues(long fromIndex, long toIndex)
		{
			List<Range<long, ValueBox<TValue>>> intersectingRanges = this.rangesTree.GetIntersectingRanges(new Range<long, ValueBox<TValue>>(fromIndex, toIndex, true, CompressedList<TValue>.DefaultValueMarker));
			foreach (Range<long, ValueBox<TValue>> range in intersectingRanges)
			{
				if (!TelerikHelper.EqualsOfT<ValueBox<TValue>>(range.Value, CompressedList<TValue>.DefaultValueMarker))
				{
					return true;
				}
			}
			return false;
		}

		void ICompressedList.SetValue(ICompressedList compressedList)
		{
			this.SetValue((CompressedList<TValue>)compressedList);
		}

		void ICompressedList.SetValue(long fromIndex, long toIndex, ICompressedList singleValueCompressedList)
		{
			long param = singleValueCompressedList.ToIndex - singleValueCompressedList.FromIndex;
			Guard.ThrowExceptionIfGreaterThan<long>(0L, param, "count");
			CompressedList<TValue> compressedList = (CompressedList<TValue>)singleValueCompressedList;
			TValue value = compressedList.GetValue(compressedList.FromIndex);
			this.SetValue(fromIndex, toIndex, value);
		}

		void ICompressedList.SetDefaultValue(long fromIndex, long toIndex)
		{
			this.SetValue(fromIndex, toIndex, this.GetDefaultValue());
		}

		long? ICompressedList.GetLastNonEmptyRangeEnd(long fromIndex, long toIndex)
		{
			List<Range<long, ValueBox<TValue>>> intersectingRanges = this.rangesTree.GetIntersectingRanges(new Range<long, ValueBox<TValue>>(fromIndex, toIndex, true, CompressedList<TValue>.DefaultValueMarker));
			intersectingRanges.Reverse();
			foreach (Range<long, ValueBox<TValue>> range in intersectingRanges)
			{
				if (!TelerikHelper.EqualsOfT<ValueBox<TValue>>(range.Value, CompressedList<TValue>.DefaultValueMarker))
				{
					return new long?(Math.Min(range.End, toIndex));
				}
			}
			return null;
		}

		ICompressedList ICompressedList.CreateInstance(long fromIndex, long toIndex)
		{
			return new CompressedList<TValue>(fromIndex, toIndex, this.GetDefaultValue());
		}

		void Sort(long fromLongIndex, long toLongIndex, int[] sortedIndexes, Func<long, ValueBox<TValue>, ValueBox<TValue>> translateValue)
		{
			ValueBox<TValue>[] array = new ValueBox<TValue>[sortedIndexes.Length];
			for (long num = fromLongIndex; num <= toLongIndex; num += 1L)
			{
				array[(int)(checked((IntPtr)(unchecked(num - fromLongIndex))))] = this.GetValueInternal(num);
			}
			for (long num2 = 0L; num2 < (long)sortedIndexes.Length; num2 += 1L)
			{
				long num3 = num2 + fromLongIndex;
				ValueBox<TValue> arg = array[sortedIndexes[(int)(checked((IntPtr)num2))]];
				ValueBox<TValue> value = translateValue(num3, arg);
				this.SetValueInternal(num3, value);
			}
		}

		void ValidatePosition(long position)
		{
			if (position < this.fromIndex && this.toIndex < position)
			{
				throw new IndexOutOfRangeException("position");
			}
		}

		void ValidateStructure()
		{
		}

		public override string ToString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Range<long, ValueBox<TValue>> range in this.rangesTree)
			{
				stringBuilder.Append(string.Format("{0}->{1}\n", range, range.Value));
			}
			return stringBuilder.ToString();
		}

		static readonly ValueBox<TValue> DefaultValueMarker;

		ValueBox<TValue> defaultValue;

		readonly long fromIndex;

		readonly long toIndex;

		readonly RangeTreeCollection<long, ValueBox<TValue>> rangesTree;
	}
}
