using System;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Formatting.FormatStrings;
using Telerik.Windows.Documents.Spreadsheet.Model;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Formatting
{
	static class CellValueParser
	{
		public static ParseResult Parse(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			Guard.ThrowExceptionIfNull<CellValueFormat>(currentFormatValue, "currentFormatValue");
			foreach (CellValueParser.TryParseCellValueDelegate tryParseCellValueDelegate in CellValueParser.tryParseCellValueHandlers)
			{
				ParseResult parseResult = tryParseCellValueDelegate(value, worksheet, rowIndex, columnIndex, currentFormatValue, out cellValue, out newFormatValue);
				if (parseResult != ParseResult.Unsuccessful)
				{
					return parseResult;
				}
			}
			throw new LocalizableException("Cannot parse value to ICellValue.", new InvalidOperationException("Cannot parse value to ICellValue."), "Spreadsheet_ErrorExpressions_CannotParseValue", null);
		}

		static ParseResult TryParseEmptyCellValue(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			cellValue = null;
			newFormatValue = null;
			if (string.IsNullOrEmpty(value))
			{
				cellValue = EmptyCellValue.EmptyValue;
				newFormatValue = currentFormatValue;
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		static ParseResult TryParseTextFormatCellValue(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			cellValue = null;
			newFormatValue = null;
			if (currentFormatValue.FormatStringInfo.FormatType == FormatStringType.Text)
			{
				return CellValueParser.TryParseTextCellValue(value, worksheet, rowIndex, columnIndex, currentFormatValue, out cellValue, out newFormatValue);
			}
			return ParseResult.Unsuccessful;
		}

		static ParseResult TryParseNumberCellValue(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			cellValue = null;
			newFormatValue = CellValueFormat.GeneralFormat;
			FormatStringType? forceParsingAsSpecificType = null;
			if (currentFormatValue.FormatStringInfo.FormatType == FormatStringType.Number && !string.IsNullOrEmpty(currentFormatValue.FormatString))
			{
				forceParsingAsSpecificType = new FormatStringType?(FormatStringType.Number);
			}
			string text;
			double doubleValue;
			if (NumberValueParser.TryParse(value, forceParsingAsSpecificType, null, out text, out doubleValue))
			{
				cellValue = new NumberCellValue(doubleValue);
				if (!string.IsNullOrEmpty(text))
				{
					newFormatValue = new CellValueFormat(text);
				}
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		static ParseResult TryParseBooleanCellValue(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			cellValue = null;
			newFormatValue = null;
			bool value2;
			if (bool.TryParse(value.ToLower(), out value2))
			{
				cellValue = new BooleanCellValue(value2);
				newFormatValue = currentFormatValue;
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		static ParseResult TryParseFormulaCellValue(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			cellValue = null;
			newFormatValue = null;
			RadExpression value2;
			InputStringCollection translatableString;
			ParseResult parseResult = RadExpression.TryParse(value, worksheet, rowIndex, columnIndex, out value2, out translatableString, false);
			if (parseResult == ParseResult.Successful)
			{
				FormulaCellValue formulaCellValue = new FormulaCellValue(translatableString, value2, rowIndex, columnIndex);
				FunctionExpression functionExpression = formulaCellValue.Value as FunctionExpression;
				newFormatValue = currentFormatValue;
				if (functionExpression != null && functionExpression.IsValid)
				{
					newFormatValue = functionExpression.Function.FunctionInfo.Format;
				}
				cellValue = formulaCellValue;
			}
			return parseResult;
		}

		static ParseResult TryParseErrorCellValue(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			cellValue = null;
			newFormatValue = null;
			ErrorCellValue errorCellValue;
			if (SpreadsheetCultureHelper.IsCharEqualTo(value[0], new string[] { "#" }) && ErrorCellValue.TryGetErrorCellValue(value, out errorCellValue))
			{
				cellValue = errorCellValue;
				newFormatValue = currentFormatValue;
				return ParseResult.Successful;
			}
			return ParseResult.Unsuccessful;
		}

		static ParseResult TryParseTextCellValue(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue)
		{
			cellValue = new TextCellValue(value);
			newFormatValue = currentFormatValue;
			return ParseResult.Successful;
		}

		static readonly CellValueParser.TryParseCellValueDelegate[] tryParseCellValueHandlers = new CellValueParser.TryParseCellValueDelegate[]
		{
			new CellValueParser.TryParseCellValueDelegate(CellValueParser.TryParseEmptyCellValue),
			new CellValueParser.TryParseCellValueDelegate(CellValueParser.TryParseTextFormatCellValue),
			new CellValueParser.TryParseCellValueDelegate(CellValueParser.TryParseNumberCellValue),
			new CellValueParser.TryParseCellValueDelegate(CellValueParser.TryParseBooleanCellValue),
			new CellValueParser.TryParseCellValueDelegate(CellValueParser.TryParseFormulaCellValue),
			new CellValueParser.TryParseCellValueDelegate(CellValueParser.TryParseErrorCellValue),
			new CellValueParser.TryParseCellValueDelegate(CellValueParser.TryParseTextCellValue)
		};

		delegate ParseResult TryParseCellValueDelegate(string value, Worksheet worksheet, int rowIndex, int columnIndex, CellValueFormat currentFormatValue, out ICellValue cellValue, out CellValueFormat newFormatValue);
	}
}
