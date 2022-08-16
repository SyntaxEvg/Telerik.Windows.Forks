using System;
using System.Collections;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class CellRangeInsertedOrRemovedAwareCollection<T> : CellRangeInsertedOrRemovedAwareCollectionBase, IEnumerable<T>, IEnumerable
	{
		public int Count
		{
			get
			{
				return this.elements.Count;
			}
		}

		protected Worksheet Worksheet
		{
			get
			{
				return this.worksheet;
			}
		}

		protected override ICollection<CellRange> CellRanges
		{
			get
			{
				return this.elements.Keys;
			}
		}

		protected IDictionary<CellRange, T> Elements
		{
			get
			{
				return this.elements;
			}
		}

		internal CellRangeInsertedOrRemovedAwareCollection(Worksheet worksheet)
			: base(worksheet.Cells)
		{
			this.worksheet = worksheet;
			this.elements = new Dictionary<CellRange, T>();
		}

		protected abstract T Add(CellRange cellRange, T element);

		protected abstract bool Remove(CellRange cellRange);

		protected override void TranslateRange(CellRange oldRange, CellRange newRange)
		{
			T element = this.elements[oldRange];
			this.Remove(oldRange);
			if (newRange != null)
			{
				this.Add(newRange, element);
			}
		}

		public IEnumerator<T> GetEnumerator()
		{
			return this.Elements.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable<T>)this).GetEnumerator();
		}

		readonly Worksheet worksheet;

		readonly Dictionary<CellRange, T> elements;
	}
}
