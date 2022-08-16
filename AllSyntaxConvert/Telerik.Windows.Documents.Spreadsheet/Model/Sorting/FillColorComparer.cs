using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	class FillColorComparer : IComparer<SortValue>
	{
		public FillColorComparer(IFill fill, SortOrder sortOrder)
		{
			this.fill = fill;
			this.sortOrder = sortOrder;
		}

		public int Compare(SortValue x, SortValue y)
		{
			IFill fill = x.Value as IFill;
			IFill fill2 = y.Value as IFill;
			int num = 0;
			if (!TelerikHelper.EqualsOfT<IFill>(fill, fill2) && fill != null && fill2 != null)
			{
				if (fill.Equals(this.fill))
				{
					num = -1;
				}
				else if (fill2.Equals(this.fill))
				{
					num = 1;
				}
			}
			num = ((this.sortOrder == SortOrder.Ascending) ? num : (num * -1));
			return (num == 0) ? x.Index.CompareTo(y.Index) : num;
		}

		readonly IFill fill;

		readonly SortOrder sortOrder;
	}
}
