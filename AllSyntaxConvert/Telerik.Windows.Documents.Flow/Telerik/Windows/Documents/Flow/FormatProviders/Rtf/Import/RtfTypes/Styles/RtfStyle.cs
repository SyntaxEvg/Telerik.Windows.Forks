using System;
using System.Windows;
using Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Borders;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Primitives;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf.Import.RtfTypes.Styles
{
	class RtfStyle
	{
		public RtfStyle(RadFlowDocument document)
		{
			this.StyleDefinitionInfo = new RtfStyleDefinitionInfo();
			this.Document = document;
			this.ParagraphStyle = new Paragraph(this.Document);
			this.SectionStyle = new Section(this.Document);
			this.DocumentSettings = new RtfDocumentSettings();
			this.ResetSectionStyle();
			this.ResetParagraphStyle();
			this.ResetSpanStyle();
			this.ResetRowStyle();
			this.ResetTabStop();
		}

		public RtfStyle(RtfStyle other)
			: this(other.Document)
		{
			this.IsHidden = other.IsHidden;
			this.CharacterStyle.Properties.CopyPropertiesFrom(other.CharacterStyle.Properties);
			this.ParagraphStyle.Properties.CopyPropertiesFrom(other.ParagraphStyle.Properties);
			this.ParagraphStyle.Properties.ParagraphMarkerProperties.CopyPropertiesFrom(other.CharacterStyle.Properties);
			this.SectionStyle.Properties.CopyPropertiesFrom(other.SectionStyle.Properties);
			this.IsInTable = other.IsInTable;
			this.CurrentTextWrapping = other.CurrentTextWrapping;
			this.CurrentNestingLevel = other.CurrentNestingLevel;
			this.RowStyle = other.RowStyle;
			this.CurrentBorderedElementType = other.CurrentBorderedElementType;
			this.CurrentListLevel = other.CurrentListLevel;
			this.StyleDefinitionInfo.CopyPropertiesFrom(other.StyleDefinitionInfo);
			this.ParagraphBorders = other.ParagraphBorders.CreateCopy();
			this.DocumentSettings = other.DocumentSettings;
			this.CurrentList = other.CurrentList;
		}

		public RtfParagraphBorder ParagraphBorders { get; set; }

		public int CurrentListLevel { get; set; }

		public bool IsHidden { get; set; }

		public Run CharacterStyle { get; set; }

		public Paragraph ParagraphStyle { get; set; }

		public Section SectionStyle { get; set; }

		public TableRowStyle RowStyle { get; set; }

		public RadFlowDocument Document { get; set; }

		public bool IsInTable { get; set; }

		public TextWrappingRestartLocation CurrentTextWrapping { get; set; }

		public int CurrentNestingLevel { get; set; }

		public BorderedElementType CurrentBorderedElementType { get; set; }

		public RtfBorder CurrentBorder { get; set; }

		public RtfTabStop CurrentTabStop { get; set; }

		public RtfDocumentSettings DocumentSettings { get; set; }

		public List CurrentList { get; set; }

		public RtfStyleDefinitionInfo StyleDefinitionInfo { get; set; }

		public void ResetSectionStyle()
		{
			this.SectionStyle.Properties.ClearLocalValues();
			this.SectionStyle.PageSize = new Size(this.DocumentSettings.PageWidth, this.DocumentSettings.PageHeight);
			this.SectionStyle.PageMargins = new Padding(this.DocumentSettings.LeftPageMargin, this.DocumentSettings.TopPageMargin, this.DocumentSettings.RightPageMargin, this.DocumentSettings.BottomPageMargin);
			this.SectionStyle.PageOrientation = this.DocumentSettings.PageOrientation;
			this.SectionStyle.VerticalAlignment = this.DocumentSettings.VerticalAlignment;
		}

		public void ResetParagraphStyle()
		{
			this.ParagraphStyle.Properties.ClearLocalValues();
			this.ParagraphStyle.Properties.ParagraphMarkerProperties.ClearLocalValues();
			this.ParagraphStyle.TextAlignment = Alignment.Left;
			this.ParagraphStyle.Spacing.LineSpacingType = HeightType.Auto;
			this.ParagraphStyle.Spacing.LineSpacing = 1.0;
			this.ParagraphStyle.Spacing.SpacingAfter = 0.0;
			this.ParagraphStyle.Spacing.SpacingBefore = 0.0;
			this.ParagraphStyle.Indentation.LeftIndent = 0.0;
			this.ParagraphStyle.Indentation.RightIndent = 0.0;
			this.ParagraphStyle.Indentation.FirstLineIndent = 0.0;
			this.ParagraphStyle.StyleId = null;
			this.IsInTable = false;
			this.CurrentNestingLevel = 0;
			this.ParagraphBorders = new RtfParagraphBorder();
			this.ParagraphStyle.TabStops = new TabStopCollection();
			this.CurrentList = null;
			this.CurrentListLevel = 0;
			this.ResetTabStop();
		}

		public void ResetSpanStyle()
		{
			this.CharacterStyle = new Run(this.Document);
			this.CurrentTextWrapping = TextWrappingRestartLocation.NextLine;
			this.IsHidden = false;
		}

		public void ResetRowStyle()
		{
			this.RowStyle = new TableRowStyle();
		}

		public void ResetTabStop()
		{
			this.CurrentTabStop = new RtfTabStop();
		}

		public void ApplyCharacterStyle(Run run)
		{
			run.Properties.CopyPropertiesFrom(this.CharacterStyle.Properties);
		}

		public void ApplyParagraphStyle(Paragraph paragraph)
		{
			paragraph.Properties.CopyPropertiesFrom(this.ParagraphStyle.Properties);
			if (this.ParagraphBorders.HasValue)
			{
				paragraph.Borders = this.ParagraphBorders.CreateBorders();
			}
			paragraph.Properties.ParagraphMarkerProperties.CopyPropertiesFrom(this.CharacterStyle.Properties);
			if (paragraph.FlowDirection == Telerik.Windows.Documents.Flow.Model.Styles.FlowDirection.RightToLeft)
			{
				if (paragraph.TextAlignment == Alignment.Left)
				{
					paragraph.TextAlignment = Alignment.Right;
					return;
				}
				if (paragraph.TextAlignment == Alignment.Right)
				{
					paragraph.TextAlignment = Alignment.Left;
				}
			}
		}

		public void ApplySectionStyle(Section section)
		{
			section.Properties.CopyPropertiesFrom(this.SectionStyle.Properties);
		}
	}
}
