using System;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Elements.StyleSheet;

namespace Telerik.Windows.Documents.Spreadsheet.FormatProviders.OpenXml.Xlsx.Model.Parts
{
	class StylesPart : XlsxPartBase
	{
		public StylesPart(XlsxPartsManager partsManager)
			: base(partsManager, "/xl/styles.xml")
		{
			base.PartsManager.CreateWorkbookRelationship(base.Name, XlsxRelationshipTypes.StylesRelationshipType, null);
			this.stylesheet = new StyleSheetElement(base.PartsManager, this);
		}

		public StylesPart(XlsxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.stylesheet = new StyleSheetElement(base.PartsManager, this);
		}

		public override string ContentType
		{
			get
			{
				return XlsxContentTypeNames.StylesContentType;
			}
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.stylesheet;
			}
		}

		public override int Level
		{
			get
			{
				return 2;
			}
		}

		readonly StyleSheetElement stylesheet;
	}
}
