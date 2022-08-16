using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core.Types
{
	static class SpreadPaneMapper
	{
		static SpreadPaneMapper()
		{
			SpreadPaneMapper.Instance.AddPair("bottomRight", SpreadPane.BottomRight);
			SpreadPaneMapper.Instance.AddPair("topRight", SpreadPane.TopRight);
			SpreadPaneMapper.Instance.AddPair("bottomLeft", SpreadPane.BottomLeft);
		}

		public static ValueMapper<string, SpreadPane> Instance { get; set; } = new ValueMapper<string, SpreadPane>("topLeft", SpreadPane.TopLeft);
	}
}
