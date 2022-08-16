using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Types;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	class RefConverter : IStringConverter<Ref>
	{
		public Ref ConvertFromString(string value)
		{
			Guard.ThrowExceptionIfNullOrEmpty(value, "value");
			string[] array = value.Split(new string[] { ":" }, StringSplitOptions.RemoveEmptyEntries);
			CellRef startCellRef = new CellRef(array[0]);
			CellRef endCellRef = ((array.Length > 1) ? new CellRef(array[1]) : new CellRef(array[0]));
			return new Ref(startCellRef, endCellRef);
		}

		public string ConvertToString(Ref value)
		{
			return NameConverter.ConvertCellRangeToName(value.StartCellRef.ToCellIndex(), value.EndCellRef.ToCellIndex());
		}
	}
}
