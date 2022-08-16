using System;
using Telerik.Windows.Documents.Flow.FormatProviders.Common;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Export;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Import;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles;
using Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Styles.ParagraphPropertiesElements;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Core;
using Telerik.Windows.Documents.FormatProviders.OpenXml.Model.Elements;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Elements.Document
{
	class ParagraphPropertiesElement : DocumentElementBase
	{
		public ParagraphPropertiesElement(DocxPartsManager partsManager)
			: base(partsManager)
		{
			this.styleIdElement = base.RegisterChildElement<StyleIdElement>("pStyle");
			this.keepWithNextParagraphElement = base.RegisterChildElement<KeepWithNextParagraphElement>("keepNext");
			this.keepOnOnePageElement = base.RegisterChildElement<KeepOnOnePageElement>("keepLines");
			this.pageBreakBefore = base.RegisterChildElement<PageBreakBeforeElement>("pageBreakBefore");
			this.numberingPropertiesChildElement = base.RegisterChildElement<NumberingPropertiesElement>("numPr");
			this.borders = base.RegisterChildElement<ParagraphBordersElement>("pBdr");
			this.shadingChildElement = base.RegisterChildElement<ShadingElement>("shd");
			this.tabStopsElement = base.RegisterChildElement<TabStopsElement>("tabs");
			this.applyEastAsianLineBreakingRulesElement = base.RegisterChildElement<ApplyEastAsianLineBreakingRulesElement>("kinsoku");
			this.allowOverflowPunctuation = base.RegisterChildElement<AllowOverflowPunctuationElement>("overflowPunct");
			this.flowDirectionElement = base.RegisterChildElement<FlowDirectionElement>("bidi");
			this.spacing = base.RegisterChildElement<SpacingElement>("spacing");
			this.indentation = base.RegisterChildElement<IndentationElement>("ind");
			this.contextualSpacing = base.RegisterChildElement<ContextualSpacingElement>("contextualSpacing");
			this.mirrorIndents = base.RegisterChildElement<MirrorIndentsElement>("mirrorIndents");
			this.textAlignmentElement = base.RegisterChildElement<AlignmentElement>("jc");
			this.outlineLevel = base.RegisterChildElement<OutlineLevelElement>("outlineLvl");
			this.runPropertiesChildElement = base.RegisterChildElement<RunPropertiesElement>("rPr");
			this.sectionPropertiesChildElement = base.RegisterChildElement<SectionPropertiesElement>("sectPr");
		}

		public override string ElementName
		{
			get
			{
				return "pPr";
			}
		}

		public ParagraphProperties ParagraphProperties
		{
			get
			{
				return this.paragraphProperties;
			}
		}

		SectionPropertiesElement SectionPropertiesElement
		{
			get
			{
				return this.sectionPropertiesChildElement.Element;
			}
		}

		RunPropertiesElement RunPropertiesElement
		{
			get
			{
				return this.runPropertiesChildElement.Element;
			}
		}

		NumberingPropertiesElement NumberingPropertiesElement
		{
			get
			{
				return this.numberingPropertiesChildElement.Element;
			}
		}

		ParagraphBordersElement ParagraphBordersElement
		{
			get
			{
				return this.borders.Element;
			}
		}

		TabStopsElement TabStopsElement
		{
			get
			{
				return this.tabStopsElement.Element;
			}
		}

		public void SetAssociatedFlowModelElement(Paragraph paragraph, Section section = null)
		{
			Guard.ThrowExceptionIfNull<Paragraph>(paragraph, "paragraph");
			this.paragraph = paragraph;
			this.paragraphProperties = paragraph.Properties;
			this.section = section;
		}

		public void SetAssociatedFlowModelElement(ParagraphProperties paragraphProperties)
		{
			Guard.ThrowExceptionIfNull<ParagraphProperties>(paragraphProperties, "paragraphProperties");
			this.paragraphProperties = paragraphProperties;
		}

		protected override bool ShouldExport(IDocxExportContext context)
		{
			return true;
		}

		protected override void OnAfterRead(IDocxImportContext context)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			if (this.flowDirectionElement.Element != null)
			{
				this.ParagraphProperties.FlowDirection.LocalValue = new FlowDirection?(this.flowDirectionElement.Element.Value ? FlowDirection.RightToLeft : FlowDirection.LeftToRight);
				base.ReleaseElement(this.flowDirectionElement);
			}
			if (this.textAlignmentElement.Element != null)
			{
				this.ParagraphProperties.TextAlignment.LocalValue = new Alignment?(this.textAlignmentElement.Element.Value);
				base.ReleaseElement(this.textAlignmentElement);
			}
			if (this.spacing.Element != null)
			{
				this.spacing.Element.ReadAttributes(this.ParagraphProperties);
				base.ReleaseElement(this.spacing);
			}
			if (this.keepOnOnePageElement.Element != null)
			{
				this.ParagraphProperties.KeepOnOnePage.LocalValue = new bool?(this.keepOnOnePageElement.Element.Value);
				base.ReleaseElement(this.keepOnOnePageElement);
			}
			if (this.keepWithNextParagraphElement.Element != null)
			{
				this.ParagraphProperties.KeepWithNextParagraph.LocalValue = new bool?(this.keepWithNextParagraphElement.Element.Value);
				base.ReleaseElement(this.keepWithNextParagraphElement);
			}
			if (this.applyEastAsianLineBreakingRulesElement.Element != null)
			{
				this.ParagraphProperties.ApplyEastAsianLineBreakingRules.LocalValue = new bool?(this.applyEastAsianLineBreakingRulesElement.Element.Value);
				base.ReleaseElement(this.applyEastAsianLineBreakingRulesElement);
			}
			if (this.pageBreakBefore.Element != null)
			{
				this.ParagraphProperties.PageBreakBefore.LocalValue = new bool?(this.pageBreakBefore.Element.Value);
				base.ReleaseElement(this.pageBreakBefore);
			}
			if (this.outlineLevel.Element != null)
			{
				this.ParagraphProperties.OutlineLevel.LocalValue = new OutlineLevel?(this.outlineLevel.Element.Value);
				base.ReleaseElement(this.outlineLevel);
			}
			if (this.contextualSpacing.Element != null)
			{
				this.ParagraphProperties.ContextualSpacing.LocalValue = new bool?(this.contextualSpacing.Element.Value);
				base.ReleaseElement(this.contextualSpacing);
			}
			if (this.mirrorIndents.Element != null)
			{
				this.ParagraphProperties.MirrorIndents.LocalValue = new bool?(this.mirrorIndents.Element.Value);
				base.ReleaseElement(this.mirrorIndents);
			}
			if (this.TabStopsElement != null)
			{
				base.ReleaseElement(this.tabStopsElement);
			}
			if (this.shadingChildElement.Element != null)
			{
				this.shadingChildElement.Element.ReadAttributes(this.ParagraphProperties);
				base.ReleaseElement(this.shadingChildElement);
			}
			if (this.indentation.Element != null)
			{
				this.indentation.Element.ReadAttributes(this.ParagraphProperties);
				base.ReleaseElement(this.indentation);
			}
			if (this.allowOverflowPunctuation.Element != null)
			{
				this.ParagraphProperties.AllowOverflowPunctuation.LocalValue = new bool?(this.allowOverflowPunctuation.Element.Value);
				base.ReleaseElement(this.allowOverflowPunctuation);
			}
			if (this.numberingPropertiesChildElement.Element != null)
			{
				base.ReleaseElement(this.numberingPropertiesChildElement);
			}
			if (this.ParagraphBordersElement != null)
			{
				base.ReleaseElement(this.borders);
			}
			if (this.styleIdElement.Element != null)
			{
				this.ParagraphProperties.StyleId = this.styleIdElement.Element.Value;
				base.ReleaseElement(this.styleIdElement);
			}
		}

		protected override void OnBeforeWrite(IDocxExportContext context)
		{
			this.CreateSectionPropertiesElement();
			if (this.ParagraphProperties.FlowDirection.HasLocalValue)
			{
				base.CreateElement(this.flowDirectionElement);
				FlowDirection value = this.ParagraphProperties.FlowDirection.LocalValue.Value;
				this.flowDirectionElement.Element.Value = value == FlowDirection.RightToLeft;
			}
			if (this.ParagraphProperties.TextAlignment.HasLocalValue)
			{
				base.CreateElement(this.textAlignmentElement);
				this.textAlignmentElement.Element.Value = this.ParagraphProperties.TextAlignment.LocalValue.Value;
			}
			if (this.ParagraphProperties.SpacingAfter.HasLocalValue || this.ParagraphProperties.SpacingBefore.HasLocalValue || this.ParagraphProperties.AutomaticSpacingAfter.HasLocalValue || this.ParagraphProperties.AutomaticSpacingBefore.HasLocalValue || this.ParagraphProperties.LineSpacing.HasLocalValue || this.ParagraphProperties.LineSpacingType.HasLocalValue)
			{
				base.CreateElement(this.spacing);
				this.spacing.Element.FillAttributes(this.ParagraphProperties);
			}
			if (this.ParagraphProperties.KeepOnOnePage.HasLocalValue)
			{
				base.CreateElement(this.keepOnOnePageElement);
				this.keepOnOnePageElement.Element.Value = this.ParagraphProperties.KeepOnOnePage.LocalValue.Value;
			}
			if (this.ParagraphProperties.KeepWithNextParagraph.HasLocalValue)
			{
				base.CreateElement(this.keepWithNextParagraphElement);
				this.keepWithNextParagraphElement.Element.Value = this.ParagraphProperties.KeepWithNextParagraph.LocalValue.Value;
			}
			if (this.ParagraphProperties.ApplyEastAsianLineBreakingRules.HasLocalValue)
			{
				base.CreateElement(this.applyEastAsianLineBreakingRulesElement);
				this.applyEastAsianLineBreakingRulesElement.Element.Value = this.ParagraphProperties.ApplyEastAsianLineBreakingRules.LocalValue.Value;
			}
			if (this.ParagraphProperties.PageBreakBefore.HasLocalValue)
			{
				base.CreateElement(this.pageBreakBefore);
				this.pageBreakBefore.Element.Value = this.ParagraphProperties.PageBreakBefore.LocalValue.Value;
			}
			if (this.ParagraphProperties.OutlineLevel.HasLocalValue)
			{
				base.CreateElement(this.outlineLevel);
				this.outlineLevel.Element.Value = this.ParagraphProperties.OutlineLevel.LocalValue.Value;
			}
			if (this.ParagraphProperties.ContextualSpacing.HasLocalValue)
			{
				base.CreateElement(this.contextualSpacing);
				this.contextualSpacing.Element.Value = this.ParagraphProperties.ContextualSpacing.LocalValue.Value;
			}
			if (this.ParagraphProperties.MirrorIndents.HasLocalValue)
			{
				base.CreateElement(this.mirrorIndents);
				this.mirrorIndents.Element.Value = this.ParagraphProperties.MirrorIndents.LocalValue.Value;
			}
			if (this.ParagraphProperties.ShadingPattern.HasLocalValue || this.ParagraphProperties.ShadingPatternColor.HasLocalValue || this.ParagraphProperties.BackgroundColor.HasLocalValue)
			{
				base.CreateElement(this.shadingChildElement);
				this.shadingChildElement.Element.FillAttributes(this.ParagraphProperties);
			}
			if (this.ParagraphProperties.FirstLineIndent.HasLocalValue || this.ParagraphProperties.HangingIndent.HasLocalValue || this.ParagraphProperties.LeftIndent.HasLocalValue || this.ParagraphProperties.RightIndent.HasLocalValue)
			{
				base.CreateElement(this.indentation);
				this.indentation.Element.FillAttributes(this.ParagraphProperties);
			}
			if (this.ParagraphProperties.TabStops.HasLocalValue)
			{
				base.CreateElement(this.tabStopsElement);
				this.TabStopsElement.SetAssociatedFlowModelElement(this.ParagraphProperties);
			}
			if (!this.ParagraphProperties.AllowOverflowPunctuation.LocalValue.Value.Equals(DocumentDefaultStyleSettings.AllowOverflowPunctuation))
			{
				base.CreateElement(this.allowOverflowPunctuation);
				this.allowOverflowPunctuation.Element.Value = this.ParagraphProperties.AllowOverflowPunctuation.LocalValue.Value;
			}
			if (this.ParagraphProperties.ListId.HasLocalValue || this.ParagraphProperties.ListLevel.HasLocalValue)
			{
				base.CreateElement(this.numberingPropertiesChildElement);
				this.NumberingPropertiesElement.SetAssociatedFlowModelElement(this.ParagraphProperties);
			}
			if (this.ParagraphProperties.Borders.HasLocalValue)
			{
				base.CreateElement(this.borders);
				this.ParagraphBordersElement.SetAssociatedFlowModelElement(this.ParagraphProperties);
			}
			if (!string.IsNullOrEmpty(this.ParagraphProperties.StyleId))
			{
				base.CreateElement(this.styleIdElement);
				this.styleIdElement.Element.Value = this.ParagraphProperties.StyleId;
			}
			if (this.ParagraphProperties.ParagraphMarkerProperties != null && this.ParagraphProperties.OwnerDocumentElement.GetType() != typeof(RadFlowDocument) && this.ParagraphProperties.ParagraphMarkerProperties.HasLocalValues())
			{
				base.CreateElement(this.runPropertiesChildElement);
				this.RunPropertiesElement.SetAssociatedFlowModelElement(this.ParagraphProperties.ParagraphMarkerProperties);
			}
		}

		protected override void OnBeforeReadChildElement(IDocxImportContext context, OpenXmlElementBase element)
		{
			Guard.ThrowExceptionIfNull<IDocxImportContext>(context, "context");
			string elementName;
			if ((elementName = element.ElementName) != null)
			{
				if (elementName == "sectPr")
				{
					this.SectionPropertiesElement.SetAssociatedFlowModelElement(context.GetCurrentSection());
					return;
				}
				if (elementName == "rPr")
				{
					this.RunPropertiesElement.SetAssociatedFlowModelElement(this.ParagraphProperties.ParagraphMarkerProperties);
					return;
				}
				if (elementName == "numPr")
				{
					this.NumberingPropertiesElement.SetAssociatedFlowModelElement(this.ParagraphProperties);
					return;
				}
				if (elementName == "pBdr")
				{
					this.ParagraphBordersElement.SetAssociatedFlowModelElement(this.ParagraphProperties);
					return;
				}
				if (!(elementName == "tabs"))
				{
					return;
				}
				this.TabStopsElement.SetAssociatedFlowModelElement(this.ParagraphProperties);
			}
		}

		protected override void ClearOverride()
		{
			base.ClearOverride();
			this.paragraph = null;
			this.paragraphProperties = null;
			this.section = null;
		}

		void CreateSectionPropertiesElement()
		{
			if (this.paragraph != null && this.paragraph.IsLastInSectionButNotLastInDocument(this.section))
			{
				base.CreateElement(this.sectionPropertiesChildElement);
				this.SectionPropertiesElement.SetAssociatedFlowModelElement(this.section);
			}
		}

		readonly OpenXmlChildElement<SectionPropertiesElement> sectionPropertiesChildElement;

		readonly OpenXmlChildElement<RunPropertiesElement> runPropertiesChildElement;

		readonly OpenXmlChildElement<NumberingPropertiesElement> numberingPropertiesChildElement;

		readonly OpenXmlChildElement<FlowDirectionElement> flowDirectionElement;

		readonly OpenXmlChildElement<AlignmentElement> textAlignmentElement;

		readonly OpenXmlChildElement<KeepOnOnePageElement> keepOnOnePageElement;

		readonly OpenXmlChildElement<KeepWithNextParagraphElement> keepWithNextParagraphElement;

		readonly OpenXmlChildElement<ApplyEastAsianLineBreakingRulesElement> applyEastAsianLineBreakingRulesElement;

		readonly OpenXmlChildElement<PageBreakBeforeElement> pageBreakBefore;

		readonly OpenXmlChildElement<OutlineLevelElement> outlineLevel;

		readonly OpenXmlChildElement<ContextualSpacingElement> contextualSpacing;

		readonly OpenXmlChildElement<MirrorIndentsElement> mirrorIndents;

		readonly OpenXmlChildElement<ShadingElement> shadingChildElement;

		readonly OpenXmlChildElement<IndentationElement> indentation;

		readonly OpenXmlChildElement<TabStopsElement> tabStopsElement;

		readonly OpenXmlChildElement<AllowOverflowPunctuationElement> allowOverflowPunctuation;

		readonly OpenXmlChildElement<SpacingElement> spacing;

		readonly OpenXmlChildElement<ParagraphBordersElement> borders;

		readonly OpenXmlChildElement<StyleIdElement> styleIdElement;

		Paragraph paragraph;

		ParagraphProperties paragraphProperties;

		Section section;
	}
}
