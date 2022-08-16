using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class CustomFilter : CellValuesFilterBase
	{
		public CustomFilterCriteria Criteria1
		{
			get
			{
				return this.criteria1;
			}
		}

		public CustomFilterCriteria Criteria2
		{
			get
			{
				return this.criteria2;
			}
		}

		public LogicalOperator LogicalOperator
		{
			get
			{
				return this.logicalOperator;
			}
		}

		public CustomFilter(int relativeColumnIndex, CustomFilterCriteria criteria)
			: this(relativeColumnIndex, criteria, LogicalOperator.Or, null)
		{
		}

		public CustomFilter(int relativeColumnIndex, CustomFilterCriteria criteria1, LogicalOperator logicalOperator, CustomFilterCriteria criteria2)
			: base(relativeColumnIndex)
		{
			Guard.ThrowExceptionIfNull<CustomFilterCriteria>(criteria1, "criteria1");
			this.criteria1 = criteria1;
			this.criteria2 = criteria2;
			this.logicalOperator = logicalOperator;
		}

		internal override IFilter Copy(int newRelativeColumnIndex)
		{
			return new CustomFilter(newRelativeColumnIndex, this.Criteria1, this.LogicalOperator, this.Criteria2);
		}

		public override bool ShouldShowValue(object value)
		{
			ICellValue cellValue = value as ICellValue;
			bool flag = this.CellValueSatisifiesCriteria(this.criteria1, cellValue);
			bool flag2 = this.CellValueSatisifiesCriteria(this.criteria2, cellValue);
			if (this.logicalOperator != LogicalOperator.And)
			{
				return flag || flag2;
			}
			return flag && flag2;
		}

		bool CellValueSatisifiesCriteria(CustomFilterCriteria criteria, ICellValue cellValue)
		{
			if (criteria == null)
			{
				return false;
			}
			IComparable comparableValueFromCellValue = this.GetComparableValueFromCellValue(criteria.ComparableFilterValue, cellValue);
			if (comparableValueFromCellValue == null)
			{
				return false;
			}
			IComparable obj = this.ToLowerString(criteria.ComparableFilterValue);
			bool result;
			switch (criteria.ComparisonOperator)
			{
			case ComparisonOperator.EqualsTo:
				result = comparableValueFromCellValue.CompareTo(obj) == 0;
				break;
			case ComparisonOperator.GreaterThan:
				result = comparableValueFromCellValue.CompareTo(obj) > 0;
				break;
			case ComparisonOperator.GreaterThanOrEqualsTo:
				result = comparableValueFromCellValue.CompareTo(obj) >= 0;
				break;
			case ComparisonOperator.LessThan:
				result = comparableValueFromCellValue.CompareTo(obj) < 0;
				break;
			case ComparisonOperator.LessThanOrEqualsTo:
				result = comparableValueFromCellValue.CompareTo(obj) <= 0;
				break;
			case ComparisonOperator.NotEqualsTo:
				result = comparableValueFromCellValue.CompareTo(obj) != 0;
				break;
			default:
				throw new FilteringException("Unknown compare operator.", new InvalidOperationException("Unknown compare operator."), "Spreadsheet_Filtering_UnknownCompareOperator");
			}
			return result;
		}

		IComparable ToLowerString(IComparable filterValue)
		{
			string text = filterValue as string;
			if (text != null)
			{
				return text.ToLower();
			}
			return filterValue;
		}

		IComparable GetComparableValueFromCellValue(IComparable filterValue, ICellValue cellValue)
		{
			IComparable result = null;
			FormulaCellValue formulaCellValue = cellValue as FormulaCellValue;
			if (filterValue is double)
			{
				NumberCellValue numberCellValue = cellValue as NumberCellValue;
				if (numberCellValue == null && formulaCellValue != null)
				{
					numberCellValue = formulaCellValue.GetResultValueAsCellValue() as NumberCellValue;
				}
				if (numberCellValue != null)
				{
					result = numberCellValue.Value;
				}
			}
			else if (filterValue is string)
			{
				TextCellValue textCellValue = cellValue as TextCellValue;
				if (textCellValue == null && formulaCellValue != null)
				{
					textCellValue = formulaCellValue.GetResultValueAsCellValue() as TextCellValue;
				}
				if (textCellValue != null)
				{
					result = textCellValue.Value.ToLower();
				}
			}
			else if (filterValue is bool)
			{
				BooleanCellValue booleanCellValue = cellValue as BooleanCellValue;
				if (booleanCellValue == null && formulaCellValue != null)
				{
					booleanCellValue = formulaCellValue.GetResultValueAsCellValue() as BooleanCellValue;
				}
				if (booleanCellValue != null)
				{
					result = booleanCellValue.Value;
				}
			}
			return result;
		}

		public override bool Equals(object obj)
		{
			CustomFilter customFilter = obj as CustomFilter;
			return customFilter != null && (TelerikHelper.EqualsOfT<CustomFilterCriteria>(this.Criteria1, customFilter.Criteria1) && TelerikHelper.EqualsOfT<CustomFilterCriteria>(this.Criteria2, customFilter.Criteria2)) && this.LogicalOperator.Equals(customFilter.LogicalOperator);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.Criteria1.GetHashCodeOrZero(), this.Criteria2.GetHashCodeOrZero(), this.LogicalOperator.GetHashCode());
		}

		readonly CustomFilterCriteria criteria1;

		readonly CustomFilterCriteria criteria2;

		readonly LogicalOperator logicalOperator;
	}
}
