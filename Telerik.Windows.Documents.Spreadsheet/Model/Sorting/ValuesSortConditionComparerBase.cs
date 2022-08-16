using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public abstract class ValuesSortConditionComparerBase : IComparer<SortValue>
	{
		public virtual int Compare(SortValue x, SortValue y)
		{
			ICellValue cellValue = x.Value as ICellValue;
			ICellValue cellValue2 = y.Value as ICellValue;
			Guard.ThrowExceptionIfNull<ICellValue>(cellValue, "value1");
			Guard.ThrowExceptionIfNull<ICellValue>(cellValue2, "value2");
			return cellValue.CompareTo(cellValue2);
		}
	}
}
