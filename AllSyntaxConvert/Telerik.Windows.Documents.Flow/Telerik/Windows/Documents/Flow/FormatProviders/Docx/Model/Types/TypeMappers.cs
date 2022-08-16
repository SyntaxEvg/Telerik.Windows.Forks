using System;
using Telerik.Windows.Documents.Core.Data;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Fields;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Protection;
using Telerik.Windows.Documents.Flow.Model.Shapes;
using Telerik.Windows.Documents.Flow.Model.Styles;
using Telerik.Windows.Documents.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Docx.Model.Types
{
	static class TypeMappers
	{
		static TypeMappers()
		{
			TypeMappers.InitBaselineAlignmentMapper();
			TypeMappers.InitThemeColorTypeMapper();
			TypeMappers.InitShadingPatternMapper();
			TypeMappers.InitUnderlinePatternMapper();
			TypeMappers.InitTableCellMergeTypeMapper();
			TypeMappers.InitAlignmentMapper();
			TypeMappers.InitHeightTypeMapper();
			TypeMappers.InitOutlineLevelMapper();
			TypeMappers.InitBorderStyleMapper();
			TypeMappers.InitHeaderFooterTypeMapper();
			TypeMappers.InitTableWidthTypeMapper();
			TypeMappers.InitTableLayoutTypeMapper();
			TypeMappers.InitVerticalAlignmentMapper();
			TypeMappers.InitStyleTypeMapper();
			TypeMappers.InitSectionTypeMapper();
			TypeMappers.InitPageOrientationMapper();
			TypeMappers.InitFileldCharTypeMapper();
			TypeMappers.InitRelativeHorizontalAlignmentMapper();
			TypeMappers.InitRelativeVerticalAlignmentMapper();
			TypeMappers.InitHorizontalRelativeFromMapper();
			TypeMappers.InitVerticalRelativeFromMapper();
			TypeMappers.InitTextWrapMapper();
			TypeMappers.InitBreakTypeMapper();
			TypeMappers.InitRestartLocationMapper();
			TypeMappers.InitMultilevelTypeMapper();
			TypeMappers.InitNumberingStyleMapper();
			TypeMappers.InitChapterSeparatorTypeMapper();
			TypeMappers.InitProtectionModeTypeMapper();
			TypeMappers.InitProtectionAlgorithmsTypeMapper();
			TypeMappers.InitEditingGroupTypeMapper();
			TypeMappers.InitTabStopTypeMapper();
			TypeMappers.InitTabStopLeaderMapper();
			TypeMappers.InitTextDirectionMapper();
		}

		public static ValueMapper<string, BaselineAlignment> BaselineAlignmentMapper { get; set; }

		public static ValueMapper<string, ThemeColorType> ThemeColorTypeMapper { get; set; }

		public static ValueMapper<string, ShadingPattern> ShadingPatternMapper { get; set; }

		public static ValueMapper<string, TableCellMergeType> TableCellMergeTypeMapper { get; set; }

		public static ValueMapper<string, UnderlinePattern> UnderlinePatternMapper { get; set; }

		public static ValueMapper<string, Alignment> AlignmentMapper { get; set; }

		public static ValueMapper<string, HeightType> HeightTypeMapper { get; set; }

		public static ValueMapper<string, OutlineLevel> OutlineLevelMapper { get; set; }

		public static ValueMapper<string, BorderStyle> BorderStyleMapper { get; set; }

		public static ValueMapper<string, TableWidthUnitType> TableWidthTypeMapper { get; set; }

		public static ValueMapper<string, TableLayoutType> TableLayoutTypeMapper { get; set; }

		public static ValueMapper<string, HeaderFooterType> HeaderFooterTypeMapper { get; set; }

		public static ValueMapper<string, VerticalAlignment> VerticalAlignmentMapper { get; set; }

		public static ValueMapper<string, StyleType> StyleTypeMapper { get; set; }

		public static ValueMapper<string, SectionType> SectionTypeMapper { get; set; }

		public static ValueMapper<string, PageOrientation> PageOrientationMapper { get; set; }

		public static ValueMapper<string, FieldCharacterType> FileldCharTypeMapper { get; set; }

		public static ValueMapper<string, RelativeHorizontalAlignment> RelativeHorizontalAlignmentMapper { get; set; }

		public static ValueMapper<string, RelativeVerticalAlignment> RelativeVerticalAlignmentMapper { get; set; }

		public static ValueMapper<string, HorizontalRelativeFrom> HorizontalRelativeFromMapper { get; set; }

		public static ValueMapper<string, VerticalRelativeFrom> VerticalRelativeFromMapper { get; set; }

		public static ValueMapper<string, TextWrap> TextWrapMapper { get; set; }

		public static ValueMapper<string, BreakType> BreakTypeMapper { get; set; }

		public static ValueMapper<string, TextWrappingRestartLocation> RestartLocationMapper { get; set; }

		public static ValueMapper<string, MultilevelType> MultilevelTypeMapper { get; set; }

		public static ValueMapper<string, NumberingStyle> NumberingStyleMapper { get; set; }

		public static ValueMapper<string, ChapterSeparatorType> ChapterSeparatorTypeMapper { get; set; }

		public static ValueMapper<string, ProtectionMode> ProtectionModeTypeMapper { get; set; }

		public static ValueMapper<string, string> ProtectionAlgorithmTypeMapper { get; set; }

		public static ValueMapper<string, EditingGroup?> EditingGroupTypeMapper { get; set; }

		public static ValueMapper<string, TabStopType> TabStopTypeMapper { get; set; }

		public static ValueMapper<string, TabStopLeader> TabStopLeaderMapper { get; set; }

		public static ValueMapper<string, TextDirection> TextDirectionMapper { get; set; }

		static void InitShadingPatternMapper()
		{
			TypeMappers.ShadingPatternMapper = new ValueMapper<string, ShadingPattern>("clear", ShadingPattern.Clear);
			TypeMappers.ShadingPatternMapper.AddPair("nil", ShadingPattern.None);
			TypeMappers.ShadingPatternMapper.AddPair("clear", ShadingPattern.Clear);
			TypeMappers.ShadingPatternMapper.AddPair("solid", ShadingPattern.Solid);
			TypeMappers.ShadingPatternMapper.AddPair("horzStripe", ShadingPattern.HorizontalStripe);
			TypeMappers.ShadingPatternMapper.AddPair("vertStripe", ShadingPattern.VerticalStripe);
			TypeMappers.ShadingPatternMapper.AddPair("reverseDiagStripe", ShadingPattern.ReverseDiagonalStripe);
			TypeMappers.ShadingPatternMapper.AddPair("diagStripe", ShadingPattern.DiagonalStripe);
			TypeMappers.ShadingPatternMapper.AddPair("horzCross", ShadingPattern.HorizontalCross);
			TypeMappers.ShadingPatternMapper.AddPair("diagCross", ShadingPattern.DiagonalCross);
			TypeMappers.ShadingPatternMapper.AddPair("thinHorzStripe", ShadingPattern.ThinHorizontalStripe);
			TypeMappers.ShadingPatternMapper.AddPair("thinVertStripe", ShadingPattern.ThinVerticalStripe);
			TypeMappers.ShadingPatternMapper.AddPair("thinReverseDiagStripe", ShadingPattern.ThinReverseDiagonalStripe);
			TypeMappers.ShadingPatternMapper.AddPair("thinDiagStripe", ShadingPattern.ThinDiagonalStripe);
			TypeMappers.ShadingPatternMapper.AddPair("thinHorzCross", ShadingPattern.ThinHorizontalCross);
			TypeMappers.ShadingPatternMapper.AddPair("thinDiagCross", ShadingPattern.ThinDiagonalCross);
			TypeMappers.ShadingPatternMapper.AddPair("pct5", ShadingPattern.Percent5);
			TypeMappers.ShadingPatternMapper.AddPair("pct10", ShadingPattern.Percent10);
			TypeMappers.ShadingPatternMapper.AddPair("pct12", ShadingPattern.Percent12);
			TypeMappers.ShadingPatternMapper.AddPair("pct15", ShadingPattern.Percent15);
			TypeMappers.ShadingPatternMapper.AddPair("pct20", ShadingPattern.Percent20);
			TypeMappers.ShadingPatternMapper.AddPair("pct25", ShadingPattern.Percent25);
			TypeMappers.ShadingPatternMapper.AddPair("pct30", ShadingPattern.Percent30);
			TypeMappers.ShadingPatternMapper.AddPair("pct35", ShadingPattern.Percent35);
			TypeMappers.ShadingPatternMapper.AddPair("pct37", ShadingPattern.Percent37);
			TypeMappers.ShadingPatternMapper.AddPair("pct40", ShadingPattern.Percent40);
			TypeMappers.ShadingPatternMapper.AddPair("pct45", ShadingPattern.Percent45);
			TypeMappers.ShadingPatternMapper.AddPair("pct50", ShadingPattern.Percent50);
			TypeMappers.ShadingPatternMapper.AddPair("pct55", ShadingPattern.Percent55);
			TypeMappers.ShadingPatternMapper.AddPair("pct60", ShadingPattern.Percent60);
			TypeMappers.ShadingPatternMapper.AddPair("pct62", ShadingPattern.Percent62);
			TypeMappers.ShadingPatternMapper.AddPair("pct65", ShadingPattern.Percent65);
			TypeMappers.ShadingPatternMapper.AddPair("pct70", ShadingPattern.Percent70);
			TypeMappers.ShadingPatternMapper.AddPair("pct75", ShadingPattern.Percent75);
			TypeMappers.ShadingPatternMapper.AddPair("pct80", ShadingPattern.Percent80);
			TypeMappers.ShadingPatternMapper.AddPair("pct85", ShadingPattern.Percent85);
			TypeMappers.ShadingPatternMapper.AddPair("pct87", ShadingPattern.Percent87);
			TypeMappers.ShadingPatternMapper.AddPair("pct90", ShadingPattern.Percent90);
			TypeMappers.ShadingPatternMapper.AddPair("pct95", ShadingPattern.Percent95);
		}

		static void InitThemeColorTypeMapper()
		{
			TypeMappers.ThemeColorTypeMapper = new ValueMapper<string, ThemeColorType>();
			TypeMappers.ThemeColorTypeMapper.AddPair("accent1", ThemeColorType.Accent1);
			TypeMappers.ThemeColorTypeMapper.AddPair("accent2", ThemeColorType.Accent2);
			TypeMappers.ThemeColorTypeMapper.AddPair("accent3", ThemeColorType.Accent3);
			TypeMappers.ThemeColorTypeMapper.AddPair("accent4", ThemeColorType.Accent4);
			TypeMappers.ThemeColorTypeMapper.AddPair("accent5", ThemeColorType.Accent5);
			TypeMappers.ThemeColorTypeMapper.AddPair("accent6", ThemeColorType.Accent6);
			TypeMappers.ThemeColorTypeMapper.AddPair("background1", ThemeColorType.Background1);
			TypeMappers.ThemeColorTypeMapper.AddPair("background2", ThemeColorType.Background2);
			TypeMappers.ThemeColorTypeMapper.AddPair("followedHyperlink", ThemeColorType.FollowedHyperlink);
			TypeMappers.ThemeColorTypeMapper.AddPair("hyperlink", ThemeColorType.Hyperlink);
			TypeMappers.ThemeColorTypeMapper.AddPair("text1", ThemeColorType.Text1);
			TypeMappers.ThemeColorTypeMapper.AddPair("text2", ThemeColorType.Text2);
			TypeMappers.ThemeColorTypeMapper.AddPair("light1", ThemeColorType.Background1);
			TypeMappers.ThemeColorTypeMapper.AddPair("light2", ThemeColorType.Background2);
			TypeMappers.ThemeColorTypeMapper.AddPair("dark1", ThemeColorType.Text1);
			TypeMappers.ThemeColorTypeMapper.AddPair("dark2", ThemeColorType.Text2);
		}

		static void InitBaselineAlignmentMapper()
		{
			TypeMappers.BaselineAlignmentMapper = new ValueMapper<string, BaselineAlignment>("baseline", BaselineAlignment.Baseline);
			TypeMappers.BaselineAlignmentMapper.AddPair("baseline", BaselineAlignment.Baseline);
			TypeMappers.BaselineAlignmentMapper.AddPair("superscript", BaselineAlignment.Superscript);
			TypeMappers.BaselineAlignmentMapper.AddPair("subscript", BaselineAlignment.Subscript);
		}

		static void InitTableCellMergeTypeMapper()
		{
			TypeMappers.TableCellMergeTypeMapper = new ValueMapper<string, TableCellMergeType>("continue", TableCellMergeType.Continue);
			TypeMappers.TableCellMergeTypeMapper.AddPair("continue", TableCellMergeType.Continue);
			TypeMappers.TableCellMergeTypeMapper.AddPair("restart", TableCellMergeType.Restart);
		}

		static void InitUnderlinePatternMapper()
		{
			TypeMappers.UnderlinePatternMapper = new ValueMapper<string, UnderlinePattern>("none", UnderlinePattern.None);
			TypeMappers.UnderlinePatternMapper.AddPair("none", UnderlinePattern.None);
			TypeMappers.UnderlinePatternMapper.AddPair("single", UnderlinePattern.Single);
			TypeMappers.UnderlinePatternMapper.AddPair("words", UnderlinePattern.Words);
			TypeMappers.UnderlinePatternMapper.AddPair("double", UnderlinePattern.Double);
			TypeMappers.UnderlinePatternMapper.AddPair("thick", UnderlinePattern.Thick);
			TypeMappers.UnderlinePatternMapper.AddPair("dotted", UnderlinePattern.Dotted);
			TypeMappers.UnderlinePatternMapper.AddPair("dottedHeavy", UnderlinePattern.DottedHeavy);
			TypeMappers.UnderlinePatternMapper.AddPair("dash", UnderlinePattern.Dash);
			TypeMappers.UnderlinePatternMapper.AddPair("dashedHeavy", UnderlinePattern.DashedHeavy);
			TypeMappers.UnderlinePatternMapper.AddPair("dashLong", UnderlinePattern.DashLong);
			TypeMappers.UnderlinePatternMapper.AddPair("dashLongHeavy", UnderlinePattern.DashLongHeavy);
			TypeMappers.UnderlinePatternMapper.AddPair("dotDash", UnderlinePattern.DotDash);
			TypeMappers.UnderlinePatternMapper.AddPair("dashDotHeavy", UnderlinePattern.DashDotHeavy);
			TypeMappers.UnderlinePatternMapper.AddPair("dotDotDash", UnderlinePattern.DotDotDash);
			TypeMappers.UnderlinePatternMapper.AddPair("dashDotDotHeavy", UnderlinePattern.DashDotDotHeavy);
			TypeMappers.UnderlinePatternMapper.AddPair("wave", UnderlinePattern.Wave);
			TypeMappers.UnderlinePatternMapper.AddPair("wavyHeavy", UnderlinePattern.WavyHeavy);
			TypeMappers.UnderlinePatternMapper.AddPair("wavyDouble", UnderlinePattern.WavyDouble);
		}

		static void InitAlignmentMapper()
		{
			TypeMappers.AlignmentMapper = new ValueMapper<string, Alignment>();
			TypeMappers.AlignmentMapper.AddPair("center", Alignment.Center);
			TypeMappers.AlignmentMapper.AddPair("distribute", Alignment.Distribute);
			TypeMappers.AlignmentMapper.AddPair("highKashida", Alignment.HighKashida);
			TypeMappers.AlignmentMapper.AddPair("both", Alignment.Justified);
			TypeMappers.AlignmentMapper.AddPair("start", Alignment.Left);
			TypeMappers.AlignmentMapper.AddPair("left", Alignment.Left);
			TypeMappers.AlignmentMapper.AddPair("lowKashida", Alignment.LowKashida);
			TypeMappers.AlignmentMapper.AddPair("mediumKashida", Alignment.MediumKashida);
			TypeMappers.AlignmentMapper.AddPair("end", Alignment.Right);
			TypeMappers.AlignmentMapper.AddPair("right", Alignment.Right);
			TypeMappers.AlignmentMapper.AddPair("thaiDistribute", Alignment.ThaiDistribute);
		}

		static void InitHeightTypeMapper()
		{
			TypeMappers.HeightTypeMapper = new ValueMapper<string, HeightType>("auto", HeightType.Auto);
			TypeMappers.HeightTypeMapper.AddPair("atLeast", HeightType.AtLeast);
			TypeMappers.HeightTypeMapper.AddPair("auto", HeightType.Auto);
			TypeMappers.HeightTypeMapper.AddPair("exact", HeightType.Exact);
		}

		static void InitOutlineLevelMapper()
		{
			TypeMappers.OutlineLevelMapper = new ValueMapper<string, OutlineLevel>("9", OutlineLevel.None);
			TypeMappers.OutlineLevelMapper.AddPair("9", OutlineLevel.None);
			TypeMappers.OutlineLevelMapper.AddPair("0", OutlineLevel.Level1);
			TypeMappers.OutlineLevelMapper.AddPair("1", OutlineLevel.Level2);
			TypeMappers.OutlineLevelMapper.AddPair("2", OutlineLevel.Level3);
			TypeMappers.OutlineLevelMapper.AddPair("3", OutlineLevel.Level4);
			TypeMappers.OutlineLevelMapper.AddPair("4", OutlineLevel.Level5);
			TypeMappers.OutlineLevelMapper.AddPair("5", OutlineLevel.Level6);
			TypeMappers.OutlineLevelMapper.AddPair("6", OutlineLevel.Level7);
			TypeMappers.OutlineLevelMapper.AddPair("7", OutlineLevel.Level8);
			TypeMappers.OutlineLevelMapper.AddPair("8", OutlineLevel.Level9);
		}

		static void InitBorderStyleMapper()
		{
			TypeMappers.BorderStyleMapper = new ValueMapper<string, BorderStyle>();
			TypeMappers.BorderStyleMapper.AddPair("none", BorderStyle.None);
			TypeMappers.BorderStyleMapper.AddPair("nil", BorderStyle.None);
			TypeMappers.BorderStyleMapper.AddPair("single", BorderStyle.Single);
			TypeMappers.BorderStyleMapper.AddPair("dotted", BorderStyle.Dotted);
			TypeMappers.BorderStyleMapper.AddPair("dashed", BorderStyle.Dashed);
			TypeMappers.BorderStyleMapper.AddPair("dashSmallGap", BorderStyle.DashSmallGap);
			TypeMappers.BorderStyleMapper.AddPair("dotDash", BorderStyle.DotDash);
			TypeMappers.BorderStyleMapper.AddPair("dotDotDash", BorderStyle.DotDotDash);
			TypeMappers.BorderStyleMapper.AddPair("double", BorderStyle.Double);
			TypeMappers.BorderStyleMapper.AddPair("triple", BorderStyle.Triple);
			TypeMappers.BorderStyleMapper.AddPair("thick", BorderStyle.Thick);
			TypeMappers.BorderStyleMapper.AddPair("thickThinSmallGap", BorderStyle.ThickThinSmallGap);
			TypeMappers.BorderStyleMapper.AddPair("thickThinMediumGap", BorderStyle.ThickThinMediumGap);
			TypeMappers.BorderStyleMapper.AddPair("thickThinLargeGap", BorderStyle.ThickThinLargeGap);
			TypeMappers.BorderStyleMapper.AddPair("thinThickSmallGap", BorderStyle.ThinThickSmallGap);
			TypeMappers.BorderStyleMapper.AddPair("thinThickMediumGap", BorderStyle.ThinThickMediumGap);
			TypeMappers.BorderStyleMapper.AddPair("thinThickLargeGap", BorderStyle.ThinThickLargeGap);
			TypeMappers.BorderStyleMapper.AddPair("thinThickThinSmallGap", BorderStyle.ThinThickThinSmallGap);
			TypeMappers.BorderStyleMapper.AddPair("thinThickThinMediumGap", BorderStyle.ThinThickThinMediumGap);
			TypeMappers.BorderStyleMapper.AddPair("thinThickThinLargeGap", BorderStyle.ThinThickThinLargeGap);
			TypeMappers.BorderStyleMapper.AddPair("wave", BorderStyle.Wave);
			TypeMappers.BorderStyleMapper.AddPair("doubleWave", BorderStyle.DoubleWave);
			TypeMappers.BorderStyleMapper.AddPair("dashDotStroked", BorderStyle.DashDotStroked);
			TypeMappers.BorderStyleMapper.AddPair("threeDEmboss", BorderStyle.ThreeDEmboss);
			TypeMappers.BorderStyleMapper.AddPair("threeDEngrave", BorderStyle.ThreeDEngrave);
			TypeMappers.BorderStyleMapper.AddPair("outset", BorderStyle.Outset);
			TypeMappers.BorderStyleMapper.AddPair("inset", BorderStyle.Inset);
		}

		static void InitTableWidthTypeMapper()
		{
			TypeMappers.TableWidthTypeMapper = new ValueMapper<string, TableWidthUnitType>();
			TypeMappers.TableWidthTypeMapper.AddPair("nil", TableWidthUnitType.Nil);
			TypeMappers.TableWidthTypeMapper.AddPair("pct", TableWidthUnitType.Percent);
			TypeMappers.TableWidthTypeMapper.AddPair("dxa", TableWidthUnitType.Fixed);
			TypeMappers.TableWidthTypeMapper.AddPair("auto", TableWidthUnitType.Auto);
		}

		static void InitTableLayoutTypeMapper()
		{
			TypeMappers.TableLayoutTypeMapper = new ValueMapper<string, TableLayoutType>("auto", TableLayoutType.AutoFit);
			TypeMappers.TableLayoutTypeMapper.AddPair("auto", TableLayoutType.AutoFit);
			TypeMappers.TableLayoutTypeMapper.AddPair("fixed", TableLayoutType.FixedWidth);
		}

		static void InitHeaderFooterTypeMapper()
		{
			TypeMappers.HeaderFooterTypeMapper = new ValueMapper<string, HeaderFooterType>();
			TypeMappers.HeaderFooterTypeMapper.AddPair("default", HeaderFooterType.Default);
			TypeMappers.HeaderFooterTypeMapper.AddPair("even", HeaderFooterType.Even);
			TypeMappers.HeaderFooterTypeMapper.AddPair("first", HeaderFooterType.First);
		}

		static void InitVerticalAlignmentMapper()
		{
			TypeMappers.VerticalAlignmentMapper = new ValueMapper<string, VerticalAlignment>("top", VerticalAlignment.Top);
			TypeMappers.VerticalAlignmentMapper.AddPair("top", VerticalAlignment.Top);
			TypeMappers.VerticalAlignmentMapper.AddPair("center", VerticalAlignment.Center);
			TypeMappers.VerticalAlignmentMapper.AddPair("bottom", VerticalAlignment.Bottom);
			TypeMappers.VerticalAlignmentMapper.AddPair("both", VerticalAlignment.Justified);
		}

		static void InitStyleTypeMapper()
		{
			TypeMappers.StyleTypeMapper = new ValueMapper<string, StyleType>();
			TypeMappers.StyleTypeMapper.AddPair("character", StyleType.Character);
			TypeMappers.StyleTypeMapper.AddPair("paragraph", StyleType.Paragraph);
			TypeMappers.StyleTypeMapper.AddPair("table", StyleType.Table);
			TypeMappers.StyleTypeMapper.AddPair("numbering", StyleType.Numbering);
		}

		static void InitSectionTypeMapper()
		{
			TypeMappers.SectionTypeMapper = new ValueMapper<string, SectionType>("nextPage", SectionType.NextPage);
			TypeMappers.SectionTypeMapper.AddPair("nextPage", SectionType.NextPage);
			TypeMappers.SectionTypeMapper.AddPair("nextColumn", SectionType.NextColumn);
			TypeMappers.SectionTypeMapper.AddPair("continuous", SectionType.Continuous);
			TypeMappers.SectionTypeMapper.AddPair("evenPage", SectionType.EvenPage);
			TypeMappers.SectionTypeMapper.AddPair("oddPage", SectionType.OddPage);
		}

		static void InitPageOrientationMapper()
		{
			TypeMappers.PageOrientationMapper = new ValueMapper<string, PageOrientation>();
			TypeMappers.PageOrientationMapper.AddPair("portrait", PageOrientation.Portrait);
			TypeMappers.PageOrientationMapper.AddPair("landscape", PageOrientation.Landscape);
		}

		static void InitRelativeHorizontalAlignmentMapper()
		{
			TypeMappers.RelativeHorizontalAlignmentMapper = new ValueMapper<string, RelativeHorizontalAlignment>();
			TypeMappers.RelativeHorizontalAlignmentMapper.AddPair("left", RelativeHorizontalAlignment.Left);
			TypeMappers.RelativeHorizontalAlignmentMapper.AddPair("right", RelativeHorizontalAlignment.Right);
			TypeMappers.RelativeHorizontalAlignmentMapper.AddPair("center", RelativeHorizontalAlignment.Center);
			TypeMappers.RelativeHorizontalAlignmentMapper.AddPair("inside", RelativeHorizontalAlignment.Inside);
			TypeMappers.RelativeHorizontalAlignmentMapper.AddPair("outside", RelativeHorizontalAlignment.Outside);
		}

		static void InitRelativeVerticalAlignmentMapper()
		{
			TypeMappers.RelativeVerticalAlignmentMapper = new ValueMapper<string, RelativeVerticalAlignment>();
			TypeMappers.RelativeVerticalAlignmentMapper.AddPair("top", RelativeVerticalAlignment.Top);
			TypeMappers.RelativeVerticalAlignmentMapper.AddPair("bottom", RelativeVerticalAlignment.Bottom);
			TypeMappers.RelativeVerticalAlignmentMapper.AddPair("center", RelativeVerticalAlignment.Center);
			TypeMappers.RelativeVerticalAlignmentMapper.AddPair("inside", RelativeVerticalAlignment.Inside);
			TypeMappers.RelativeVerticalAlignmentMapper.AddPair("outside", RelativeVerticalAlignment.Outside);
		}

		static void InitHorizontalRelativeFromMapper()
		{
			TypeMappers.HorizontalRelativeFromMapper = new ValueMapper<string, HorizontalRelativeFrom>();
			TypeMappers.HorizontalRelativeFromMapper.AddPair("character", HorizontalRelativeFrom.Character);
			TypeMappers.HorizontalRelativeFromMapper.AddPair("column", HorizontalRelativeFrom.Column);
			TypeMappers.HorizontalRelativeFromMapper.AddPair("insideMargin", HorizontalRelativeFrom.InsideMargin);
			TypeMappers.HorizontalRelativeFromMapper.AddPair("leftMargin", HorizontalRelativeFrom.LeftMargin);
			TypeMappers.HorizontalRelativeFromMapper.AddPair("margin", HorizontalRelativeFrom.Margin);
			TypeMappers.HorizontalRelativeFromMapper.AddPair("outsideMargin", HorizontalRelativeFrom.OutsideMargin);
			TypeMappers.HorizontalRelativeFromMapper.AddPair("page", HorizontalRelativeFrom.Page);
			TypeMappers.HorizontalRelativeFromMapper.AddPair("rightMargin", HorizontalRelativeFrom.RightMargin);
		}

		static void InitVerticalRelativeFromMapper()
		{
			TypeMappers.VerticalRelativeFromMapper = new ValueMapper<string, VerticalRelativeFrom>();
			TypeMappers.VerticalRelativeFromMapper.AddPair("bottomMargin", VerticalRelativeFrom.BottomMargin);
			TypeMappers.VerticalRelativeFromMapper.AddPair("insideMargin", VerticalRelativeFrom.InsideMargin);
			TypeMappers.VerticalRelativeFromMapper.AddPair("line", VerticalRelativeFrom.Line);
			TypeMappers.VerticalRelativeFromMapper.AddPair("margin", VerticalRelativeFrom.Margin);
			TypeMappers.VerticalRelativeFromMapper.AddPair("outsideMargin", VerticalRelativeFrom.OutsideMargin);
			TypeMappers.VerticalRelativeFromMapper.AddPair("page", VerticalRelativeFrom.Page);
			TypeMappers.VerticalRelativeFromMapper.AddPair("paragraph", VerticalRelativeFrom.Paragraph);
			TypeMappers.VerticalRelativeFromMapper.AddPair("topMargin", VerticalRelativeFrom.TopMargin);
		}

		static void InitFileldCharTypeMapper()
		{
			TypeMappers.FileldCharTypeMapper = new ValueMapper<string, FieldCharacterType>();
			TypeMappers.FileldCharTypeMapper.AddPair("begin", FieldCharacterType.Start);
			TypeMappers.FileldCharTypeMapper.AddPair("separate", FieldCharacterType.Separator);
			TypeMappers.FileldCharTypeMapper.AddPair("end", FieldCharacterType.End);
		}

		static void InitTextWrapMapper()
		{
			TypeMappers.TextWrapMapper = new ValueMapper<string, TextWrap>();
			TypeMappers.TextWrapMapper.AddPair("bothSides", TextWrap.BothSides);
			TypeMappers.TextWrapMapper.AddPair("right", TextWrap.RightOnly);
			TypeMappers.TextWrapMapper.AddPair("left", TextWrap.LeftOnly);
			TypeMappers.TextWrapMapper.AddPair("largest", TextWrap.Largest);
		}

		static void InitBreakTypeMapper()
		{
			TypeMappers.BreakTypeMapper = new ValueMapper<string, BreakType>("textWrapping", BreakType.LineBreak);
			TypeMappers.BreakTypeMapper.AddPair("page", BreakType.PageBreak);
			TypeMappers.BreakTypeMapper.AddPair("column", BreakType.ColumnBreak);
		}

		static void InitRestartLocationMapper()
		{
			TypeMappers.RestartLocationMapper = new ValueMapper<string, TextWrappingRestartLocation>("none", TextWrappingRestartLocation.NextLine);
			TypeMappers.RestartLocationMapper.AddPair("all", TextWrappingRestartLocation.NextFullLine);
			TypeMappers.RestartLocationMapper.AddPair("left", TextWrappingRestartLocation.NextTextRegionUnblockedOnLeft);
			TypeMappers.RestartLocationMapper.AddPair("right", TextWrappingRestartLocation.NextTextRegionUnblockedOnRight);
		}

		static void InitMultilevelTypeMapper()
		{
			TypeMappers.MultilevelTypeMapper = new ValueMapper<string, MultilevelType>("hybridMultilevel", MultilevelType.HybridMultilevel);
			TypeMappers.MultilevelTypeMapper.AddPair("multilevel", MultilevelType.Multilevel);
			TypeMappers.MultilevelTypeMapper.AddPair("singleLevel", MultilevelType.SingleLevel);
		}

		static void InitNumberingStyleMapper()
		{
			TypeMappers.NumberingStyleMapper = new ValueMapper<string, NumberingStyle>();
			TypeMappers.NumberingStyleMapper.AddPair("decimal", NumberingStyle.Decimal);
			TypeMappers.NumberingStyleMapper.AddPair("upperRoman", NumberingStyle.UpperRoman);
			TypeMappers.NumberingStyleMapper.AddPair("lowerRoman", NumberingStyle.LowerRoman);
			TypeMappers.NumberingStyleMapper.AddPair("upperLetter", NumberingStyle.UpperLetter);
			TypeMappers.NumberingStyleMapper.AddPair("lowerLetter", NumberingStyle.LowerLetter);
			TypeMappers.NumberingStyleMapper.AddPair("ordinal", NumberingStyle.Ordinal);
			TypeMappers.NumberingStyleMapper.AddPair("cardinalText", NumberingStyle.CardinalText);
			TypeMappers.NumberingStyleMapper.AddPair("ordinalText", NumberingStyle.OrdinalText);
			TypeMappers.NumberingStyleMapper.AddPair("hex", NumberingStyle.Hex);
			TypeMappers.NumberingStyleMapper.AddPair("chicago", NumberingStyle.Chicago);
			TypeMappers.NumberingStyleMapper.AddPair("ideographDigital", NumberingStyle.IdeographDigital);
			TypeMappers.NumberingStyleMapper.AddPair("japaneseCounting", NumberingStyle.JapaneseCounting);
			TypeMappers.NumberingStyleMapper.AddPair("aiueo", NumberingStyle.Aiueo);
			TypeMappers.NumberingStyleMapper.AddPair("iroha", NumberingStyle.Iroha);
			TypeMappers.NumberingStyleMapper.AddPair("decimalFullWidth", NumberingStyle.DecimalFullWidth);
			TypeMappers.NumberingStyleMapper.AddPair("decimalHalfWidth", NumberingStyle.DecimalHalfWidth);
			TypeMappers.NumberingStyleMapper.AddPair("japaneseLegal", NumberingStyle.JapaneseLegal);
			TypeMappers.NumberingStyleMapper.AddPair("japaneseDigitalTenThousand", NumberingStyle.JapaneseDigitalTenThousand);
			TypeMappers.NumberingStyleMapper.AddPair("decimalEnclosedCircle", NumberingStyle.DecimalEnclosedCircle);
			TypeMappers.NumberingStyleMapper.AddPair("decimalFullWidth2", NumberingStyle.DecimalFullWidth2);
			TypeMappers.NumberingStyleMapper.AddPair("aiueoFullWidth", NumberingStyle.AiueoFullWidth);
			TypeMappers.NumberingStyleMapper.AddPair("irohaFullWidth", NumberingStyle.IrohaFullWidth);
			TypeMappers.NumberingStyleMapper.AddPair("decimalZero", NumberingStyle.DecimalZero);
			TypeMappers.NumberingStyleMapper.AddPair("bullet", NumberingStyle.Bullet);
			TypeMappers.NumberingStyleMapper.AddPair("ganada", NumberingStyle.Ganada);
			TypeMappers.NumberingStyleMapper.AddPair("chosung", NumberingStyle.Chosung);
			TypeMappers.NumberingStyleMapper.AddPair("decimalEnclosedFullstop", NumberingStyle.DecimalEnclosedFullStop);
			TypeMappers.NumberingStyleMapper.AddPair("decimalEnclosedParen", NumberingStyle.DecimalEnclosedParent);
			TypeMappers.NumberingStyleMapper.AddPair("decimalEnclosedCircleChinese", NumberingStyle.DecimalEnclosedCircleChinese);
			TypeMappers.NumberingStyleMapper.AddPair("ideographEnclosedCircle", NumberingStyle.IdeographEnclosedCircle);
			TypeMappers.NumberingStyleMapper.AddPair("ideographTraditional", NumberingStyle.IdeographTraditional);
			TypeMappers.NumberingStyleMapper.AddPair("ideographZodiac", NumberingStyle.IdeographZodiac);
			TypeMappers.NumberingStyleMapper.AddPair("ideographZodiacTraditional", NumberingStyle.IdeographZodiacTraditional);
			TypeMappers.NumberingStyleMapper.AddPair("taiwaneseCounting", NumberingStyle.TaiwaneseCounting);
			TypeMappers.NumberingStyleMapper.AddPair("ideographLegalTraditional", NumberingStyle.IdeographLegalTraditional);
			TypeMappers.NumberingStyleMapper.AddPair("taiwaneseCountingThousand", NumberingStyle.TaiwaneseCountingThousand);
			TypeMappers.NumberingStyleMapper.AddPair("taiwaneseDigital", NumberingStyle.TaiwaneseDigital);
			TypeMappers.NumberingStyleMapper.AddPair("chineseCounting", NumberingStyle.ChineseCounting);
			TypeMappers.NumberingStyleMapper.AddPair("chineseLegalSimplified", NumberingStyle.ChineseLegalSimplified);
			TypeMappers.NumberingStyleMapper.AddPair("chineseCountingThousand", NumberingStyle.ChineseCountingThousand);
			TypeMappers.NumberingStyleMapper.AddPair("koreanDigital", NumberingStyle.KoreanDigital);
			TypeMappers.NumberingStyleMapper.AddPair("koreanCounting", NumberingStyle.KoreanCounting);
			TypeMappers.NumberingStyleMapper.AddPair("koreanLegal", NumberingStyle.KoreanLegal);
			TypeMappers.NumberingStyleMapper.AddPair("koreanDigital2", NumberingStyle.KoreanDigital2);
			TypeMappers.NumberingStyleMapper.AddPair("vietnameseCounting", NumberingStyle.VietnameseCounting);
			TypeMappers.NumberingStyleMapper.AddPair("russianLower", NumberingStyle.RussianLower);
			TypeMappers.NumberingStyleMapper.AddPair("russianUpper", NumberingStyle.RussianUpper);
			TypeMappers.NumberingStyleMapper.AddPair("none", NumberingStyle.None);
			TypeMappers.NumberingStyleMapper.AddPair("numberInDash", NumberingStyle.NumberInDash);
			TypeMappers.NumberingStyleMapper.AddPair("hebrew1", NumberingStyle.Hebrew1);
			TypeMappers.NumberingStyleMapper.AddPair("hebrew2", NumberingStyle.Hebrew2);
			TypeMappers.NumberingStyleMapper.AddPair("arabicAlpha", NumberingStyle.ArabicAlpha);
			TypeMappers.NumberingStyleMapper.AddPair("arabicAbjad", NumberingStyle.ArabicAbjad);
			TypeMappers.NumberingStyleMapper.AddPair("hindiVowels", NumberingStyle.HindiVowels);
			TypeMappers.NumberingStyleMapper.AddPair("hindiConsonants", NumberingStyle.HindiConsonants);
			TypeMappers.NumberingStyleMapper.AddPair("hindiNumbers", NumberingStyle.HindiNumbers);
			TypeMappers.NumberingStyleMapper.AddPair("hindiCounting", NumberingStyle.HindiCounting);
			TypeMappers.NumberingStyleMapper.AddPair("thaiLetters", NumberingStyle.ThaiLetters);
			TypeMappers.NumberingStyleMapper.AddPair("thaiNumbers", NumberingStyle.ThaiNumbers);
			TypeMappers.NumberingStyleMapper.AddPair("thaiCounting", NumberingStyle.ThaiCounting);
			TypeMappers.NumberingStyleMapper.AddPair("bahtText", NumberingStyle.Decimal);
			TypeMappers.NumberingStyleMapper.AddPair("dollarText", NumberingStyle.Decimal);
			TypeMappers.NumberingStyleMapper.AddPair("custom", NumberingStyle.Decimal);
		}

		static void InitChapterSeparatorTypeMapper()
		{
			TypeMappers.ChapterSeparatorTypeMapper = new ValueMapper<string, ChapterSeparatorType>();
			TypeMappers.ChapterSeparatorTypeMapper.AddPair("colon", ChapterSeparatorType.Colon);
			TypeMappers.ChapterSeparatorTypeMapper.AddPair("hyphen", ChapterSeparatorType.Hyphen);
			TypeMappers.ChapterSeparatorTypeMapper.AddPair("emDash", ChapterSeparatorType.EmDash);
			TypeMappers.ChapterSeparatorTypeMapper.AddPair("enDash", ChapterSeparatorType.EnDash);
			TypeMappers.ChapterSeparatorTypeMapper.AddPair("period", ChapterSeparatorType.Period);
		}

		static void InitProtectionModeTypeMapper()
		{
			TypeMappers.ProtectionModeTypeMapper = new ValueMapper<string, ProtectionMode>("none", ProtectionMode.NoProtection);
			TypeMappers.ProtectionModeTypeMapper.AddPair("none", ProtectionMode.NoProtection);
			TypeMappers.ProtectionModeTypeMapper.AddPair("comments", ProtectionMode.AllowComments);
			TypeMappers.ProtectionModeTypeMapper.AddPair("readOnly", ProtectionMode.ReadOnly);
		}

		static void InitProtectionAlgorithmsTypeMapper()
		{
			TypeMappers.ProtectionAlgorithmTypeMapper = new ValueMapper<string, string>("4", "SHA-1");
			TypeMappers.ProtectionAlgorithmTypeMapper.AddPair("4", "SHA-1");
			TypeMappers.ProtectionAlgorithmTypeMapper.AddPair("12", "SHA-256");
			TypeMappers.ProtectionAlgorithmTypeMapper.AddPair("13", "SHA-384");
			TypeMappers.ProtectionAlgorithmTypeMapper.AddPair("14", "SHA-512");
		}

		static void InitEditingGroupTypeMapper()
		{
			TypeMappers.EditingGroupTypeMapper = new ValueMapper<string, EditingGroup?>();
			TypeMappers.EditingGroupTypeMapper.AddPair("none", new EditingGroup?(EditingGroup.None));
			TypeMappers.EditingGroupTypeMapper.AddPair("everyone", new EditingGroup?(EditingGroup.Everyone));
			TypeMappers.EditingGroupTypeMapper.AddPair("administrators", new EditingGroup?(EditingGroup.Administrators));
			TypeMappers.EditingGroupTypeMapper.AddPair("contributors", new EditingGroup?(EditingGroup.Contributors));
			TypeMappers.EditingGroupTypeMapper.AddPair("editors", new EditingGroup?(EditingGroup.Editors));
			TypeMappers.EditingGroupTypeMapper.AddPair("owners", new EditingGroup?(EditingGroup.Owners));
			TypeMappers.EditingGroupTypeMapper.AddPair("current", new EditingGroup?(EditingGroup.Current));
		}

		static void InitTabStopLeaderMapper()
		{
			TypeMappers.TabStopLeaderMapper = new ValueMapper<string, TabStopLeader>();
			TypeMappers.TabStopLeaderMapper.AddPair("none", TabStopLeader.None);
			TypeMappers.TabStopLeaderMapper.AddPair("dot", TabStopLeader.Dot);
			TypeMappers.TabStopLeaderMapper.AddPair("hyphen", TabStopLeader.Hyphen);
			TypeMappers.TabStopLeaderMapper.AddPair("middleDot", TabStopLeader.MiddleDot);
			TypeMappers.TabStopLeaderMapper.AddPair("underscore", TabStopLeader.Underscore);
			TypeMappers.TabStopLeaderMapper.AddPair("heavy", TabStopLeader.Underscore);
		}

		static void InitTabStopTypeMapper()
		{
			TypeMappers.TabStopTypeMapper = new ValueMapper<string, TabStopType>();
			TypeMappers.TabStopTypeMapper.AddPair("bar", TabStopType.Bar);
			TypeMappers.TabStopTypeMapper.AddPair("center", TabStopType.Center);
			TypeMappers.TabStopTypeMapper.AddPair("clear", TabStopType.Clear);
			TypeMappers.TabStopTypeMapper.AddPair("decimal", TabStopType.Decimal);
			TypeMappers.TabStopTypeMapper.AddPair("start", TabStopType.Left);
			TypeMappers.TabStopTypeMapper.AddPair("num", TabStopType.Left);
			TypeMappers.TabStopTypeMapper.AddPair("left", TabStopType.Left);
			TypeMappers.TabStopTypeMapper.AddPair("end", TabStopType.Right);
			TypeMappers.TabStopTypeMapper.AddPair("right", TabStopType.Right);
		}

		static void InitTextDirectionMapper()
		{
			TypeMappers.TextDirectionMapper = new ValueMapper<string, TextDirection>();
			TypeMappers.TextDirectionMapper.AddPair("lrTb", TextDirection.LeftToRightTopToBottom);
			TypeMappers.TextDirectionMapper.AddPair("tbRl", TextDirection.TopToBottomRightToLeft);
			TypeMappers.TextDirectionMapper.AddPair("btLr", TextDirection.BottomToTopLeftToRight);
			TypeMappers.TextDirectionMapper.AddPair("lrTbV", TextDirection.LeftToRightTopToBottomRotated);
			TypeMappers.TextDirectionMapper.AddPair("tbRlV", TextDirection.TopToBottomRightToLeftRotated);
			TypeMappers.TextDirectionMapper.AddPair("tbLrV", TextDirection.TopToBottomLeftToRightRotated);
		}
	}
}
