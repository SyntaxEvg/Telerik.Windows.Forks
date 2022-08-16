using System;
using System.Collections.Generic;
using Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Converters;
using Telerik.Windows.Documents.Flow.Model;
using Telerik.Windows.Documents.Flow.Model.Styles.Core;

namespace Telerik.Windows.Documents.Flow.FormatProviders.Html.Model.Styles
{
	static class HtmlPropertyMappers
	{
		static HtmlPropertyMappers()
		{
			HtmlPropertyMappers.InitializeMappers();
		}

		internal static IEnumerable<HtmlPropertyMapperElement> TryGetPropertyMappingElements(HtmlStyleProperty property)
		{
			return HtmlPropertyMappers.TryGetPropertyMappingElements(property.Name);
		}

		internal static IEnumerable<HtmlPropertyMapperElement> TryGetPropertyMappingElements(string propertyName)
		{
			if (HtmlPropertyMappers.htmlPropertiesToMapperElement.ContainsKey(propertyName))
			{
				foreach (HtmlPropertyMapperElement mapperElement in HtmlPropertyMappers.htmlPropertiesToMapperElement[propertyName])
				{
					yield return mapperElement;
				}
			}
			yield break;
		}

		internal static IEnumerable<HtmlPropertyMapperElement> TryGetPropertyMappingElements(IStylePropertyDefinition stylePropertyDefinition)
		{
			if (HtmlPropertyMappers.stylePropertyDefinitionsToMapperElement.ContainsKey(stylePropertyDefinition))
			{
				foreach (HtmlPropertyMapperElement mapperElement in HtmlPropertyMappers.stylePropertyDefinitionsToMapperElement[stylePropertyDefinition])
				{
					yield return mapperElement;
				}
			}
			yield break;
		}

		static void AddMapperElement(HtmlPropertyMapperElement mapperElement)
		{
			if (mapperElement.IsDescriptorToDefinitionElement)
			{
				if (!HtmlPropertyMappers.htmlPropertiesToMapperElement.ContainsKey(mapperElement.HtmlPropertyDescriptor.Name))
				{
					HtmlPropertyMappers.htmlPropertiesToMapperElement[mapperElement.HtmlPropertyDescriptor.Name] = new List<HtmlPropertyMapperElement>();
				}
				HtmlPropertyMappers.htmlPropertiesToMapperElement[mapperElement.HtmlPropertyDescriptor.Name].Add(mapperElement);
			}
			if (mapperElement.IsDefinitionToDescriptorElement)
			{
				IStylePropertyDefinition propertyDefinition = mapperElement.PropertyDefinition;
				if (!HtmlPropertyMappers.stylePropertyDefinitionsToMapperElement.ContainsKey(propertyDefinition))
				{
					HtmlPropertyMappers.stylePropertyDefinitionsToMapperElement[propertyDefinition] = new List<HtmlPropertyMapperElement>();
				}
				HtmlPropertyMappers.stylePropertyDefinitionsToMapperElement[propertyDefinition].Add(mapperElement);
			}
		}

		static void InitializeMappers()
		{
			HtmlPropertyMappers.InitFontStyleMapper();
			HtmlPropertyMappers.InitFontWeighteMapper();
			HtmlPropertyMappers.InitForegroundColorMapper();
			HtmlPropertyMappers.InitStrikethroughMapper();
			HtmlPropertyMappers.InitUnderlineMapper();
			HtmlPropertyMappers.InitFontSizeMapper();
			HtmlPropertyMappers.InitFontFamilyMapper();
			HtmlPropertyMappers.InitTextAlignMapper();
			HtmlPropertyMappers.InitBackgroundColorMapper();
			HtmlPropertyMappers.InitHighlightColorMapper();
			HtmlPropertyMappers.InitBaselineAlignmentMapper();
			HtmlPropertyMappers.InitMarginMappers();
			HtmlPropertyMappers.InitFlowDirectionMapper();
			HtmlPropertyMappers.InitFirstLineIndentMapper();
			HtmlPropertyMappers.InitLineSpacingMapper();
			HtmlPropertyMappers.InitTableAndTableCellWidthMappers();
			HtmlPropertyMappers.InitCellSpacingMapper();
			HtmlPropertyMappers.InitTableLayoutMapper();
			HtmlPropertyMappers.InitTableBordersColorMapper();
			HtmlPropertyMappers.InitTableBordersMapper();
			HtmlPropertyMappers.InitTableCellBordersMapper();
			HtmlPropertyMappers.InitTableCellVerticalAlignmentMapper();
			HtmlPropertyMappers.InitRowHeightMapper();
			HtmlPropertyMappers.InitPaddingMappers();
		}

