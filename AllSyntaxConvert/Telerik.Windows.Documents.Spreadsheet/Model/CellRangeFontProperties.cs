using System;
using Telerik.Windows.Documents.Spreadsheet.Core.DataStructures;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	class CellRangeFontProperties
	{
		public CellRangeFontProperties(Cells cells, CellRange cellRange, bool onlyPropertiesAffectingLayout = false)
		{
			this.onlyPropertiesAffectingLayout = onlyPropertiesAffectingLayout;
			this.fontFamilyValues = cells.PropertyBag.GetPropertyValueRespectingStyle<ThemableFontFamily>(CellPropertyDefinitions.FontFamilyProperty, cells.Worksheet, cellRange);
			this.fontSizeValues = cells.PropertyBag.GetPropertyValueRespectingStyle<double>(CellPropertyDefinitions.FontSizeProperty, cells.Worksheet, cellRange);
			this.isBoldValues = cells.PropertyBag.GetPropertyValueRespectingStyle<bool>(CellPropertyDefinitions.IsBoldProperty, cells.Worksheet, cellRange);
			if (!onlyPropertiesAffectingLayout)
			{
				this.isItalicValues = cells.PropertyBag.GetPropertyValueRespectingStyle<bool>(CellPropertyDefinitions.IsItalicProperty, cells.Worksheet, cellRange);
				this.underlineValues = cells.PropertyBag.GetPropertyValueRespectingStyle<UnderlineType>(CellPropertyDefinitions.UnderlineProperty, cells.Worksheet, cellRange);
				this.foreColorValues = cells.PropertyBag.GetPropertyValueRespectingStyle<ThemableColor>(CellPropertyDefinitions.ForeColorProperty, cells.Worksheet, cellRange);
			}
		}

		public FontProperties GetFontProperties(long index, DocumentTheme theme)
		{
			FontProperties result = new FontProperties(this.fontFamilyValues.GetValue(index).GetActualValue(theme), this.fontSizeValues.GetValue(index), this.isBoldValues.GetValue(index));
			if (!this.onlyPropertiesAffectingLayout)
			{
				result.IsItalic = this.isItalicValues.GetValue(index);
				result.Underline = this.underlineValues.GetValue(index);
				result.ForeColor = this.foreColorValues.GetValue(index);
			}
			return result;
		}

		readonly bool onlyPropertiesAffectingLayout;

		readonly ICompressedList<ThemableFontFamily> fontFamilyValues;

		readonly ICompressedList<double> fontSizeValues;

		readonly ICompressedList<bool> isBoldValues;

		readonly ICompressedList<bool> isItalicValues;

		readonly ICompressedList<UnderlineType> underlineValues;

		readonly ICompressedList<ThemableColor> foreColorValues;
	}
}
