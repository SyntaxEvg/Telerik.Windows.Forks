using System;
using System.IO;
using Telerik.Documents.SpreadsheetStreaming.Model.Formatting;

namespace Telerik.Documents.SpreadsheetStreaming.Exporters.Xlsx
{
	sealed class XlsxWorkbookExporter : XlsxWorkbookExporterBase
	{
		internal XlsxWorkbookExporter(Stream stream)
			: base(stream)
		{
		}

		protected override StylesRepository InitializeStylesRepository()
		{
			SpreadCellStyle byName = this.CellStyles.GetByName(SpreadCellStyleCollection.DefaultStyleName);
			return new StylesRepository(byName);
		}
	}
}
