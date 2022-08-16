using System;
using System.Collections;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class CellRangeEnumerator : IEnumerator<CellIndex>, IDisposable, IEnumerator
	{
		public CellIndex Current
		{
			get
			{
				Guard.ThrowExceptionIfNull<CellIndex>(this.current, "current");
				return this.current;
			}
		}

		object IEnumerator.Current
		{
			get
			{
				return (IEnumerator<CellIndex>)this.Current;
			}
		}

		public bool IsAtRowColumnStart
		{
			get
			{
				if (this.current == null)
				{
					return false;
				}
				if (this.orientation == CellOrientation.Horizontal)
				{
					if (!this.isReversed)
					{
						return this.current.ColumnIndex == this.cellRange.FromIndex.ColumnIndex;
					}
					return this.current.ColumnIndex == this.cellRange.ToIndex.ColumnIndex;
				}
				else
				{
					if (!this.isReversed)
					{
						return this.current.RowIndex == this.cellRange.FromIndex.RowIndex;
					}
					return this.current.RowIndex == this.cellRange.ToIndex.RowIndex;
				}
			}
		}

		public int RowColumnLength
		{
			get
			{
				if (this.orientation == CellOrientation.Horizontal)
				{
					return this.cellRange.ColumnCount;
				}
				return this.cellRange.RowCount;
			}
		}

		public CellRangeEnumerator(CellRange cellRange, CellOrientation orientation, bool isReversed)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellRange, "cellRange");
			this.cellRange = cellRange;
			this.orientation = orientation;
			this.isReversed = isReversed;
		}

		public void Reset()
		{
			this.current = null;
		}

		public bool MoveNext()
		{
			if (this.current == null)
			{
				if (!this.isReversed)
				{
					this.current = this.cellRange.FromIndex;
				}
				else if (this.orientation == CellOrientation.Horizontal)
				{
					this.current = new CellIndex(this.cellRange.FromIndex.RowIndex, this.cellRange.ToIndex.ColumnIndex);
				}
				else
				{
					this.current = new CellIndex(this.cellRange.ToIndex.RowIndex, this.cellRange.FromIndex.ColumnIndex);
				}
				return true;
			}
			if (this.orientation == CellOrientation.Horizontal)
			{
				if (!this.isReversed)
				{
					if (this.current.ColumnIndex + 1 <= this.cellRange.ToIndex.ColumnIndex)
					{
						this.current = new CellIndex(this.current.RowIndex, this.current.ColumnIndex + 1);
						return true;
					}
					if (this.current.RowIndex + 1 <= this.cellRange.ToIndex.RowIndex)
					{
						this.current = new CellIndex(this.current.RowIndex + 1, this.cellRange.FromIndex.ColumnIndex);
						return true;
					}
				}
				else
				{
					if (this.current.ColumnIndex - 1 >= this.cellRange.FromIndex.ColumnIndex)
					{
						this.current = new CellIndex(this.current.RowIndex, this.current.ColumnIndex - 1);
						return true;
					}
					if (this.current.RowIndex + 1 <= this.cellRange.ToIndex.RowIndex)
					{
						this.current = new CellIndex(this.current.RowIndex + 1, this.cellRange.ToIndex.ColumnIndex);
						return true;
					}
				}
			}
			else if (!this.isReversed)
			{
				if (this.current.RowIndex + 1 <= this.cellRange.ToIndex.RowIndex)
				{
					this.current = new CellIndex(this.current.RowIndex + 1, this.current.ColumnIndex);
					return true;
				}
				if (this.current.ColumnIndex + 1 <= this.cellRange.ToIndex.ColumnIndex)
				{
					this.current = new CellIndex(this.cellRange.FromIndex.RowIndex, this.current.ColumnIndex + 1);
					return true;
				}
			}
			else
			{
				if (this.current.RowIndex - 1 >= this.cellRange.FromIndex.RowIndex)
				{
					this.current = new CellIndex(this.current.RowIndex - 1, this.current.ColumnIndex);
					return true;
				}
				if (this.current.ColumnIndex + 1 <= this.cellRange.ToIndex.ColumnIndex)
				{
					this.current = new CellIndex(this.cellRange.ToIndex.RowIndex, this.current.ColumnIndex + 1);
					return true;
				}
			}
			return false;
		}

		public void Dispose()
		{
		}

		readonly CellRange cellRange;

		readonly CellOrientation orientation;

		readonly bool isReversed;

		CellIndex current;
	}
}
