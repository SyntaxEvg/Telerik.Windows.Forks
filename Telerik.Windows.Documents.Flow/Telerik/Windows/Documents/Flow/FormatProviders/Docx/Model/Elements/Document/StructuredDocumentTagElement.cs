using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model.Watermarks;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class StructuredDocumentTagElement : DocxElementBase
	{
		public StructuredDocumentTagElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		public override string ElementName
		{
			get
			{
				return "sdt";
			}
		}

		public Watermark Watermark
		{
			get
			{
				return this.watermark;
			}
		}

		protected override void OnBeforeReadInnerElements(IDocxImportContext context)
		{
			context.SuspendImport();
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			if (childElement.ElementName == "sdtContent")
			{
				StructuredDocumentTagContentElement structuredDocumentTagContentElement = childElement as StructuredDocumentTagContentElement;
				if (structuredDocumentTagContentElement.Watermark != null)
				{
					this.watermark = structuredDocumentTagContentElement.Watermark;
				}
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			context.ResumeImport();
		}

		protected override void ClearOverride()
		{
			this.watermark = null;
		}

		Watermark watermark;
	}
}
