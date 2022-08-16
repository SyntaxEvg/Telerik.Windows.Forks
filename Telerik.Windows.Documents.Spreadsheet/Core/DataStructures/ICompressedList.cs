using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	interface ICompressedList : ITranslateableCollection, IEnumerable
	{
		bool ContainsNonDefaultValues(long start, long end);

		void Sort(long fromIndex, long toIndex, int[] sortedIndexes);

		long FromIndex { get; }

		long ToIndex { get; }

		void SetValue(ICompressedList compressedList);

		void SetValue(long fromIndex, long toIndex, ICompressedList singleValueCompressedList);

		void SetDefaultValue(long fromIndex, long toIndex);

		IEnumerable<LongRange> GetRanges(bool getDefaultRanges);

		long? GetLastNonEmptyRangeEnd(long fromIndex, long toIndex);

		ICompressedList Offset(long delta);

		void ClearValue(long index);

		void ClearValue(long fromIndex, long toIndex);

		ICompressedList GetValue(long fromIndex, long toIndex);

		ICompressedList CreateInstance(long fromIndex, long toIndex);
	}
}
