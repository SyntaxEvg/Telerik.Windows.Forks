using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	class ValuesSortConditionComparer : ValuesSortConditionComparerBase
	{
		public ValuesSortConditionComparer(bool isInverted)
		{
			this.isInverted = isInverted;
		}

		public override int Compare(SortValue x, SortValue y)
		{
			int num = base.Compare(x, y);
			if (this.isInverted && !(x.Value is EmptyCellValue) && !(y.Value is EmptyCellValue))
			{
				num *= -1;
			}
			if (num == 0)
			{
				num = x.Index.CompareTo(y.Index);
			}
			return num;
		}

		readonly bool isInverted;
	}
}
