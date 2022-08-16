using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class PageNumberingSettingsElement : DocumentElementBase
	{
		public PageNumberingSettingsElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.chapterSeparatorCharacterAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<ChapterSeparatorType>>(new MappedOpenXmlAttribute<ChapterSeparatorType>("chapSep", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.ChapterSeparatorTypeMapper, false));
			this.chapterHeadingStyleIndexAttribute = base.RegisterAttribute<int>("chapStyle", OpenXmlNamespaces.WordprocessingMLNamespace, false);
			this.pageNumberFormatAttribute = base.RegisterAttribute<MappedOpenXmlAttribute<NumberingStyle>>(new MappedOpenXmlAttribute<NumberingStyle>("fmt", OpenXmlNamespaces.WordprocessingMLNamespace, TypeMappers.NumberingStyleMapper, false));
			this.startingPageNumberAttribute = base.RegisterAttribute<int>("start", OpenXmlNamespaces.WordprocessingMLNamespace, false);
		}

		public override string ElementName
		{
			get
			{
				return "pgNumType";
			}
		}

		internal void CopyPropertiesFrom(PageNumberingSettings pageNumberingSettings)
		{
			if (pageNumberingSettings.ChapterSeparatorCharacter != null)
			{
				this.chapterSeparatorCharacterAttribute.Value = pageNumberingSettings.ChapterSeparatorCharacter.Value;
			}
			if (pageNumberingSettings.ChapterHeadingStyleIndex != null)
			{
				this.chapterHeadingStyleIndexAttribute.Value = pageNumberingSettings.ChapterHeadingStyleIndex.Value;
			}
			if (pageNumberingSettings.PageNumberFormat != null)
			{
				this.pageNumberFormatAttribute.Value = pageNumberingSettings.PageNumberFormat.Value;
			}
			if (pageNumberingSettings.StartingPageNumber != null)
			{
				this.startingPageNumberAttribute.Value = pageNumberingSettings.StartingPageNumber.Value;
			}
		}

		internal void CopyPropertiesTo(PageNumberingSettings pageNumberingSettings)
		{
			if (this.chapterSeparatorCharacterAttribute.HasValue)
			{
				pageNumberingSettings.ChapterSeparatorCharacter = new ChapterSeparatorType?(this.chapterSeparatorCharacterAttribute.Value);
			}
			if (this.chapterHeadingStyleIndexAttribute.HasValue)
			{
				pageNumberingSettings.ChapterHeadingStyleIndex = new int?(this.chapterHeadingStyleIndexAttribute.Value);
			}
			if (this.pageNumberFormatAttribute.HasValue)
			{
				pageNumberingSettings.PageNumberFormat = new NumberingStyle?(this.pageNumberFormatAttribute.Value);
			}
			if (this.startingPageNumberAttribute.HasValue)
			{
				pageNumberingSettings.StartingPageNumber = new int?(this.startingPageNumberAttribute.Value);
			}
		}

		readonly MappedOpenXmlAttribute<ChapterSeparatorType> chapterSeparatorCharacterAttribute;

		readonly OpenXmlAttribute<int> chapterHeadingStyleIndexAttribute;

		readonly MappedOpenXmlAttribute<NumberingStyle> pageNumberFormatAttribute;

		readonly OpenXmlAttribute<int> startingPageNumberAttribute;
	}
}
