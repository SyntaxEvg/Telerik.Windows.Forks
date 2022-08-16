using System;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	static class TelerikHelper
	{
		public static int CombineHashCodes(int h1, int h2)
		{
			return ((h1 << 5) + h1) ^ h2;
		}

		public static int CombineHashCodes(int h1, int h2, int h3)
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(h1, h2), h3);
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4)
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(h1, h2), TelerikHelper.CombineHashCodes(h3, h4));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5)
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(h1, h2, h3, h4), h5);
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6)
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(h1, h2, h3, h4), TelerikHelper.CombineHashCodes(h5, h6));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7)
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(h1, h2, h3, h4), TelerikHelper.CombineHashCodes(h5, h6, h7));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8)
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(h1, h2, h3, h4), TelerikHelper.CombineHashCodes(h5, h6, h7, h8));
		}

		public static int CombineHashCodes(int h1, int h2, int h3, int h4, int h5, int h6, int h7, int h8, int h9)
		{
			return TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(TelerikHelper.CombineHashCodes(h1, h2, h3, h4), TelerikHelper.CombineHashCodes(h5, h6, h7, h8)), h9);
		}

		public static int CombineHashCodes(int h1, int h2, params int[] h)
		{
			int num = TelerikHelper.CombineHashCodes(h1, h2);
			foreach (int h3 in h)
			{
				num = TelerikHelper.CombineHashCodes(num, h3);
			}
			return num;
		}

		public static void Swap<T>(ref T first, ref T second)
		{
			T t = first;
			first = second;
			second = t;
		}

		public static T Max<T>(T first, T second) where T : IComparable<T>
		{
			if (first.CompareTo(second) <= 0)
			{
				return second;
			}
			return first;
		}

		public static T Min<T>(T first, T second) where T : IComparable<T>
		{
			if (first.CompareTo(second) >= 0)
			{
				return second;
			}
			return first;
		}

		public static DependencyObject GetFocusedElement(UIElement element)
		{
			return (DependencyObject)FocusManager.GetFocusedElement(FocusManager.GetFocusScope(element));
		}

		public static bool EqualsOfT<T>(T first, T second)
		{
			return (first == null && second == null) || (first != null && first.Equals(second));
		}

		public static bool IsValidRowIndex(int rowIndex)
		{
			return rowIndex >= 0 && rowIndex < SpreadsheetDefaultValues.RowCount;
		}

		public static bool IsValidFontSizeInPoints(double fontSize)
		{
			return fontSize >= 1.0 && fontSize <= 409.0;
		}

		public static bool IsValidColumnIndex(int columnIndex)
		{
			return columnIndex >= 0 && columnIndex < SpreadsheetDefaultValues.ColumnCount;
		}
	}
}
