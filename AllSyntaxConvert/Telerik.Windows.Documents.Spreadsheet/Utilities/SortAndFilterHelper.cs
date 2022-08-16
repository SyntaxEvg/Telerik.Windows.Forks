using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Theming;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class SortAndFilterHelper
	{
		public static bool IsValidSelection(IEnumerable<CellRange> cellRanges)
		{
			return cellRanges.Count<CellRange>() <= 1;
		}

		public static bool IsEmptySortRange(CellSelection selection)
		{
			RangePropertyValue<ICellValue> value = selection.GetValue();
			RangePropertyValue<IFill> fill = selection.GetFill();
			RangePropertyValue<ThemableColor> foreColor = selection.GetForeColor();
			bool flag = true;
			if (!value.IsIndeterminate)
			{
				flag = !(value.Value is EmptyCellValue);
			}
			bool flag2 = true;
			if (!fill.IsIndeterminate)
			{
				flag2 = !fill.Value.Equals(PatternFill.Default);
			}
			bool flag3 = true;
			if (!foreColor.IsIndeterminate)
			{
				flag3 = !foreColor.Value.Equals(SpreadsheetDefaultValues.DefaultForeColor);
			}
			return !flag && !flag2 && !flag3;
		}

		public static IFill GetIFillWithLocalColors(IFill fill, ThemeColorScheme scheme)
		{
			PatternFill patternFill = fill as PatternFill;
			if (patternFill != null && patternFill.PatternColor.IsFromTheme)
			{
				Color actualValue = patternFill.PatternColor.GetActualValue(scheme);
				return new PatternFill(patternFill.PatternType, new ThemableColor(actualValue), patternFill.BackgroundColor);
			}
			GradientFill gradientFill = fill as GradientFill;
			if (gradientFill != null && (gradientFill.Color1.IsFromTheme || gradientFill.Color2.IsFromTheme))
			{
				Color actualValue2 = gradientFill.Color1.GetActualValue(scheme);
				Color actualValue3 = gradientFill.Color2.GetActualValue(scheme);
				return new GradientFill(gradientFill.GradientType, actualValue2, actualValue3);
			}
			return fill;
		}

		public static ThemableColor GetThemableColorWithLocalColors(ThemableColor color, ThemeColorScheme scheme)
		{
			if (color.IsFromTheme)
			{
				Color actualValue = color.GetActualValue(scheme);
				return new ThemableColor(actualValue);
			}
			return color;
		}

		public static CellRange GetActualFilteredRange(CellRange filterRange, Worksheet worksheet)
		{
			if (filterRange == null)
			{
				return null;
			}
			int num = filterRange.FromIndex.RowIndex + 1;
			int columnIndex = filterRange.FromIndex.ColumnIndex;
			int num2 = System.Math.Min(filterRange.ToIndex.RowIndex, worksheet.UsedCellRange.ToIndex.RowIndex);
			int columnIndex2 = filterRange.ToIndex.ColumnIndex;
			if (num > num2)
			{
				return null;
			}
			return new CellRange(num, columnIndex, num2, columnIndex2);
		}
	}
}
