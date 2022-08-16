using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	public sealed class ParagraphProperties : DocumentElementPropertiesBase, IPropertiesWithShading
	{
		internal ParagraphProperties(Style style)
			: base(style)
		{
			this.InitProperties();
		}

		internal ParagraphProperties(Paragraph paragraph)
			: base(paragraph, false)
		{
			this.InitProperties();
		}

		internal ParagraphProperties(RadFlowDocument document, bool suppressStylePropertyEvaluation)
			: base(document, suppressStylePropertyEvaluation)
		{
			this.InitProperties();
		}

		internal ParagraphProperties(ListLevel listLevel, bool suppressStylePropertyEvaluation)
			: base(listLevel, suppressStylePropertyEvaluation)
		{
			this.InitProperties();
		}

		public IStyleProperty<FlowDirection?> FlowDirection
		{
			get
			{
				return this.flowDirectionStyleProperty;
			}
		}

		public IStyleProperty<Alignment?> TextAlignment
		{
			get
			{
				return this.textAlignmentStyleProperty;
			}
		}

		public IStyleProperty<double?> SpacingBefore
		{
			get
			{
				return this.spacingBeforeStyleProperty;
			}
		}

		public IStyleProperty<bool?> AutomaticSpacingBefore
		{
			get
			{
				return this.automaticSpacingBeforeStyleProperty;
			}
		}

		public IStyleProperty<double?> SpacingAfter
		{
			get
			{
				return this.spacingAfterStyleProperty;
			}
		}

		public IStyleProperty<bool?> AutomaticSpacingAfter
		{
			get
			{
				return this.automaticSpacingAfterStyleProperty;
			}
		}

		public IStyleProperty<double?> LineSpacing
		{
			get
			{
				return this.lineSpacingStyleProperty;
			}
		}

		public IStyleProperty<HeightType?> LineSpacingType
		{
			get
			{
				return this.lineSpacingTypeStyleProperty;
			}
		}

		public IStyleProperty<bool?> KeepOnOnePage
		{
			get
			{
				return this.keepOnOnePageStyleProperty;
			}
		}

		public IStyleProperty<bool?> KeepWithNextParagraph
		{
			get
			{
				return this.keepWithNextParagraphStyleProperty;
			}
		}

		public IStyleProperty<OutlineLevel?> OutlineLevel
		{
			get
			{
				return this.outlineLevelStyleProperty;
			}
		}

		public IStyleProperty<bool?> ApplyEastAsianLineBreakingRules
		{
			get
			{
				return this.applyEastAsianLineBreakingRulesStyleProperty;
			}
		}

		public IStyleProperty<bool?> PageBreakBefore
		{
			get
			{
				return this.pageBreakBeforeStyleProperty;
			}
		}

		public IStyleProperty<ParagraphBorders> Borders
		{
			get
			{
				return this.bordersStyleProperty;
			}
		}

		public IStyleProperty<bool?> ContextualSpacing
		{
			get
			{
				return this.contextualSpacingStyleProperty;
			}
		}

		public IStyleProperty<bool?> MirrorIndents
		{
			get
			{
				return this.mirrorIndentsStyleProperty;
			}
		}

		public IStyleProperty<ThemableColor> BackgroundColor
		{
			get
			{
				return this.backgroundColorStyleProperty;
			}
		}

		public IStyleProperty<ThemableColor> ShadingPatternColor
		{
			get
			{
				return this.shadingPatternColorStyleProperty;
			}
		}

		public IStyleProperty<ShadingPattern?> ShadingPattern
		{
			get
			{
				return this.shadingPatternStyleProperty;
			}
		}

		public IStyleProperty<double?> FirstLineIndent
		{
			get
			{
				return this.firstLineIndentStyleProperty;
			}
		}

		public IStyleProperty<double?> HangingIndent
		{
			get
			{
				return this.hangingIndentStyleProperty;
			}
		}

		public IStyleProperty<double?> LeftIndent
		{
			get
			{
				return this.leftIndentStyleProperty;
			}
		}

		public IStyleProperty<double?> RightIndent
		{
			get
			{
				return this.rightIndentStyleProperty;
			}
		}

		public IStyleProperty<bool?> AllowOverflowPunctuation
		{
			get
			{
				return this.allowOverflowPunctuationStyleProperty;
			}
		}

		public IStyleProperty<TabStopCollection> TabStops
		{
			get
			{
				return this.tabStopsStyleProperty;
			}
		}

		public CharacterProperties ParagraphMarkerProperties { get; set; }

		RadFlowDocument IPropertiesWithShading.Document
		{
			get
			{
				return base.Document;
			}
		}

		public IStyleProperty<int?> ListId
		{
			get
			{
				return this.listIdStyleProperty;
			}
		}

		public IStyleProperty<int?> ListLevel
		{
			get
			{
				return this.listLevelStyleProperty;
			}
		}

		public override void CopyPropertiesFrom(DocumentElementPropertiesBase fromProperties)
		{
			base.CopyPropertiesFrom(fromProperties);
			if (fromProperties == null || this.ParagraphMarkerProperties == null || fromProperties.GetType() != base.GetType())
			{
				return;
			}
			this.ParagraphMarkerProperties.CopyPropertiesFrom(((ParagraphProperties)fromProperties).ParagraphMarkerProperties);
		}

		protected override IEnumerable<IStyleProperty> EnumerateStyleProperties()
		{
			yield return this.FlowDirection;
			yield return this.TextAlignment;
			yield return this.SpacingBefore;
			yield return this.AutomaticSpacingBefore;
			yield return this.SpacingAfter;
			yield return this.AutomaticSpacingAfter;
			yield return this.LineSpacing;
			yield return this.LineSpacingType;
			yield return this.KeepOnOnePage;
			yield return this.KeepWithNextParagraph;
			yield return this.OutlineLevel;
			yield return this.ApplyEastAsianLineBreakingRules;
			yield return this.PageBreakBefore;
			yield return this.Borders;
			yield return this.ContextualSpacing;
			yield return this.MirrorIndents;
			yield return this.BackgroundColor;
			yield return this.ShadingPatternColor;
			yield return this.ShadingPattern;
			yield return this.FirstLineIndent;
			yield return this.HangingIndent;
			yield return this.LeftIndent;
			yield return this.RightIndent;
			yield return this.AllowOverflowPunctuation;
			yield return this.TabStops;
			yield return this.ListLevel;
			yield return this.ListId;
			yield break;
		}

		protected override IStyleProperty GetStylePropertyOverride(IStylePropertyDefinition propertyDefinition)
		{
			if (propertyDefinition != DocumentElementPropertiesBase.StyleIdPropertyDefinition && propertyDefinition.StylePropertyType != StylePropertyType.Paragraph)
			{
				return null;
			}
			return ParagraphProperties.stylePropertyGetters[propertyDefinition.GlobalPropertyIndex](this);
		}

		static void InitStylePropertyGetters()
		{
			if (ParagraphProperties.stylePropertyGetters != null)
			{
				return;
			}
			ParagraphProperties.stylePropertyGetters = new Func<ParagraphProperties, IStyleProperty>[28];
			ParagraphProperties.stylePropertyGetters[DocumentElementPropertiesBase.StyleIdPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.StyleIdProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.AllowOverflowPunctuationPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.allowOverflowPunctuationStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.TextAlignmentPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.textAlignmentStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.FlowDirectionPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.flowDirectionStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.LineSpacingPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.lineSpacingStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.LineSpacingTypePropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.lineSpacingTypeStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.KeepOnOnePagePropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.keepOnOnePageStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.KeepWithNextParagraphPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.keepWithNextParagraphStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.ListIdPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.listIdStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.ListLevelPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.listLevelStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.OutlineLevelPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.outlineLevelStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.ApplyEastAsianLineBreakingRulesPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.applyEastAsianLineBreakingRulesStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.PageBreakBeforePropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.pageBreakBeforeStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.ContextualSpacingPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.contextualSpacingStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.MirrorIndentsPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.mirrorIndentsStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.SpacingAfterPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.spacingAfterStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.SpacingBeforePropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.spacingBeforeStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.AutomaticSpacingBeforePropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.automaticSpacingBeforeStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.AutomaticSpacingAfterPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.automaticSpacingAfterStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.BordersPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.bordersStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.BackgroundColorPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.backgroundColorStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.ShadingPatternColorPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.shadingPatternColorStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.ShadingPatternPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.shadingPatternStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.FirstLineIndentPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.firstLineIndentStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.HangingIndentPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.hangingIndentStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.LeftIndentPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.leftIndentStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.RightIndentPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.rightIndentStyleProperty;
			ParagraphProperties.stylePropertyGetters[Paragraph.TabStopsPropertyDefinition.GlobalPropertyIndex] = (ParagraphProperties prop) => prop.tabStopsStyleProperty;
		}

		void InitProperties()
		{
			this.allowOverflowPunctuationStyleProperty = new LocalProperty<bool?>(Paragraph.AllowOverflowPunctuationPropertyDefinition, this);
			this.textAlignmentStyleProperty = new StyleProperty<Alignment?>(Paragraph.TextAlignmentPropertyDefinition, this);
			this.flowDirectionStyleProperty = new StyleProperty<FlowDirection?>(Paragraph.FlowDirectionPropertyDefinition, this);
			this.lineSpacingStyleProperty = new StyleProperty<double?>(Paragraph.LineSpacingPropertyDefinition, this);
			this.lineSpacingTypeStyleProperty = new LineSpacingTypeStyleProperty(Paragraph.LineSpacingTypePropertyDefinition, this);
			this.keepOnOnePageStyleProperty = new StyleProperty<bool?>(Paragraph.KeepOnOnePagePropertyDefinition, this);
			this.keepWithNextParagraphStyleProperty = new StyleProperty<bool?>(Paragraph.KeepWithNextParagraphPropertyDefinition, this);
			this.listIdStyleProperty = new StyleProperty<int?>(Paragraph.ListIdPropertyDefinition, this);
			this.listLevelStyleProperty = new StyleProperty<int?>(Paragraph.ListLevelPropertyDefinition, this);
			this.outlineLevelStyleProperty = new StyleProperty<OutlineLevel?>(Paragraph.OutlineLevelPropertyDefinition, this);
			this.applyEastAsianLineBreakingRulesStyleProperty = new StyleProperty<bool?>(Paragraph.ApplyEastAsianLineBreakingRulesPropertyDefinition, this);
			this.pageBreakBeforeStyleProperty = new StyleProperty<bool?>(Paragraph.PageBreakBeforePropertyDefinition, this);
			this.contextualSpacingStyleProperty = new StyleProperty<bool?>(Paragraph.ContextualSpacingPropertyDefinition, this);
			this.mirrorIndentsStyleProperty = new StyleProperty<bool?>(Paragraph.MirrorIndentsPropertyDefinition, this);
			this.spacingAfterStyleProperty = new StyleProperty<double?>(Paragraph.SpacingAfterPropertyDefinition, this);
			this.spacingBeforeStyleProperty = new StyleProperty<double?>(Paragraph.SpacingBeforePropertyDefinition, this);
			this.automaticSpacingBeforeStyleProperty = new StyleProperty<bool?>(Paragraph.AutomaticSpacingBeforePropertyDefinition, this);
			this.automaticSpacingAfterStyleProperty = new StyleProperty<bool?>(Paragraph.AutomaticSpacingAfterPropertyDefinition, this);
			this.bordersStyleProperty = new StyleProperty<ParagraphBorders>(Paragraph.BordersPropertyDefinition, this);
			this.backgroundColorStyleProperty = new StyleProperty<ThemableColor>(Paragraph.BackgroundColorPropertyDefinition, this);
			this.shadingPatternColorStyleProperty = new StyleProperty<ThemableColor>(Paragraph.ShadingPatternColorPropertyDefinition, this);
			this.shadingPatternStyleProperty = new StyleProperty<ShadingPattern?>(Paragraph.ShadingPatternPropertyDefinition, this);
			this.firstLineIndentStyleProperty = new StyleProperty<double?>(Paragraph.FirstLineIndentPropertyDefinition, this);
			this.hangingIndentStyleProperty = new StyleProperty<double?>(Paragraph.HangingIndentPropertyDefinition, this);
			this.leftIndentStyleProperty = new StyleProperty<double?>(Paragraph.LeftIndentPropertyDefinition, this);
			this.rightIndentStyleProperty = new StyleProperty<double?>(Paragraph.RightIndentPropertyDefinition, this);
			this.tabStopsStyleProperty = new TabStopCollectionStyleProperty(Paragraph.TabStopsPropertyDefinition, this);
			if (base.OwnerDocumentElement != null)
			{
				this.ParagraphMarkerProperties = new CharacterProperties(base.OwnerDocumentElement, true);
			}
			ParagraphProperties.InitStylePropertyGetters();
		}

		static Func<ParagraphProperties, IStyleProperty>[] stylePropertyGetters;

		LocalProperty<bool?> allowOverflowPunctuationStyleProperty;

		StyleProperty<FlowDirection?> flowDirectionStyleProperty;

		StyleProperty<double?> lineSpacingStyleProperty;

		LineSpacingTypeStyleProperty lineSpacingTypeStyleProperty;

		StyleProperty<bool?> keepOnOnePageStyleProperty;

		StyleProperty<bool?> keepWithNextParagraphStyleProperty;

		StyleProperty<int?> listIdStyleProperty;

		StyleProperty<int?> listLevelStyleProperty;

		StyleProperty<OutlineLevel?> outlineLevelStyleProperty;

		StyleProperty<bool?> applyEastAsianLineBreakingRulesStyleProperty;

		StyleProperty<bool?> pageBreakBeforeStyleProperty;

		StyleProperty<bool?> contextualSpacingStyleProperty;

		StyleProperty<bool?> mirrorIndentsStyleProperty;

		StyleProperty<Alignment?> textAlignmentStyleProperty;

		StyleProperty<bool?> automaticSpacingBeforeStyleProperty;

		StyleProperty<bool?> automaticSpacingAfterStyleProperty;

		StyleProperty<double?> spacingAfterStyleProperty;

		StyleProperty<double?> spacingBeforeStyleProperty;

		StyleProperty<ParagraphBorders> bordersStyleProperty;

		StyleProperty<ThemableColor> backgroundColorStyleProperty;

		StyleProperty<ThemableColor> shadingPatternColorStyleProperty;

		StyleProperty<ShadingPattern?> shadingPatternStyleProperty;

		StyleProperty<double?> firstLineIndentStyleProperty;

		StyleProperty<double?> hangingIndentStyleProperty;

		StyleProperty<double?> leftIndentStyleProperty;

		StyleProperty<double?> rightIndentStyleProperty;

		TabStopCollectionStyleProperty tabStopsStyleProperty;
	}
}
