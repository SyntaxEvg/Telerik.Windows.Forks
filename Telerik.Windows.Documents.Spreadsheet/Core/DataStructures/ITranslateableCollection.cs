using System;

namespace Telerik.Windows.Documents.Spreadsheet.Core.DataStructures
{
	interface ITranslateableCollection
	{
		void Translate(long fromIndex, long toIndex, long offset, bool useSameValueAsPrevious);

		void Copy(long fromIndex, long toIndex);

		void Copy(long fromIndex, long itemsCount, ITranslateableCollection toCollection, long toIndex);

		void Copy(long fromIndex, long itemsCount, long toIndex);

		void Clear(long index);

		void Clear(long fromIndex, long toIndex);
	}
}
