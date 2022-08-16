using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet
{
	class CellStyleFormatsElement : CellFormatsElementBase
	{
		public CellStyleFormatsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "cellStyleXfs";
			}
		}

		protected override IEnumerable<FormattingRecord> GetFormattings(IXlsxWorkbookExportContext context)
		{
			return context.StyleSheet.StyleFormattingTable;
		}

		protected override void SetFormatting(IXlsxWorkbookImportContext context, FormatElement format)
		{
			context.StyleSheet.StyleFormattingTable.Add(format.CreateFormattingRecord(context));
		}
	}
}
