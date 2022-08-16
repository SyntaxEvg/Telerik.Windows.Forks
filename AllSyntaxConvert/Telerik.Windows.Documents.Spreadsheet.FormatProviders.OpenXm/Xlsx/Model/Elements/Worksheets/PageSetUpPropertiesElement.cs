using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Import;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets
{
	class PageSetUpPropertiesElement : WorksheetElementBase
	{
		public PageSetUpPropertiesElement(XlsxPartsManager partsManager)
			: base(partsManager)
		{
			this.fitToPage = base.RegisterAttribute<BoolOpenXmlAttribute>(new BoolOpenXmlAttribute("fitToPage", true, false));
		}

		public override string ElementName
		{
			get
			{
				return "pageSetUpPr";
			}
		}

		public bool FitToPage
		{
			get
			{
				return this.fitToPage.Value;
			}
			set
			{
				this.fitToPage.Value = value;
			}
		}

		protected override bool ShouldExport(IXlsxWorksheetExportContext context)
		{
			return context.Worksheet.WorksheetPageSetup.FitToPages;
		}

		protected override void OnBeforeWrite(IXlsxWorksheetExportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetExportContext>(context, "context");
			this.FitToPage = context.Worksheet.WorksheetPageSetup.FitToPages;
		}

		protected override void OnAfterRead(IXlsxWorksheetImportContext context)
		{
			Guard.ThrowExceptionIfNull<IXlsxWorksheetImportContext>(context, "context");
			context.Worksheet.WorksheetPageSetup.FitToPages = this.FitToPage;
		}

		readonly BoolOpenXmlAttribute fitToPage;
	}
}