		static void InitTableBordersMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderPropertyDescriptor, Table.TableBordersPropertyDefinition, HtmlConverters.TableBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderLeftPropertyDescriptor, Table.TableBordersPropertyDefinition, HtmlConverters.TableBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderTopPropertyDescriptor, Table.TableBordersPropertyDefinition, HtmlConverters.TableBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderRightPropertyDescriptor, Table.TableBordersPropertyDefinition, HtmlConverters.TableBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderBottomPropertyDescriptor, Table.TableBordersPropertyDefinition, HtmlConverters.TableBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitTableBordersColorMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderColorPropertyDescriptor, Table.TableBordersPropertyDefinition, HtmlConverters.TableBordersColorConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitTableCellBordersMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderPropertyDescriptor, TableCell.BordersPropertyDefinition, HtmlConverters.TableCellBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderLeftPropertyDescriptor, TableCell.BordersPropertyDefinition, HtmlConverters.TableCellBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderTopPropertyDescriptor, TableCell.BordersPropertyDefinition, HtmlConverters.TableCellBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderRightPropertyDescriptor, TableCell.BordersPropertyDefinition, HtmlConverters.TableCellBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderBottomPropertyDescriptor, TableCell.BordersPropertyDefinition, HtmlConverters.TableCellBordersConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitTableCellVerticalAlignmentMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.VerticalAlignmentPropertyDescriptor, TableCell.VerticalAlignmentPropertyDefinition, HtmlConverters.VerticalAlignmentConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitPaddingMappers()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.PaddingPropertyDescriptor, TableCell.PaddingPropertyDefinition, HtmlConverters.PaddingShortHandConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			HtmlPropertyMapperElement mapperElement2 = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.PaddingLeftPropertyDescriptor, TableCell.PaddingPropertyDefinition, HtmlConverters.PaddingConverter, true, false);
			HtmlPropertyMappers.AddMapperElement(mapperElement2);
			HtmlPropertyMapperElement mapperElement3 = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.PaddingRightPropertyDescriptor, TableCell.PaddingPropertyDefinition, HtmlConverters.PaddingConverter, true, false);
			HtmlPropertyMappers.AddMapperElement(mapperElement3);
			HtmlPropertyMapperElement mapperElement4 = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.PaddingTopPropertyDescriptor, TableCell.PaddingPropertyDefinition, HtmlConverters.PaddingConverter, true, false);
			HtmlPropertyMappers.AddMapperElement(mapperElement4);
			HtmlPropertyMapperElement mapperElement5 = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.PaddingBottomPropertyDescriptor, TableCell.PaddingPropertyDefinition, HtmlConverters.PaddingConverter, true, false);
			HtmlPropertyMappers.AddMapperElement(mapperElement5);
		}

		static void InitCellSpacingMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderSpacingPropertyDescriptor, Table.TableCellSpacingPropertyDefinition, HtmlConverters.CellSpacingConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			HtmlPropertyMapperElement mapperElement2 = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BorderCollapsePropertyDescriptor, Table.TableCellSpacingPropertyDefinition, HtmlConverters.BorderCollapseConverter, false, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement2);
		}

		static void InitTableAndTableCellWidthMappers()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.WidthPropertyDescriptor, Table.PreferredWidthPropertyDefinition, HtmlConverters.PreferredWidthConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			HtmlPropertyMapperElement mapperElement2 = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.WidthPropertyDescriptor, TableCell.PreferredWidthPropertyDefinition, HtmlConverters.PreferredWidthConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement2);
		}

		static void InitTableLayoutMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.TableLayoutPropertyDescriptor, Table.LayoutTypePropertyDefinition, HtmlConverters.TableLayoutTypeConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitRowHeightMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.RowHeightPropertyDescriptor, TableRow.HeightPropertyDefinition, HtmlConverters.RowHeightConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitLineSpacingMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.LineHeightPropertyDescriptor, Paragraph.LineSpacingPropertyDefinition, HtmlConverters.LineSpacingConverter, true, false);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.LineHeightPropertyDescriptor, Paragraph.LineSpacingTypePropertyDefinition, HtmlConverters.LineSpacingTypeConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitFirstLineIndentMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.TextIndentPropertyDescriptor, Paragraph.FirstLineIndentPropertyDefinition, HtmlConverters.FirstLineIndentConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			HtmlPropertyMapperElement mapperElement2 = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.TextIndentPropertyDescriptor, Paragraph.HangingIndentPropertyDefinition, HtmlConverters.HangingIndentConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement2);
		}

		static void InitFlowDirectionMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.DirectionPropertyDescriptor, Paragraph.FlowDirectionPropertyDefinition, HtmlConverters.FlowDirectionConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.DirectionPropertyDescriptor, Run.FlowDirectionPropertyDefinition, HtmlConverters.FlowDirectionConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitMarginMappers()
		{
			HtmlPropertyMappers.AddMapperElement(new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.MarginLeftPropertyDescriptor, Paragraph.LeftIndentPropertyDefinition, HtmlConverters.DoubleConverter, true, true));
			HtmlPropertyMappers.AddMapperElement(new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.MarginRightPropertyDescriptor, Paragraph.RightIndentPropertyDefinition, HtmlConverters.DoubleConverter, true, true));
			HtmlPropertyMappers.AddMapperElement(new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.MarginTopPropertyDescriptor, Paragraph.SpacingBeforePropertyDefinition, HtmlConverters.SpacingBeforeConverter, true, true));
			HtmlPropertyMappers.AddMapperElement(new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.MarginBottomPropertyDescriptor, Paragraph.SpacingAfterPropertyDefinition, HtmlConverters.SpacingAfterConverter, true, true));
			HtmlPropertyMappers.AddMapperElement(new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.MarginTopPropertyDescriptor, Paragraph.AutomaticSpacingBeforePropertyDefinition, HtmlConverters.AutomaticSpacingBeforeConverter, true, false));
			HtmlPropertyMappers.AddMapperElement(new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.MarginBottomPropertyDescriptor, Paragraph.AutomaticSpacingAfterPropertyDefinition, HtmlConverters.AutomaticSpacingAfterConverter, true, false));
			HtmlPropertyMappers.AddMapperElement(new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.MarginLeftPropertyDescriptor, Table.IndentPropertyDefinition, HtmlConverters.DoubleConverter, true, true));
		}

		static void InitHighlightColorMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BackgroundColorPropertyDescriptor, Run.HighlightColorPropertyDefinition, HtmlConverters.ColorMediaConverter, false, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitBackgroundColorMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BackgroundColorPropertyDescriptor, Run.BackgroundColorPropertyDefinition, HtmlConverters.ThemableColorConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BackgroundColorPropertyDescriptor, Paragraph.BackgroundColorPropertyDefinition, HtmlConverters.ThemableColorConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BackgroundColorPropertyDescriptor, TableCell.BackgroundColorPropertyDefinition, HtmlConverters.ThemableColorConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
			mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.BackgroundColorPropertyDescriptor, Table.BackgroundColorPropertyDefinition, HtmlConverters.ThemableColorConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitTextAlignMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.TextAlignPropertyDescriptor, Paragraph.TextAlignmentPropertyDefinition, HtmlConverters.AlignmentConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitFontFamilyMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.FontFamilyPropertyDescriptor, Run.FontFamilyPropertyDefinition, HtmlConverters.ThemableFontFamilyConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitFontSizeMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.FontSizePropertyDescriptor, Run.FontSizePropertyDefinition, HtmlConverters.FontSizeConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitUnderlineMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.TextDecorationPropertyDescriptor, Run.UnderlinePatternPropertyDefinition, HtmlConverters.UnderlinePatternConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitStrikethroughMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.TextDecorationPropertyDescriptor, Run.StrikethroughPropertyDefinition, HtmlConverters.StrikethroughConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitForegroundColorMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.ColorPropertyDescriptor, Run.ForegroundColorPropertyDefinition, HtmlConverters.ThemableColorConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitFontWeighteMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.FontWeightPropertyDescriptor, Run.FontWeightPropertyDefinition, HtmlConverters.FontWeightConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitFontStyleMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.FontStylePropertyDescriptor, Run.FontStylePropertyDefinition, HtmlConverters.FontStyleConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static void InitBaselineAlignmentMapper()
		{
			HtmlPropertyMapperElement mapperElement = new HtmlPropertyMapperElement(HtmlStylePropertyDescriptors.VerticalAlignmentPropertyDescriptor, Run.BaselineAlignmentPropertyDefinition, HtmlConverters.BaselineAlignmentConverter, true, true);
			HtmlPropertyMappers.AddMapperElement(mapperElement);
		}

		static readonly Dictionary<string, List<HtmlPropertyMapperElement>> htmlPropertiesToMapperElement = new Dictionary<string, List<HtmlPropertyMapperElement>>();

		static readonly Dictionary<IStylePropertyDefinition, List<HtmlPropertyMapperElement>> stylePropertyDefinitionsToMapperElement = new Dictionary<IStylePropertyDefinition, List<HtmlPropertyMapperElement>>();
	}
}
