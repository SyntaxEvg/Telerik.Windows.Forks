using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Flow.Model.Cloning;
using Telerik.Windows.Documents.Flow.Model.Collections;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Flow.Model
{
	[DebuggerDisplay("\\{Paragraph\\}: {GetChildrenText()}")]
	public sealed class Paragraph : BlockBase, IElementWithStyle, IElementWithProperties
	{
		public Paragraph(RadFlowDocument document)
			: base(document)
		{
			this.inlines = new InlineCollection(this);
			this.properties = new ParagraphProperties(this);
			this.Spacing = new ParagraphSpacing(this.Properties);
			this.Shading = new Shading(this.Properties);
			this.Indentation = new ParagraphIndentation(this.Properties);
		}

		public InlineCollection Inlines
		{
			get
			{
				return this.inlines;
			}
		}

		public string StyleId
		{
			get
			{
				return this.Properties.StyleId;
			}
			set
			{
				this.Properties.StyleId = value;
			}
		}

		DocumentElementPropertiesBase IElementWithProperties.Properties
		{
			get
			{
				return this.properties;
			}
		}

		public ParagraphProperties Properties
		{
			get
			{
				return this.properties;
			}
		}

		public FlowDirection FlowDirection
		{
			get
			{
				return this.Properties.FlowDirection.GetActualValue().Value;
			}
			set
			{
				this.Properties.FlowDirection.LocalValue = new FlowDirection?(value);
			}
		}

		public Alignment TextAlignment
		{
			get
			{
				return this.Properties.TextAlignment.GetActualValue().Value;
			}
			set
			{
				this.Properties.TextAlignment.LocalValue = new Alignment?(value);
			}
		}

		public ParagraphSpacing Spacing { get; set; }

		public bool KeepOnOnePage
		{
			get
			{
				return this.Properties.KeepOnOnePage.GetActualValue().Value;
			}
			set
			{
				this.Properties.KeepOnOnePage.LocalValue = new bool?(value);
			}
		}

		public bool KeepWithNextParagraph
		{
			get
			{
				return this.Properties.KeepWithNextParagraph.GetActualValue().Value;
			}
			set
			{
				this.Properties.KeepWithNextParagraph.LocalValue = new bool?(value);
			}
		}

		public OutlineLevel OutlineLevel
		{
			get
			{
				return this.Properties.OutlineLevel.GetActualValue().Value;
			}
			set
			{
				this.Properties.OutlineLevel.LocalValue = new OutlineLevel?(value);
			}
		}

		public bool ApplyEastAsianLineBreakingRules
		{
			get
			{
				return this.Properties.ApplyEastAsianLineBreakingRules.GetActualValue().Value;
			}
			set
			{
				this.Properties.ApplyEastAsianLineBreakingRules.LocalValue = new bool?(value);
			}
		}

		public bool PageBreakBefore
		{
			get
			{
				return this.Properties.PageBreakBefore.GetActualValue().Value;
			}
			set
			{
				this.Properties.PageBreakBefore.LocalValue = new bool?(value);
			}
		}

		public ParagraphBorders Borders
		{
			get
			{
				return this.Properties.Borders.GetActualValue();
			}
			set
			{
				this.Properties.Borders.LocalValue = value;
			}
		}

		public bool ContextualSpacing
		{
			get
			{
				return this.Properties.ContextualSpacing.GetActualValue().Value;
			}
			set
			{
				this.Properties.ContextualSpacing.LocalValue = new bool?(value);
			}
		}

		public bool MirrorIndents
		{
			get
			{
				return this.Properties.MirrorIndents.GetActualValue().Value;
			}
			set
			{
				this.Properties.MirrorIndents.LocalValue = new bool?(value);
			}
		}

		public TabStopCollection TabStops
		{
			get
			{
				return this.Properties.TabStops.GetActualValue();
			}
			set
			{
				this.Properties.TabStops.LocalValue = value;
			}
		}

		public Shading Shading { get; set; }

		public ParagraphIndentation Indentation { get; set; }

		public bool AllowOverflowPunctuation
		{
			get
			{
				return this.Properties.AllowOverflowPunctuation.GetActualValue().Value;
			}
			set
			{
				this.Properties.AllowOverflowPunctuation.LocalValue = new bool?(value);
			}
		}

		public int ListId
		{
			get
			{
				return this.Properties.ListId.GetActualValue().Value;
			}
			set
			{
				this.Properties.ListId.LocalValue = new int?(value);
			}
		}

		public int ListLevel
		{
			get
			{
				return this.Properties.ListLevel.GetActualValue().Value;
			}
			set
			{
				this.Properties.ListLevel.LocalValue = new int?(value);
			}
		}

		internal override IEnumerable<DocumentElementBase> Children
		{
			get
			{
				return this.Inlines;
			}
		}

		internal override DocumentElementType Type
		{
			get
			{
				return DocumentElementType.Paragraph;
			}
		}

		public Paragraph Clone()
		{
			return this.CloneInternal(null);
		}

		public Paragraph Clone(RadFlowDocument document)
		{
			Guard.ThrowExceptionIfNull<RadFlowDocument>(document, "document");
			return this.CloneInternal(document);
		}

		internal override DocumentElementBase CloneCore(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Paragraph paragraph = this.CloneWithoutChildren(cloneContext);
			paragraph.Inlines.AddClonedChildrenFrom(this.Inlines, cloneContext);
			if (cloneContext.CurrentSection == null)
			{
				paragraph.RemoveUnpairedAnnotationMarkers(cloneContext);
			}
			return paragraph;
		}

		internal Paragraph CloneWithoutChildren(CloneContext cloneContext)
		{
			Guard.ThrowExceptionIfNull<CloneContext>(cloneContext, "cloneContext");
			Paragraph paragraph = new Paragraph(cloneContext.Document);
			paragraph.Properties.CopyPropertiesFrom(this.Properties);
			if (cloneContext.RenamedStyles.ContainsKey(paragraph.StyleId))
			{
				paragraph.StyleId = cloneContext.RenamedStyles[paragraph.StyleId];
			}
			if (cloneContext.ReinitializedLists.ContainsKey(paragraph.ListId))
			{
				paragraph.ListId = cloneContext.ReinitializedLists[paragraph.ListId];
			}
			return paragraph;
		}

		Paragraph CloneInternal(RadFlowDocument document)
		{
			CloneContext cloneContext = new CloneContext(document ?? this.Document);
			return (Paragraph)this.CloneCore(cloneContext);
		}

		void RemoveUnpairedAnnotationMarkers(CloneContext cloneContext)
		{
			this.RemoveUnpairedFieldCharacters(cloneContext);
			this.RemoveUnpairedBookmarkRanges(cloneContext);
			this.RemoveUnpairedCommentRanges(cloneContext);
			this.RemoveUnpairedPermissionRanges(cloneContext);
		}

		void RemoveUnpairedCommentRanges(CloneContext cloneContext)
		{
			foreach (CommentRangeStart item in cloneContext.CommentContext.GetHangingAnnotationStarts())
			{
				this.Inlines.Remove(item);
			}
			foreach (CommentRangeEnd item2 in cloneContext.CommentContext.GetHangingAnnotationEnds())
			{
				this.Inlines.Remove(item2);
			}
		}

		void RemoveUnpairedBookmarkRanges(CloneContext cloneContext)
		{
			foreach (BookmarkRangeStart item in cloneContext.BookmarkContext.GetHangingAnnotationStarts())
			{
				this.Inlines.Remove(item);
			}
			foreach (BookmarkRangeEnd item2 in cloneContext.BookmarkContext.GetHangingAnnotationEnds())
			{
				this.Inlines.Remove(item2);
			}
		}

		void RemoveUnpairedFieldCharacters(CloneContext cloneContext)
		{
			foreach (FieldCharacter item in cloneContext.FieldContext.GetUnpairedFieldCharacters())
			{
				this.Inlines.Remove(item);
			}
		}

		void RemoveUnpairedPermissionRanges(CloneContext cloneContext)
		{
			foreach (PermissionRangeStart item in cloneContext.PermissionContext.GetHangingAnnotationStarts())
			{
				this.Inlines.Remove(item);
			}
			foreach (PermissionRangeEnd item2 in cloneContext.PermissionContext.GetHangingAnnotationEnds())
			{
				this.Inlines.Remove(item2);
			}
		}

		string GetChildrenText()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Run run in from i in this.Inlines
				where i is Run
				select i as Run)
			{
				stringBuilder.Append(run.Text);
			}
			return stringBuilder.ToString();
		}

		static Paragraph()
		{
			Paragraph.FirstLineIndentPropertyDefinition.Validation.AddRule(new ValidationRule<double>((double value) => value >= 0.0));
			Paragraph.HangingIndentPropertyDefinition.Validation.AddRule(new ValidationRule<double>((double value) => value >= 0.0));
		}

		readonly InlineCollection inlines;

		readonly ParagraphProperties properties;

		public static readonly StylePropertyDefinition<Alignment?> TextAlignmentPropertyDefinition = new StylePropertyDefinition<Alignment?>("TextAlignment", new Alignment?(DocumentDefaultStyleSettings.TextAlignment), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<double?> SpacingAfterPropertyDefinition = new StylePropertyDefinition<double?>("SpacingAfter", new double?(DocumentDefaultStyleSettings.SpacingAfter), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<double?> SpacingBeforePropertyDefinition = new StylePropertyDefinition<double?>("SpacingBefore", new double?(DocumentDefaultStyleSettings.SpacingBefore), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> AllowOverflowPunctuationPropertyDefinition = new StylePropertyDefinition<bool?>("AllowOverflowPunctuation", new bool?(DocumentDefaultStyleSettings.AllowOverflowPunctuation), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<ParagraphBorders> BordersPropertyDefinition = new StylePropertyDefinition<ParagraphBorders>("Borders", DocumentDefaultStyleSettings.ParagraphBorders, StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<FlowDirection?> FlowDirectionPropertyDefinition = new StylePropertyDefinition<FlowDirection?>("FlowDirection", new FlowDirection?(DocumentDefaultStyleSettings.FlowDirection), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<double?> LineSpacingPropertyDefinition = new StylePropertyDefinition<double?>("LineSpacing", new double?(DocumentDefaultStyleSettings.LineSpacing), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<HeightType?> LineSpacingTypePropertyDefinition = new StylePropertyDefinition<HeightType?>("LineSpacingType", new HeightType?(DocumentDefaultStyleSettings.LineSpacingType), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> KeepOnOnePagePropertyDefinition = new StylePropertyDefinition<bool?>("KeepOnOnePage", new bool?(DocumentDefaultStyleSettings.KeepOnOnePage), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> KeepWithNextParagraphPropertyDefinition = new StylePropertyDefinition<bool?>("KeepWithNextParagraph", new bool?(DocumentDefaultStyleSettings.KeepWithNextParagraph), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<int?> ListIdPropertyDefinition = new StylePropertyDefinition<int?>("ListId", new int?(DocumentDefaultStyleSettings.ListId), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<int?> ListLevelPropertyDefinition = new StylePropertyDefinition<int?>("ListLevel", new int?(DocumentDefaultStyleSettings.ListLevel), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<OutlineLevel?> OutlineLevelPropertyDefinition = new StylePropertyDefinition<OutlineLevel?>("OutlineLevel", new OutlineLevel?(DocumentDefaultStyleSettings.OutlineLevel), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> ApplyEastAsianLineBreakingRulesPropertyDefinition = new StylePropertyDefinition<bool?>("ApplyEastAsianLineBreakingRules", new bool?(DocumentDefaultStyleSettings.ApplyEastAsianLineBreakingRules), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> PageBreakBeforePropertyDefinition = new StylePropertyDefinition<bool?>("PageBreakBefore", new bool?(DocumentDefaultStyleSettings.PageBreakBefore), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> ContextualSpacingPropertyDefinition = new StylePropertyDefinition<bool?>("ContextualSpacing", new bool?(DocumentDefaultStyleSettings.ContextualSpacing), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> MirrorIndentsPropertyDefinition = new StylePropertyDefinition<bool?>("MirrorIndents", new bool?(DocumentDefaultStyleSettings.MirrorIndents), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> AutomaticSpacingBeforePropertyDefinition = new StylePropertyDefinition<bool?>("AutomaticSpacingBefore", new bool?(DocumentDefaultStyleSettings.AutomaticSpacingBefore), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<bool?> AutomaticSpacingAfterPropertyDefinition = new StylePropertyDefinition<bool?>("AutomaticSpacingAfter", new bool?(DocumentDefaultStyleSettings.AutomaticSpacingAfter), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<double?> FirstLineIndentPropertyDefinition = new StylePropertyDefinition<double?>("FirstLineIndent", new double?(DocumentDefaultStyleSettings.FirstLineIndent), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<double?> HangingIndentPropertyDefinition = new StylePropertyDefinition<double?>("HangingIndent", new double?(DocumentDefaultStyleSettings.HangingIndent), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<double?> LeftIndentPropertyDefinition = new StylePropertyDefinition<double?>("LeftIndent", new double?(DocumentDefaultStyleSettings.LeftIndent), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<double?> RightIndentPropertyDefinition = new StylePropertyDefinition<double?>("RightIndent", new double?(DocumentDefaultStyleSettings.RightIndent), StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<TabStopCollection> TabStopsPropertyDefinition = new StylePropertyDefinition<TabStopCollection>("TabStops", DocumentDefaultStyleSettings.TabStops, StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<ThemableColor> BackgroundColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("BackgroundColor", DocumentDefaultStyleSettings.ParagraphBackgroundColor, StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<ThemableColor> ShadingPatternColorPropertyDefinition = new StylePropertyDefinition<ThemableColor>("ShadingPatternColor", DocumentDefaultStyleSettings.ParagraphShadingPatternColor, StylePropertyType.Paragraph);

		public static readonly StylePropertyDefinition<ShadingPattern?> ShadingPatternPropertyDefinition = new StylePropertyDefinition<ShadingPattern?>("ShadingPattern", new ShadingPattern?(DocumentDefaultStyleSettings.ParagraphShadingPattern), StylePropertyType.Paragraph);
	}
}
