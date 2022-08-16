using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Lists;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts
{
	class ListsPart : DocxPartBase
	{
		public ListsPart(DocxPartsManager partsManager)
			: this(partsManager, "/word/numbering.xml")
		{
			base.PartsManager.CreateDocumentRelationship(base.Name, DocxRelationshipTypes.ListsRelationshipType, null);
		}

		public ListsPart(DocxPartsManager partsManager, string partName)
			: base(partsManager, partName)
		{
			this.listsElement = new ListsElement(base.PartsManager, this);
		}

		public override string ContentType
		{
			get
			{
				return DocxContentTypeNames.ListsContentType;
			}
		}

		public override int Level
		{
			get
			{
				return 2;
			}
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.listsElement;
			}
		}

		readonly ListsElement listsElement;
	}
}
