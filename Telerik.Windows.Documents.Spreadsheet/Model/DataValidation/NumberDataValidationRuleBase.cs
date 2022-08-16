using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.DataValidation
{
	public abstract class NumberDataValidationRuleBase : SingleArgumentDataValidationRuleBase
	{
		public ComparisonOperator ComparisonOperator
		{
			get
			{
				return this.comparisonOperator;
			}
		}

		public ICellValue Argument2
		{
			get
			{
				return this.argument2;
			}
		}

		protected virtual bool RequireWholeNumbers
		{
			get
			{
				return false;
			}
		}

		protected NumberDataValidationRuleBase(NumberDataValidationRuleContext context)
			: base(context)
		{
			Guard.ThrowExceptionIfNull<NumberDataValidationRuleContext>(context, "context");
			if (context.ComparisonOperator == ComparisonOperator.Between || context.ComparisonOperator == ComparisonOperator.NotBetween)
			{
				Guard.ThrowExceptionIfNullOrEmpty(context.Argument2, "Argument2");
			}
			this.comparisonOperator = context.ComparisonOperator;
			ICellValue cellValue = base.ConvertToCellValue(context.Argument2, context.Worksheet, context.CultureInfo);
			this.argument2 = cellValue;
		}

		protected override bool EvaluateOverride(Worksheet worksheet, int rowIndex, int columnIndex, ICellValue cellValue)
		{
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfNull<ICellValue>(cellValue, "cellValue");
			Guard.ThrowExceptionIfNull<ICellValue>(base.Argument1, "Argument1");
			if (base.ShouldIgnore(cellValue))
			{
				return true;
			}
			double? asDoubleOrNull = cellValue.GetAsDoubleOrNull();
			double? argument = base.EvaluateArgument(base.Argument1, worksheet, rowIndex, columnIndex);
			double? num = ((this.Argument2 == null) ? null : base.EvaluateArgument(this.Argument2, worksheet, rowIndex, columnIndex));
			return asDoubleOrNull != null && argument != null && (!this.RequireWholeNumbers || this.IsWholeNumber(asDoubleOrNull.Value)) && this.CompareValues(asDoubleOrNull, argument, num);
		}

		protected bool CompareValues(double? cellValueResult, double? argument1, double? argument2)
		{
			switch (this.ComparisonOperator)
			{
			case ComparisonOperator.EqualsTo:
				return cellValueResult.Value == argument1.Value;
			case ComparisonOperator.GreaterThan:
				return cellValueResult.Value > argument1.Value;
			case ComparisonOperator.GreaterThanOrEqualsTo:
				return cellValueResult.Value >= argument1.Value;
			case ComparisonOperator.LessThan:
				return cellValueResult.Value < argument1.Value;
			case ComparisonOperator.LessThanOrEqualsTo:
				return cellValueResult.Value <= argument1.Value;
			case ComparisonOperator.NotEqualsTo:
				return cellValueResult.Value != argument1.Value;
			case ComparisonOperator.Between:
				if (argument2 != null)
				{
					Guard.ThrowExceptionIfNull<double?>(argument2, "argument2");
					return argument1.Value <= cellValueResult.Value && cellValueResult.Value <= argument2.Value;
				}
				break;
			case ComparisonOperator.NotBetween:
				if (argument2 != null)
				{
					Guard.ThrowExceptionIfNull<double?>(argument2, "argument2");
					return argument1.Value > cellValueResult.Value || cellValueResult.Value > argument2.Value;
				}
				break;
			}
			return false;
		}

		bool IsWholeNumber(double value)
		{
			int num = (int)value;
			return value.EqualsDouble((double)num);
		}

		internal override void CloneAndTranslateOverride(DataValidationRuleContextBase context, Worksheet worksheet, int rowIndex, int columnIndex, bool shouldTranslateArguments)
		{
			Guard.ThrowExceptionIfNull<DataValidationRuleContextBase>(context, "context");
			Guard.ThrowExceptionIfNull<Worksheet>(worksheet, "worksheet");
			Guard.ThrowExceptionIfInvalidRowIndex(rowIndex);
			Guard.ThrowExceptionIfInvalidColumnIndex(columnIndex);
			NumberDataValidationRuleContext numberDataValidationRuleContext = context as NumberDataValidationRuleContext;
			Guard.ThrowExceptionIfNull<NumberDataValidationRuleContext>(numberDataValidationRuleContext, "numberContext");
			numberDataValidationRuleContext.Argument2 = (shouldTranslateArguments ? base.GetExpressionString(this.Argument2, worksheet, rowIndex, columnIndex) : base.GetExpressionString(this.Argument2, worksheet, base.CellIndex.RowIndex, base.CellIndex.ColumnIndex));
		}

		public override bool Equals(object obj)
		{
			NumberDataValidationRuleBase numberDataValidationRuleBase = obj as NumberDataValidationRuleBase;
			return numberDataValidationRuleBase != null && (base.Equals(obj) && TelerikHelper.EqualsOfT<ComparisonOperator>(this.ComparisonOperator, numberDataValidationRuleBase.ComparisonOperator)) && base.SimilarEquals(this.Argument2, numberDataValidationRuleBase.Argument2);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(base.GetHashCode(), this.ComparisonOperator.GetHashCodeOrZero(), this.Argument2.GetHashCodeOrZero());
		}

		readonly ComparisonOperator comparisonOperator;

		readonly ICellValue argument2;
	}
}
