using System;
using System.Linq;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	class ListLevelElement : ParagraphElementBase
	{
		public ListLevelElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.value = base.RegisterAttribute<int>("value", false);
		}

		public override string Name
		{
			get
			{
				return "li";
			}
		}

		HtmlImportListManager ImportListManager
		{
			get
			{
				return base.ContentManager.ImportListManager;
			}
		}

		HtmlExportListManager ExportListManager
		{
			get
			{
				return base.ContentManager.ExportListManager;
			}
		}

		internal void SetValue(int value)
		{
			this.value.Value = value;
		}

		internal void PrepareToExport(IHtmlExportContext context, int listLevelIndex)
		{
			List list = context.Document.Lists.GetList(base.Paragraph.ListId);
			ListLevel listLevel = list.Levels[listLevelIndex];
			CharacterProperties paragraphMarkerProperties = base.Paragraph.Properties.ParagraphMarkerProperties;
			foreach (IStyleProperty styleProperty in paragraphMarkerProperties.StyleProperties)
			{
				IStylePropertyDefinition propertyDefinition = styleProperty.PropertyDefinition;
				IStyleProperty styleProperty2 = listLevel.CharacterProperties.GetStyleProperty(propertyDefinition);
				if (styleProperty2.HasLocalValue)
				{
					base.LocalProperties.CopyFrom(context, styleProperty2, listLevel.CharacterProperties, true);
					this.ExportListManager.ExportedCharacterProperties.Add(styleProperty2);
				}
				else if (styleProperty.HasLocalValue)
				{
					base.LocalProperties.CopyFrom(context, styleProperty, paragraphMarkerProperties, true);
					this.ExportListManager.ExportedCharacterProperties.Add(styleProperty);
				}
			}
		}

		protected override void OnAfterWrite(IHtmlWriter writer, IHtmlExportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlWriter>(writer, "writer");
			Guard.ThrowExceptionIfNull<IHtmlExportContext>(context, "context");
			base.OnAfterWrite(writer, context);
			this.ExportListManager.ExportedCharacterProperties.Clear();
		}

		protected override void OnAfterReadAttributes(IHtmlReader reader, IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlReader>(reader, "reader");
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			base.OnAfterReadAttributes(reader, context);
			if (this.ImportListManager.List == null)
			{
				this.ImportListManager.BeginLevel(context.Document);
			}
			this.ImportListManager.ListIndexes.AddLevelIndex(this.ImportListManager.LevelIndex.Value);
			this.ImportListManager.LevelBulletSerialIndex++;
			this.ImportListManager.ResertAllSubsequentLevels();
			if (this.ImportListManager.LevelBulletSerialIndex == 1)
			{
				this.ApplyAttributesToListLevel(context);
				this.ImportListManager.LevelBulletSerialIndex = this.ImportListManager.GetLevel().StartIndex;
				return;
			}
			if (this.value.HasValue)
			{
				int levelBulletSerialIndex = this.ImportListManager.LevelBulletSerialIndex;
				if (levelBulletSerialIndex != this.value.Value)
				{
					this.ImportListManager.LevelIndex = new int?(0);
					this.ImportListManager.EndLevel();
					this.ImportListManager.BeginLevel(context.Document);
					this.ApplyAttributesToListLevel(context);
				}
			}
		}

		protected override void OnBeforeRead(IHtmlReader reader, IHtmlImportContext context)
		{
			base.OnBeforeRead(reader, context);
			context.ListImportContext.BeginListLevel();
		}

		protected override void OnBeforeReadChildElement(IHtmlReader reader, IHtmlImportContext context, HtmlElementBase innerElement)
		{
			base.OnBeforeReadChildElement(reader, context, innerElement);
			if (base.Paragraph == null)
			{
				base.SetAssociatedFlowElement(base.EnsureParagraph(context));
				base.ApplyStyle(context, context.CurrentParagraph);
			}
			base.Paragraph.ListId = this.ImportListManager.List.Id;
			base.Paragraph.ListLevel = this.ImportListManager.LevelIndex.Value;
			context.ListImportContext.CurrentListLevelContentImporter.BeginChildRead();
		}

		protected override void OnAfterReadChildElement(IHtmlReader reader, IHtmlImportContext context, HtmlElementBase innerElement)
		{
			base.OnAfterReadChildElement(reader, context, innerElement);
			base.ApplyListLevelIndentationOnCurrentParagraphIfNecessary(context);
			context.ListImportContext.CurrentListLevelContentImporter.EndChildRead(base.Paragraph.Inlines.Count == 0);
		}

		protected override void OnAfterRead(IHtmlReader reader, IHtmlImportContext context)
		{
			base.OnAfterRead(reader, context);
			context.EndParagraph();
			context.ListImportContext.EndListLevel();
		}

		void ApplyAttributesToListLevel(IHtmlImportContext context)
		{
			Guard.ThrowExceptionIfNull<IHtmlImportContext>(context, "context");
			ListLevel level = this.ImportListManager.GetLevel();
			if (this.value.HasValue)
			{
				level.StartIndex = this.value.Value;
			}
			HtmlStyleProperty htmlStyleProperty = (from i in base.LocalProperties
				where i.Name == HtmlStylePropertyDescriptors.ListStyleTypeDescriptor.Name
				select i).FirstOrDefault<HtmlStyleProperty>();
			NumberingStyle numberingStyle;
			if (htmlStyleProperty != null && HtmlConverters.ListNumberingStyleTypeConverter.Convert(context, htmlStyleProperty, null, out numberingStyle) && numberingStyle != NumberingStyle.None)
			{
				bool flag = level.NumberingStyle == NumberingStyle.Bullet && numberingStyle != NumberingStyle.Bullet;
				level.NumberingStyle = numberingStyle;
				if (numberingStyle == NumberingStyle.Bullet)
				{
					BulletListInfo bulletListInfoByHtmlName = base.ContentManager.ImportListManager.GetBulletListInfoByHtmlName(htmlStyleProperty.UnparsedValue);
					if (bulletListInfoByHtmlName != null)
					{
						level.NumberTextFormat = bulletListInfoByHtmlName.BulletSymbol;
						level.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(bulletListInfoByHtmlName.SymbolFontFamily);
						return;
					}
				}
				else if (flag)
				{
					base.ContentManager.ImportListManager.ClearListLevel();
					level.NumberingStyle = numberingStyle;
				}
			}
		}

		readonly HtmlAttribute<int> value;
	}
}
