using System;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Commands
{
	abstract class WorkbookCommandContextBase
	{
		public Workbook Workbook
		{
			get
			{
				return this.workbook;
			}
		}

		public Sheet ActiveSheet
		{
			get
			{
				return this.activeSheet;
			}
		}

		public bool IsFirstExecution
		{
			get
			{
				return this.isFirstExecution;
			}
		}

		public WorkbookCommandContextBase(Workbook workbook)
		{
			Guard.ThrowExceptionIfNull<Workbook>(workbook, "workbook");
			this.workbook = workbook;
			this.isFirstExecution = true;
		}

		internal void OnExecuting()
		{
			if (this.isFirstExecution)
			{
				this.activeSheet = this.Workbook.ActiveSheet;
			}
		}

		internal void OnExecuted()
		{
			this.isFirstExecution = false;
		}

		internal virtual void InvalidateLayout()
		{
			this.Workbook.InvalidateLayout();
		}

		readonly Workbook workbook;

		bool isFirstExecution;

		Sheet activeSheet;
	}
}
