using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Tables;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Text;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Model.Drawing.Shapes;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model
{
	class HtmlContentManager
	{
		public HtmlContentManager()
		{
			this.importListManager = new HtmlImportListManager();
			this.exportListManager = new HtmlExportListManager();
			this.elementsPool = new HtmlElementsPool(this);
			this.hyperlinkElementsStack = new Stack<HyperlinkElement>();
			this.skipUntilSeparateCharacter = false;
		}

		internal HtmlImportListManager ImportListManager
		{
			get
			{
				return this.importListManager;
			}
		}

		internal HtmlExportListManager ExportListManager
		{
			get
			{
				return this.exportListManager;
			}
		}

		internal ListElement CurrentExportingList
		{
			get
			{
				return this.currentExportingList;
			}
			set
			{
				this.currentExportingList = value;
			}
		}

		HyperlinkElement HyperlinkElement
		{
			get
			{
				if (this.hyperlinkElementsStack.Count > 0)
				{
					return this.hyperlinkElementsStack.Peek();
				}
				return null;
			}
		}

		public HtmlElementBase CreateElement(string name, bool fallback = false)
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			HtmlElementBase htmlElementBase = this.elementsPool.CreateElement(name);
			if (htmlElementBase == null && fallback)
			{
				htmlElementBase = this.elementsPool.CreateElement("DEFAULT");
			}
			return htmlElementBase;
		}

		public T CreateElement<T>(string name) where T : HtmlElementBase
		{
			Guard.ThrowExceptionIfNullOrEmpty(name, "name");
			return (T)((object)this.CreateElement(name, false));
		}

		public void ReleaseElement(HtmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<HtmlElementBase>(element, "element");
			element.Clear();
			this.elementsPool.ReleaseElement(element.Name, element);
		}

		internal ListElement CreateListElement(IHtmlExportContext context, Paragraph paragraph, int? forListLevelIndex = null)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			List list = context.Document.Lists.GetList(paragraph.ListId);
			int num;
			if (forListLevelIndex == null)
			{
				num = ((paragraph.ListLevel == DocumentDefaultStyleSettings.ListLevel) ? 0 : paragraph.ListLevel);
			}
			else
			{
				num = forListLevelIndex.Value;
			}
			ListElement listElement;
			if (list.Levels[num].NumberingStyle == NumberingStyle.Bullet)
			{
				listElement = this.CreateElement<UnorderedListElement>("ul");
			}
			else
			{
				listElement = this.CreateElement<OrderedListElement>("ol");
			}
			listElement.ListId = paragraph.ListId;
			listElement.ListLevelIndex = num;
			return listElement;
		}

		internal IEnumerable<HtmlElementBase> ExportParagraphInlines(IHtmlExportContext context, Paragraph paragraph)
		{
			foreach (InlineBase inline in paragraph.Inlines)
			{
				foreach (HtmlElementBase element in this.ExportInline(context, inline))
				{
					yield return element;
				}
			}
			if (this.HyperlinkElement != null)
			{
				Hyperlink hyperlink = this.HyperlinkElement.Hyperlink;
				yield return this.hyperlinkElementsStack.Pop();
				HyperlinkElement hyperlinkElement = this.CreateElement<HyperlinkElement>("a");
				hyperlinkElement.CopyPropertiesFrom(context, hyperlink);
				this.hyperlinkElementsStack.Push(hyperlinkElement);
			}
			yield break;
		}

		internal IEnumerable<HtmlElementBase> ExportBlockContainer(IHtmlExportContext context, BlockContainerBase blockContainer)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<BlockContainerBase>(blockContainer, "blockContainerBase");
			foreach (BlockBase block in blockContainer.Blocks)
			{
				foreach (HtmlElementBase element in this.ExportBlocks(context, block))
				{
					yield return element;
				}
			}
			if (this.CurrentExportingList != null)
			{
				this.CurrentExportingList.ClearNextListLevelElement();
				yield return this.CurrentExportingList;
				this.CurrentExportingList = null;
			}
			yield break;
		}

		internal IEnumerable<HtmlElementBase> ExportBlocks(IHtmlExportContext context, BlockBase block)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<BlockBase>(block, "block");
			switch (block.Type)
			{
			case DocumentElementType.Paragraph:
				return this.ExportParagraph(context, (Paragraph)block);
			case DocumentElementType.Table:
				return this.ExportTable(context, (Table)block);
			default:
				return Enumerable.Empty<HtmlElementBase>();
			}
		}

		IEnumerable<HtmlElementBase> ExportInline(IHtmlExportContext context, InlineBase inline)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<InlineBase>(inline, "inline");
			if (this.skipUntilSeparateCharacter)
			{
				this.skipUntilSeparateCharacter = inline.Type == DocumentElementType.FieldCharacter && ((FieldCharacter)inline).FieldCharacterType == FieldCharacterType.Separator;
			}
			else
			{
				switch (inline.Type)
				{
				case DocumentElementType.Run:
					return this.ExportInline(this.CreateTextBasedElement(context, (Run)inline));
				case DocumentElementType.FieldCharacter:
					return this.ExportFieldCharacter(context, (FieldCharacter)inline);
				case DocumentElementType.ImageInline:
					return this.ExportInline(this.CreateImageElement(context, ((ImageInline)inline).Image));
				case DocumentElementType.FloatingImage:
					return this.ExportInline(this.CreateImageElement(context, ((FloatingImage)inline).Image));
				case DocumentElementType.Break:
					return this.ExportBreak(context, (Break)inline);
				}
			}
			return Enumerable.Empty<HtmlElementBase>();
		}

		IEnumerable<HtmlElementBase> ExportInline(HtmlElementBase element)
		{
			if (this.HyperlinkElement != null)
			{
				this.HyperlinkElement.AppendInnerElement(element);
			}
			else
			{
				yield return element;
			}
			yield break;
		}

		IEnumerable<HtmlElementBase> ExportBreak(IHtmlExportContext context, Break b)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			BreakType breakType = b.BreakType;
			if (breakType == BreakType.LineBreak)
			{
				return this.ExportInline(this.CreateBreakElement(context, b));
			}
			return Enumerable.Empty<HtmlElementBase>();
		}

		IEnumerable<HtmlElementBase> ExportFieldCharacter(IHtmlExportContext context, FieldCharacter fieldCharacter)
		{
			Hyperlink hyperlink = fieldCharacter.FieldInfo.Field as Hyperlink;
			if (fieldCharacter.FieldCharacterType == FieldCharacterType.Start)
			{
				this.skipUntilSeparateCharacter = true;
				if (hyperlink != null)
				{
					HyperlinkElement hyperlinkElement = this.CreateElement<HyperlinkElement>("a");
					hyperlinkElement.CopyPropertiesFrom(context, hyperlink);
					if (this.HyperlinkElement != null)
					{
						this.HyperlinkElement.AppendInnerElement(hyperlinkElement);
					}
					this.hyperlinkElementsStack.Push(hyperlinkElement);
				}
			}
			else if (fieldCharacter.FieldCharacterType == FieldCharacterType.End && hyperlink != null)
			{
				yield return this.hyperlinkElementsStack.Pop();
			}
			yield break;
		}

		TextBasedElement CreateTextBasedElement(IHtmlExportContext context, Run run)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Run>(run, "run");
			TextBasedElement textBasedElement = this.CreateElement<TextBasedElement>("span");
			textBasedElement.SetAssociatedFlowElement(run);
			return textBasedElement;
		}

		ImageElement CreateImageElement(IHtmlExportContext context, Image image)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Image>(image, "image");
			ImageElement imageElement = this.CreateElement<ImageElement>("img");
			imageElement.SetAsociatedFlowElement(image);
			return imageElement;
		}

		LineBreakElement CreateBreakElement(IHtmlExportContext context, Break b)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Break>(b, "b");
			return this.CreateElement<LineBreakElement>("br");
		}

		IEnumerable<HtmlElementBase> ExportTable(IHtmlExportContext context, Table table)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Table>(table, "table");
			TableElement tableElement = this.CreateElement<TableElement>("table");
			tableElement.SetAssociatedFlowElement(table);
			yield return tableElement;
			yield break;
		}

		IEnumerable<HtmlElementBase> ExportParagraph(IHtmlExportContext context, Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			if (this.CurrentExportingList != null && this.CurrentExportingList.ListId != paragraph.ListId)
			{
				this.CurrentExportingList.ClearNextListLevelElement();
				yield return this.CurrentExportingList;
				this.CurrentExportingList = null;
			}
			if (paragraph.ListId == DocumentDefaultStyleSettings.ListId)
			{
				ParagraphElement paragraphElement = this.CreateElement<ParagraphElement>("p");
				paragraphElement.SetAssociatedFlowElement(paragraph);
				yield return paragraphElement;
			}
			else if (this.CurrentExportingList != null && this.CurrentExportingList.ListId == paragraph.ListId)
			{
				this.CurrentExportingList.AppendInnerElement(context, paragraph);
			}
			else
			{
				ListElement listElement = this.CreateListElement(context, paragraph, new int?(0));
				ListIndexes listIndexes = new ListIndexes(listElement.ListId);
				if (this.ExportListManager.ListIndexes != null && listElement.ListId == this.ExportListManager.ListIndexes.ListId)
				{
					listIndexes.TransferListIndexes(this.ExportListManager.ListIndexes);
				}
				this.ExportListManager.ListIndexes = listIndexes;
				List list = context.Document.Lists.GetList(listElement.ListId);
				listElement.ShouldRestartLevel = this.ExportListManager.ShouldRestartLevel(list, 0);
				listElement.AppendInnerElement(context, paragraph);
				this.CurrentExportingList = listElement;
			}
			yield break;
		}

		readonly HtmlImportListManager importListManager;

		readonly HtmlExportListManager exportListManager;

		readonly HtmlElementsPool elementsPool;

		readonly Stack<HyperlinkElement> hyperlinkElementsStack;

		ListElement currentExportingList;

		bool skipUntilSeparateCharacter;
	}
}
