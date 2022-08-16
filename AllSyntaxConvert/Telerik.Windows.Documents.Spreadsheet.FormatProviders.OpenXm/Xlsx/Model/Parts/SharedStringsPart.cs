using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.SharedStrings;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts
{
	class SharedStringsPart : XlsxPartBase
	{
		public SharedStringsPart(XlsxPartsManager partsManager)
			: base(partsManager, "/xl/sharedStrings.xml")
		{
			base.PartsManager.CreateWorkbookRelationship(base.Name, XlsxRelationshipTypes.SharedStringsRelationshipType, null);
			this.sharedStringTableElement = new SharedStringTableElement(base.PartsManager, this);
		}

		public SharedStringsPart(XlsxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.sharedStringTableElement = new SharedStringTableElement(base.PartsManager, this);
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.sharedStringTableElement;
			}
		}

		public override bool OverrideDefaultContentType
		{
			get
			{
				return true;
			}
		}

		public override int Level
		{
			get
			{
				return 2;
			}
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.SharedStringsContentType;
			}
		}

		readonly SharedStringTableElement sharedStringTableElement;
	}
}
