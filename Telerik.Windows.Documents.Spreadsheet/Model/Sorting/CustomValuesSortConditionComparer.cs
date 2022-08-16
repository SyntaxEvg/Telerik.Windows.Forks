using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	class CustomValuesSortConditionComparer : ValuesSortConditionComparerBase
	{
		public CustomValuesSortConditionComparer(string[] customList, SortOrder sortOrder)
		{
			Guard.ThrowExceptionIfLessThan<int>(1, customList.Length, "customList");
			this.customList = customList;
			this.sortOrder = sortOrder;
		}

		public override int Compare(SortValue x, SortValue y)
		{
			ICellValue cellValue = x.Value as ICellValue;
			ICellValue cellValue2 = y.Value as ICellValue;
			string valueAsString = cellValue.GetValueAsString(CellValueFormat.GeneralFormat);
			string valueAsString2 = cellValue2.GetValueAsString(CellValueFormat.GeneralFormat);
			int num = Array.IndexOf<string>(this.customList, valueAsString);
			int num2 = Array.IndexOf<string>(this.customList, valueAsString2);
			if (num >= 0 && num2 >= 0)
			{
				int num3 = num - num2;
				num3 = ((this.sortOrder == SortOrder.Ascending) ? num3 : (num3 * -1));
				return (num3 == 0) ? x.Index.CompareTo(y.Index) : num3;
			}
			if (num >= 0 && num2 < 0)
			{
				int num4 = -1;
				return (this.sortOrder == SortOrder.Ascending) ? num4 : (num4 * -1);
			}
			if (num < 0 && num2 >= 0)
			{
				int num5 = 1;
				return (this.sortOrder == SortOrder.Ascending) ? num5 : (num5 * -1);
			}
			return base.Compare(x, y);
		}

		readonly string[] customList;

		readonly SortOrder sortOrder;
	}
}
