using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public class RangePropertyValue<T>
	{
		public bool IsIndeterminate
		{
			get
			{
				return this.isIndeterminate;
			}
		}

		public T Value
		{
			get
			{
				return this.value;
			}
		}

		internal RangePropertyValue(bool isIndeterminate, T value)
		{
			this.isIndeterminate = isIndeterminate;
			this.value = value;
		}

		public override bool Equals(object obj)
		{
			RangePropertyValue<T> rangePropertyValue = obj as RangePropertyValue<T>;
			return rangePropertyValue != null && TelerikHelper.EqualsOfT<T>(this.Value, rangePropertyValue.Value) && this.IsIndeterminate == rangePropertyValue.IsIndeterminate;
		}

		public override int GetHashCode()
		{
			int h = 0;
			if (this.Value != null)
			{
				T t = this.Value;
				h = t.GetHashCode();
			}
			return TelerikHelper.CombineHashCodes(h, this.IsIndeterminate.GetHashCode());
		}

		readonly bool isIndeterminate;

		readonly T value;
	}
}
