using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	abstract class ParagraphContentElementBase : DocumentElementBase
	{
		public ParagraphContentElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		protected Paragraph Paragraph { get; set; }

		public void SetAssociatedFlowModelElementParent(Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			this.Paragraph = paragraph;
		}

		protected override void OnBeforeReadInnerElements(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			this.tempSection = new Section(context.Document);
			this.tempParagraph = this.tempSection.Blocks.AddParagraph();
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "element");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (!(elementName == "r"))
				{
					return;
				}
				((RunElement)childElement).SetAssociatedFlowModelElementParent(this.tempParagraph);
			}
		}

		protected void MoveInlinesToParagraph()
		{
			if (this.tempParagraph == null)
			{
				return;
			}
			while (this.tempParagraph.Inlines.Count > 0)
			{
				InlineBase item = this.tempParagraph.Inlines[0];
				this.tempParagraph.Inlines.RemoveAt(0);
				this.Paragraph.Inlines.Add(item);
			}
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.tempSection = null;
			this.tempParagraph = null;
		}

		Section tempSection;

		Paragraph tempParagraph;
	}
}
