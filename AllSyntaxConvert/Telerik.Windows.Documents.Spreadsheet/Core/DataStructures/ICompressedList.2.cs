using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	interface ICompressedList<T> : ICompressedList, ITranslateableCollection, IEnumerable<Range<long, T>>, IEnumerable
	{
		void Sort(long fromIndex, long toIndex, int[] sortedIndexes, Func<long, ValueBox<T>, ValueBox<T>> translateValue);

		void SetValue(long index, T value);

		void SetValue(long fromIndex, IEnumerable<T> values);

		void SetValue(long fromIndex, long toIndex, T value);

		ValueBox<T> GetValueInternal(long index);

		T GetValue(long index);

		IEnumerable<Range<long, T>> GetNonDefaultRanges();

		T GetDefaultValue();

		ICompressedList<T> GetValue(long fromIndex, long toIndex);

		void SetDefaultValue(T defaultValue);
	}
}
