using System;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class NameCollectionChangingEventArgs : EventArgs
	{
		public string NewName
		{
			get
			{
				return this.newName;
			}
		}

		public string OldName
		{
			get
			{
				return this.oldName;
			}
		}

		public string NewRefersTo
		{
			get
			{
				return this.newRefersTo;
			}
		}

		public string OldRefersTo
		{
			get
			{
				return this.oldRefersTo;
			}
		}

		public NameCollectionChangingEventArgs(string oldName, string oldRefersTo, string newName, string newRefersTo)
		{
			this.oldRefersTo = oldRefersTo;
			this.oldName = oldName;
			this.newRefersTo = newRefersTo;
			this.newName = newName;
		}

		readonly string newName;

		readonly string oldName;

		readonly string newRefersTo;

		readonly string oldRefersTo;
	}
}
