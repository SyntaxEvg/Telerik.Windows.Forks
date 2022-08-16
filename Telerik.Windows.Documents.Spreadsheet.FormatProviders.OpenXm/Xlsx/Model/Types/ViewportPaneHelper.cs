using System;
using Telerik.Windows.Documents.Spreadsheet.Layout;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Types
{
	static class ViewportPaneHelper
	{
		public static ViewportPaneType GetActivePaneType(Pane contextPane)
		{
			int ysplit = contextPane.YSplit;
			int xsplit = contextPane.XSplit;
			if (ysplit > 0 && xsplit > 0)
			{
				return ViewportPaneType.Scrollable;
			}
			if (ysplit == 0 && xsplit > 0)
			{
				return ViewportPaneType.HorizontalScrollable;
			}
			return ViewportPaneType.VerticalScrollable;
		}
	}
}
