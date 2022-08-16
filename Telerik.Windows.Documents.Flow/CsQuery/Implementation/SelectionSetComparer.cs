using System;
using System.Collections.Generic;

namespace CsQuery.Implementation
{
	class SelectionSetComparer : IComparer<IDomObject>
	{
		public SelectionSetComparer(SelectionSetOrder order)
		{
			if (order != SelectionSetOrder.Ascending && order != SelectionSetOrder.Descending)
			{
				throw new InvalidOperationException("This comparer can only be used to sort.");
			}
			this.Order = order;
		}

		public int Compare(IDomObject x, IDomObject y)
		{
			if (this.Order != SelectionSetOrder.Ascending)
			{
				return PathKeyComparer.Comparer.Compare(y.NodePath, x.NodePath);
			}
			return PathKeyComparer.Comparer.Compare(x.NodePath, y.NodePath);
		}

		SelectionSetOrder Order;
	}
}
