using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Export;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Worksheets;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts
{
	class WorksheetPart : SheetPartBase
	{
		public WorksheetPart(XlsxPartsManager partsManager, IXlsxWorksheetExportContext context)
			: this(partsManager, string.Format("/xl/worksheets/sheet{0}.xml", context.SheetNo))
		{
		}

		public WorksheetPart(XlsxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.worksheetElement = new WorksheetElement(base.PartsManager, this);
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.WorksheetContentType;
			}
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.worksheetElement;
			}
		}

		public override int Level
		{
			get
			{
				return 4;
			}
		}

		public override SheetPartType SheetType
		{
			get
			{
				return SheetPartType.Worksheet;
			}
		}

		readonly WorksheetElement worksheetElement;
	}
}
