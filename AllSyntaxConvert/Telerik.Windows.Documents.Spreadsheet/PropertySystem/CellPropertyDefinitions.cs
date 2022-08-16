using System;
using System.Collections.ObjectModel;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Model.DataValidation;

namespace Telerik.Windows.Documents.Spreadsheet.PropertySystem
{
	public static class CellPropertyDefinitions
	{
		public static readonly IPropertyDefinition<CellValueFormat> FormatProperty = new PropertyDefinition<CellValueFormat>("Format", true, StylePropertyGroup.Number, CellValueFormat.GeneralFormat, false);

		public static readonly IPropertyDefinition<ICellValue> ValueProperty = new PropertyDefinition<ICellValue>("CellValue", true, StylePropertyGroup.None, EmptyCellValue.EmptyValue, false);

		public static readonly IPropertyDefinition<string> StyleNameProperty = new PropertyDefinition<string>("StyleName", true, StylePropertyGroup.None, SpreadsheetDefaultValues.DefaultStyleName, true);

		public static readonly IPropertyDefinition<CellBorder> LeftBorderProperty = new PropertyDefinition<CellBorder>("LeftBorder", true, StylePropertyGroup.Border, CellBorder.Default, true);

		public static readonly IPropertyDefinition<CellBorder> TopBorderProperty = new PropertyDefinition<CellBorder>("TopBorder", true, StylePropertyGroup.Border, CellBorder.Default, true);

		public static readonly IPropertyDefinition<CellBorder> RightBorderProperty = new PropertyDefinition<CellBorder>("RightBorder", true, StylePropertyGroup.Border, CellBorder.Default, true);

		public static readonly IPropertyDefinition<CellBorder> BottomBorderProperty = new PropertyDefinition<CellBorder>("BottomBorder", true, StylePropertyGroup.Border, CellBorder.Default, true);

		public static readonly IPropertyDefinition<CellBorder> DiagonalUpBorderProperty = new PropertyDefinition<CellBorder>("DiagonalUpBorder", true, StylePropertyGroup.Border, CellBorder.Default, true);

		public static readonly IPropertyDefinition<CellBorder> DiagonalDownBorderProperty = new PropertyDefinition<CellBorder>("DiagonalDownBorder", true, StylePropertyGroup.Border, CellBorder.Default, true);

		public static readonly IPropertyDefinition<IFill> FillProperty = new PropertyDefinition<IFill>("Fill", true, StylePropertyGroup.Fill, PatternFill.Default, true);

		public static readonly IPropertyDefinition<ThemableFontFamily> FontFamilyProperty = new PropertyDefinition<ThemableFontFamily>("FontFamily", true, StylePropertyGroup.Font, SpreadsheetDefaultValues.DefaultFontFamily, true);

		public static readonly IPropertyDefinition<double> FontSizeProperty = new PropertyDefinition<double>("FontSize", true, StylePropertyGroup.Font, SpreadsheetDefaultValues.DefaultFontSize, true);

		public static readonly IPropertyDefinition<bool> IsBoldProperty = new PropertyDefinition<bool>("IsBold", true, StylePropertyGroup.Font, false, true);

		public static readonly IPropertyDefinition<bool> IsItalicProperty = new PropertyDefinition<bool>("IsItalic", true, StylePropertyGroup.Font, false, true);

		public static readonly IPropertyDefinition<UnderlineType> UnderlineProperty = new PropertyDefinition<UnderlineType>("Underline", true, StylePropertyGroup.Font, UnderlineType.None, true);

		public static readonly IPropertyDefinition<ThemableColor> ForeColorProperty = new PropertyDefinition<ThemableColor>("ForeColor", true, StylePropertyGroup.Font, SpreadsheetDefaultValues.DefaultForeColor, true);

		public static readonly IPropertyDefinition<RadHorizontalAlignment> HorizontalAlignmentProperty = new PropertyDefinition<RadHorizontalAlignment>("HorizontalAlignment", true, StylePropertyGroup.Alignment, RadHorizontalAlignment.General, true);

		public static readonly IPropertyDefinition<RadVerticalAlignment> VerticalAlignmentProperty = new PropertyDefinition<RadVerticalAlignment>("VerticalAlignment", true, StylePropertyGroup.Alignment, RadVerticalAlignment.Bottom, true);

		public static readonly IPropertyDefinition<int> IndentProperty = new PropertyDefinition<int>("Indent", true, StylePropertyGroup.Alignment, 0, true);

		public static readonly IPropertyDefinition<bool> IsWrappedProperty = new PropertyDefinition<bool>("IsWrapped", true, StylePropertyGroup.Alignment, false, true);

		public static readonly IPropertyDefinition<bool> IsLockedProperty = new PropertyDefinition<bool>("IsLocked", false, StylePropertyGroup.Protection, true, true);

		public static readonly IPropertyDefinition<IDataValidationRule> DataValidationRuleProperty = new PropertyDefinition<IDataValidationRule>("DataValidationRule", true, StylePropertyGroup.None, new AnyValueDataValidationRule(new AnyValueDataValidationRuleContext()), true);

		public static readonly ReadOnlyCollection<IPropertyDefinition> AllPropertyDefinitions = new ReadOnlyCollection<IPropertyDefinition>(new IPropertyDefinition[]
		{
			CellPropertyDefinitions.FormatProperty,
			CellPropertyDefinitions.ValueProperty,
			CellPropertyDefinitions.StyleNameProperty,
			CellPropertyDefinitions.LeftBorderProperty,
			CellPropertyDefinitions.TopBorderProperty,
			CellPropertyDefinitions.RightBorderProperty,
			CellPropertyDefinitions.BottomBorderProperty,
			CellPropertyDefinitions.DiagonalUpBorderProperty,
			CellPropertyDefinitions.DiagonalDownBorderProperty,
			CellPropertyDefinitions.FillProperty,
			CellPropertyDefinitions.FontFamilyProperty,
			CellPropertyDefinitions.FontSizeProperty,
			CellPropertyDefinitions.IsBoldProperty,
			CellPropertyDefinitions.IsItalicProperty,
			CellPropertyDefinitions.UnderlineProperty,
			CellPropertyDefinitions.ForeColorProperty,
			CellPropertyDefinitions.HorizontalAlignmentProperty,
			CellPropertyDefinitions.VerticalAlignmentProperty,
			CellPropertyDefinitions.IndentProperty,
			CellPropertyDefinitions.IsWrappedProperty,
			CellPropertyDefinitions.IsLockedProperty,
			CellPropertyDefinitions.DataValidationRuleProperty
		});
	}
}
