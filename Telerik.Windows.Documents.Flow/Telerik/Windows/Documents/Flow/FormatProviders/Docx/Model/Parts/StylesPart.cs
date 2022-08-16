using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts
{
	class StylesPart : DocxPartBase
	{
		public StylesPart(DocxPartsManager partsManager)
			: this(partsManager, "/word/styles.xml")
		{
			base.PartsManager.CreateDocumentRelationship(base.Name, DocxRelationshipTypes.StylesRelationshipType, null);
		}

		public StylesPart(DocxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.stylesElement = new StylesElement(base.PartsManager, this);
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.stylesElement;
			}
		}

		public override int Level
		{
			get
			{
				return 3;
			}
		}

		public override string ContentType
		{
			get
			{
				return DocxContentTypeNames.StylesContentType;
			}
		}

		readonly StylesElement stylesElement;
	}
}
