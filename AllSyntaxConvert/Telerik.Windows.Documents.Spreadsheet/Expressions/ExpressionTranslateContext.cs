using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Expressions
{
	class ExpressionTranslateContext
	{
		public Worksheet RenamedWorksheet { get; set; }

		public Workbook RenamedWorkbook { get; set; }

		public string OldSpreadsheetName { get; set; }

		public string NewSpreadsheetName { get; set; }

		public bool IsWorkbookRenamed
		{
			get
			{
				return this.RenamedWorkbook != null;
			}
		}

		public bool IsWorksheetRenamed
		{
			get
			{
				return this.RenamedWorksheet != null;
			}
		}

		public bool IsNameRenamed
		{
			get
			{
				return !string.IsNullOrEmpty(this.OldSpreadsheetName) && !string.IsNullOrEmpty(this.NewSpreadsheetName);
			}
		}

		public ExpressionTranslateContext(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.RenamedWorksheet = worksheet;
		}

		public ExpressionTranslateContext(Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			this.RenamedWorkbook = workbook;
		}

		public ExpressionTranslateContext(string oldName, string newName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(oldName, "oldName");
			Guard.ThrowExceptionIfNullOrEmpty(newName, "newName");
			this.OldSpreadsheetName = oldName;
			this.NewSpreadsheetName = newName;
		}
	}
}
