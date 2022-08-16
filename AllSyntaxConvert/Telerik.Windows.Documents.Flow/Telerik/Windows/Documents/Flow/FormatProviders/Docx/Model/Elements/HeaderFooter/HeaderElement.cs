using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Parts;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.HeaderFooter
{
	class HeaderElement : HeaderFooterElementBase<Header>
	{
		public HeaderElement(DocxPartsManager partsManager, OpenXmlPartBase part)
			: base(partsManager, part)
		{
		}

		public override string ElementName
		{
			get
			{
				return "hdr";
			}
		}

		public override IEnumerable<OpenXmlNamespace> Namespaces
		{
			get
			{
				yield return OpenXmlNamespaces.OfficeDocumentRelationshipsNamespace;
				yield break;
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			IEnumerable<OpenXmlElementBase> baseResults = base.EnumerateChildElements(context);
			foreach (OpenXmlElementBase result in baseResults)
			{
				yield return result;
			}
			foreach (object obj in base.BlockContainer.Watermarks)
			{
				Watermark watermark = (Watermark)obj;
				yield return this.CreateWatermarkContainerParagraphElement(watermark);
			}
			yield break;
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			base.OnAfterReadChildElement(context, childElement);
			if (childElement.ElementName == "p")
			{
				ParagraphElement paragraphElement = (ParagraphElement)childElement;
				if (paragraphElement.Watermark != null)
				{
					base.BlockContainer.Blocks.Remove(paragraphElement.Paragraph);
					base.BlockContainer.Watermarks.Add(paragraphElement.Watermark);
				}
			}
			if (childElement.ElementName == "sdt")
			{
				StructuredDocumentTagElement structuredDocumentTagElement = (StructuredDocumentTagElement)childElement;
				if (structuredDocumentTagElement.Watermark != null)
				{
					base.BlockContainer.Watermarks.Add(structuredDocumentTagElement.Watermark);
				}
			}
		}

		ParagraphElement CreateWatermarkContainerParagraphElement(Watermark watermark)
		{
			RadFlowDocument document = new RadFlowDocument();
			Paragraph paragraph = new Paragraph(document);
			paragraph.Inlines.Add(new Run(document));
			ParagraphElement paragraphElement = base.CreateElement<ParagraphElement>("p");
			paragraphElement.SetAssociatedFlowModelElement(paragraph, null);
			paragraphElement.Watermark = watermark;
			return paragraphElement;
		}
	}
}
