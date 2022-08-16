using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class CellRangeInsertedOrRemovedAwareCollection : CellRangeInsertedOrRemovedAwareCollectionBase, IEnumerable<CellRange>, IEnumerable
	{
		protected override ICollection<CellRange> CellRanges
		{
			get
			{
				return this.cellRanges;
			}
		}

		public int Count
		{
			get
			{
				return this.cellRanges.Count;
			}
		}

		internal CellRangeInsertedOrRemovedAwareCollection(Cells cells)
			: base(cells)
		{
			Guard.ThrowExceptionIfNull<Cells>(cells, "cells");
			this.cellRanges = new HashSet<CellRange>();
		}

		protected override void TranslateRange(CellRange oldRange, CellRange newRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(oldRange, "oldRange");
			Guard.ThrowExceptionIfNull<CellRange>(newRange, "newRange");
			this.Remove(oldRange);
			if (newRange != null)
			{
				this.Add(newRange);
			}
		}

		protected virtual void Add(CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.CellRanges.Add(cellRange);
		}

		protected virtual void Remove(CellRange cellRange)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.CellRanges.Remove(cellRange);
		}

		public IEnumerator<CellRange> GetEnumerator()
		{
			return this.cellRanges.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<CellRange>)this).GetEnumerator();
		}

		readonly HashSet<CellRange> cellRanges;
	}
}
