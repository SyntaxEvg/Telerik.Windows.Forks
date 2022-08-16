using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	abstract class WorksheetElementBase : XlsxElementBase
	{
		public WorksheetElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		protected sealed override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			return this.ShouldExport(context.GetWorksheetContextFromWorksheetPart((WorksheetPart)base.Part));
		}

		protected sealed override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			this.OnBeforeWrite(context.GetWorksheetContextFromWorksheetPart((WorksheetPart)base.Part));
		}

		protected sealed override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			return this.EnumerateChildElements(context.GetWorksheetContextFromWorksheetPart((WorksheetPart)base.Part));
		}

		protected sealed override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			this.OnAfterRead(context.GetWorksheetContextFromWorksheetPart((WorksheetPart)base.Part));
		}

		protected sealed override void OnAfterReadAttributes(IXlsxWorkbookImportContext context)
		{
			this.OnAfterReadAttributes(context.GetWorksheetContextFromWorksheetPart((WorksheetPart)base.Part));
		}

		protected sealed override void OnAfterReadChildElement(IXlsxWorkbookImportContext context, OpenXmlElementBase childElement)
		{
			this.OnAfterReadChildElement(context.GetWorksheetContextFromWorksheetPart((WorksheetPart)base.Part), childElement);
		}

		protected virtual void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
		}

		protected virtual bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return base.ShouldExport(context.WorkbookContext);
		}

		protected virtual IEnumerable<XlsxElementBase> EnumerateChildElements(IXlsxWorksheetExportContext context)
		{
			return Enumerable.Empty<XlsxElementBase>();
		}

		protected virtual void OnAfterReadAttributes(IXlsxWorksheetImportContext context)
		{
		}

		protected virtual void OnAfterRead(IXlsxWorksheetImportContext context)
		{
		}

		protected virtual void OnAfterReadChildElement(IXlsxWorksheetImportContext context, OpenXmlElementBase childElement)
		{
		}
	}
}
