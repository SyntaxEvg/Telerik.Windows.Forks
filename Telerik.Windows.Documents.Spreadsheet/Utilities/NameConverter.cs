using System;
using System.Linq;
using System.Text;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Model;

namespace Telerik.Windows.Documents.Spreadsheet.Utilities
{
	public static class NameConverter
	{
		public static string ConvertRowIndexToName(int rowIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			rowIndex++;
			return rowIndex.ToString();
		}

		public static int ConvertRowNameToIndex(string rowName)
		{
			int num;
			if (!int.TryParse(rowName, out num))
			{
				throw new LocalizableException(string.Format("'{0}' is invalid row name.", rowName), new InvalidOperationException(string.Format("'{0}' is invalid row name.", rowName)), "Spreadsheet_ErrorExpressions_InvalidRowName", new string[] { rowName });
			}
			num--;
			Guard.ThrowExceptionIfInvalidRowIndex(num);
			return num;
		}

		internal static bool TryConvertRowNameToIndex(string rowName, out int rowIndex)
		{
			rowIndex = -1;
			if (!NameConverter.IsValidRowName(rowName))
			{
				return false;
			}
			if (int.TryParse(rowName, out rowIndex))
			{
				rowIndex--;
				return rowIndex >= 0 && rowIndex <= SpreadsheetDefaultValues.RowCount - 1;
			}
			return false;
		}

		public static string ConvertColumnIndexToName(int columnIndex)
		{
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			string arg = string.Empty;
			while (columnIndex >= 26)
			{
				arg = (char)(columnIndex % NameConverter.LatinAlphabetLettersCount + 65) + arg;
				columnIndex = columnIndex / NameConverter.LatinAlphabetLettersCount - 1;
			}
			return (char)(columnIndex % NameConverter.LatinAlphabetLettersCount + 65) + arg;
		}

		public static int ConvertColumnNameToIndex(string columnName)
		{
			int num;
			if (!NameConverter.TryConvertColumnNameToIndexInternal(columnName, out num))
			{
				throw new LocalizableException(string.Format("'{0}' is invalid column name.", columnName), new InvalidOperationException(string.Format("'{0}' is invalid column name.", columnName)), "Spreadsheet_ErrorExpressions_InvalidColumnName", new string[] { columnName });
			}
			Guard.ThrowExceptionIfInvalidColumnIndex(num);
			return num;
		}

		public static bool TryConvertNamesToCellReferenceRangeExpression(string cellRangesNames, Worksheet worksheet, int rowIndex, int columnIndex, out CellReferenceRangeExpression expression)
		{
			expression = null;
			if (string.IsNullOrEmpty(cellRangesNames))
			{
				return false;
			}
			string text = "=";
			if (cellRangesNames.Length > 1 && cellRangesNames[0].ToString() != text)
			{
				cellRangesNames = text + cellRangesNames;
			}
			expression = null;
			RadExpression radExpression;
			ParseResult parseResult = RadExpression.TryParse(cellRangesNames, worksheet, rowIndex, columnIndex, out radExpression);
			if (parseResult == ParseResult.Successful)
			{
				expression = radExpression as CellReferenceRangeExpression;
				if (expression == null)
				{
					UnionExpression unionExpression = radExpression as UnionExpression;
					if (unionExpression != null)
					{
						expression = unionExpression.GetValue() as CellReferenceRangeExpression;
					}
				}
			}
			return expression != null;
		}

		internal static bool TryConvertColumnNameToIndex(string columnName, out int columnIndex)
		{
			return NameConverter.TryConvertColumnNameToIndexInternal(columnName, out columnIndex) && columnIndex >= 0 && columnIndex <= SpreadsheetDefaultValues.ColumnCount - 1;
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
			return index <= SpreadsheetDefaultValues.ColumnCount - 1;
		}

		public static string ConvertCellIndexToName(int rowIndex, int columnIndex)
		{
			string str = NameConverter.ConvertRowIndexToName(rowIndex);
			string str2 = NameConverter.ConvertColumnIndexToName(columnIndex);
			return str2 + str;
		}

		internal static string ConvertCellIndexToAbsoluteName(CellIndex cellIndex)
		{
			string value = NameConverter.ConvertRowIndexToName(cellIndex.RowIndex);
			string value2 = NameConverter.ConvertColumnIndexToName(cellIndex.ColumnIndex);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("$");
			stringBuilder.Append(value2);
			stringBuilder.Append("$");
			stringBuilder.Append(value);
			return stringBuilder.ToString();
		}

