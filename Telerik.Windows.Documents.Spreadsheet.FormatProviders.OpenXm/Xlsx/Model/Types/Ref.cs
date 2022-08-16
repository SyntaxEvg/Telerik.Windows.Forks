using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class Ref
	{
		public Ref(CellRange range)
		{
			this.startCellRef = new CellRef(range.FromIndex.RowIndex, range.FromIndex.ColumnIndex);
			this.endCellRef = new CellRef(range.ToIndex.RowIndex, range.ToIndex.ColumnIndex);
		}

		public Ref(CellRef startCellRef, CellRef endCellRef)
		{
			this.startCellRef = startCellRef;
			this.endCellRef = endCellRef;
		}

		public Ref(string range)
		{
			Guard.ThrowExceptionIfNullOrEmpty(range, "range");
			string[] array = range.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
			this.startCellRef = new CellRef(array[0]);
			this.endCellRef = ((array.Length > 1) ? new CellRef(array[1]) : new CellRef(array[0]));
		}

		public CellRef StartCellRef
		{
			get
			{
				return this.startCellRef;
			}
		}

		public CellRef EndCellRef
		{
			get
			{
				return this.endCellRef;
			}
		}

		public CellRange ToCellRange()
		{
			return new CellRange(this.StartCellRef.ToCellIndex(), this.EndCellRef.ToCellIndex());
		}

		public override string ToString()
		{
			return NameConverter.ConvertCellRangeToName(this.StartCellRef.ToCellIndex(), this.EndCellRef.ToCellIndex());
		}

		readonly CellRef startCellRef;

		readonly CellRef endCellRef;
	}
}
