using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public interface ISortCondition
	{
		int RelativeIndex { get; }

		IComparer<SortValue> Comparer { get; }

		object GetValue(Cells cells, int rowIndex, int columnIndex);
	}
}
