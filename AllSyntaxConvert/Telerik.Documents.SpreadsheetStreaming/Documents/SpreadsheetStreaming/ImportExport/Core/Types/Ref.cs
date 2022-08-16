using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class Ref
	{
		public Ref(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			this.startCellRef = new CellRef(fromRowIndex, fromColumnIndex);
			this.endCellRef = new CellRef(toRowIndex, toColumnIndex);
		}

		public Ref(CellRef startCellRef, CellRef endCellRef)
		{
			this.startCellRef = startCellRef;
			this.endCellRef = endCellRef;
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

		public override string ToString()
		{
			return NameConverter.ConvertCellRangeToName(this.StartCellRef.RowIndex, this.StartCellRef.ColumnIndex, this.EndCellRef.RowIndex, this.EndCellRef.ColumnIndex);
		}

		readonly CellRef startCellRef;

		readonly CellRef endCellRef;
	}
}
