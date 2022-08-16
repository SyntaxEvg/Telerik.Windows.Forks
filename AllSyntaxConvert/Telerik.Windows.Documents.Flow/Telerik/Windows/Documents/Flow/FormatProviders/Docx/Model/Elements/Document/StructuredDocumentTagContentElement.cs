using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class StructuredDocumentTagContentElement : DocxElementBase
	{
		public StructuredDocumentTagContentElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "sdtContent";
			}
		}

		public Watermark Watermark
		{
			get
			{
				return this.watermark;
			}
		}

		protected override bool ShouldImport(IDocxImportContext context)
		{
			return true;
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			if (childElement.ElementName == "p")
			{
				this.watermarkOwnerDummyParagraph = new Paragraph(context.Document);
				((ParagraphElement)childElement).SetAssociatedFlowModelElement(this.watermarkOwnerDummyParagraph, null);
			}
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			if (childElement.ElementName == "p")
			{
				ParagraphElement paragraphElement = childElement as ParagraphElement;
				if (paragraphElement.Watermark != null)
				{
					this.watermark = paragraphElement.Watermark;
				}
			}
		}

		protected override void ClearOverride()
		{
			this.watermark = null;
			this.watermarkOwnerDummyParagraph = null;
		}

		Watermark watermark;

		Paragraph watermarkOwnerDummyParagraph;
	}
}
