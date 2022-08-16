using System;
using System.Text;
using Telerik.Documents.SpreadsheetStreaming.Model;

namespace Telerik.Documents.SpreadsheetStreaming.Utilities
{
	static class NameConverter
	{
		public static string ConvertRowIndexToName(int rowIndex)
		{
			rowIndex++;
			return rowIndex.ToString();
		}

		public static int ConvertRowNameToIndex(string rowName)
		{
			int num;
			if (!int.TryParse(rowName, out num))
			{
				throw new InvalidOperationException(string.Format("'{0}' is invalid row name.", rowName));
			}
			num--;
			return num;
		}

		public static string ConvertColumnIndexToName(int columnIndex)
		{
			string arg = string.Empty;
			while (columnIndex >= 26)
			{
				arg = (char)(columnIndex % NameConverter.LatinAlphabetLettersCount + 65) + arg;
				columnIndex = columnIndex / NameConverter.LatinAlphabetLettersCount - 1;
			}
			return (char)(columnIndex % NameConverter.LatinAlphabetLettersCount + 65) + arg;
		}

		public static string ConvertCellIndexToName(int rowIndex, int columnIndex)
		{
			string str = NameConverter.ConvertRowIndexToName(rowIndex);
			string str2 = NameConverter.ConvertColumnIndexToName(columnIndex);
			return str2 + str;
		}

		public static void ConvertCellNameToIndex(string cellName, out int rowIndex, out int columnIndex)
		{
			int firstDigitIndex = NameConverter.GetFirstDigitIndex(cellName);
			rowIndex = NameConverter.ConvertRowNameToIndex(cellName.Substring(firstDigitIndex));
			columnIndex = NameConverter.ConvertColumnNameToIndex(cellName.Substring(0, firstDigitIndex));
		}

		public static void ConvertCellNameToIndex(string cellName, out bool isRowAbsolute, out int rowIndex, out bool isColumnAbsolute, out int columnIndex)
		{
			Guard.ThrowExceptionIfNullOrEmpty(cellName, "cellName");
			string text = cellName.Replace("$", string.Empty);
			NameConverter.ConvertCellNameToIndex(text, out rowIndex, out columnIndex);
			int num = cellName.Length - text.Length;
			isColumnAbsolute = cellName.StartsWith("$");
			if (isColumnAbsolute)
			{
				num--;
			}
			isRowAbsolute = num > 0;
		}

		public static int ConvertColumnNameToIndex(string columnName)
		{
			int result;
			if (!NameConverter.TryConvertColumnNameToIndexInternal(columnName, out result))
			{
				throw new InvalidOperationException(string.Format("'{0}' is invalid column name.", columnName));
			}
			return result;
		}

		public static string ConvertCellRangeToName(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			return NameConverter.ConvertCellIndexesToName(fromRowIndex, fromColumnIndex, toRowIndex, toColumnIndex);
		}

		public static string ConvertCellIndexesToName(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			return string.Format("{0}:{1}", NameConverter.ConvertCellIndexToName(fromRowIndex, fromColumnIndex), NameConverter.ConvertCellIndexToName(toRowIndex, toColumnIndex));
		}

		internal static void ConvertCellNameToCellRange(string cellName, out SpreadCellRange cellRange)
		{
			Guard.ThrowExceptionIfNullOrEmpty(cellName, "cellName");
			string[] array = cellName.Split(new char[] { ':' });
			int num;
			int num2;
			if (array.Length == 1)
			{
				NameConverter.ConvertCellNameToIndex(cellName, out num, out num2);
				cellRange = new SpreadCellRange(num, num2, num, num2);
				return;
			}
			int toRowIndex = 0;
			int toColumnIndex = 0;
			bool flag;
			bool flag2;
			NameConverter.ConvertCellNameToIndex(array[0], out flag, out num, out flag2, out num2);
			bool flag3;
			bool flag4;
			NameConverter.ConvertCellNameToIndex(array[1], out flag3, out toRowIndex, out flag4, out toColumnIndex);
			cellRange = new SpreadCellRange(num, num2, toRowIndex, toColumnIndex);
		}

		static bool TryConvertColumnNameToIndexInternal(string columnName, out int index)
		{
			index = -1;
			if (!NameConverter.IsValidColumnName(columnName))
			{
				return false;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < columnName.Length; i++)
			{
				stringBuilder.Append(char.ToUpperInvariant(columnName[i]));
			}
			string text = stringBuilder.ToString();
			int num = NameConverter.LatinAlphabetLettersCount;
			index = (int)(text[text.Length - 1] - 'A');
			for (int j = text.Length - 2; j >= 0; j--)
			{
				index += num * (int)(text[j] - 'A' + '\u0001');
				num *= NameConverter.LatinAlphabetLettersCount;
			}
			return index <= DefaultValues.ColumnCount - 1;
		}

		static bool IsValidColumnName(string columnName)
		{
			if (string.IsNullOrEmpty(columnName))
			{
				return false;
			}
			for (int i = 0; i < columnName.Length; i++)
			{
				if (!char.IsLetter(columnName[i]))
				{
					return false;
				}
			}
			return true;
		}

		static int GetFirstDigitIndex(string cellName)
		{
			for (int i = 0; i < cellName.Length; i++)
			{
				if (char.IsDigit(cellName[i]))
				{
					return i;
				}
			}
			return 0;
		}

		static readonly int LatinAlphabetLettersCount = 26;
	}
}
