using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	abstract class BlockLevelElementsContainerElementBase<T> : DocumentElementBase where T : BlockContainerBase
	{
		public BlockLevelElementsContainerElementBase(DocxPartsManager partsManager)
			: base(partsManager)
		{
		}

		protected T BlockContainer
		{
			get
			{
				return this.blockContainer;
			}
		}

		public void SetAssociatedFlowModelElement(T blockContainer)
		{
			Guard.ThrowExceptionIfNull<T>(blockContainer, "blockContainer");
			this.blockContainer = blockContainer;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			return this.blockContainer.Blocks.Any<BlockBase>() || base.ShouldExport(context);
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			foreach (BlockBase block in this.blockContainer.Blocks)
			{
				switch (block.Type)
				{
				case DocumentElementType.Paragraph:
				{
					ParagraphElement paragraphElement = base.CreateElement<ParagraphElement>("p");
					paragraphElement.SetAssociatedFlowModelElement((Paragraph)block, null);
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
			yield break;
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "p")
				{
					((ParagraphElement)childElement).SetAssociatedFlowModelElement(this.blockContainer.Blocks.AddParagraph(), null);
					return;
				}
				if (!(elementName == "tbl"))
				{
					return;
				}
				((TableElement)childElement).SetAssociatedFlowModelElement(this.blockContainer.Blocks.AddTable());
			}
		}

		T blockContainer;
	}
}
