using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Attributes;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Elements.Lists
{
	class OrderedListElement : ListElement
	{
		public OrderedListElement(HtmlContentManager contentManager)
			: base(contentManager)
		{
			this.numberingStyleTypeAttribute = base.RegisterAttribute<string>("type", false);
		}

		public override string Name
		{
			get
			{
				return "ol";
			}
		}

		protected override void ApplyAttributesOnListLevelAfterRead(IHtmlImportContext context, ListLevel listLevel)
		{
			NumberingStyle numberingStyle;
			if (this.numberingStyleTypeAttribute.HasValue && HtmlValueMappers.NumberingStyleMapper.TryGetToValue(this.numberingStyleTypeAttribute.Value, out numberingStyle))
			{
				if (numberingStyle != NumberingStyle.Bullet)
				{
					listLevel.NumberingStyle = numberingStyle;
					return;
				}
				BulletListInfo bulletListInfoByHtmlName = base.ContentManager.ImportListManager.GetBulletListInfoByHtmlName(this.numberingStyleTypeAttribute.Value);
				if (bulletListInfoByHtmlName != null)
				{
					listLevel.NumberingStyle = NumberingStyle.Bullet;
					listLevel.NumberTextFormat = bulletListInfoByHtmlName.BulletSymbol;
					listLevel.CharacterProperties.FontFamily.LocalValue = new ThemableFontFamily(bulletListInfoByHtmlName.SymbolFontFamily);
				}
			}
		}

		readonly HtmlAttribute<string> numberingStyleTypeAttribute;
	}
}
