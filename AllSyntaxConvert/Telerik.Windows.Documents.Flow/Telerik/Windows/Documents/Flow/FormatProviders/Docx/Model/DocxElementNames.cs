using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model
{
	static class DocxElementNames
	{
		public const string Document = "document";

		public const string Body = "body";

		public const string Paragraph = "p";

		public const string Run = "r";

		public const string Text = "t";

		public const string InstructionText = "instrText";

		public const string Table = "tbl";

		public const string TableProperties = "tblPr";

		public const string TableRow = "tr";

		public const string TableRowProperties = "trPr";

		public const string TableCell = "tc";

		public const string Header = "hdr";

		public const string Footer = "ftr";

		public const string FieldCharacter = "fldChar";

		public const string Hyperlink = "hyperlink";

		public const string SimpleField = "fldSimple";

		public const string BookmarkStart = "bookmarkStart";

		public const string BookmarkEnd = "bookmarkEnd";

		public const string Break = "br";

		public const string CommentRangeStart = "commentRangeStart";

		public const string CommentRangeEnd = "commentRangeEnd";

		public const string CommentReference = "commentReference";

		public const string PermissionStart = "permStart";

		public const string PermissionEnd = "permEnd";

		public const string Comments = "comments";

		public const string Comment = "comment";

		public const string Lists = "numbering";

		public const string AbstractList = "abstractNum";

		public const string NumberedList = "num";

		public const string AbstractListId = "abstractNumId";

		public const string NumStyleLink = "numStyleLink";

		public const string StyleLink = "styleLink";

		public const string MultiLevelType = "multiLevelType";

		public const string ListLevel = "lvl";

		public const string ListLevelOverride = "lvlOverride";

		public const string StartIndex = "start";

		public const string NumberingStyle = "numFmt";

		public const string StartIndexOverride = "startOverride";

		public const string RestartAfterLevel = "lvlRestart";

		public const string IsLegal = "isLgl";

		public const string NumberTextFormat = "lvlText";

		public const string ListLevelAlignment = "lvlJc";

		public const string Styles = "styles";

		public const string DocumentDefaultStyles = "docDefaults";

		public const string RunDefaultProperties = "rPrDefault";

		public const string ParagraphDefaultProperties = "pPrDefault";

		public const string Style = "style";

		public const string StyleName = "name";

		public const string IsPrimary = "qFormat";

		public const string NextStyleId = "next";

		public const string LinkedStyleId = "link";

		public const string UIPriority = "uiPriority";

		public const string BasedOnStyleId = "basedOn";

		public const string RunProperties = "rPr";

		public const string ParagraphProperties = "pPr";

		public const string SectionProperties = "sectPr";

		public const string TableCellProperties = "tcPr";

		public const string Left = "left";

		public const string Top = "top";

		public const string Right = "right";

		public const string Bottom = "bottom";

		public const string RunStyleId = "rStyle";

		public const string Font = "rFonts";

		public const string Bold = "b";

		public const string Italic = "i";

		public const string Strikethrough = "strike";

		public const string ForegroundColor = "color";

		public const string FontSize = "sz";

		public const string Highlight = "highlight";

		public const string Underline = "u";

		public const string Shading = "shd";

		public const string BaselineAlignment = "vertAlign";

		public const string RunFlowDirection = "rtl";

		public const string ParagraphStyleId = "pStyle";

		public const string KeepWithNextParagraph = "keepNext";

		public const string KeepOnOnePage = "keepLines";

		public const string PageBreakBefore = "pageBreakBefore";

		public const string NumberingProperties = "numPr";

		public const string ParagraphBorders = "pBdr";

		public const string TabStops = "tabs";

		public const string ApplyEastAsianLineBreakingRules = "kinsoku";

		public const string AllowOverflowPunctuation = "overflowPunct";

		public const string FlowDirection = "bidi";

		public const string Spacing = "spacing";

		public const string Indentation = "ind";

		public const string ContextualSpacing = "contextualSpacing";

		public const string MirrorIndents = "mirrorIndents";

		public const string Alignment = "jc";

		public const string OutlineLevel = "outlineLvl";

		public const string Tab = "tab";

		public const string ParagraphListLevel = "ilvl";

		public const string ListId = "numId";

		public const string TableBorders = "tblBorders";

		public const string TableTableCellSpacing = "tblCellSpacing";

		public const string TableTableCellPadding = "tblCellMar";

		public const string TableIndent = "tblInd";

		public const string RowBanding = "tblStyleRowBandSize";

		public const string ColumnBanding = "tblStyleColBandSize";

		public const string TablePreferredWidth = "tblW";

		public const string TableFlowDirection = "bidiVisual";

		public const string TableLayoutType = "tblLayout";

		public const string TableLooks = "tblLook";

		public const string Overlap = "tblOverlap";

		public const string IgnoreCellMarkerInRowHeightCalculation = "hideMark";

		public const string TableStyleId = "tblStyle";

		public const string RepeatOnEveryPage = "tblHeader";

		public const string CanSplit = "cantSplit";

		public const string TableRowHeight = "trHeight";

		public const string TableCellPreferredWidth = "tcW";

		public const string GridSpan = "gridSpan";

		public const string VerticalMerge = "vMerge";

		public const string TableCellBorders = "tcBorders";

		public const string CanWrapContent = "noWrap";

		public const string TableCellPadding = "tcMar";

		public const string HorizontalMerge = "hMerge";

		public const string TextDirection = "textDirection";

		public const string HeaderReference = "headerReference";

		public const string FooterReference = "footerReference";

		public const string TitlePage = "titlePg";

		public const string SectionType = "type";

		public const string PageMargins = "pgMar";

		public const string PageSize = "pgSz";

		public const string PageNumberingSettings = "pgNumType";

		public const string VerticalAlignment = "vAlign";

		public const string BorderBetween = "between";

		public const string BorderInsideHorizontal = "insideH";

		public const string BorderInsideVertical = "insideV";

		public const string BorderDiagonalUp = "tr2bl";

		public const string BorderDiagonalDown = "tl2br";

		public const string Drawing = "drawing";

		public const string Inline = "inline";

		public const string Anchor = "anchor";

		public const string SimplePositioning = "simplePos";

		public const string DrawingObjectNonVisualProperties = "docPr";

		public const string Extent = "extent";

		public const string HorizontalPosition = "positionH";

		public const string VerticalPosition = "positionV";

		public const string DrawingAlignment = "align";

		public const string Offset = "posOffset";

		public const string WrapNone = "wrapNone";

		public const string WrapSquare = "wrapSquare";

		public const string WrapTopAndBottom = "wrapTopAndBottom";

		public const string EffectExtent = "effectExtent";

		public const string VmlContainer = "pict";

		public const string Shape = "shape";

		public const string ImageData = "imagedata";

		public const string Textpath = "textpath";

		public const string Fill = "fill";

		public const string DocumentSettings = "settings";

		public const string EvenAndOddHeadersFooters = "evenAndOddHeaders";

		public const string UpdateFields = "updateFields";

		public const string Compatibility = "compat";

		public const string CompatibilitySetting = "compatSetting";

		public const string DocumentVariableCollection = "docVars";

		public const string DocumentVariable = "docVar";

		public const string DocumentProtection = "documentProtection";

		public const string DefaultTabStopWidth = "defaultTabStop";

		public const string StructuredDocumentTag = "sdt";

		public const string StructuredDocumentTagContent = "sdtContent";
	}
}
