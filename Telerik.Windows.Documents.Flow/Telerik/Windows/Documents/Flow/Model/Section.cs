using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	public sealed class Section : BlockContainerBase, IElementWithProperties
	{
		public Section(RadFlowDocument document)
			: base(document)
		{
			this.properties = new SectionProperties(this);
			this.headers = new Headers(document, this);
			this.footers = new Footers(document, this);
			this.pageNumberingSettings = new PageNumberingSettings(this);
		}

		public Headers Headers
		{
			get
			{
				return this.headers;
			}
		}

		public Footers Footers
		{
			get
			{
				return this.footers;
			}
		}

		DocumentElementPropertiesBase IElementWithProperties.Properties
		{
			get
			{
				return this.properties;
			}
		}

		public SectionProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public PageNumberingSettings PageNumberingSettings
		{
			get
			{
				return this.pageNumberingSettings;
			}
		}

		public bool HasDifferentFirstPageHeaderFooter
		{
			get
			{
				return this.Properties.HasDifferentFirstPageHeaderFooter.GetActualValue().Value;
			}
			set
			{
				this.Properties.HasDifferentFirstPageHeaderFooter.LocalValue = new bool?(value);
			}
		}

		public PageOrientation PageOrientation
		{
			get
			{
				return this.Properties.PageOrientation.GetActualValue().Value;
			}
			set
			{
				this.Properties.PageOrientation.LocalValue = new PageOrientation?(value);
			}
		}

		public Size PageSize
		{
			get
			{
				return this.Properties.PageSize.GetActualValue().Value;
			}
			set
			{
				this.Properties.PageSize.LocalValue = new Size?(value);
			}
		}

		public Padding PageMargins
		{
			get
			{
				return this.Properties.PageMargins.GetActualValue();
			}
			set
			{
				this.Properties.PageMargins.LocalValue = value;
			}
		}

		public double HeaderTopMargin
		{
			get
			{
				return this.Properties.HeaderTopMargin.GetActualValue().Value;
			}
			set
			{
				this.Properties.HeaderTopMargin.LocalValue = new double?(value);
			}
		}

		public double FooterBottomMargin
		{
			get
			{
				return this.Properties.FooterBottomMargin.GetActualValue().Value;
			}
			set
			{
				this.Properties.FooterBottomMargin.LocalValue = new double?(value);
			}
		}

		public SectionType SectionType
		{
			get
			{
				return this.Properties.SectionType.GetActualValue().Value;
			}
			set
			{
				this.Properties.SectionType.LocalValue = new SectionType?(value);
			}
		}

		public VerticalAlignment VerticalAlignment
		{
			get
			{
				return this.Properties.VerticalAlignment.GetActualValue();
			}
			set
			{
				this.Properties.VerticalAlignment.LocalValue = value;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Section;
			}
		}

		internal override IEnumerable<DocumentElementBase> Children
		{
			get
			{
				return base.Children.Concat(this.Headers.GetInstances()).Concat(this.Footers.GetInstances());
			}
		}

		internal override IEnumerable<DocumentElementBase> ContentChildren
		{
			get
			{
				return base.Children;
			}
		}

		public void Rotate(PageOrientation orientation)
		{
			if (orientation != PageOrientation.Portrait && orientation != PageOrientation.Landscape)
			{
				throw new ArgumentException("The page orientation is not supported.", "orientation");
			}
			if (this.PageOrientation == orientation)
			{
				return;
			}
			Padding padding = this.PageMargins;
			switch (orientation)
			{
			case PageOrientation.Portrait:
				padding = new Padding(padding.Top, padding.Right, padding.Bottom, padding.Left);
				break;
			case PageOrientation.Landscape:
				padding = new Padding(padding.Bottom, padding.Left, padding.Top, padding.Right);
				break;
			}
			this.PageMargins = padding;
			this.PageSize = new Size(this.PageSize.Height, this.PageSize.Width);
			this.PageOrientation = orientation;
		}

		public Section Clone()
		{
			return this.CloneInternal(null);
		}

		public Section Clone(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			return this.CloneInternal(document);
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Section section = this.ClonePropertiesAndHeadersFooters(cloneContext);
			section.Blocks.AddClonedChildrenFrom(base.Blocks, cloneContext);
			return section;
		}

		internal Section ClonePropertiesAndHeadersFooters(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Section section = new Section(cloneContext.Document);
			cloneContext.CurrentSection = section;
			section.Headers.CloneHeadersFootersFrom(this.Headers, cloneContext);
			section.Footers.CloneHeadersFootersFrom(this.Footers, cloneContext);
			section.Properties.CopyPropertiesFrom(this.Properties);
			return section;
		}

		Section CloneInternal(RadFlowDocument document = null)
		{
			return (Section)this.CloneCore(new CloneContext(document ?? this.Document));
		}

		public static readonly StylePropertyDefinition<bool?> HasDifferentFirstPageHeaderFooterPropertyDefinition = new StylePropertyDefinition<bool?>("HasDifferentFirstPageHeaderFooter", new bool?(DocumentDefaultStyleSettings.HasDifferentFirstPageHeaderFooter), StylePropertyType.Section);

		public static readonly StylePropertyDefinition<PageOrientation?> PageOrientationPropertyDefinition = new StylePropertyDefinition<PageOrientation?>("PageOrientation", new PageOrientation?(DocumentDefaultStyleSettings.PageOrientation), StylePropertyType.Section);

		public static readonly StylePropertyDefinition<Size?> PageSizePropertyDefinition = new StylePropertyDefinition<Size?>("PageSize", new Size?(DocumentDefaultStyleSettings.PageSize), StylePropertyType.Section);

		public static readonly StylePropertyDefinition<Padding> PageMarginsPropertyDefinition = new StylePropertyDefinition<Padding>("PageMargins", DocumentDefaultStyleSettings.PageMargins, StylePropertyType.Section);

		public static readonly StylePropertyDefinition<double?> HeaderTopMarginPropertyDefinition = new StylePropertyDefinition<double?>("HeaderTopMargin", new double?(DocumentDefaultStyleSettings.SectionHeaderTopMargin), StylePropertyType.Section);

		public static readonly StylePropertyDefinition<double?> FooterBottomMarginPropertyDefinition = new StylePropertyDefinition<double?>("FooterBottomMargin", new double?(DocumentDefaultStyleSettings.SectionFooterBottomMargin), StylePropertyType.Section);

		public static readonly StylePropertyDefinition<SectionType?> SectionTypePropertyDefinition = new StylePropertyDefinition<SectionType?>("SectionType", new SectionType?(DocumentDefaultStyleSettings.SectionType), StylePropertyType.Section);

		public static readonly StylePropertyDefinition<VerticalAlignment> VerticalAlignmentPropertyDefinition = new StylePropertyDefinition<VerticalAlignment>("VerticalAlignment", DocumentDefaultStyleSettings.VerticalAlignment, StylePropertyType.Section);

		public static readonly StylePropertyDefinition<ChapterSeparatorType?> ChapterSeparatorCharacterPropertyDefinition = new StylePropertyDefinition<ChapterSeparatorType?>("ChapterSeparatorCharacter", null, StylePropertyType.Section);

		public static readonly StylePropertyDefinition<int?> ChapterHeadingStyleIndexPropertyDefinition = new StylePropertyDefinition<int?>("ChapterHeadingStyleIndex", null, StylePropertyType.Section);

		public static readonly StylePropertyDefinition<NumberingStyle?> PageNumberFormatPropertyDefinition = new StylePropertyDefinition<NumberingStyle?>("PageNumberFormat", null, StylePropertyType.Section);

		public static readonly StylePropertyDefinition<int?> StartingPageNumberPropertyDefinition = new StylePropertyDefinition<int?>("StartingPageNumber", null, StylePropertyType.Section);

		readonly SectionProperties properties;

		readonly Headers headers;

		readonly Footers footers;

		readonly PageNumberingSettings pageNumberingSettings;
	}
}
