using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption
{
	class CryptFilter : PdfObject
	{
		public CryptFilter()
		{
			this.cryptFilterMethod = base.RegisterDirectProperty<PdfName>(new PdfPropertyDescriptor("CFM"));
		}

		public PdfName CryptFilterMethod
		{
			get
			{
				return this.cryptFilterMethod.GetValue();
			}
			set
			{
				this.cryptFilterMethod.SetValue(value);
			}
		}

		readonly DirectProperty<PdfName> cryptFilterMethod;
	}
}
