using System;
using Telerik.Windows.Documents.Spreadsheet.Utilities;

namespace Telerik.Windows.Documents.Spreadsheet.Model
{
	public abstract class CellValueBase<T> : ICellValue
	{
		public abstract CellValueType ValueType { get; }

		public virtual CellValueType ResultValueType
		{
			get
			{
				return this.ValueType;
			}
		}

		public virtual string RawValue
		{
			get
			{
				T t = this.value;
				return t.ToString();
			}
		}

		public T Value
		{
			get
			{
				return this.value;
			}
		}

		protected CellValueBase(T value)
		{
			Guard.ThrowExceptionIfNull<T>(value, "value");
			this.value = value;
		}

		public string GetValueAsString(CellValueFormat format)
		{
			if (this.valueAsString == null || !this.isEditFormatStringValid)
			{
				this.valueAsString = this.GetValueAsStringOverride(format);
			}
			return this.valueAsString;
		}

		public void InvalidateEditFormatString()
		{
			this.isEditFormatStringValid = false;
		}

		protected virtual string GetValueAsStringOverride(CellValueFormat format = null)
		{
			T t = this.value;
			return t.ToString();
		}

		public virtual string GetResultValueAsString(CellValueFormat format)
		{
			return format.GetFormatResult(this).VisibleInfosText;
		}

		public override bool Equals(object obj)
		{
			CellValueBase<T> cellValueBase = obj as CellValueBase<T>;
			return cellValueBase != null && this.ValueType == cellValueBase.ValueType && this.GetValueAsStringOverride(null) == cellValueBase.GetValueAsStringOverride(null);
		}

		public override int GetHashCode()
		{
			T t = this.Value;
			return TelerikHelper.CombineHashCodes(t.GetHashCode(), this.GetValueAsStringOverride(null).GetHashCode());
		}

		readonly T value;

		string valueAsString;

		bool isEditFormatStringValid;
	}
}
