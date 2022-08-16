using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Common;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Utilities;
using VerticalAlignment = Telerik.Windows.Documents.Flow.Model.Styles.VerticalAlignment;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class SectionPropertiesElement : DocxElementBase
	{
		public SectionPropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.sectionTypeChildElement = base.RegisterChildElement<SectionTypeElement>("type");
			this.pageSizeChildElement = base.RegisterChildElement<PageSizeElement>("pgSz");
			this.pageMarginsChildElement = base.RegisterChildElement<PageMarginsElement>("pgMar");
			this.pageNumberingSettingsElement = base.RegisterChildElement<PageNumberingSettingsElement>("pgNumType");
			this.verticalAlignmentElement = base.RegisterChildElement<VerticalAlignmentElement>("vAlign");
			this.titlePageChildElement = base.RegisterChildElement<TitlePageElement>("titlePg");
		}

		public override string ElementName
		{
			get
			{
				return "sectPr";
			}
		}

		TitlePageElement TitlePageElement
		{
			get
			{
				return this.titlePageChildElement.Element;
			}
		}

		SectionTypeElement SectionTypeElement
		{
			get
			{
				return this.sectionTypeChildElement.Element;
			}
		}

		PageMarginsElement PageMarginsElement
		{
			get
			{
				return this.pageMarginsChildElement.Element;
			}
		}

		PageSizeElement PageSizeElement
		{
			get
			{
				return this.pageSizeChildElement.Element;
			}
		}

		VerticalAlignmentElement VerticalAlignmentElement
		{
			get
			{
				return this.verticalAlignmentElement.Element;
			}
		}

		PageNumberingSettingsElement PageNumberingSettingsElement
		{
			get
			{
				return this.pageNumberingSettingsElement.Element;
			}
		}

		public void SetAssociatedFlowModelElement(Section section)
		{
			Guard.ThrowExceptionIfNull<Section>(section, "section");
			this.section = section;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		protected override void OnAfterReadChildElement(IDocxImportContext context, OpenXmlElementBase childElement)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			Guard.ThrowExceptionIfNull<OpenXmlElementBase>(childElement, "childElement");
			string elementName;
			if ((elementName = childElement.ElementName) != null)
			{
				if (elementName == "headerReference")
				{
					HeaderReferenceElement headerReferenceElement = (HeaderReferenceElement)childElement;
					Header header = this.section.Headers.Add(headerReferenceElement.HeaderFooterType);
					context.RegisterHeader(header, headerReferenceElement.RelationshipId);
					return;
				}
				if (!(elementName == "footerReference"))
				{
					return;
				}
				FooterReferenceElement footerReferenceElement = (FooterReferenceElement)childElement;
				Footer footer = this.section.Footers.Add(footerReferenceElement.HeaderFooterType);
				context.RegisterFooter(footer, footerReferenceElement.RelationshipId);
			}
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			if (this.TitlePageElement != null)
			{
				this.section.Properties.HasDifferentFirstPageHeaderFooter.LocalValue = new bool?(this.TitlePageElement.Value);
				base.ReleaseElement(this.titlePageChildElement);
			}
			if (this.SectionTypeElement != null)
			{
				this.section.Properties.SectionType.LocalValue = new SectionType?(this.SectionTypeElement.SectionType);
				base.ReleaseElement(this.sectionTypeChildElement);
			}
			if (this.PageMarginsElement != null)
			{
				Padding localValue = new Padding(this.PageMarginsElement.Left, this.PageMarginsElement.Top, this.PageMarginsElement.Right, this.PageMarginsElement.Bottom);
				this.section.Properties.PageMargins.LocalValue = localValue;
				this.section.Properties.HeaderTopMargin.LocalValue = new double?(this.PageMarginsElement.Header);
				this.section.Properties.FooterBottomMargin.LocalValue = new double?(this.PageMarginsElement.Footer);
				base.ReleaseElement(this.pageMarginsChildElement);
			}
			if (this.PageSizeElement != null)
			{
				Size value = new Size(this.PageSizeElement.Width, this.PageSizeElement.Height);
				this.section.Properties.PageSize.LocalValue = new Size?(value);
				this.section.Properties.PageOrientation.LocalValue = new PageOrientation?(this.PageSizeElement.PageOrientation);
				base.ReleaseElement(this.pageSizeChildElement);
			}
			if (this.VerticalAlignmentElement != null)
			{
				this.section.Properties.VerticalAlignment.LocalValue = this.VerticalAlignmentElement.Value;
				base.ReleaseElement(this.verticalAlignmentElement);
			}
			if (this.PageNumberingSettingsElement != null)
			{
				this.PageNumberingSettingsElement.CopyPropertiesTo(this.section.PageNumberingSettings);
				base.ReleaseElement(this.pageNumberingSettingsElement);
			}
			context.CloseCurrentSection();
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			bool value = this.section.Properties.HasDifferentFirstPageHeaderFooter.LocalValue.Value;
			if (!value.Equals(DocumentDefaultStyleSettings.HasDifferentFirstPageHeaderFooter))
			{
				base.CreateElement(this.titlePageChildElement);
				this.TitlePageElement.Value = value;
			}
			SectionType value2 = this.section.Properties.SectionType.LocalValue.Value;
			if (!value2.Equals(DocumentDefaultStyleSettings.SectionType))
			{
				base.CreateElement(this.sectionTypeChildElement);
				this.SectionTypeElement.SectionType = value2;
			}
			Padding localValue = this.section.Properties.PageMargins.LocalValue;
			double value3 = this.section.Properties.HeaderTopMargin.LocalValue.Value;
			double value4 = this.section.Properties.FooterBottomMargin.LocalValue.Value;
			if (localValue != DocumentDefaultStyleSettings.PageMargins || value3 != DocumentDefaultStyleSettings.SectionHeaderTopMargin || value4 != DocumentDefaultStyleSettings.SectionFooterBottomMargin)
			{
				base.CreateElement(this.pageMarginsChildElement);
				this.PageMarginsElement.Top = localValue.Top;
				this.PageMarginsElement.Right = localValue.Right;
				this.PageMarginsElement.Bottom = localValue.Bottom;
				this.PageMarginsElement.Left = localValue.Left;
				this.PageMarginsElement.Gutter = 0.0;
				this.PageMarginsElement.Header = value3;
				this.PageMarginsElement.Footer = value4;
			}
			Size value5 = this.section.Properties.PageSize.LocalValue.Value;
			PageOrientation value6 = this.section.Properties.PageOrientation.LocalValue.Value;
			if (value5 != DocumentDefaultStyleSettings.PageSize || value6 != DocumentDefaultStyleSettings.PageOrientation)
			{
				base.CreateElement(this.pageSizeChildElement);
				this.PageSizeElement.Width = value5.Width;
				this.PageSizeElement.Height = value5.Height;
				this.PageSizeElement.PageOrientation = value6;
			}
			VerticalAlignment localValue2 = this.section.Properties.VerticalAlignment.LocalValue;
			if (localValue2 != DocumentDefaultStyleSettings.VerticalAlignment)
			{
				base.CreateElement(this.verticalAlignmentElement);
				this.VerticalAlignmentElement.Value = localValue2;
			}
			PageNumberingSettings pageNumberingSettings = this.section.PageNumberingSettings;
			if (pageNumberingSettings.ChapterSeparatorCharacter != null || pageNumberingSettings.ChapterHeadingStyleIndex != null || pageNumberingSettings.PageNumberFormat != null || pageNumberingSettings.StartingPageNumber != null)
			{
				base.CreateElement(this.pageNumberingSettingsElement);
				this.PageNumberingSettingsElement.CopyPropertiesFrom(pageNumberingSettings);
			}
		}

		protected override IEnumerable<OpenXmlElementBase> EnumerateChildElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			return this.CreateHeaderFooterReferenceElements(context);
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.section = null;
		}

		IEnumerable<HeaderFooterReferenceElementBase> CreateHeaderFooterReferenceElements(IDocxExportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			HeaderFooterBase headerFooter = this.section.Headers.Even;
			if (headerFooter != null)
			{
				HeaderReferenceElement headerReference = this.CreateHeaderFooterReferenceElement<HeaderReferenceElement>(context, "headerReference", headerFooter, HeaderFooterType.Even);
				yield return headerReference;
			}
			headerFooter = this.section.Headers.Default;
			if (headerFooter != null)
			{
				HeaderReferenceElement headerReference2 = this.CreateHeaderFooterReferenceElement<HeaderReferenceElement>(context, "headerReference", headerFooter, HeaderFooterType.Default);
				yield return headerReference2;
			}
			headerFooter = this.section.Headers.First;
			if (headerFooter != null)
			{
				HeaderReferenceElement headerReference3 = this.CreateHeaderFooterReferenceElement<HeaderReferenceElement>(context, "headerReference", headerFooter, HeaderFooterType.First);
				yield return headerReference3;
			}
			headerFooter = this.section.Footers.Even;
			if (headerFooter != null)
			{
				FooterReferenceElement footerReference = this.CreateHeaderFooterReferenceElement<FooterReferenceElement>(context, "footerReference", headerFooter, HeaderFooterType.Even);
				yield return footerReference;
			}
			headerFooter = this.section.Footers.Default;
			if (headerFooter != null)
			{
				FooterReferenceElement footerReference2 = this.CreateHeaderFooterReferenceElement<FooterReferenceElement>(context, "footerReference", headerFooter, HeaderFooterType.Default);
				yield return footerReference2;
			}
			headerFooter = this.section.Footers.First;
			if (headerFooter != null)
			{
				FooterReferenceElement footerReference3 = this.CreateHeaderFooterReferenceElement<FooterReferenceElement>(context, "footerReference", headerFooter, HeaderFooterType.First);
				yield return footerReference3;
			}
			yield break;
		}

		T CreateHeaderFooterReferenceElement<T>(IDocxExportContext context, string elementName, HeaderFooterBase headerFooter, HeaderFooterType headerFooterType) where T : HeaderFooterReferenceElementBase
		{
			Guard.ThrowExceptionIfNull<IDocxExportContext>(context, "context");
			Guard.ThrowExceptionIfNull<string>(elementName, "elementName");
			Guard.ThrowExceptionIfNull<HeaderFooterBase>(headerFooter, "headerFooter");
			T result = base.CreateElement<T>(elementName);
			result.HeaderFooterType = headerFooterType;
			result.RelationshipId = context.GetRelationshipIdByHeaderFooter(headerFooter);
			return result;
		}

		readonly OpenXmlChildElement<TitlePageElement> titlePageChildElement;

		readonly OpenXmlChildElement<SectionTypeElement> sectionTypeChildElement;

		readonly OpenXmlChildElement<PageMarginsElement> pageMarginsChildElement;

		readonly OpenXmlChildElement<PageSizeElement> pageSizeChildElement;

		readonly OpenXmlChildElement<VerticalAlignmentElement> verticalAlignmentElement;

		readonly OpenXmlChildElement<PageNumberingSettingsElement> pageNumberingSettingsElement;

		Section section;
	}
}
