using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks
{
	class BookViewsElement : WorkbookElementBase
	{
		public BookViewsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.workbookViewElement = base.RegisterChildElement<WorkbookViewElement>("workbookView");
		}

		public WorkbookViewElement WorkbookViewElement
		{
			get
			{
				return this.workbookViewElement.Element;
			}
			set
			{
				this.workbookViewElement.Element = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "bookViews";
			}
		}

		protected override bool ShouldExport(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			return context.GetActiveTabIndex() > 0;
		}

		protected override void OnBeforeWrite(IXlsxWorkbookExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookExportContext>(context, "context");
			base.CreateElement(this.workbookViewElement);
		}

		protected override void OnAfterRead(IXlsxWorkbookImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorkbookImportContext>(context, "context");
			base.ReleaseElement(this.workbookViewElement);
		}

		readonly OpenXmlChildElement<WorkbookViewElement> workbookViewElement;
	}
}
