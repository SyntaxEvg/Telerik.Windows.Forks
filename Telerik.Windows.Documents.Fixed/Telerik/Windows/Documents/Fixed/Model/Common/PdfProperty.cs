using System;
using Telerik.Windows.Documents.Utilities;

namespace Telerik.Windows.Documents.Fixed.Model.Common
{
	class PdfProperty<T>
	{
		public PdfProperty()
			: this(() => default(T))
		{
		}

		public PdfProperty(Func<T> getDefaultValue)
		{
			this.hasValue = false;
			this.getDefaultValue = getDefaultValue;
		}

		public T Value
		{
			get
			{
				if (this.hasValue)
				{
					return this.value;
				}
				return this.getDefaultValue();
			}
			set
			{
				this.hasValue = true;
				this.value = value;
			}
		}

		public bool HasValue
		{
			get
			{
				return this.hasValue;
			}
		}

		public void SetDefaultValueFunc(Func<T> getDefautValue)
		{
			Guard.ThrowExceptionIfNull<Func<T>>(getDefautValue, "getDefautValue");
			this.getDefaultValue = getDefautValue;
		}

		public void ClearValue()
		{
			this.hasValue = false;
			this.value = default(T);
		}

		public override string ToString()
		{
			if (this.HasValue)
			{
				return string.Format("{0}", this.Value);
			}
			return "<Empty property>";
		}

		public override bool Equals(object obj)
		{
			PdfProperty<T> pdfProperty = obj as PdfProperty<T>;
			if (pdfProperty == null)
			{
				return false;
			}
			if (this.Value == null)
			{
				return pdfProperty.Value == null;
			}
			T t = this.Value;
			return t.Equals(pdfProperty.Value);
		}

		public override int GetHashCode()
		{
			if (this.Value != null)
			{
				T t = this.Value;
				return t.GetHashCode();
			}
			return 0;
		}

		T value;

		bool hasValue;

		Func<T> getDefaultValue;
	}
}
