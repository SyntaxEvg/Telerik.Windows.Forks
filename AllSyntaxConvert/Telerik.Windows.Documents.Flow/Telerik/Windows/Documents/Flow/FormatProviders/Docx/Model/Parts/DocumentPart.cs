using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts
{
	class DocumentPart : DocxPartBase
	{
		public DocumentPart(DocxPartsManager partsManager)
			: base(partsManager, "/word/document.xml")
		{
			base.PartsManager.CreateApplicationRelationship(base.Name, DocxRelationshipTypes.OfficeDocumentRelationshipType, null);
			this.documentElement = new DocumentElement(base.PartsManager, this);
		}

		public DocumentPart(DocxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.documentElement = new DocumentElement(base.PartsManager, this);
		}

		public override int Level
		{
			get
			{
				return 3;
			}
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.documentElement;
			}
		}

		public override string ContentType
		{
			get
			{
				return DocxContentTypeNames.DocumentContentType;
			}
		}

		readonly DocumentElement documentElement;
	}
}
