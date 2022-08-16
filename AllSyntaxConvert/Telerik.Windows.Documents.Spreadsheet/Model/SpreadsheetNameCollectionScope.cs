using System;
using Telerik.Windows.Documents.Spreadsheet.Copying;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class SpreadsheetNameCollectionScope : ICopyable<SpreadsheetNameCollectionScope>
	{
		public bool IsGlobal
		{
			get
			{
				return this.worksheet == null;
			}
		}

		internal Worksheet CurrentWorksheet
		{
			get
			{
				if (this.IsGlobal)
				{
					return this.workbook.ActiveWorksheet;
				}
				return this.worksheet;
			}
		}

		internal Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		internal SpreadsheetNameCollectionScope(Worksheet worksheet)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			this.worksheet = worksheet;
			this.workbook = worksheet.Workbook;
		}

		internal SpreadsheetNameCollectionScope(Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			this.workbook = workbook;
		}

		SpreadsheetNameCollectionScope ICopyable<SpreadsheetNameCollectionScope>.Copy(CopyContext context)
		{
			if (this.IsGlobal && !context.SpreadsheetNameExistsInTargetWorkbook)
			{
				return new SpreadsheetNameCollectionScope(context.TargetWorksheet.Workbook);
			}
			return new SpreadsheetNameCollectionScope(context.TargetWorksheet);
		}

		readonly Workbook workbook;

		readonly Worksheet worksheet;
	}
}
