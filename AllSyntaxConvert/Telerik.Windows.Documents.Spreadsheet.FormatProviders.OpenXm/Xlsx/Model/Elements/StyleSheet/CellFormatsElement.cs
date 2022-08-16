using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class CellFormatsElement : CellFormatsElementBase
	{
		public CellFormatsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "cellXfs";
			}
		}

		protected override IEnumerable<FormattingRecord> GetFormattings(IXlsxWorkbookExportContext context)
		{
			return context.StyleSheet.DirectFormattingTable;
		}

		protected override void SetFormatting(IXlsxWorkbookImportContext context, FormatElement format)
		{
			context.StyleSheet.DirectFormattingTable.Add(format.CreateFormattingRecord(context));
		}
	}
}
