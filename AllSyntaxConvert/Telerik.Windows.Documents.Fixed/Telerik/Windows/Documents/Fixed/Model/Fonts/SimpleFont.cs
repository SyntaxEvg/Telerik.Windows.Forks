using System;
using Telerik.Windows.Documents.Core.Fonts;
using Telerik.Windows.Documents.Fixed.Model.Common;
using Telerik.Windows.Documents.Fixed.Model.Fonts.Encoding;

namespace Telerik.Windows.Documents.Fixed.Model.Fonts
{
	abstract class SimpleFont : FontBase
	{
		internal SimpleFont(string name, FontSource fontSource)
			: base(name, fontSource)
		{
			this.firstChar = new PdfProperty<int>();
			this.lastChar = new PdfProperty<int>();
			this.widths = new PdfProperty<int[]>();
			this.encoding = new PdfProperty<SimpleEncoding>();
		}

		public PdfProperty<int> FirstChar
		{
			get
			{
				return this.firstChar;
			}
		}

		public PdfProperty<int> LastChar
		{
			get
			{
				return this.lastChar;
			}
		}

		public PdfProperty<int[]> Widths
		{
			get
			{
				return this.widths;
			}
		}

		public PdfProperty<SimpleEncoding> Encoding
		{
			get
			{
				return this.encoding;
			}
		}

		internal override double GetWidth(int charCode)
		{
			if (this.FirstChar.HasValue && this.Widths.HasValue)
			{
				int num = charCode - this.FirstChar.Value;
				if (0 <= num && num < this.Widths.Value.Length)
				{
					return (double)this.Widths.Value[num];
				}
			}
			return base.GetWidth(charCode);
		}

		readonly PdfProperty<int> firstChar;

		readonly PdfProperty<int> lastChar;

		readonly PdfProperty<int[]> widths;

		readonly PdfProperty<SimpleEncoding> encoding;
	}
}
