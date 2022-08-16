using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Core
{
	class ValueBox<T>
	{
		public T Value
		{
			get
			{
				return this.value;
			}
		}

		public ValueBox(T value)
		{
			this.value = value;
		}

		public override int GetHashCode()
		{
			T t = this.value;
			return t.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			ValueBox<T> valueBox = obj as ValueBox<T>;
			return valueBox != null && TelerikHelper.EqualsOfT<T>(this.value, valueBox.value);
		}

		public override string ToString()
		{
			if (this.value == null)
			{
				return "null";
			}
			T t = this.value;
			return t.ToString();
		}

		readonly T value;
	}
}
