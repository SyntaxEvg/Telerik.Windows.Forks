using System;
using System.Collections.Generic;
using System.Linq;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public sealed class ListDataValidationRule : SingleArgumentDataValidationRuleBase
	{
		public bool InCellDropdown { get; set; }

		public ListDataValidationRule(ListDataValidationRuleContext context)
			: base(context)
		{
			this.InCellDropdown = context.InCellDropdown;
		}

		protected override bool EvaluateOverride(Worksheet worksheet, int rowIndex, int columnIndex, ICellValue cellValue)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<ICellValue>(cellValue, "cellValue");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			if (base.ShouldIgnore(cellValue))
			{
				return true;
			}
			IEnumerable<string> source = this.ExtractListItems(worksheet, rowIndex, columnIndex);
			string resultValueAsString = cellValue.GetResultValueAsString(CellValueFormat.GeneralFormat);
			return source.Contains(resultValueAsString);
		}

		public IEnumerable<string> ExtractListItems(Worksheet worksheet, CellIndex cellIndex)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<CellIndex>(cellIndex, "cellIndex");
			return this.ExtractListItems(worksheet, cellIndex.RowIndex, cellIndex.ColumnIndex);
		}

		public IEnumerable<string> ExtractListItems(Worksheet worksheet, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			ICellValue cellValue = base.GetTranslatedArgument(base.Argument1, worksheet, rowIndex, columnIndex);
			FormulaCellValue formulaValue = cellValue as FormulaCellValue;
			if (formulaValue != null)
			{
				RadExpression value = formulaValue.Value.GetValue();
				ArrayExpression arrayExpression = value as ArrayExpression;
				if (arrayExpression == null)
				{
					arrayExpression = value.GetValue() as ArrayExpression;
				}
				if (arrayExpression != null)
				{
					foreach (string item in this.GetArrayExpressionItemsAsStrings(arrayExpression))
					{
						yield return item;
					}
				}
			}
			else
			{
				string[] items = cellValue.RawValue.Split(new char[] { SpreadsheetCultureInfo.ListSeparator[0] });
				foreach (string item2 in items)
				{
					yield return item2.Trim();
				}
			}
			yield break;
		}

		IEnumerable<string> GetArrayExpressionItemsAsStrings(ArrayExpression arrayExpression)
		{
			if (arrayExpression != null)
			{
				foreach (RadExpression expression in arrayExpression)
				{
					ArrayExpression array = expression as ArrayExpression;
					if (array != null)
					{
						foreach (string item in this.GetArrayExpressionItemsAsStrings(array))
						{
							yield return item;
						}
					}
					else
					{
						yield return expression.GetValueAsString();
					}
				}
			}
			yield break;
		}

		public override bool Equals(object obj)
		{
			ListDataValidationRule listDataValidationRule = obj as ListDataValidationRule;
			return listDataValidationRule != null && base.Equals(obj) && TelerikHelper.EqualsOfT<bool>(this.InCellDropdown, listDataValidationRule.InCellDropdown);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(base.GetHashCode(), this.InCellDropdown.GetHashCodeOrZero());
		}
	}
}
