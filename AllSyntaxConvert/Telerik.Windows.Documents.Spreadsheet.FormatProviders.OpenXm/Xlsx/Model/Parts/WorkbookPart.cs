using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.Workbooks;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts
{
	class WorkbookPart : XlsxPartBase
	{
		public WorkbookPart(XlsxPartsManager partsManager)
			: base(partsManager, "/xl/workbook.xml")
		{
			base.PartsManager.CreateApplicationRelationship(base.Name, XlsxRelationshipTypes.OfficeDocumentRelationshipType, null);
			this.workbookElement = new WorkbookElement(base.PartsManager, this);
		}

		public WorkbookPart(XlsxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.workbookElement = new WorkbookElement(base.PartsManager, this);
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.workbookElement;
			}
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.WorkbookContentType;
			}
		}

		public override int Level
		{
			get
			{
				return 3;
			}
		}

		readonly WorkbookElement workbookElement;
	}
}
