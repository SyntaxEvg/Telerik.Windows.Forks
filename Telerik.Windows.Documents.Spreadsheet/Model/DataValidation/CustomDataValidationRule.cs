using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public sealed class CustomDataValidationRule : SingleArgumentDataValidationRuleBase
	{
		public CustomDataValidationRule(SingleArgumentDataValidationRuleContext context)
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
			bool result = false;
			ICellValue translatedArgument = base.GetTranslatedArgument(base.Argument1, worksheet, rowIndex, columnIndex);
			string resultValueAsString = translatedArgument.GetResultValueAsString(CellValueFormat.GeneralFormat);
			if (!string.IsNullOrEmpty(resultValueAsString))
			{
				bool.TryParse(resultValueAsString, out result);
			}
			return result;
		}
	}
}
