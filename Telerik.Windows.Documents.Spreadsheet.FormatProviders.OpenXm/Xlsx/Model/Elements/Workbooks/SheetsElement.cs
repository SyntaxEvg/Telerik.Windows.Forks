using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	class SheetsElement : WorkbookElementBase
	{
		public SheetsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "sheets";
			}
		}

		protected override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			return context.WorksheetContexts.Any<IXlsxWorksheetExportContext>() || base.ShouldExport(context);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			int sheetId = 1;
			foreach (IXlsxWorksheetExportContext exportContext in context.WorksheetContexts)
			{
				SheetElement sheetElement = base.CreateElement<SheetElement>("sheet");
				SheetElement sheetElement2 = sheetElement;
				IXlsxWorksheetExportContext sheetContext = exportContext;
				int sheetId2;
				sheetId = (sheetId2 = sheetId) + 1;
				sheetElement2.CopyPropertiesFrom(context, sheetContext, sheetId2);
				yield return sheetElement;
			}
			yield break;
		}
	}
}
