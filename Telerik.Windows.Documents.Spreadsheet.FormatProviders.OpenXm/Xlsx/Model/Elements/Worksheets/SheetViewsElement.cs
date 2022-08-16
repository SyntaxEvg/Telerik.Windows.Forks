using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SheetViewsElement : WorksheetElementBase
	{
		public SheetViewsElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.sheetViewElement = base.RegisterChildElement<SheetViewElement>("sheetView");
		}

		public SheetViewElement SheetViewElement
		{
			get
			{
				return this.sheetViewElement.Element;
			}
			set
			{
				this.sheetViewElement.Element = value;
			}
		}

		public override string ElementName
		{
			get
			{
				return "sheetViews";
			}
		}

		public override bool AlwaysExport
		{
			get
			{
				return true;
			}
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			base.CreateElement(this.sheetViewElement);
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			base.ReleaseElement(this.sheetViewElement);
		}

		readonly OpenXmlChildElement<SheetViewElement> sheetViewElement;
	}
}
