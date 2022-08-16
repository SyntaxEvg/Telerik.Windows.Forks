using System;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Documents.Media;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Primitives;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Flow.Model.Styles
{
	static class DocumentDefaultStyleSettings
	{
		public static readonly bool HasDifferentEvenOddPageHeadersFooters = false;

		public static readonly DocumentViewType ViewType = DocumentViewType.PrintLayout;

		public static readonly double DefaultTabStopWidth = Unit.TwipToDip(720.0);

		public static readonly ThemableFontFamily FontFamily = new ThemableFontFamily("Verdana");

		public static readonly double FontSize = Unit.PointToDip(10.0);

		public static readonly FontStyle FontStyle = FontStyles.Normal;

		public static readonly FontWeight FontWeight = FontWeights.Normal;

		public static readonly ThemableColor ForegroundColor = new ThemableColor(Colors.Black);

		public static readonly Color HighlightColor = Colors.Transparent;

		public static readonly BaselineAlignment BaselineAlignment = BaselineAlignment.Baseline;

		public static readonly bool Strikethrough = false;

		public static readonly ThemableColor BackgroundColor = new ThemableColor(Colors.Transparent);

		public static readonly ThemableColor ShadingPatternColor = new ThemableColor(Colors.Transparent);

		public static readonly ShadingPattern ShadingPattern = ShadingPattern.Clear;

		public static readonly ThemableColor UnderlineColor = new ThemableColor(Colors.Black, true);

		public static readonly UnderlinePattern UnderlinePattern = UnderlinePattern.None;

		public static readonly Alignment TextAlignment = Alignment.Left;

		public static readonly bool AllowOverflowPunctuation = true;

		public static readonly bool ApplyEastAsianLineBreakingRules = true;

		public static readonly double SpacingAfter = 0.0;

		public static readonly double SpacingBefore = 0.0;

		public static readonly double LineSpacing = 1.0;

		public static readonly HeightType LineSpacingType = HeightType.Auto;

		public static readonly ParagraphBorders ParagraphBorders = new ParagraphBorders();

		public static readonly FlowDirection FlowDirection = FlowDirection.LeftToRight;

		public static readonly bool KeepOnOnePage = false;

		public static readonly bool KeepWithNextParagraph = false;

		public static readonly int ListId = -1;

		public static readonly int ListLevel = -1;

		public static readonly OutlineLevel OutlineLevel = OutlineLevel.None;

		public static readonly bool PageBreakBefore = false;

		public static readonly bool ContextualSpacing = false;

		public static readonly bool MirrorIndents = false;

		public static readonly bool AutomaticSpacingAfter = false;

		public static readonly bool AutomaticSpacingBefore = false;

		public static readonly double FirstLineIndent = 0.0;

		public static readonly double HangingIndent = 0.0;

		public static readonly double LeftIndent = 0.0;

		public static readonly double RightIndent = 0.0;

		public static readonly ThemableColor ParagraphShadingPatternColor = new ThemableColor(Colors.Transparent);

		public static readonly ThemableColor ParagraphBackgroundColor = new ThemableColor(Colors.Transparent);

		public static readonly ShadingPattern ParagraphShadingPattern = ShadingPattern.Clear;

		public static readonly TabStopCollection TabStops = new TabStopCollection();

		public static readonly Alignment TableAlignment = Alignment.Left;

		public static readonly Padding TableTableCellPadding = Padding.Empty;

		public static readonly double TableTableCellSpacing = 0.0;

		public static readonly double TableIndent = 0.0;

		public static readonly int RowBanding = 0;

		public static readonly int ColumnBanding = 0;

		public static readonly TableBorders TableBorders = new TableBorders();

		public static readonly ThemableColor TableShadingPatternColor = new ThemableColor(Colors.Transparent);

		public static readonly ThemableColor TableBackgroundColor = new ThemableColor(Colors.Transparent);

		public static readonly ShadingPattern TableShadingPattern = ShadingPattern.Clear;

		public static readonly FlowDirection TableFlowDirection = FlowDirection.LeftToRight;

		public static readonly TableWidthUnit TablePreferredWidth = new TableWidthUnit(TableWidthUnitType.Auto);

		public static readonly TableLooks TableLooks = TableLooks.BandedRows | TableLooks.BandedColumns;

		public static readonly TableLayoutType TableLayoutType = TableLayoutType.AutoFit;

		public static readonly bool TableOverlap = true;

		public static readonly double TableRowTableCellSpacing = 0.0;

		public static readonly bool TableRowRepeatOnEveryPage = false;

		public static readonly TableRowHeight TableRowHeight = new TableRowHeight(HeightType.Auto);

		public static readonly bool TableRowCanSplit = true;

		public static readonly TableCellBorders TableCellBorders = new TableCellBorders();

		public static readonly ThemableColor TableCellShadingPatternColor = new ThemableColor(Colors.Transparent);

		public static readonly ThemableColor TableCellBackgroundColor = new ThemableColor(Colors.Transparent);

		public static readonly ShadingPattern TableCellShadingPattern = ShadingPattern.Clear;

		public static readonly int ColumnSpan = 1;

		public static readonly int RowSpan = 1;

		public static readonly bool IgnoreCellMarkerInRowHeightCalculation = false;

		public static readonly bool CanWrapContent = true;

		public static readonly Padding TableCellPadding = new Padding(Unit.InchToDip(0.08), 0.0, Unit.InchToDip(0.08), 0.0);

		public static readonly TableWidthUnit TableCellPreferredWidth = new TableWidthUnit(TableWidthUnitType.Auto);

		public static readonly VerticalAlignment TableCellVerticalAlignment = VerticalAlignment.Top;

		public static readonly TextDirection TableCellTextDirection = TextDirection.LeftToRightTopToBottom;

		public static readonly int UIPriority = int.MaxValue;

		public static readonly bool HasDifferentFirstPageHeaderFooter = false;

		public static readonly PageOrientation PageOrientation = PageOrientation.Portrait;

		public static readonly Size PageSize = PaperTypeConverter.ToSize(PaperTypes.Letter);

		public static readonly Padding PageMargins = new Padding(96.0);

		public static readonly double SectionHeaderTopMargin = 48.0;

		public static readonly double SectionFooterBottomMargin = 48.0;

		public static readonly SectionType SectionType = SectionType.NextPage;

		public static readonly VerticalAlignment VerticalAlignment = VerticalAlignment.Top;

		public static readonly Padding FloatingImageMargin = new Padding(12.0, 0.0, 12.0, 0.0);

		public static readonly int RestartAfterLevel = -1;

		public static readonly int StartIndex = 1;
	}
}
