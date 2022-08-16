using System;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public class SingleArgumentDataValidationRuleContext : DataValidationRuleContextBase
	{
		public bool IgnoreBlank { get; set; }

		public string Argument1 { get; set; }

		public CellIndex CellIndex { get; set; }

		public Worksheet Worksheet { get; set; }

		internal SpreadsheetCultureHelper CultureInfo
		{
			get
			{
				return this.cultureInfo;
			}
			set
			{
				this.cultureInfo = value;
			}
		}

		public SingleArgumentDataValidationRuleContext(Worksheet worksheet, int rowIndex, int columnIndex)
			: this(worksheet, new CellIndex(rowIndex, columnIndex))
		{
		}

		public SingleArgumentDataValidationRuleContext(Worksheet worksheet, CellIndex cellIndex)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			this.IgnoreBlank = true;
			this.Worksheet = worksheet;
			this.CellIndex = cellIndex;
			this.cultureInfo = FormatHelper.DefaultSpreadsheetCulture;
		}

		SpreadsheetCultureHelper cultureInfo;
	}
}
