using System;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	public class RadCancelEventArgs : EventArgs
	{
		public bool Canceled { get; set; }

		public void Cancel()
		{
			this.Canceled = true;
		}
	}
}
