using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class ExtensionMethods
	{
		internal static bool IsDateFormat(this CellValueFormat format)
		{
			return format.FormatStringInfo.FormatType == FormatStringType.DateTime;
		}

		public static string GetAsStringOrNull(this ICellValue cellValue, CellValueFormat format)
		{
			if (cellValue == null)
			{
				return null;
			}
			return cellValue.GetValueAsString(format);
		}

		public static double? GetAsDoubleOrNull(this ICellValue cellValue)
		{
			NumberCellValue numberCellValue = cellValue as NumberCellValue;
			if (numberCellValue != null)
			{
				return new double?(numberCellValue.Value);
			}
			FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
			if (formulaCellValue != null)
			{
				RadExpression value = formulaCellValue.Value.GetValue();
				if (value is NumberExpression)
				{
					return new double?(value.NumberValue());
				}
				ArrayExpression arrayExpression = value as ArrayExpression;
				if (arrayExpression != null && arrayExpression.Value.Length == 1)
				{
					return ExtensionMethods.GetSingleValueArrayValueAsDoubleOrNull(arrayExpression);
				}
			}
			return null;
		}

		static double? GetSingleValueArrayValueAsDoubleOrNull(RadExpression expressionValue)
		{
			ArrayExpression arrayExpression = expressionValue as ArrayExpression;
			if (arrayExpression != null && arrayExpression.Value.Length == 1)
			{
				RadExpression value = arrayExpression.Value[0, 0].GetValue();
				return ExtensionMethods.GetSingleValueArrayValueAsDoubleOrNull(value);
			}
			if (expressionValue is NumberExpression)
			{
				return new double?(expressionValue.NumberValue());
			}
			return null;
		}

		public static Color Darker(this Color color)
		{
			return color.Darker(24);
		}

		public static Color Darker(this Color color, byte faktor)
		{
			if (color.R - faktor >= 0)
			{
				color.R -= faktor;
			}
			else
			{
				color.R = 0;
			}
			if (color.G - faktor >= 0)
			{
				color.G -= faktor;
			}
			else
			{
				color.G = 0;
			}
			if (color.B - faktor >= 0)
			{
				color.B -= faktor;
			}
			else
			{
				color.B = 0;
			}
			return color;
		}

		public static Color Lighter(this Color color)
		{
			return color.Lighter(24);
		}

		public static Color Lighter(this Color color, byte faktor)
		{
			if (color.R + faktor < 255)
			{
				color.R += faktor;
			}
			else
			{
				color.R = byte.MaxValue;
			}
			if (color.G + faktor < 255)
			{
				color.G += faktor;
			}
			else
			{
				color.G = byte.MaxValue;
			}
			if (color.B + faktor < 255)
			{
				color.B += faktor;
			}
			else
			{
				color.B = byte.MaxValue;
			}
			return color;
		}

		public static string GetSourceOrNull(this FontFamily family)
		{
			if (family == null)
			{
				return null;
			}
			return family.Source;
		}

		public static int GetHashCodeOrZero(this object obj)
		{
			if (obj == null)
			{
				return 0;
			}
			return obj.GetHashCode();
		}

		public static bool EqualsDouble(this double d1, double d2)
		{
			return Math.Abs(d1 - d2) <= ((Math.Abs(d1) > Math.Abs(d2)) ? Math.Abs(d2) : Math.Abs(d1)) * 1E-08;
		}

		public static bool GreaterOrEqualDouble(this double d1, double d2)
		{
			return d1.EqualsDouble(d2) || d1 > d2;
		}

		public static bool LessOrEqualDouble(this double d1, double d2)
		{
			return d1.EqualsDouble(d2) || d1 < d2;
		}

		public static bool CompareIgnoringSpecialSymbols(this string value, string otherValue)
		{
			Guard.ThrowExceptionIfNull<string>(value, "value");
			Guard.ThrowExceptionIfNull<string>(otherValue, "otherValue");
			List<char> list = new List<char> { '"', '_', '\\' };
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < value.Length; i++)
			{
				if (!list.Contains(value[i]))
				{
					stringBuilder.Append(value[i]);
				}
			}
			StringBuilder stringBuilder2 = new StringBuilder();
			for (int j = 0; j < otherValue.Length; j++)
			{
				if (!list.Contains(otherValue[j]))
				{
					stringBuilder2.Append(otherValue[j]);
				}
			}
			string text = stringBuilder.ToString();
			string text2 = stringBuilder2.ToString();
			if (text.Length != text2.Length)
			{
				return false;
			}
			for (int k = 0; k < text.Length; k++)
			{
				char c = text[k];
				char c2 = text2[k];
				if (c != c2 && (char.GetUnicodeCategory(c) != UnicodeCategory.SpaceSeparator || char.GetUnicodeCategory(c2) != UnicodeCategory.SpaceSeparator))
				{
					return false;
				}
			}
			return true;
		}

		internal static MemoryStream ToMemoryStream(this Stream stream)
		{
			MemoryStream memoryStream = new MemoryStream();
			stream.CopyTo(memoryStream);
			memoryStream.Seek(0L, SeekOrigin.Begin);
			return memoryStream;
		}

		public static DateTime GetBeginningOfWeek(this DateTime date)
		{
			DayOfWeek dayOfWeek = date.DayOfWeek;
			return date.AddDays((double)(-(double)dayOfWeek));
		}

		public static int GetQuarter(this DateTime date)
		{
			if (date.Month >= 1 && date.Month <= 3)
			{
				return 1;
			}
			if (date.Month >= 4 && date.Month <= 6)
			{
				return 2;
			}
			if (date.Month >= 7 && date.Month <= 9)
			{
				return 3;
			}
			return 4;
		}

		public static bool DateIsNextQuarter(this DateTime otherDate)
		{
			DateTime today = DateTime.Today;
			int quarter = today.GetQuarter();
			int quarter2 = otherDate.GetQuarter();
			int num = quarter2 - quarter;
			return (num == 1 && today.Year == otherDate.Year) || (num == -3 && today.Year + 1 == otherDate.Year);
		}

		public static bool DateIsLastQuarter(this DateTime otherDate)
		{
			DateTime today = DateTime.Today;
			int quarter = today.GetQuarter();
			int quarter2 = otherDate.GetQuarter();
			int num = quarter - quarter2;
			return (num == 1 && today.Year == otherDate.Year) || (num == -3 && today.Year - 1 == otherDate.Year);
		}

		public static bool ContainsOverlappingRanges(this IEnumerable<CellRange> cellRanges)
		{
			List<CellRange> list = cellRanges.ToList<CellRange>();
			for (int i = 0; i < list.Count; i++)
			{
				for (int j = i + 1; j < list.Count; j++)
				{
					if (list[i].IntersectsWith(list[j]))
					{
						return true;
					}
				}
			}
			return false;
		}

		public static bool IntersectsWith(this CellRange cellrange, int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			Guard.ThrowExceptionIfNull<CellRange>(cellrange, "cellrange");
			return fromColumnIndex <= cellrange.ToIndex.ColumnIndex && toColumnIndex >= cellrange.FromIndex.ColumnIndex && fromRowIndex <= cellrange.ToIndex.RowIndex && toRowIndex >= cellrange.FromIndex.RowIndex;
		}
	}
}
