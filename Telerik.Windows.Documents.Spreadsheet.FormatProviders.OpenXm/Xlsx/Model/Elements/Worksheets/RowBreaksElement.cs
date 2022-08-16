using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class RowBreaksElement : BreaksElementBase
	{
		public RowBreaksElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "rowBreaks";
			}
		}

		protected override IEnumerable<PageBreakInfo> EnumeratePageBreaks(IXlsxWorksheetExportContext context)
		{
			return context.GetPageBreaksInfo().HorizontalPageBreaks;
		}

		protected override void ApplyPageBreakInfo(IXlsxWorksheetImportContext context, PageBreakInfo pageBreakInfo)
		{
			context.ApplyHorizontalPageBreakInfo(pageBreakInfo);
		}
	}
}
