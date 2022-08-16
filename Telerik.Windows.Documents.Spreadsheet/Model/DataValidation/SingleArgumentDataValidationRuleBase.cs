using System;
using Telerik.Windows.Documents.Spreadsheet.Expressions;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public abstract class SingleArgumentDataValidationRuleBase : DataValidationRuleBase
	{
		public bool IgnoreBlank
		{
			get
			{
				return this.ignoreBlank;
			}
		}

		public CellIndex CellIndex
		{
			get
			{
				return this.cellIndex;
			}
		}

		public ICellValue Argument1
		{
			get
			{
				return this.argument1;
			}
		}

		protected SingleArgumentDataValidationRuleBase(SingleArgumentDataValidationRuleContext context)
			: base(context)
		{
			Guard.ThrowExceptionIfNull<SingleArgumentDataValidationRuleContext>(context, "context");
			Guard.ThrowExceptionIfNull<Worksheet>(context.Worksheet, "Worksheet");
			Guard.ThrowExceptionIfNull<CellIndex>(context.CellIndex, "CellIndex");
			Guard.ThrowExceptionIfNullOrEmpty(context.Argument1, "Argument1");
			this.context = context;
			this.ignoreBlank = context.IgnoreBlank;
			this.worksheet = context.Worksheet;
			this.cellIndex = context.CellIndex;
			this.argument1 = this.ConvertToCellValue(context.Argument1, context.Worksheet, context.CultureInfo);
		}

		internal ICellValue ConvertToCellValue(string argument, Worksheet worksheet, SpreadsheetCultureHelper cultureInfo)
		{
			ICellValue result;
			if (RadExpression.StartsLikeExpression(argument))
			{
				result = CellValueFactory.Create(argument, worksheet, this.CellIndex.RowIndex, this.CellIndex.ColumnIndex, cultureInfo);
			}
			else
			{
				result = argument.ToCellValue(worksheet, this.CellIndex.RowIndex, this.CellIndex.ColumnIndex, CellValueFormat.GeneralFormat);
			}
			return result;
		}

		internal bool ShouldIgnore(ICellValue cellValue)
		{
			Guard.ThrowExceptionIfNull<ICellValue>(cellValue, "cellValue");
			return this.IgnoreBlank && cellValue is EmptyCellValue;
		}

		internal string GetExpressionString(ICellValue argument, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfNull<ICellValue>(argument, "argument");
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			ICellValue translatedArgument = this.GetTranslatedArgument(argument, worksheet, rowIndex, columnIndex);
			return translatedArgument.RawValue;
		}

		public override bool Equals(object obj)
		{
			SingleArgumentDataValidationRuleBase singleArgumentDataValidationRuleBase = obj as SingleArgumentDataValidationRuleBase;
			return singleArgumentDataValidationRuleBase != null && (base.Equals(singleArgumentDataValidationRuleBase) && TelerikHelper.EqualsOfT<bool>(this.IgnoreBlank, singleArgumentDataValidationRuleBase.IgnoreBlank)) && this.SimilarEquals(this.Argument1, singleArgumentDataValidationRuleBase.Argument1);
		}

		internal bool SimilarEquals(ICellValue first, ICellValue second)
		{
			FormulaCellValue formulaCellValue = first as FormulaCellValue;
			FormulaCellValue formulaCellValue2 = second as FormulaCellValue;
			if (formulaCellValue != null && formulaCellValue2 != null)
			{
				first = formulaCellValue.CloneAndTranslate(this.worksheet, this.cellIndex, true);
				second = formulaCellValue2.CloneAndTranslate(this.worksheet, this.cellIndex, true);
			}
			return TelerikHelper.EqualsOfT<string>(first.RawValue, second.RawValue);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(base.GetHashCode(), this.IgnoreBlank.GetHashCodeOrZero(), this.argument1.GetHashCodeOrZero());
		}

		internal double? EvaluateArgument(ICellValue argument, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfNull<ICellValue>(argument, "argument");
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			ICellValue translatedArgument = this.GetTranslatedArgument(argument, worksheet, rowIndex, columnIndex);
			return translatedArgument.GetAsDoubleOrNull();
		}

		internal ICellValue GetTranslatedArgument(ICellValue argument, Worksheet worksheet, int rowIndex, int columnIndex)
		{
			Guard.ThrowExceptionIfNull<ICellValue>(argument, "argument");
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			FormulaCellValue formulaCellValue = argument as FormulaCellValue;
			if (formulaCellValue != null)
			{
				argument = formulaCellValue.CloneAndTranslate(worksheet, rowIndex, columnIndex, true);
			}
			return argument;
		}

		internal IDataValidationRule CloneAndTranslate(Worksheet worksheet, int rowIndex, int columnIndex, bool shouldTranslateArguments)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			this.context.Worksheet = worksheet;
			this.context.CellIndex = new CellIndex(rowIndex, columnIndex);
			this.context.Argument1 = (shouldTranslateArguments ? this.GetExpressionString(this.Argument1, worksheet, rowIndex, columnIndex) : this.GetExpressionString(this.Argument1, worksheet, this.CellIndex.RowIndex, this.CellIndex.ColumnIndex));
			this.CloneAndTranslateOverride(this.context, worksheet, rowIndex, columnIndex, shouldTranslateArguments);
			return (IDataValidationRule)Activator.CreateInstance(base.GetType(), new object[] { this.context });
		}

		internal virtual void CloneAndTranslateOverride(DataValidationRuleContextBase context, Worksheet worksheet, int rowIndex, int columnIndex, bool shouldTranslateArguments)
		{
		}

		readonly bool ignoreBlank;

		readonly ICellValue argument1;

		readonly CellIndex cellIndex;

		readonly Worksheet worksheet;

		readonly SingleArgumentDataValidationRuleContext context;
	}
}
