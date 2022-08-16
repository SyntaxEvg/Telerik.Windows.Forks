using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Sorting
{
	public abstract class OrderedSortConditionBase<T> : SortConditionBase<T>
	{
		public SortOrder SortOrder
		{
			get
			{
				return this.sortOrder;
			}
		}

		protected OrderedSortConditionBase(int relativeIndex, SortOrder sortOrder)
			: base(relativeIndex)
		{
			this.sortOrder = sortOrder;
		}

		public override bool Equals(object obj)
		{
			OrderedSortConditionBase<T> orderedSortConditionBase = obj as OrderedSortConditionBase<T>;
			return orderedSortConditionBase != null && this.SortOrder.Equals(orderedSortConditionBase.SortOrder) && base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(base.GetHashCode(), this.SortOrder.GetHashCode());
		}

		readonly SortOrder sortOrder;
	}
}
