using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts
{
	public class CellInfo
	{
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
			set
			{
				this.rowIndex = value;
				this.isCellIndexInvalidated = true;
				this.IsCellIndexKnown = true;
			}
		}

		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
			set
			{
				this.columnIndex = value;
				this.isCellIndexInvalidated = true;
				this.IsCellIndexKnown = true;
			}
		}

		public CellIndex CellIndex
		{
			get
			{
				if (this.isCellIndexInvalidated || this.cellIndex == null)
				{
					this.cellIndex = new CellIndex(this.rowIndex, this.columnIndex);
				}
				return this.cellIndex;
			}
			set
			{
				Guard.ThrowExceptionIfNull<CellIndex>(value, "value");
				this.cellIndex = value;
				this.rowIndex = this.cellIndex.RowIndex;
				this.columnIndex = this.cellIndex.ColumnIndex;
				this.IsCellIndexKnown = true;
			}
		}

		public ICellValue CellValue { get; set; }

		public int StyleIndex
		{
			get
			{
				return this.styleIndex;
			}
			set
			{
				this.styleIndex = value;
			}
		}

		internal bool IsCellIndexKnown { get; set; }

		public CellInfo()
		{
			this.isCellIndexInvalidated = true;
			this.IsCellIndexKnown = false;
		}

		internal CellInfo(int rowIndex, int columnIndex, Cells cells)
			: this()
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			Guard.ThrowExceptionIfNull<Cells>(cells, "cells");
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			this.CellValue = cells.PropertyBag.GetPropertyValueCollection<ICellValue>(CellPropertyDefinitions.ValueProperty).GetValue(index);
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
		}

		public CellInfo(CellIndex cellIndex, CellSelection cellSelection)
			: this()
		{
			Guard.ThrowExceptionIfNull<CellSelection>(cellSelection, "cellSelection");
			this.CellIndex = cellIndex;
			this.CellValue = cellSelection.GetValue().Value;
		}

		int rowIndex;

		int columnIndex;

		CellIndex cellIndex;

		bool isCellIndexInvalidated;

		int styleIndex;
	}
}
