using System;
using Telerik.Documents.SpreadsheetStreaming.Utilities;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	class RefConverter : IStringConverter<Ref>
	{
		public Ref ConvertFromString(string value)
		{
			string[] array = value.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
			CellRef startCellRef = new CellRef(array[0]);
			CellRef endCellRef = ((array.Length > 1) ? new CellRef(array[1]) : new CellRef(array[0]));
			return new Ref(startCellRef, endCellRef);
		}

		public string ConvertToString(Ref value)
		{
			return NameConverter.ConvertCellRangeToName(value.StartCellRef.RowIndex, value.StartCellRef.ColumnIndex, value.EndCellRef.RowIndex, value.EndCellRef.ColumnIndex);
		}
	}
}
