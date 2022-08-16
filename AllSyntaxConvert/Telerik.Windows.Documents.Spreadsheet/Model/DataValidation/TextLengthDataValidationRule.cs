using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public sealed class TextLengthDataValidationRule : NumberDataValidationRuleBase
	{
		public TextLengthDataValidationRule(NumberDataValidationRuleContext context)
			: base(context)
		{
		}

		protected override bool EvaluateOverride(Worksheet worksheet, int rowIndex, int columnIndex, ICellValue cellValue)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			Guard.ThrowExceptionIfNull<ICellValue>(cellValue, "cellValue");
			Guard.ThrowExceptionIfNull<ICellValue>(base.Argument1, "Argument1");
			if (base.ShouldIgnore(cellValue))
			{
				return true;
			}
			CellValueFormat generalFormat = CellValueFormat.GeneralFormat;
			double? cellValueResult = new double?((double)cellValue.GetResultValueAsString(generalFormat).Length);
			double? argument = base.EvaluateArgument(base.Argument1, worksheet, rowIndex, columnIndex);
			double? argument2 = base.EvaluateArgument(base.Argument2, worksheet, rowIndex, columnIndex);
			return cellValueResult != null && argument != null && base.CompareValues(cellValueResult, argument, argument2);
		}
	}
}
