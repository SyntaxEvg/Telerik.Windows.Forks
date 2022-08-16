using System;

namespace Telerik.Documents.SpreadsheetStreaming.ImportExport.Core
{
	class ValueBox<T>
	{
		public ValueBox(T value)
		{
			this.value = value;
		}

		public T Value
		{
			get
			{
				return this.value;
			}
		}

		public override int GetHashCode()
		{
			T t = this.value;
			return t.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			ValueBox<T> valueBox = obj as ValueBox<T>;
			return valueBox != null && object.Equals(this.value, valueBox.value);
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
