using System;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Rtf
{
	static class RtfTags
	{
		public const string TagRtf = "rtf";

		public const string TagStyleSheet = "stylesheet";

		public const string TagThemeData = "themedata";

		public const string TagColorSchemeMapping = "colorschememapping";

		public const string TagFileTable = "filetbl";

		public const string TagDefaultCharacterProperties = "defchp";

		public const string TagDefaultParagraphProperties = "defpap";

		public const string TagListTable = "listtable";

		public const string TagListOverrideTable = "listoverridetable";

		public const string TagParagraphGroupProperties = "pgptbl";

		public const string TagRevisionMarksTable = "revtbl";

		public const string TagRevisionSaveIdTable = "rsidtbl";

		public const string TagUserProtectionInfoTable = "protusertbl";

		public const string TagGenerator = "generator";

		public const string TagDefaultLanguage = "deflang";

		public const string TagEncodingAnsi = "ansi";

		public const string TagEncodingMac = "mac";

		public const string TagEncodingPc = "pc";

		public const string TagEncodingPca = "pca";

		public const string TagEncodingAnsiCodePage = "ansicpg";

		public const int SymbolFakeCodePage = 42;

		public const string TagUnicodeSkipCount = "uc";

		public const string TagUnicodeCode = "u";

		public const string TagUnicodeAlternativeChoices = "upr";

		public const string TagUnicodeAlternativeUnicode = "ud";

		public const string TagFontTable = "fonttbl";

		public const string TagDefaultFont = "deff";

		public const string TagDefaultBiDiFont = "adeff";

		public const string TagFont = "f";

		public const string TagFontAlt = "falt";

		public const string TagFontKindNil = "fnil";

		public const string TagFontKindRoman = "froman";

		public const string TagFontKindSwiss = "fswiss";

		public const string TagFontKindModern = "fmodern";

		public const string TagFontKindScript = "fscript";

		public const string TagFontKindDecor = "fdecor";

		public const string TagFontKindTech = "ftech";

		public const string TagFontKindBidi = "fbidi";

		public const string TagFontCharset = "fcharset";

		public const string TagFontPitch = "fprq";

		public const string TagFontDown = "dn";

		public const string TagFontUp = "up";

		public const string TagThemeFontLoMajor = "flomajor";

		public const string TagThemeFontHiMajor = "fhimajor";

		public const string TagThemeFontDbMajor = "fdbmajor";

		public const string TagThemeFontBiMajor = "fbimajor";

		public const string TagThemeFontLoMinor = "flominor";

		public const string TagThemeFontHiMinor = "fhiminor";

		public const string TagThemeFontDbMinor = "fdbminor";

		public const string TagThemeFontBiMinor = "fbiminor";

		public const int DefaultFontSize = 24;

		public const string TagCodePage = "cpg";

		public const string TagColorTable = "colortbl";

		public const string TagColorRed = "red";

		public const string TagColorGreen = "green";

		public const string TagColorBlue = "blue";

		public const string TagColorForeground = "cf";

		public const string TagColorBackground = "cb";

		public const string TagColorBackgroundWord = "chcbpat";

		public const string TagColorHighlight = "highlight";

		public const char TagDelimiterChar = ';';

		public const string TagDelimiter = ";";

		public const string TagExtensionDestination = "*";

		public const string TagTilde = "~";

		public const string TagHyphen = "-";

		public const string TagNonBreakingHyphen = "_";

		public const string TagPageBreak = "page";

		public const string TagLineBreak = "line";

		public const string TagColumnBreak = "column";

		public const string TagLineBreakTextWrapping = "lbr";

		public const string TagTabulator = "tab";

		public const string TagEmDash = "emdash";

		public const string TagEnDash = "endash";

		public const string TagEmSpace = "emspace";

		public const string TagEnSpace = "enspace";

		public const string TagQmSpace = "qmspace";

		public const string TagBulltet = "bullet";

		public const string TagLeftSingleQuote = "lquote";

		public const string TagRightSingleQuote = "rquote";

		public const string TagLeftDoubleQuote = "ldblquote";

		public const string TagRightDoubleQuote = "rdblquote";

		public const string TagInfo = "info";

		public const string TagInfoVersion = "version";

		public const string TagInfoRevision = "vern";

		public const string TagInfoNumberOfPages = "nofpages";

		public const string TagInfoNumberOfWords = "nofwords";

		public const string TagInfoNumberOfChars = "nofchars";

		public const string TagInfoId = "id";

		public const string TagInfoTitle = "title";

		public const string TagInfoSubject = "subject";

		public const string TagInfoAuthor = "author";

		public const string TagInfoManager = "manager";

		public const string TagInfoCompany = "company";

		public const string TagInfoOperator = "operator";

		public const string TagInfoCategory = "category";

		public const string TagInfoKeywords = "keywords";

		public const string TagInfoComment = "comment";

		public const string TagInfoDocumentComment = "doccomm";

		public const string TagInfoHyperLinkBase = "hlinkbase";

		public const string TagInfoCreationTime = "creatim";

		public const string TagInfoRevisionTime = "revtim";

		public const string TagInfoPrintTime = "printim";

		public const string TagInfoBackupTime = "buptim";

		public const string TagInfoYear = "yr";

		public const string TagInfoMonth = "mo";

		public const string TagInfoDay = "dy";

		public const string TagInfoHour = "hr";

		public const string TagInfoMinute = "min";

		public const string TagInfoSecond = "sec";

		public const string TagInfoEditingTimeMinutes = "edmins";

		public const string TagUserProperties = "userprops";

		public const string TagUserPropertyType = "proptype";

		public const string TagUserPropertyName = "propname";

		public const string TagUserPropertyValue = "staticval";

		public const string TagUserPropertyLink = "linkval";

		public const string TagLayoutMode = "viewkind";

		public const string TagDocumentPageWidth = "paperw";

		public const string TagDocumentPageHeight = "paperh";

		public const string TagDocumentPageMarginLeft = "margl";

		public const string TagDocumentPageMarginRight = "margr";

		public const string TagDocumentPageMarginTop = "margt";

		public const string TagDocumentPageMarginBottom = "margb";

		public const string TagDocumentLandscapeMode = "landscape";

		public const string TagDocumentTabDefaultWidth = "deftab";

		public const string TagSectionDefaults = "sectd";

		public const string TagSectionWidth = "pgwsxn";

		public const string TagSectionHeight = "pghsxn";

		public const string TagSectionMarginLeft = "marglsxn";

		public const string TagSectionMarginRight = "margrsxn";

		public const string TagSectionMarginTop = "margtsxn";

		public const string TagSectionMarginBottom = "margbsxn";

		public const string TagSectionLandscapeMode = "lndscpsxn";

		public const string TagSectionStartingPageNumber = "pgnstarts";

		public const string TagSectionRestartPageNumberingFromStartingPageNumber = "pgnrestart";

		public const string TagSectionChapterHeadingStyle = "pgnhn";

		public const string TagSectionChapterSeparatorCharacterHyphen = "pgnhnsh";

		public const string TagSectionChapterSeparatorCharacterPeriod = "pgnhnsp";

		public const string TagSectionChapterSeparatorCharacterColon = "pgnhnsc";

		public const string TagSectionChapterSeparatorCharacterEmDash = "pgnhnsm";

		public const string TagSectionChapterSeparatorCharacterEnDash = "pgnhnsn";

		public const string TagSectionVerticalAlignmentTop = "vertalt";

		public const string TagSectionVerticalAlignmentCenter = "vertalc";

		public const string TagSectionVerticalAlignmentJustified = "vertalj";

		public const string TagSectionVerticalAlignmentBottom = "vertalb";

		public const string TagSectionVerticalAlignmentBottomAlias = "vertal";

		public const string TagSectionBreakNone = "sbknone";

		public const string TagSectionBreakColumn = "sbkcol";

		public const string TagSectionBreakNewPage = "sbkpage";

		public const string TagSectionBreakEvenPage = "sbkeven";

		public const string TagSectionBreakOddPage = "sbkodd";

		public const string TagSectionPageNumberFormatDecimal = "pgndec";

		public const string TagSectionPageNumberFormatUppercaseRomanNumeral = "pgnucrm";

		public const string TagSectionPageNumberFormatLowerRomanNumeral = "pgnlcrm";

		public const string TagSectionPageNumberFormatUppercaseLetter = "pgnucltr";

		public const string TagSectionPageNumberFormatLowercaseLetter = "pgnlcltr";

		public const string TagSectionPageNumberFormatKoreanNumbering = "pgnchosung";

		public const string TagSectionPageNumberFormatKoreanNumbering2 = "pgnganada";

		public const string TagSectionPageNumberFormatCircleNumbering = "pgncnum";

		public const string TagSectionPageNumberFormatArabicAbjad = "pgnbidia";

		public const string TagSectionPageNumberFormatHindiVowel = "pgnhindia";

		public const string TagSectionPageNumberFormatHindiConsonants = "pgnhindib";

		public const string TagSectionPageNumberFormatHindiNumbers = "pgnhindic";

		public const string TagSectionPageNumberFormatHindiCounting = "pgnhindid";

		public const string TagSectionPageNumberFormatThaiLetters = "pgnthaia";

		public const string TagSectionPageNumberFormatThaiNumbers = "pgnthaib";

		public const string TagSectionPageNumberFormatThaiCounting = "pgnthaic";

		public const string TagSectionPageNumberFormatVietnameseCounting = "pgnvieta";

		public const string TagParagraphDefaults = "pard";

		public const string TagAlignLeft = "ql";

		public const string TagAlignCenter = "qc";

		public const string TagAlignRight = "qr";

		public const string TagAlignJustify = "qj";

		public const string TagAlignDistribute = "qd";

		public const string TagAlignKashida = "qk";

		public const string TagAlignThai = "qt";

		public const string TagParagraphFirstLineIndent = "fi";

		public const string TagParagraphFistLineIndentInCharUnits = "cufi";

		public const string TagParagraphLeftIndent = "li";

		public const string TagParagraphLeftIndentBiDirectional = "lin";

		public const string TagParagraphLeftIndentInCharUnits = "culi";

		public const string TagParagraphRightIndent = "ri";

		public const string TagParagraphRightIndentBiDirectional = "rin";

		public const string TagParagraphRightIndentInCharUnits = "curi";

		public const string TagParagraphSpacingBefore = "sb";

		public const string TagParagraphAutomaticSpacingBefore = "sbauto";

		public const string TagParagraphSpacingAfter = "sa";

		public const string TagParagraphAutomaticSpacingAfter = "saauto";

		public const string TagParagraphSpacingBeforeInCharUnits = "lisb";

		public const string TagParagraphSpacingAfterInCharUnits = "lisa";

		public const string TagParagraphLineSpacing = "sl";

		public const string TagParagraphLineSpacingType = "slmult";

		public const string TagParagraphShadingBackground = "cbpat";

		public const string TagParagraphShadingForeground = "cfpat";

		public const string TagParagraphShadingPercent = "shading";

		public const string TagParagraphBorderLeft = "brdrl";

		public const string TagParagraphBorderTop = "brdrt";

		public const string TagParagraphBorderRight = "brdrr";

		public const string TagParagraphBorderBottom = "brdrb";

		public const string TagParagraphBorderBetween = "brdrbtw";

		public const string TagTabStopPosition = "tx";

		public const string TagTabStopRight = "tqr";

		public const string TagTabStopCenter = "tqc";

		public const string TagTabStopDecimal = "tqdec";

		public const string TagTabStopBarPosition = "tb";

		public const string TagTabStopLeaderDots = "tldot";

		public const string TagTabStopLeaderMiddleDots = "tlmdot";

		public const string TagTabStopLeaderHyphens = "tlhyph";

		public const string TagTabStopLeaderUnderline = "tlul";

		public const string TagTabStopLeaderThickLine = "tlth";

		public const string TagResetFormatting = "plain";

		public const string TagBold = "b";

		public const string TagItalic = "i";

		public const string TagStrikethrough = "strike";

		public const string TagDoubleStrikethrough = "striked";

		public const string TagSpanHighlight = "cb";

		public const string TagSpanHighlightWord = "highlight";

		public const string TagSpanForecolor = "cf";

		public const string TagFontSize = "fs";

		public const string TagSubscript = "sub";

		public const string TagSuperscript = "super";

		public const string TagResetBaselineAlignement = "nosupersub";

		public const string TagSpanStyle = "cs";

		public const string TagUnderlineColor = "ulc";

		public const string TagUnderlineNone = "ulnone";

		public const string TagUnderlineLine = "ul";

		public const string TagUnderlineDotted = "uld";

		public const string TagUnderlineDashed = "uldash";

		public const string TagUnderlineDashDotted = "uldashd";

		public const string TagUnderlineDashDotDotted = "uldashdd";

		public const string TagUnderlineDouble = "uldb";

		public const string TagUnderlineHeavyWave = "ulhwave";

		public const string TagUnderlineLongDashed = "ulldash";

		public const string TagUnderlineThick = "ulth";

		public const string TagUnderlineThickDotted = "ulthd";

		public const string TagUnderlineThickDashed = "ulthdash";

		public const string TagUnderlineThickDashDotted = "ulthdashd";

		public const string TagUnderlineThickDashDotDotted = "ulthdashdd";

		public const string TagUnderlineThickLongDashed = "ulthldash";

		public const string TagUnderlineDoubleWave = "ululdbwave";

		public const string TagUnderlineWord = "ulw";

		public const string TagUnderlineWave = "ulwave";

		public const string TagHidden = "v";

		public const string TagSpanShadingForeground = "chcfpat";

		public const string TagSpanShadingBackground = "chcbpat";

		public const string TagSpanShadingPercent = "chshdng";

		public const string TagNoNestedTablesDestenation = "nonesttables";

		public const string TagSectionEnd = "sect";

		public const string TagParagraphEnd = "par";

		public const string TagCellEnd = "cell";

		public const string TagRowEnd = "row";

		public const string TagLastRow = "lastrow";

		public const string TagNestedRowEnd = "nestrow";

		public const string TagNestedCellEnd = "nestcell";

		public const string TagRowIndex = "irow";

		public const string TagRowIndexHeaderAdjusted = "irowband";

		public const string TagParagraphInTable = "intbl";

		public const string TagParagraphNestingLevel = "itap";

		public const string TagRowDefaults = "trowd";

		public const string TagNestedTablePropertiesDestenation = "nesttableprops";

		public const string TagTableHeader = "trhdr";

		public const string TagFirstColumnSpannedCell = "clmgf";

		public const string TagColumnSpannedCell = "clmrg";

		public const string TagFirstRowSpannedCell = "clvmgf";

		public const string TagRowSpannedCell = "clvmrg";

		public const string TagRowPosition = "trleft";

		public const string TagRowAlignemntCenter = "trqc";

		public const string TagRowAlignemntLeft = "trql";

		public const string TagRowAlignemntRight = "trqr";

		public const string TagRowHeight = "trrh";

		public const string TagRowAutoFit = "trautofit";

		public const string TagRowPreferredWidth = "trwWidth";

		public const string TagTableRowBanding = "tscbandsh";

		public const string TagTableColumnBanding = "tscbandsv";

		public const string TagRowPreferredWidthUnit = "trftsWidth";

		public const string TagSpaceBetweenCells = "trgaph";

		public const string TagRowDefaultCellPaddingLeft = "trpaddl";

		public const string TagRowDefaultCellPaddingTop = "trpaddt";

		public const string TagRowDefaultCellPaddingRight = "trpaddr";

		public const string TagRowDefaultCellPaddingBottom = "trpaddb";

		public const string TagRowDefaultCellPaddingUnitLeft = "trpaddfl";

		public const string TagRowDefaultCellPaddingUnitTop = "trpaddft";

		public const string TagRowDefaultCellPaddingUnitRight = "trpaddfr";

		public const string TagRowDefaultCellPaddingUnitBottom = "trpaddfb";

		public const string TagRowCellSpacingLeft = "trspdl";

		public const string TagRowCellSpacingTop = "trspdt";

		public const string TagRowCellSpacingRight = "trspdr";

		public const string TagRowCellSpacingBottom = "trspdb";

		public const string TagRowCellSpacingUnitLeft = "trspdfl";

		public const string TagRowCellSpacingUnitTop = "trspdft";

		public const string TagRowCellSpacingUnitRight = "trspdfr";

		public const string TagRowCellSpacingUnitBottom = "trspdfb";

		public const string TagRowNoSplit = "trkeep";

		public const string TagRowRepeatOnEveryPage = "trhdr";

		public const string TagCellPaddingLeft = "clpadt";

		public const string TagCellPaddingTop = "clpadl";

		public const string TagCellPaddingRight = "clpadr";

		public const string TagCellPaddingBottom = "clpadb";

		public const string TagCellPaddingUnitLeft = "clpadft";

		public const string TagCellPaddingUnitTop = "clpadfl";

		public const string TagCellPaddingUnitRight = "clpadfr";

		public const string TagCellPaddingUnitBottom = "clpadfb";

		public const string TagTableIndent = "tblind";

		public const string TagTableIndentType = "tblindtype";

		public const string TagTableLookFirstRow = "tbllkhdrrows";

		public const string TagTableLookLastRow = "tbllklastrow";

		public const string TagTableLookFirstColumn = "tbllkhdrcols";

		public const string TagTableLookLastColumn = "tbllklastcol";

		public const string TagTableLookNoBandedRows = "tbllknorowband";

		public const string TagTableLookNoBandedColumns = "tbllknocolband";

		public const string TagRowShadingBackground = "trcbpat";

		public const string TagRowShadingForeground = "trcfpat";

		public const string TagRowShadingPercent = "trshdng";

		public const string TagCellDefaults = "tcelld";

		public const string TagCellNoWrapText = "clNoWrap";

		public const string TagCellIgnoreCellMarkerInRowHeight = "clhidemark";

		public const string TagCellContentAlignmentTop = "clvertalt";

		public const string TagCellContentAlignmentCenter = "clvertalc";

		public const string TagCellContentAlignmentBottom = "clvertalb";

		public const string TagCellContentAlignmentTopInStyle = "tsvertalt";

		public const string TagCellContentAlignmentCenterInStyle = "tsvertalc";

		public const string TagCellContentAlignmentBottomInStyle = "tsvertalb";

		public const string TagCellBackground = "clcbpat";

		public const string TagCellBackgroundWithStyles = "clcbpatraw";

		public const string TagCellBackgroundInStyle = "tscellcbpat";

		public const string TagCellForeground = "clcfpat";

		public const string TagCellForegroundWithStyles = "clcfpatraw";

		public const string TagCellForegroundInStyle = "tscellcfpat";

		public const string TagCellShadingPercent = "clshdng";

		public const string TagCellShadingPercentWithStyles = "clshdngraw";

		public const string TagCellShadingPercentInStyles = "tscellpct";

		public const string TagCellRightBoundry = "cellx";

		public const string TagCellFirstInHorizontalRange = "clmgf";

		public const string TagCellInHorizontalRange = "clmrg";

		public const string TagCellFirstInVerticalRange = "clvmgf";

		public const string TagCellInVerticalRange = "clvmrg";

		public const string TagCellPreferredWidth = "clwWidth";

		public const string TagCellPreferredWidthUnit = "clftsWidth";

		public const string TagCellTextFlowLeftRightTopBottom = "cltxlrtb";

		public const string TagCellTextFlowRightLeftTopBottom = "cltxtbrl";

		public const string TagCellTextFlowLeftRightBottomTop = "cltxbtlr";

		public const string TagCellTextFlowLeftRightTopBottomVertical = "cltxlrtbv";

		public const string TagCellTextFlowRightLeftTopBottomVertical = "cltxtbrlv";

		public const string TagRowBordersTop = "trbrdrt";

		public const string TagRowBordersLeft = "trbrdrl";

		public const string TagRowBordersBottom = "trbrdrb";

		public const string TagRowBordersRight = "trbrdrr";

		public const string TagRowBordersInnerHorizontal = "trbrdrh";

		public const string TagRowBordersInnerVertical = "trbrdrv";

		public const string TagCellBorderNone = "brdrnil";

		public const string TagCellBorderTop = "clbrdrt";

		public const string TagCellBorderLeft = "clbrdrl";

		public const string TagCellBorderBottom = "clbrdrb";

		public const string TagCellBorderRight = "clbrdrr";

		public const string TagCellBorderDiagonalUpperLeft = "cldglu";

		public const string TagCellBorderDiagonalLowerLeft = "cldgll";

		public const string TagBorderTypeBox = "box";

		public const string TagBorderTypeSingleThickness = "brdrs";

		public const string TagBorderTypeDoubleThickness = "brdrth";

		public const string TagBorderTypeShadowed = "brdrsh";

		public const string TagBorderTypeDouble = "brdrdb";

		public const string TagBorderTypeDotted = "brdrdot";

		public const string TagBorderTypeDashed = "brdrdash";

		public const string TagBorderTypeHairline = "brdrhair";

		public const string TagBorderTypeDashedSmall = "brdrdashsm";

		public const string TagBorderTypeDotDashed = "brdrdashd";

		public const string TagBorderTypeDotDotDashed = "brdrdashdd";

		public const string TagBorderTypeDotDashedAlt = "brdrdashdot";

		public const string TagBorderTypeDotDotDashedAlt = "brdrdashdotdot";

		public const string TagBorderTypeInset = "brdrinset";

		public const string TagBorderTypeNoBorder = "brdrnone";

		public const string TagBorderTypeOutset = "brdroutset";

		public const string TagBorderTypeTriple = "brdrtriple";

		public const string TagBorderTypeThickThinS = "brdrtnthsg";

		public const string TagBorderTypeThinThickS = "brdrthtnsg";

		public const string TagBorderTypeThinThickThinS = "brdrtnthtnsg";

		public const string TagBorderTypeThickThinM = "brdrtnthmg";

		public const string TagBorderTypeThinThickM = "brdrthtnmg";

		public const string TagBorderTypeThinThickThinM = "brdrtnthtnmg";

		public const string TagBorderTypeThickThinL = "brdrtnthlg";

		public const string TagBorderTypeThinThickL = "brdrthtnlg";

		public const string TagBorderTypeThinThickThinL = "brdrtnthtnlg";

		public const string TagBorderTypeWavy = "brdrwavy";

		public const string TagBorderTypeDoubleWavy = "brdrwavydb";

		public const string TagBorderTypeStriped = "brdrdashdotstr";

		public const string TagBorderTypeEmbossed = "brdremboss";

		public const string TagBorderTypeEngraved = "brdrengrave";

		public const string TagBorderTypeFrame = "brdrframe";

		public const string TagBorderColor = "brdrcf";

		public const string TagBorderThickness = "brdrw";

		public const string TagPicture = "pict";

		public const string TagInlinePictureShapeProperties = "picprop";

		public const string TagIsWordArtShape = "defshp";

		public const string TagPictureWrapper = "shppict";

		public const string TagPictureWrapperAlternative = "nonshppict";

		public const string TagPictureFormatEmf = "emfblip";

		public const string TagPictureFormatPng = "pngblip";

		public const string TagPictureFormatJpg = "jpegblip";

		public const string TagPictureFormatQuickDraw = "macpict";

		public const string TagPictureFormatWinDib = "dibitmap";

		public const string TagPictureFormatWinBmp = "wbitmap";

		public const string TagPictureFormatOs2Metafile = "pmmetafile";

		public const string TagPictureFormatWinMetafile = "wmetafile";

		public const string TagBinary = "bin";

		public const string TagPictureWidth = "picw";

		public const string TagPictureHeight = "pich";

		public const string TagPictureWidthGoal = "picwgoal";

		public const string TagPictureHeightGoal = "pichgoal";

		public const string TagPictureScaleX = "picscalex";

		public const string TagPictureScaleY = "picscaley";

		public const string TagNoUICompatibilityTag = "nouicompat";

		public const string TagShape = "shp";

		public const string TagShapeInstructions = "shpinst";

		public const string TagShapeResultDestination = "shprslt";

		public const string TagShapeTop = "shptop";

		public const string TagShapeLeft = "shpleft";

		public const string TagShapeBottom = "shpbottom";

		public const string TagShapeRight = "shpright";

		public const string TagShapeZOrder = "shpz";

		public const string TagShapeIsInHeader = "shpfhdr";

		public const string TagShapeHorizontalRelativeToPage = "shpbxpage";

		public const string TagShapeHorizontalRelativeToMargin = "shpbxmargin";

		public const string TagShapeHorizontalRelativeToColumn = "shpbxcolumn";

		public const string TagShapeHorizontalRelativeToIgnore = "shpbxignore";

		public const string TagShapeVerticalRelativeToPage = "shpbypage";

		public const string TagShapeVerticalRelativeToMargin = "shpbymargin";

		public const string TagShapeVerticalRelativeToParagraph = "shpbypara";

		public const string TagShapeVerticalRelativeToIgnore = "shpbyignore";

		public const string TagShapeWrappingType = "shpwr";

		public const string TagShapeTextWrap = "shpwrk";

		public const string TagShapeIsBelowText = "shpfblwtxt";

		public const string TagShapePopertyDestenation = "sp";

		public const string TagShapePopertyNameDestenation = "sn";

		public const string TagShapePopertyValueDestenation = "sv";

		public const string ShapeTypeProperty = "shapeType";

		public const string ShapePictureDataProperty = "pib";

		public const string ShapeHorizontalPositionProperty = "posh";

		public const string ShapeHorizontalRelativeToPositionProperty = "posrelh";

		public const string ShapeVerticalPositionProperty = "posv";

		public const string ShapeVerticalRelativeToPositionProperty = "posrelv";

		public const string ShapeLayoutInCellProperty = "fLayoutInCell";

		public const string ShapeAllowOverlapProperty = "fAllowOverlap";

		public const string ShapeMarginLeftProperty = "dxWrapDistLeft";

		public const string ShapeMarginTopProperty = "dyWrapDistTop";

		public const string ShapeMarginRightProperty = "dxWrapDistRight";

		public const string ShapeMarginBottomProperty = "dyWrapDistBottom";

		public const string ShapeBehindDocumentProperty = "fBehindDocument";

		public const string ShapePseudoInlineProperty = "fPseudoInline";

		public const string ShapeRotationProperty = "rotation";

		public const string ShapeNameProperty = "wzName";

		public const string ShapeTextProperty = "gtextUNICODE";

		public const string ShapeTextSizeProperty = "gtextSize";

		public const string ShapeTextFontProperty = "gtextFont";

		public const string ShapeFillColorProperty = "fillColor";

		public const string ShapeFillOpacityProperty = "fillOpacity";

		public const string ShapeLineProperty = "fLine";

		public const string ShapeVerticalFlipProperty = "fFlipV";

		public const string ShapeHorizontalFlipProperty = "fFlipH";

		public const string ShapeZIndexProperty = "dhgt";

		public const string TagParagraphNumberText = "pntext";

		public const string TagListText = "listtext";

		public const string TagList = "list";

		public const string TagListId = "listid";

		public const string TagListTemplateId = "listtemplateid";

		public const string TagListSingleLevel = "listsimple";

		public const string TagListIsHybrid = "listhybrid";

		public const string TagListName = "listname";

		public const string TagListStyleId = "liststyleid";

		public const string TagListStyleName = "liststylename";

		public const string TagListLevel = "listlevel";

		public const string TagListLevelNumberingFormat = "levelnfc";

		public const string TagListLevelNumberingFormatAlt = "levelnfcn";

		public const string TagListLevelJustification = "leveljc";

		public const string TagListLevelJustificationAlt = "leveljcn";

		public const string TagListLevelFollowChar = "levelfollow";

		public const string TagListLevelStartIndex = "levelstartat";

		public const string TagListLevelRestartAfterLevel = "levelnorestart";

		public const string TagListLevelIsLegal = "levellegal";

		public const string TagListLevelText = "leveltext";

		public const string TagListLevelNumbers = "levelnumbers";

		public const string TagListLevelTemplateId = "leveltemplateid";

		public const string TagListOverride = "listoverride";

		public const string TagListOverrideCount = "listoverridecount";

		public const string TagListOverrideStartAt = "listoverridestartat";

		public const string TagListOverrideFormat = "listoverrideformat";

		public const string TagListOverrideLevelDestination = "lfolevel";

		public const string TagListOverrideId = "ls";

		public const string TagParagraphListLevel = "ilvl";

		public const string TagBookmarkStart = "bkmkstart";

		public const string TagBookmarkEnd = "bkmkend";

		public const string TagField = "field";

		public const string TagFieldInstructions = "fldinst";

		public const string TagFieldResult = "fldrslt";

		public const string TagFieldIsLocked = "fldlock";

		public const string TagFieldIsDirty = "flddirty";

		public const string TagDocumentVariable = "docvar";

		public const string CommentStart = "atrfstart";

		public const string CommentEnd = "atrfend";

		public const string CommentID = "atnid";

		public const string CommentAuthor = "atnauthor";

		public const string CommentDefinition = "annotation";

		public const string CommentTime = "atntime";

		public const string CommentDate = "atndate";

		public const string CommentParent = "atnparent";

		public const string CommentRef = "atnref";

		public const string CommentMarkerTag = "chatn";

		public const string TagHeaderAllPages = "header";

		public const string TagHeaderFirstPage = "headerf";

		public const string TagHeaderLeftPages = "headerl";

		public const string TagHeaderRightPages = "headerr";

		public const string TagFooterAllPages = "footer";

		public const string TagFooterFirstPage = "footerf";

		public const string TagFooterLeftPages = "footerl";

		public const string TagFooterRightPages = "footerr";

		public const string TagDifferentFirstHeaderFooter = "titlepg";

		public const string TagDifferentOddEvenHeaderFooter = "facingp";

		public const string TagHeaderTopMargin = "headery";

		public const string TagFooterBottomMargin = "footery";

		public const string TagFootnote = "footnote";

		public const string TagSpanRtl = "rtlch";

		public const string TagSpanLtr = "ltrch";

		public const string TagParagraphRtl = "rtlpar";

		public const string TagParagraphLtr = "ltrpar";

		public const string TagRowRtl = "rtlrow";

		public const string TagRowLtr = "ltrrow";

		public const string TagTableRtl = "taprtl";

		public const string TagStyleParagraph = "s";

		public const string TagStyleCharacter = "cs";

		public const string TagStyleTable = "ts";

		public const string TagStyleTableForParagraph = "yts";

		public const string TagStyleSection = "ds";

		public const string TagStyleRowDefaults = "tsrowd";

		public const string TagStyleBasedOn = "sbasedon";

		public const string TagStyleLinked = "slink";

		public const string TagStyleNextParagraphStyle = "snext";

		public const string TagStyleIsPrimary = "sqformat";

		public const string TagStyleAdditive = "additive";

		public const string TagStyleUIPriority = "spriority";

		public const int PropertyTypeInteger = 3;

		public const int PropertyTypeRealNumber = 5;

		public const int PropertyTypeDate = 64;

		public const int PropertyTypeBoolean = 11;

		public const int PropertyTypeText = 30;

		public static string TextWatermarkShapeName = "PowerPlusWaterMarkObject";

		public static string ImageWatermarkShapeName = "WordPictureWatermark";
	}
}
