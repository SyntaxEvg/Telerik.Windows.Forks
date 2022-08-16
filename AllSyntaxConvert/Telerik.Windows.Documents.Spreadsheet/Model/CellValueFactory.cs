using System;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Formatting;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public static class CellValueFactory
	{
		public static ICellValue ToCellValue(this string value, Worksheet worksheet, CellIndex cellIndex)
		{
			return value.ToCellValue(worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public static ICellValue ToCellValue(this string value, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			CellValueFormat value2 = worksheet.Cells[rowIndex, columnIndex].GetFormat().Value;
			return value.ToCellValue(worksheet, rowIndex, columnIndex, value2);
		}

		internal static ICellValue ToCellValue(this string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormat)
		{
			ICellValue result;
			CellValueFormat cellValueFormat;
			CellValueFactory.Create(value, worksheet, rowIndex, columnIndex, currentFormat, out result, out cellValueFormat);
			return result;
		}

		public static ICellValue ToCellValue(this CellIndex value, Worksheet worksheet)
		{
			return worksheet.Cells[value].GetValue().Value;
		}

		public static ICellValue ToCellValue(this double value)
		{
			return CellValueFactory.Create(value);
		}

		public static ICellValue ToCellValue(this bool value)
		{
			return CellValueFactory.Create(value);
		}

		public static ICellValue ToCellValue(this DateTime value)
		{
			return CellValueFactory.Create(value);
		}

		public static ICellValue ToTextCellValue(this string value)
		{
			return CellValueFactory.CreateTextCellValue(value);
		}

		public static ICellValue ToFormulaCellValue(this string value, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			return CellValueFactory.CreateFormulaCellValue(value, worksheet, rowIndex, columnIndex);
		}

		public static CellValueType GetCellValueType(this ConstantExpression value)
		{
			if (value is NumberExpression)
			{
				return CellValueType.Number;
			}
			if (value is StringExpression)
			{
				return CellValueType.Text;
			}
			if (value is BooleanExpression)
			{
				return CellValueType.Boolean;
			}
			if (value is ErrorExpression)
			{
				return CellValueType.Error;
			}
			if (value is EmptyExpression)
			{
				return CellValueType.Empty;
			}
			throw new NotImplementedException();
		}

		internal static ICellValue Create(string value, Worksheet worksheet, int rowIndex, int columnIndex, SpreadsheetCultureHelper cultureInfo)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<SpreadsheetCultureHelper>(cultureInfo, "cultureInfo");
			Guard.ThrowExceptionIfNull<string>(value, "value");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			RadExpression value2;
			InputStringCollection translatableString;
			ParseResult parseResult = RadExpression.TryParse(value, worksheet, rowIndex, columnIndex, cultureInfo, out value2, out translatableString, false);
			if (parseResult != ParseResult.Successful)
			{
				return new TextCellValue(value);
			}
			return new FormulaCellValue(translatableString, value2, rowIndex, columnIndex);
		}

		public static void Create(string value, Worksheet worksheet, CellIndex cellIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			CellValueFactory.Create(value, worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex, currentFormatValue, out cellValue, out newFormatValue);
		}

		public static void Create(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			ParseResult parseResult = CellValueParser.Parse(value, worksheet, rowIndex, columnIndex, currentFormatValue, out cellValue, out newFormatValue);
			if (parseResult == ParseResult.Error)
			{
				ExpressionException ex = new ExpressionException(SpreadsheetStrings.GeneralErrorMessage, "Spreadsheet_ErrorExpressions_GeneralErrorMessage", null);
				throw new ParseException(ex.Message, ex, ex.LocalizationKey, ex.FormatStringArguments);
			}
		}

		internal static void CreateIgnoreErrors(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			ParseResult parseResult = CellValueParser.Parse(value, worksheet, rowIndex, columnIndex, currentFormatValue, out cellValue, out newFormatValue);
			if (parseResult != ParseResult.Successful)
			{
				newFormatValue = currentFormatValue;
				cellValue = new TextCellValue(value);
			}
		}

		internal static ParseResult TryCreate(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			return CellValueParser.Parse(value, worksheet, rowIndex, columnIndex, currentFormatValue, out cellValue, out newFormatValue);
		}

		public static ICellValue Create(double value)
		{
			return new NumberCellValue(value);
		}

		public static ICellValue Create(bool value)
		{
			return new BooleanCellValue(value);
		}

		public static ICellValue Create(DateTime value)
		{
			double value2 = FormatHelper.ConvertDateTimeToDouble(value);
			return CellValueFactory.Create(value2);
		}

		public static ICellValue CreateTextCellValue(string value)
		{
			return new TextCellValue(value);
		}

		public static ICellValue CreateFormulaCellValue(string value, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			RadExpression value2;
			InputStringCollection translatableString;
			ParseResult parseResult = RadExpression.TryParse(value, worksheet, rowIndex, columnIndex, out value2, out translatableString, false);
			if (parseResult != ParseResult.Successful)
			{
				throw new InvalidOperationException();
			}
			return new FormulaCellValue(translatableString, value2, rowIndex, columnIndex);
		}
	}
}
