using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public class ListDataValidationRuleContext : SingleArgumentDataValidationRuleContext
	{
		public bool InCellDropdown { get; set; }

		public ListDataValidationRuleContext(Worksheet worksheet, int rowIndex, int columnIndex)
			: this(worksheet, new CellIndex(rowIndex, columnIndex))
		{
		}

		public ListDataValidationRuleContext(Worksheet worksheet, CellIndex cellIndex)
			: base(worksheet, cellIndex)
		{
			this.InCellDropdown = true;
		}
	}
}
