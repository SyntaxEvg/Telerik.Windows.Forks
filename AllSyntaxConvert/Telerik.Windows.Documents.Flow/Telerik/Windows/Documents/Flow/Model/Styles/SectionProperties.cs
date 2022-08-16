using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public sealed class SectionProperties : DocumentElementPropertiesBase
	{
		internal SectionProperties(Section section)
			: base(section, false)
		{
			this.InitProperties();
		}

		public IStyleProperty<bool?> HasDifferentFirstPageHeaderFooter
		{
			get
			{
				return this.hasDifferentFirstPageHeaderFooterProperty;
			}
		}

		public IStyleProperty<PageOrientation?> PageOrientation
		{
			get
			{
				return this.pageOrientationProperty;
			}
		}

		public IStyleProperty<Size?> PageSize
		{
			get
			{
				return this.pageSizeProperty;
			}
		}

		public IStyleProperty<Padding> PageMargins
		{
			get
			{
				return this.pageMarginsProperty;
			}
		}

		public IStyleProperty<double?> HeaderTopMargin
		{
			get
			{
				return this.headerTopMarginProperty;
			}
		}

		public IStyleProperty<double?> FooterBottomMargin
		{
			get
			{
				return this.footerBottomMarginProperty;
			}
		}

		public IStyleProperty<SectionType?> SectionType
		{
			get
			{
				return this.sectionTypeProperty;
			}
		}

		public IStyleProperty<VerticalAlignment> VerticalAlignment
		{
			get
			{
				return this.verticalAlignmentProperty;
			}
		}

		public IStyleProperty<ChapterSeparatorType?> ChapterSeparatorCharacter
		{
			get
			{
				return this.chapterSeparatorCharacterProperty;
			}
		}

		public IStyleProperty<int?> ChapterHeadingStyleIndex
		{
			get
			{
				return this.chapterHeadingStyleIndexProperty;
			}
		}

		public IStyleProperty<NumberingStyle?> PageNumberFormat
		{
			get
			{
				return this.pageNumberFormatProperty;
			}
		}

		public IStyleProperty<int?> StartingPageNumber
		{
			get
			{
				return this.startingPageNumberProperty;
			}
		}

		protected override IEnumerable<IStyleProperty> EnumerateStyleProperties()
		{
			yield return this.HasDifferentFirstPageHeaderFooter;
			yield return this.PageOrientation;
			yield return this.PageSize;
			yield return this.PageMargins;
			yield return this.VerticalAlignment;
			yield return this.HeaderTopMargin;
			yield return this.FooterBottomMargin;
			yield return this.SectionType;
			yield return this.ChapterSeparatorCharacter;
			yield return this.ChapterHeadingStyleIndex;
			yield return this.PageNumberFormat;
			yield return this.StartingPageNumber;
			yield break;
		}

		protected override IStyleProperty GetStylePropertyOverride(IStylePropertyDefinition propertyDefinition)
		{
			if (propertyDefinition != DocumentElementPropertiesBase.StyleIdPropertyDefinition && propertyDefinition.StylePropertyType != StylePropertyType.Section)
			{
				return null;
			}
			return SectionProperties.stylePropertyGetters[propertyDefinition.GlobalPropertyIndex](this);
		}

		static void InitStylePropertyGetters()
		{
			if (SectionProperties.stylePropertyGetters != null)
			{
				return;
			}
			SectionProperties.stylePropertyGetters = new Func<SectionProperties, IStyleProperty>[13];
			SectionProperties.stylePropertyGetters[DocumentElementPropertiesBase.StyleIdPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.StyleIdProperty;
			SectionProperties.stylePropertyGetters[Section.HasDifferentFirstPageHeaderFooterPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.hasDifferentFirstPageHeaderFooterProperty;
			SectionProperties.stylePropertyGetters[Section.PageOrientationPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.pageOrientationProperty;
			SectionProperties.stylePropertyGetters[Section.PageSizePropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.pageSizeProperty;
			SectionProperties.stylePropertyGetters[Section.PageMarginsPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.pageMarginsProperty;
			SectionProperties.stylePropertyGetters[Section.VerticalAlignmentPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.verticalAlignmentProperty;
			SectionProperties.stylePropertyGetters[Section.HeaderTopMarginPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.headerTopMarginProperty;
			SectionProperties.stylePropertyGetters[Section.FooterBottomMarginPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.footerBottomMarginProperty;
			SectionProperties.stylePropertyGetters[Section.SectionTypePropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.sectionTypeProperty;
			SectionProperties.stylePropertyGetters[Section.ChapterSeparatorCharacterPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.chapterSeparatorCharacterProperty;
			SectionProperties.stylePropertyGetters[Section.ChapterHeadingStyleIndexPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.chapterHeadingStyleIndexProperty;
			SectionProperties.stylePropertyGetters[Section.PageNumberFormatPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.pageNumberFormatProperty;
			SectionProperties.stylePropertyGetters[Section.StartingPageNumberPropertyDefinition.GlobalPropertyIndex] = (SectionProperties prop) => prop.startingPageNumberProperty;
		}

		void InitProperties()
		{
			this.hasDifferentFirstPageHeaderFooterProperty = new LocalProperty<bool?>(Section.HasDifferentFirstPageHeaderFooterPropertyDefinition, this);
			this.pageOrientationProperty = new LocalProperty<PageOrientation?>(Section.PageOrientationPropertyDefinition, this);
			this.pageSizeProperty = new LocalProperty<Size?>(Section.PageSizePropertyDefinition, this);
			this.pageMarginsProperty = new LocalProperty<Padding>(Section.PageMarginsPropertyDefinition, this);
			this.verticalAlignmentProperty = new LocalProperty<VerticalAlignment>(Section.VerticalAlignmentPropertyDefinition, this);
			this.headerTopMarginProperty = new LocalProperty<double?>(Section.HeaderTopMarginPropertyDefinition, this);
			this.footerBottomMarginProperty = new LocalProperty<double?>(Section.FooterBottomMarginPropertyDefinition, this);
			this.sectionTypeProperty = new LocalProperty<SectionType?>(Section.SectionTypePropertyDefinition, this);
			this.chapterSeparatorCharacterProperty = new LocalProperty<ChapterSeparatorType?>(Section.ChapterSeparatorCharacterPropertyDefinition, this);
			this.chapterHeadingStyleIndexProperty = new LocalProperty<int?>(Section.ChapterHeadingStyleIndexPropertyDefinition, this);
			this.pageNumberFormatProperty = new LocalProperty<NumberingStyle?>(Section.PageNumberFormatPropertyDefinition, this);
			this.startingPageNumberProperty = new LocalProperty<int?>(Section.StartingPageNumberPropertyDefinition, this);
			SectionProperties.InitStylePropertyGetters();
		}

		static Func<SectionProperties, IStyleProperty>[] stylePropertyGetters;

		LocalProperty<bool?> hasDifferentFirstPageHeaderFooterProperty;

		LocalProperty<PageOrientation?> pageOrientationProperty;

		LocalProperty<Size?> pageSizeProperty;

		LocalProperty<Padding> pageMarginsProperty;

		LocalProperty<double?> headerTopMarginProperty;

		LocalProperty<double?> footerBottomMarginProperty;

		LocalProperty<SectionType?> sectionTypeProperty;

		LocalProperty<VerticalAlignment> verticalAlignmentProperty;

		LocalProperty<ChapterSeparatorType?> chapterSeparatorCharacterProperty;

		LocalProperty<int?> chapterHeadingStyleIndexProperty;

		LocalProperty<NumberingStyle?> pageNumberFormatProperty;

		LocalProperty<int?> startingPageNumberProperty;
	}
}
