using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Protection
{
	class WorkbookProtectionOptions
	{
		public bool LockStructure
		{
			get
			{
				return this.lockStructure;
			}
		}

		public bool LockWindows
		{
			get
			{
				return this.lockWindows;
			}
		}

		public WorkbookProtectionOptions(bool lockStructure = true, bool lockWindows = false)
		{
			this.lockStructure = lockStructure;
			this.lockWindows = lockWindows;
		}

		public static readonly WorkbookProtectionOptions Default = new WorkbookProtectionOptions(true, false);

		readonly bool lockStructure;

		readonly bool lockWindows;
	}
}
