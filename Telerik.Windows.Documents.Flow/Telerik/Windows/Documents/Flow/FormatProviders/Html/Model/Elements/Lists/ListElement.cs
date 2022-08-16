using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	abstract class ListElement : BlockContainerElementBase
	{
		public ListElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.innerElements = new List<HtmlElementBase>();
			this.SetInitialIds();
			this.startIndexAttribute = base.RegisterAttribute<int>("start", false);
		}

		internal int ListId { get; set; }

		internal int ListLevelIndex { get; set; }

		internal bool ShouldRestartLevel { get; set; }

		public void AppendInnerElement(IHtmlExportContext context, Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			Guard.ThrowExceptionIfLessThan<int>(0, paragraph.ListId, "paragraph.ListId");
			int num = ((paragraph.ListLevel == DocumentDefaultStyleSettings.ListLevel) ? 0 : paragraph.ListLevel);
			if (this.ListLevelIndex == num)
			{
				this.ClearNextListLevelElement();
				List list = context.Document.Lists.GetList(this.ListId);
				base.ContentManager.ExportListManager.UpdateListIndexes(list, this.ListLevelIndex, this.ShouldRestartLevel);
				int bulletSerialIndex = base.ContentManager.ExportListManager.ListIndexes.TotalListItemsPerLevel[this.ListLevelIndex].BulletSerialIndex;
				ListLevelElement listLevelElement = base.CreateElement<ListLevelElement>("li");
				listLevelElement.SetAssociatedFlowElement(paragraph);
				listLevelElement.SetValue(bulletSerialIndex);
				if (this.ShouldRestartLevel)
				{
					this.ShouldRestartLevel = false;
				}
				this.innerElements.Add(listLevelElement);
				return;
			}
			ListElement listElement = this.GetListElement(num);
			if (listElement != null)
			{
				if (listElement.innerElements.Count > 0)
				{
					HtmlElementBase htmlElementBase = listElement.innerElements.Last<HtmlElementBase>();
					if (htmlElementBase is ListElement)
					{
						listElement.ClearNextListLevelElement();
					}
				}
				listElement.AppendInnerElement(context, paragraph);
				return;
			}
			if (this.nextListLevelElement != null)
			{
				this.nextListLevelElement.AppendInnerElement(context, paragraph);
				return;
			}
			this.AppendNewListLevel(context, paragraph);
		}

		public void AppendInnerElement(HtmlElementBase innerElement)
		{
			Guard.ThrowExceptionIfNull<HtmlElementBase>(innerElement, "innerElement");
			this.innerElements.Add(innerElement);
		}

		internal void ClearNextListLevelElement()
		{
			if (this.nextListLevelElement != null)
			{
				this.nextListLevelElement.ClearNextListLevelElement();
				this.nextListLevelElement = null;
			}
		}

		protected override void OnBeforeRead(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			base.OnBeforeRead(reader, context);
			context.EndParagraph();
		}

		protected override void OnBeforeWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			List list = context.Document.Lists.GetList(this.ListId);
			ListLevel listLevel = list.Levels[this.ListLevelIndex];
			base.ContentManager.ExportListManager.BeginList();
			string empty = string.Empty;
			string value = "disc";
			if (listLevel.NumberingStyle == NumberingStyle.Bullet)
			{
				BulletListInfo bulletListInfoBySymbol = base.ContentManager.ExportListManager.GetBulletListInfoBySymbol(listLevel.NumberTextFormat);
				if (bulletListInfoBySymbol != null)
				{
					value = bulletListInfoBySymbol.HtmlName;
				}
			}
			else if (HtmlConverters.ListNumberingStyleTypeConverter.ConvertBack(context, listLevel.NumberingStyle, null, out empty))
			{
				value = empty;
			}
			base.LocalProperties.RegisterProperty("list-style-type", value);
		}

		protected override IEnumerable<HtmlElementBase> OnEnumerateInnerElements(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			foreach (HtmlElementBase innerElement in this.innerElements)
			{
				if (!(innerElement is ListElement))
				{
					((ListLevelElement)innerElement).PrepareToExport(context, this.ListLevelIndex);
				}
				yield return innerElement;
			}
			yield break;
		}

		protected override void OnAfterWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			base.OnAfterWrite(writer, context);
			base.ContentManager.ExportListManager.EndList();
		}

		protected override void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
			ListLevel listLevel = base.ContentManager.ImportListManager.BeginLevel(context.Document);
			if (this.startIndexAttribute.HasValue)
			{
				listLevel.StartIndex = this.startIndexAttribute.Value;
			}
			this.ApplyAttributesOnListLevelAfterRead(context, listLevel);
		}

		protected abstract void ApplyAttributesOnListLevelAfterRead(IHtmlImportContext context, ListLevel listLevel);

		protected override void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
			base.ContentManager.ImportListManager.EndLevel();
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			if (this.nextListLevelElement != null)
			{
				this.nextListLevelElement.ClearOverride();
				this.nextListLevelElement = null;
			}
			this.SetInitialIds();
			this.innerElements.Clear();
		}

		void SetInitialIds()
		{
			this.ListId = DocumentDefaultStyleSettings.ListId;
			this.ListLevelIndex = DocumentDefaultStyleSettings.ListId;
		}

		ListElement GetListElement(int listLevelIndex)
		{
			if (this.ListLevelIndex == listLevelIndex)
			{
				return this;
			}
			if (this.nextListLevelElement != null)
			{
				return this.nextListLevelElement.GetListElement(listLevelIndex);
			}
			return null;
		}

		void AppendNewListLevel(IHtmlExportContext context, Paragraph paragraph)
		{
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			int num = this.ListLevelIndex + 1;
			ListElement listElement = base.ContentManager.CreateListElement(context, paragraph, new int?(num));
			List list = context.Document.Lists.GetList(listElement.ListId);
			listElement.ShouldRestartLevel = base.ContentManager.ExportListManager.ShouldRestartLevel(list, num);
			listElement.AppendInnerElement(context, paragraph);
			this.nextListLevelElement = listElement;
			this.AppendInnerElement(listElement);
		}

		readonly HtmlAttribute<int> startIndexAttribute;

		readonly List<HtmlElementBase> innerElements;

		ListElement nextListLevelElement;
	}
}