		public static string ConvertCellReferenceToName(CellReference cellReference)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (cellReference.ColumnReference.IsAbsolute)
			{
				stringBuilder.Append('$');
			}
			stringBuilder.Append(NameConverter.ConvertColumnIndexToName(cellReference.ColumnReference.ActualIndex));
			if (cellReference.RowReference.IsAbsolute)
			{
				stringBuilder.Append('$');
			}
			stringBuilder.Append(NameConverter.ConvertRowIndexToName(cellReference.RowReference.ActualIndex));
			return stringBuilder.ToString();
		}

		public static string ConvertCellRangeToName(CellIndex fromIndex, CellIndex toIndex)
		{
			Guard.ThrowExceptionIfNull<CellIndex>(fromIndex, "fromIndex");
			Guard.ThrowExceptionIfNull<CellIndex>(toIndex, "toIndex");
			return NameConverter.ConvertCellIndexesToName(fromIndex.RowIndex, fromIndex.ColumnIndex, toIndex.RowIndex, toIndex.ColumnIndex);
		}

		public static string ConvertCellReferenceRangeToName(CellReference fromCellReference, CellReference toCellReference)
		{
			Guard.ThrowExceptionIfNull<CellReference>(fromCellReference, "fromCellReference");
			Guard.ThrowExceptionIfNull<CellReference>(toCellReference, "toCellReference");
			Guard.ThrowExceptionIfInvalidRowIndex(fromCellReference.ActualRowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(fromCellReference.ActualColumnIndex);
			Guard.ThrowExceptionIfInvalidRowIndex(toCellReference.ActualRowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(toCellReference.ActualColumnIndex);
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(NameConverter.ConvertCellReferenceToName(fromCellReference));
			stringBuilder.Append(':');
			stringBuilder.Append(NameConverter.ConvertCellReferenceToName(toCellReference));
			return stringBuilder.ToString();
		}

		public static string ConvertCellIndexesToName(int fromRowIndex, int fromColumnIndex, int toRowIndex, int toColumnIndex)
		{
			Guard.ThrowExceptionIfInvalidRowIndex(fromRowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(fromColumnIndex);
			Guard.ThrowExceptionIfInvalidRowIndex(toRowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(toColumnIndex);
			return string.Format("{0}:{1}", NameConverter.ConvertCellIndexToName(fromRowIndex, fromColumnIndex), NameConverter.ConvertCellIndexToName(toRowIndex, toColumnIndex));
		}

		public static string ConvertCellIndexToName(CellIndex cellIndex)
		{
			return NameConverter.ConvertCellIndexToName(cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public static void ConvertCellNameToIndex(string cellName, out int rowIndex, out int columnIndex)
		{
			Guard.ThrowExceptionIfNullOrEmpty(cellName, "cellName");
			int firstDigitIndex = NameConverter.GetFirstDigitIndex(cellName);
			rowIndex = NameConverter.ConvertRowNameToIndex(cellName.Substring(firstDigitIndex));
			columnIndex = NameConverter.ConvertColumnNameToIndex(cellName.Substring(0, firstDigitIndex));
		}

		public static bool TryConvertCellNameToIndex(string cellName, out int rowIndex, out int columnIndex)
		{
			rowIndex = 0;
			columnIndex = 0;
			if (string.IsNullOrEmpty(cellName))
			{
				return false;
			}
			int firstDigitIndex = NameConverter.GetFirstDigitIndex(cellName);
			return firstDigitIndex != 0 && NameConverter.TryConvertRowNameToIndex(cellName.Substring(firstDigitIndex), out rowIndex) && NameConverter.TryConvertColumnNameToIndex(cellName.Substring(0, firstDigitIndex), out columnIndex);
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

		internal static void ConvertCellNameToCellRange(string cellName, out CellRange cellRange)
		{
			Guard.ThrowExceptionIfNullOrEmpty(cellName, "cellName");
			string[] array = cellName.Split(new char[] { ':' });
			int rowIndex;
			int columnIndex;
			if (array.Length == 1)
			{
				NameConverter.ConvertCellNameToIndex(cellName, out rowIndex, out columnIndex);
				CellIndex cellIndex = new CellIndex(rowIndex, columnIndex);
				cellRange = new CellRange(cellIndex, cellIndex);
				return;
			}
			int rowIndex2 = 0;
			int columnIndex2 = 0;
			bool flag;
			bool flag2;
			NameConverter.ConvertCellNameToIndex(array[0], out flag, out rowIndex, out flag2, out columnIndex);
			bool flag3;
			bool flag4;
			NameConverter.ConvertCellNameToIndex(array[1], out flag3, out rowIndex2, out flag4, out columnIndex2);
			cellRange = new CellRange(new CellIndex(rowIndex, columnIndex), new CellIndex(rowIndex2, columnIndex2));
		}

		internal static bool TryConvertCellNameToCellRange(string cellName, out CellRange cellRange)
		{
			cellRange = null;
			bool result;
			try
			{
				NameConverter.ConvertCellNameToCellRange(cellName, out cellRange);
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
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

		public static bool TryConvertCellNameToIndex(string cellName, out bool isRowAbsolute, out int rowIndex, out bool isColumnAbsolute, out int columnIndex)
		{
			isRowAbsolute = false;
			isColumnAbsolute = false;
			rowIndex = 0;
			columnIndex = 0;
			if (string.IsNullOrEmpty(cellName))
			{
				return false;
			}
			string text = cellName.Replace("$", string.Empty);
			if (NameConverter.TryConvertCellNameToIndex(text, out rowIndex, out columnIndex))
			{
				int num = cellName.Length - text.Length;
				isColumnAbsolute = cellName.StartsWith("$");
				if (isColumnAbsolute)
				{
					num--;
				}
				isRowAbsolute = num > 0;
				return true;
			}
			return false;
		}

		internal static string ConvertCellRangesToName(Worksheet worksheet, CellRange[] ranges, bool referencesAreAbsolute)
		{
			CellRange cellRange = ranges.Last<CellRange>();
			InputStringCollection inputStringCollection = new InputStringCollection();
			string worksheetName = TextHelper.EncodeWorksheetName(worksheet.Name, false);
			foreach (CellRange cellRange2 in ranges)
			{
				RowColumnReference rowReference;
				RowColumnReference columnReference;
				RowColumnReference rowReference2;
				RowColumnReference columnReference2;
				if (referencesAreAbsolute)
				{
					rowReference = RowColumnReference.CreateAbsoluteRowColumnReference(cellRange2.FromIndex.RowIndex);
					columnReference = RowColumnReference.CreateAbsoluteRowColumnReference(cellRange2.FromIndex.ColumnIndex);
					rowReference2 = RowColumnReference.CreateAbsoluteRowColumnReference(cellRange2.ToIndex.RowIndex);
					columnReference2 = RowColumnReference.CreateAbsoluteRowColumnReference(cellRange2.ToIndex.ColumnIndex);
				}
				else
				{
					rowReference = RowColumnReference.CreateRelativeRowColumnReference(cellRange2.FromIndex.RowIndex, 0);
					columnReference = RowColumnReference.CreateRelativeRowColumnReference(cellRange2.FromIndex.ColumnIndex, 0);
					rowReference2 = RowColumnReference.CreateRelativeRowColumnReference(cellRange2.ToIndex.RowIndex, 0);
					columnReference2 = RowColumnReference.CreateRelativeRowColumnReference(cellRange2.ToIndex.ColumnIndex, 0);
				}
				CellAreaReference cellReferenceRange = new CellAreaReference(new CellReference(rowReference, columnReference), new CellReference(rowReference2, columnReference2), false);
				CellReferenceRangeExpression expression = new CellReferenceRangeExpression(worksheet.Workbook, worksheetName, true, cellReferenceRange);
				CellReferenceInputString stringExpression = new CellReferenceInputString(expression);
				inputStringCollection.Add(stringExpression);
				if (cellRange2 != cellRange)
				{
					inputStringCollection.Add(new ListSeparatorInputString());
				}
			}
			return inputStringCollection.BuildStringInCulture(SpreadsheetCultureHelper.InvariantSpreadsheetCultureInfo);
		}

		public static bool IsValidA1CellName(string cellName)
		{
			bool flag;
			int num;
			bool flag2;
			int num2;
			return !string.IsNullOrEmpty(cellName) && NameConverter.TryConvertCellNameToIndex(cellName, out flag, out num, out flag2, out num2);
		}

		static bool IsValidColumnName(string columnName)
		{
			if (string.IsNullOrEmpty(columnName))
			{
				return false;
			}
			return columnName.All((char x) => char.IsLetter(x));
		}

		static bool IsValidRowName(string rowName)
		{
			return rowName.All((char x) => char.IsDigit(x));
		}

		static readonly int LatinAlphabetLettersCount = 26;
	}
}
