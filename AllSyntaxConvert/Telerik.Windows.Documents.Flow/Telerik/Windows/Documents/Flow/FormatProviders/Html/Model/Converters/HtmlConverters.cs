using System;
using Telerik.Windows.Documents.Flow.Model.Lists;
using Telerik.Windows.Documents.Flow.Model.Styles;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters
{
	static class HtmlConverters
	{
		static HtmlConverters()
		{
			HtmlConverters.ThemableFontFamilyConverter = new ThemableFontFamilyConverter();
			HtmlConverters.ColorMediaConverter = new ColorMediaConverter();
			HtmlConverters.FlowDirectionConverter = new FlowDirectionConverter();
			HtmlConverters.LineSpacingTypeConverter = new LineSpacingTypeConverter();
			HtmlConverters.FontSizeConverter = new FontSizeConverter();
			HtmlConverters.PreferredWidthConverter = new PreferredWidthConverter();
			HtmlConverters.BaselineAlignmentConverter = new MappedConverter<BaselineAlignment>(HtmlValueMappers.BaselineAlignmentMapper);
			HtmlConverters.TableLayoutTypeConverter = new MappedConverter<TableLayoutType>(HtmlValueMappers.TableLayoutTypeMapper);
			HtmlConverters.VerticalAlignmentConverter = new MappedConverter<VerticalAlignment>(HtmlValueMappers.VerticalAlignmentMapper);
			HtmlConverters.CellSpacingConverter = new CellSpacingConverter();
			HtmlConverters.TableBordersConverter = new TableBordersConverter();
			HtmlConverters.TableBordersColorConverter = new TableBordersColorConverter();
			HtmlConverters.BorderConverter = new BorderConverter();
			HtmlConverters.LineSpacingConverter = new LineSpacingConverter();
			HtmlConverters.RowHeightConverter = new RowHeightConverter();
			HtmlConverters.TableCellBordersConverter = new TableCellBordersConverter();
			HtmlConverters.BorderCollapseConverter = new BorderCollapseConverter();
			HtmlConverters.PaddingConverter = new PaddingConverter();
			HtmlConverters.PaddingShortHandConverter = new PaddingShortHandConverter();
			HtmlConverters.ListNumberingStyleTypeConverter = new MappedConverter<NumberingStyle>(HtmlValueMappers.NumberingStyleTypeMapper);
			HtmlConverters.FirstLineIndentConverter = new FirstLineIndentConverter();
			HtmlConverters.HangingIndentConverter = new HangingIndentConverter();
		}

		public static FirstLineIndentConverter FirstLineIndentConverter { get; set; }

		public static HangingIndentConverter HangingIndentConverter { get; set; }

		public static FontStyleConverter FontStyleConverter { get; set; } = new FontStyleConverter();

		public static FontWeightConverter FontWeightConverter { get; set; } = new FontWeightConverter();

		public static ThemableColorConverter ThemableColorConverter { get; set; } = new ThemableColorConverter();

		public static UnderlinePatternConverter UnderlinePatternConverter { get; set; } = new UnderlinePatternConverter();

		public static UnderlineColorConverter UnderlineColorConverter { get; set; } = new UnderlineColorConverter();

		public static StrikethroughConverter StrikethroughConverter { get; set; } = new StrikethroughConverter();

		public static AlignmentConverter AlignmentConverter { get; set; } = new AlignmentConverter();

		public static DoubleConverter DoubleConverter { get; set; } = new DoubleConverter();

		public static ThemableFontFamilyConverter ThemableFontFamilyConverter { get; set; }

		public static ColorMediaConverter ColorMediaConverter { get; set; }

		public static FlowDirectionConverter FlowDirectionConverter { get; set; }

		public static LineSpacingTypeConverter LineSpacingTypeConverter { get; set; }

		public static FontSizeConverter FontSizeConverter { get; set; }

		public static PreferredWidthConverter PreferredWidthConverter { get; set; }

		public static MappedConverter<BaselineAlignment> BaselineAlignmentConverter { get; set; }

		public static MappedConverter<TableLayoutType> TableLayoutTypeConverter { get; set; }

		public static MappedConverter<VerticalAlignment> VerticalAlignmentConverter { get; set; }

		public static CellSpacingConverter CellSpacingConverter { get; set; }

		public static TableBordersConverter TableBordersConverter { get; set; }

		public static TableBordersColorConverter TableBordersColorConverter { get; set; }

		public static TableCellBordersConverter TableCellBordersConverter { get; set; }

		public static BorderConverter BorderConverter { get; set; }

		public static LineSpacingConverter LineSpacingConverter { get; set; }

		public static RowHeightConverter RowHeightConverter { get; set; }

		public static PaddingConverter PaddingConverter { get; set; }

		public static PaddingShortHandConverter PaddingShortHandConverter { get; set; }

		public static SpacingAfterConverter SpacingAfterConverter { get; set; } = new SpacingAfterConverter();

		public static SpacingBeforeConverter SpacingBeforeConverter { get; set; } = new SpacingBeforeConverter();

		public static AutomaticSpacingAfterConverter AutomaticSpacingAfterConverter { get; set; } = new AutomaticSpacingAfterConverter();

		public static AutomaticSpacingBeforeConverter AutomaticSpacingBeforeConverter { get; set; } = new AutomaticSpacingBeforeConverter();

		public static BorderCollapseConverter BorderCollapseConverter { get; set; }

		public static MappedConverter<NumberingStyle> ListNumberingStyleTypeConverter { get; set; }
	}
}
