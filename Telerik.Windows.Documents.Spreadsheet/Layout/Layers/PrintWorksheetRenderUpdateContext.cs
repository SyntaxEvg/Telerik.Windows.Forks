using System;
using System.Windows;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Layout.Layers
{
	class PrintWorksheetRenderUpdateContext : WorksheetRenderUpdateContext
	{
		internal PrintWorksheetRenderUpdateContext(RadWorksheetLayout worksheetLayout, SheetViewport sheetViewport, Size scaleFactor, HeaderFooterRenderContext headerFooterContext)
			: base(worksheetLayout, sheetViewport, scaleFactor, null)
		{
			Guard.ThrowExceptionIfNull<HeaderFooterRenderContext>(headerFooterContext, "headerFooterContext");
			this.headerFooterContext = headerFooterContext;
		}

		public HeaderFooterRenderContext HeaderFooterContext
		{
			get
			{
				return this.headerFooterContext;
			}
		}

		readonly HeaderFooterRenderContext headerFooterContext;
	}
}
