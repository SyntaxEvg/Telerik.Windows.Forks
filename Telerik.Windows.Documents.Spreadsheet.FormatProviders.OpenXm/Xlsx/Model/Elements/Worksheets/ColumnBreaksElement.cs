using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class ColumnBreaksElement : BreaksElementBase
	{
		public ColumnBreaksElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "colBreaks";
			}
		}

		protected override IEnumerable<PageBreakInfo> EnumeratePageBreaks(IXlsxWorksheetExportContext context)
		{
			return context.GetPageBreaksInfo().VerticalPageBreaks;
		}

		protected override void ApplyPageBreakInfo(IXlsxWorksheetImportContext context, PageBreakInfo pageBreakInfo)
		{
			context.ApplyVerticalPageBreakInfo(pageBreakInfo);
		}
	}
}
