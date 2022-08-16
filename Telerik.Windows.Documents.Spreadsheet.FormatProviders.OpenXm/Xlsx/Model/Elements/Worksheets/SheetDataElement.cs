using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Contexts;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SheetDataElement : WorksheetElementBase
	{
		public SheetDataElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "sheetData";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		protected override IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			foreach (RowInfo row in context.GetNonEmptyRows())
			{
				RowElement rowElement = base.CreateElement<RowElement>("row");
				rowElement.CopyPropertiesFrom(context, row);
				yield return rowElement;
			}
			yield break;
		}
	}
}
