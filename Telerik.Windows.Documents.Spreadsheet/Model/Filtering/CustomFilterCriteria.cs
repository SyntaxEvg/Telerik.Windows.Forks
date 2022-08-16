using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model.Filtering
{
	public class CustomFilterCriteria
	{
		public ComparisonOperator ComparisonOperator
		{
			get
			{
				return this.comparisonOperator;
			}
		}

		public string FilterValue
		{
			get
			{
				return this.filterValue;
			}
		}

		internal IComparable ComparableFilterValue
		{
			get
			{
				return this.comparableFilterValue;
			}
		}

		public CustomFilterCriteria(ComparisonOperator comparisonOperator, string filterValue)
		{
			this.comparisonOperator = comparisonOperator;
			this.filterValue = filterValue;
			this.comparableFilterValue = this.GetComparableValue(this.filterValue);
		}

		IComparable GetComparableValue(string value)
		{
			if (value == null)
			{
				return null;
			}
			double num;
			if (double.TryParse(value, out num))
			{
				return num;
			}
			bool flag;
			if (bool.TryParse(value, out flag))
			{
				return flag;
			}
			return value;
		}

		public override bool Equals(object obj)
		{
			CustomFilterCriteria customFilterCriteria = obj as CustomFilterCriteria;
			return customFilterCriteria != null && this.ComparisonOperator.Equals(customFilterCriteria.ComparisonOperator) && TelerikHelper.EqualsOfT<string>(this.FilterValue, customFilterCriteria.FilterValue);
		}

		public override int GetHashCode()
		{
			return TelerikHelper.CombineHashCodes(this.ComparisonOperator.GetHashCode(), this.FilterValue.GetHashCodeOrZero());
		}

		readonly ComparisonOperator comparisonOperator;

		readonly string filterValue;

		readonly IComparable comparableFilterValue;
	}
}
