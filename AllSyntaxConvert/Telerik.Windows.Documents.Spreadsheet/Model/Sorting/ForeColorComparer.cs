using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	class ForeColorComparer : IComparer<SortValue>
	{
		public ForeColorComparer(ThemableColor color, SortOrder sortOrder)
		{
			this.color = color;
			this.sortOrder = sortOrder;
		}

		public int Compare(SortValue x, SortValue y)
		{
			ThemableColor first = x.Value as ThemableColor;
			ThemableColor themableColor = y.Value as ThemableColor;
			int num = 0;
			if (!TelerikHelper.EqualsOfT<ThemableColor>(first, themableColor) && first != null && themableColor != null)
			{
				if (first == this.color)
				{
					num = -1;
				}
				else if (themableColor == this.color)
				{
					num = 1;
				}
			}
			num = ((this.sortOrder == SortOrder.Ascending) ? num : (num * -1));
			return (num == 0) ? x.Index.CompareTo(y.Index) : num;
		}

		readonly ThemableColor color;

		readonly SortOrder sortOrder;
	}
}
