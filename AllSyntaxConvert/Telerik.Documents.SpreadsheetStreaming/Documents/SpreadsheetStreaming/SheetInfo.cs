using System;
using System.Diagnostics;

namespace Telerik.Documents.SpreadsheetStreaming
{
	[DebuggerDisplay("{Name}")]
	public class SheetInfo
	{
		internal SheetInfo(string name)
		{
			this.Name = name;
		}

		public string Name { get; set; }
	}
}
