using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	struct CellProperties
	{
		public FontProperties FontProperties { get; set; }

		public bool IsWrapped { get; set; }

		public ICellValue Value { get; set; }

		public RadHorizontalAlignment HorizontalAlignment { get; set; }

		public int Indent { get; set; }

		public CellValueFormat Format { get; set; }
	}
}
