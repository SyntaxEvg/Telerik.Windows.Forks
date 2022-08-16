using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Fonts
{
	class SimpleFontEncoding : PdfObject
	{
		public SimpleFontEncoding()
		{
			this.baseEncoding = base.RegisterReferenceProperty<PdfName>(new PdfPropertyDescriptor("BaseEncoding"));
			this.differences = base.RegisterReferenceProperty<PdfArray>(new PdfPropertyDescriptor("Differences"));
		}

		public override PdfElementType ExportAs
		{
			get
			{
				return PdfElementType.Dictionary;
			}
		}

		public PdfName BaseEncoding
		{
			get
			{
				return this.baseEncoding.GetValue();
			}
			set
			{
				this.baseEncoding.SetValue(value);
			}
		}

		public PdfArray Differences
		{
			get
			{
				return this.differences.GetValue();
			}
			set
			{
				this.differences.SetValue(value);
			}
		}

		readonly ReferenceProperty<PdfName> baseEncoding;

		readonly ReferenceProperty<PdfArray> differences;
	}
}
