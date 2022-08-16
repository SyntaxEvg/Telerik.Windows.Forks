using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class SheetCollectionChangedEventArgs : EventArgs
	{
		public SheetCollectionChangeType ChangeType
		{
			get
			{
				return this.changeType;
			}
		}

		internal Sheet Sheet
		{
			get
			{
				return this.sheet;
			}
		}

		public SheetCollectionChangedEventArgs(SheetCollectionChangeType changeType)
		{
			this.changeType = changeType;
		}

		internal SheetCollectionChangedEventArgs(SheetCollectionChangeType changeType, Sheet sheet)
			: this(changeType)
		{
			this.sheet = sheet;
		}

		readonly SheetCollectionChangeType changeType;

		readonly Sheet sheet;
	}
}
