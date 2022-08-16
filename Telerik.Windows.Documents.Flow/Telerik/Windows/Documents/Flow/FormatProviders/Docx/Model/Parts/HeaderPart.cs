using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.HeaderFooter;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Import;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Parts
{
	class HeaderPart : HeaderFooterPartBase
	{
		public HeaderPart(DocxPartsManager partsManager, Header header, int headerNumber)
			: base(partsManager, "/word/header{0}.xml", headerNumber)
		{
			base.RelationshipId = base.PartsManager.CreateDocumentRelationship(base.Name, DocxRelationshipTypes.HeaderRelationshipType, null);
			this.headerElement = new HeaderElement(base.PartsManager, this);
			this.headerElement.SetAssociatedFlowModelElement(header);
		}

		public HeaderPart(DocxPartsManager partsManager, string name)
			: base(partsManager, name)
		{
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.headerElement;
			}
		}

		public override string ContentType
		{
			get
			{
				return DocxContentTypeNames.HeaderContentType;
			}
		}

		public override void Import(IOpenXmlReader reader, IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			this.headerElement = new HeaderElement(base.PartsManager, this);
			base.RelationshipId = base.PartsManager.GetDocumentRelationshipId(base.Name);
			this.headerElement.SetAssociatedFlowModelElement(((IDocxImportContext)context).GetHeaderByRelationshipId(base.RelationshipId));
			base.Import(reader, context);
		}

		HeaderElement headerElement;
	}
}
