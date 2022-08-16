using System;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types
{
	abstract class PdfSimpleType<T> : PdfPrimitive, IPdfSimpleType
	{
		public PdfSimpleType()
		{
		}

		public PdfSimpleType(T initialValue)
		{
			this.Value = initialValue;
		}

		public T Value { get; set; }

		object IPdfSimpleType.Value
		{
			get
			{
				return this.Value;
			}
		}

		public override string ToString()
		{
			T value = this.Value;
			return value.ToString();
		}

		public override bool Equals(object obj)
		{
			IPdfSimpleType pdfSimpleType = obj as IPdfSimpleType;
			return pdfSimpleType != null && object.Equals(this.Value, pdfSimpleType.Value);
		}

		public override int GetHashCode()
		{
			int num = 17;
			int num2 = num * 23;
			T value = this.Value;
			return num2 + value.GetHashCode();
		}
	}
}
