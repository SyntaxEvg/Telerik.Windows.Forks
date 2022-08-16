using System;
using Telerik.Windows.Documents.Spreadsheet.PropertySystem;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	static class DataValidationRulesExtensionMethods
	{
		public static bool Evaluate(this IDataValidationRule rule, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<IDataValidationRule>(rule, "rule");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			long index = WorksheetPropertyBagBase.ConvertCellIndexToLong(rowIndex, columnIndex);
			ICellValue propertyValue = worksheet.Cells.PropertyBag.GetPropertyValue<ICellValue>(CellPropertyDefinitions.ValueProperty, index);
			return rule.Evaluate(worksheet, rowIndex, columnIndex, propertyValue);
		}

		public static bool Evaluate(this IDataValidationRule rule, Worksheet worksheet, CellIndex cellIndex, string cellValue)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<IDataValidationRule>(rule, "rule");
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			return rule.Evaluate(worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex, cellValue);
		}

		public static bool Evaluate(this IDataValidationRule rule, Worksheet worksheet, int rowIndex, int columnIndex, string cellValue)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<IDataValidationRule>(rule, "rule");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			ICellValue cellValue2 = cellValue.ToCellValue(worksheet, rowIndex, columnIndex);
			return rule.Evaluate(worksheet, rowIndex, columnIndex, cellValue2);
		}
	}
}
