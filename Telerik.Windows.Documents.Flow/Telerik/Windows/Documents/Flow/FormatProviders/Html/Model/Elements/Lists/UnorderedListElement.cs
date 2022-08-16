using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	class UnorderedListElement : ListElement
	{
		public UnorderedListElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.numberingStyleTypeAttribute = base.RegisterAttribute<string>("type", false);
		}

		public override string Name
		{
			get
			{
				return "ul";
			}
		}

		protected override void ApplyAttributesOnListLevelAfterRead(IHtmlImportContext context, ListLevel listLevel)
		{
			listLevel.NumberingStyle = NumberingStyle.Bullet;
			listLevel.NumberTextFormat = ListFactory.BulletSymbols[0];
			listLevel.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(ListFactory.BulletsFontFamily[0]);
			if (this.numberingStyleTypeAttribute.HasValue)
			{
				BulletListInfo bulletListInfoByHtmlName = base.ContentManager.ImportListManager.GetBulletListInfoByHtmlName(this.numberingStyleTypeAttribute.Value);
				if (bulletListInfoByHtmlName != null)
				{
					listLevel.NumberTextFormat = bulletListInfoByHtmlName.BulletSymbol;
					listLevel.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(bulletListInfoByHtmlName.SymbolFontFamily);
					return;
				}
				base.ContentManager.ImportListManager.ClearListLevel();
				NumberingStyle numberingStyle;
				if (HtmlValueMappers.NumberingStyleMapper.TryGetToValue(this.numberingStyleTypeAttribute.Value, out numberingStyle) && numberingStyle != NumberingStyle.Bullet)
				{
					listLevel.NumberingStyle = numberingStyle;
				}
			}
		}

		readonly HtmlAttribute<string> numberingStyleTypeAttribute;
	}
}
