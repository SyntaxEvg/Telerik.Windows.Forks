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
	class FooterPart : HeaderFooterPartBase
	{
		public FooterPart(DocxPartsManager partsManager, Footer footer, int partNumber)
			: base(partsManager, "/word/footer{0}.xml", partNumber)
		{
			base.RelationshipId = base.PartsManager.CreateDocumentRelationship(base.Name, DocxRelationshipTypes.FooterRelationshipType, null);
			this.footerElement = new FooterElement(base.PartsManager, this);
			this.footerElement.SetAssociatedFlowModelElement(footer);
		}

		public FooterPart(DocxPartsManager partsManager, string name)
			: base(partsManager, name)
		{
		}

		public override OpenXmlElementBase RootElement
		{
			get
			{
				return this.footerElement;
			}
		}

		public override string ContentType
		{
			get
			{
				return DocxContentTypeNames.FooterContentType;
			}
		}

		public override void Import(IOpenXmlReader reader, IOpenXmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IOpenXmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IOpenXmlImportContext>(context, "context");
			this.footerElement = new FooterElement(base.PartsManager, this);
			base.RelationshipId = base.PartsManager.GetDocumentRelationshipId(base.Name);
			this.footerElement.SetAssociatedFlowModelElement(((IDocxImportContext)context).GetFooterByRelationshipId(base.RelationshipId));
			base.Import(reader, context);
		}

		FooterElement footerElement;
	}
}
