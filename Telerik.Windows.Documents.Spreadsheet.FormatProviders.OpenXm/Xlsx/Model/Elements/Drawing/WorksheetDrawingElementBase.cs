using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Common;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Drawing
{
	abstract class WorksheetDrawingElementBase : XlsxElementBase
	{
		public WorksheetDrawingElementBase(XlsxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override OpenXmlNamespace Namespace
		{
			get
			{
				return OpenXmlNamespaces.SpreadsheetDrawingMLNamespace;
			}
		}

		protected override bool ShouldImport(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			return context.HasRegisteredDrawingPart((DrawingPart)base.Part);
		}

		protected sealed override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			return this.ShouldExport(context.GetWorksheetContextFromDrawingPart((DrawingPart)base.Part));
		}

		protected sealed override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			this.OnBeforeWrite(context.GetWorksheetContextFromDrawingPart((DrawingPart)base.Part));
		}

		protected sealed override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IXlsxWorkbookExportContext context)
		{
			return this.EnumerateChildElements(context.GetWorksheetContextFromDrawingPart((DrawingPart)base.Part));
		}

		protected sealed override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			this.OnAfterRead(context.GetWorksheetContextFromDrawingPart((DrawingPart)base.Part));
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

		protected virtual void OnAfterRead(IXlsxWorksheetImportContext context)
		{
		}
	}
}
