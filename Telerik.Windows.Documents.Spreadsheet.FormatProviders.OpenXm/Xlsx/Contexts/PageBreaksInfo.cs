using System;
using System.Collections.Generic;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts
{
	class PageBreaksInfo
	{
		public IEnumerable<PageBreakInfo> HorizontalPageBreaks { get; set; }

		public IEnumerable<PageBreakInfo> VerticalPageBreaks { get; set; }
	}
}
