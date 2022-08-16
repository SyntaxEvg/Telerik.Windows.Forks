using System;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.Contexts
{
	class WorkbookProtectionInfo : ProtectionInfoBase
	{
		public bool LockStructure
		{
			get
			{
				return this.lockStructure;
			}
			set
			{
				this.lockStructure = value;
			}
		}

		public bool LockWindows
		{
			get
			{
				return this.lockWindows;
			}
			set
			{
				this.lockWindows = value;
			}
		}

		bool lockStructure;

		bool lockWindows;
	}
}
