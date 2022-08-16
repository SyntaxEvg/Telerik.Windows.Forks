using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	public class NamedObjectsItemReplaceEventArgs : EventArgs
	{
		public string ItemName { get; set; }

		public string NewItemName { get; set; }

		public NamedObjectsItemReplaceEventArgs(string itemName, string newItemName)
		{
			Guard.ThrowExceptionIfNullOrEmpty(itemName, "itemName");
			Guard.ThrowExceptionIfNullOrEmpty(newItemName, "newItemName");
			this.ItemName = itemName;
			this.NewItemName = newItemName;
		}
	}
}
