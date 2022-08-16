using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class SheetPropertiesElement : WorksheetElementBase
	{
		public SheetPropertiesElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.outlinePropertiesElement = base.RegisterChildElement<OutlinePropertiesElement>("outlinePr");
			this.pageSetUpPropertiesElement = base.RegisterChildElement<PageSetUpPropertiesElement>("pageSetUpPr");
		}

		public override string ElementName
		{
			get
			{
				return "sheetPr";
			}
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			base.CreateElement(this.outlinePropertiesElement);
			base.CreateElement(this.pageSetUpPropertiesElement);
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			base.ReleaseElement(this.outlinePropertiesElement);
			base.ReleaseElement(this.pageSetUpPropertiesElement);
		}

		readonly OpenXmlChildElement<OutlinePropertiesElement> outlinePropertiesElement;

		readonly OpenXmlChildElement<PageSetUpPropertiesElement> pageSetUpPropertiesElement;
	}
}
