using System;
using Telerik.Documents.SpreadsheetStreaming.Model;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class ViewportStateMapper
	{
		public static ValueMapper<string, PaneState> Instance { get; set; } = new ValueMapper<string, PaneState>("frozen", PaneState.Frozen);
	}
}
