using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public class NumberDataValidationRuleContext : SingleArgumentDataValidationRuleContext
	{
		public ComparisonOperator ComparisonOperator { get; set; }

		public string Argument2 { get; set; }

		public NumberDataValidationRuleContext(Worksheet worksheet, int rowIndex, int columnIndex)
			: this(worksheet, new CellIndex(rowIndex, columnIndex))
		{
		}

		public NumberDataValidationRuleContext(Worksheet worksheet, CellIndex cellIndex)
			: base(worksheet, cellIndex)
		{
		}
	}
}
