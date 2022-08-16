using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class CellRangeInsertedOrRemovedAwareOrderedCollection : CellRangeInsertedOrRemovedAwareCollectionBase
	{
		protected CellRangeInsertedOrRemovedAwareOrderedCollection(Cells cells)
			: base(cells)
		{
			this.rangesList = new List<CellRange>();
		}

		protected List<CellRange> RangesList
		{
			get
			{
				return this.rangesList;
			}
		}

		protected override ICollection<CellRange> CellRanges
		{
			get
			{
				return this.RangesList;
			}
		}

		protected override void TranslateRange(CellRange oldRange, CellRange newRange)
		{
			int num = this.RangesList.IndexOf(oldRange);
			if (num > -1)
			{
				this.RemoveCellRangeAt(num);
				if (newRange != null)
				{
					this.Insert(num, newRange);
				}
			}
		}

		protected virtual void RemoveCellRangeAt(int index)
		{
			this.RangesList.RemoveAt(index);
		}

		protected virtual void Insert(int index, CellRange range)
		{
			this.RangesList.Insert(index, range);
		}

		readonly List<CellRange> rangesList;
	}
}
