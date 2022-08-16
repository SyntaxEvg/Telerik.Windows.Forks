using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Utilities;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class BodyElement : DocxElementBase
	{
		public BodyElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.hasSectionPropertiesChild = false;
		}

		public override string ElementName
		{
			get
			{
				return "body";
			}
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			return context.GetSections().Any<Section>() || base.ShouldExport(context);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			List<Section> sections = context.GetSections().ToList<Section>();
			foreach (Section section in sections)
			{
				foreach (BlockBase block in section.Blocks)
				{
					switch (block.Type)
					{
					case DocumentElementType.Paragraph:
					{
						ParagraphElement paragraphElement = base.CreateElement<ParagraphElement>("p");
						paragraphElement.SetAssociatedFlowModelElement((Paragraph)block, section);
						yield return paragraphElement;
						break;
					}
					case DocumentElementType.Table:
					{
						TableElement tableElement = base.CreateElement<TableElement>("tbl");
						tableElement.SetAssociatedFlowModelElement((Table)block);
						yield return tableElement;
						break;
					}
					}
				}
			}
			SectionPropertiesElement sectionPropertiesElement = base.CreateElement<SectionPropertiesElement>("sectPr");
			sectionPropertiesElement.SetAssociatedFlowModelElement(sections.Last<Section>());
			yield return sectionPropertiesElement;
			yield break;
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "sectPr")
				{
					((SectionPropertiesElement)childElement).SetAssociatedFlowModelElement(context.GetCurrentSection());
					this.hasSectionPropertiesChild = true;
					return;
				}
				if (elementName == "p")
				{
					((ParagraphElement)childElement).SetAssociatedFlowModelElement(context.GetCurrentSection().Blocks.AddParagraph(), null);
					return;
				}
				if (!(elementName == "tbl"))
				{
					return;
				}
				((TableElement)childElement).SetAssociatedFlowModelElement(context.GetCurrentSection().Blocks.AddTable());
			}
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			DocxHelper.AddHangingAnnotation(context, childElement);
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			if (!this.hasSectionPropertiesChild)
			{
				context.GetCurrentSection();
				context.CloseCurrentSection();
			}
		}

		protected override void ClearOverride()
		{
			this.hasSectionPropertiesChild = false;
		}

		bool hasSectionPropertiesChild;
	}
}
