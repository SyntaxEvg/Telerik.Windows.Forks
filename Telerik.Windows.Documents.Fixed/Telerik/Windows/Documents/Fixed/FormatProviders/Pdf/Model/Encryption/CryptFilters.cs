using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Elements.Common;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Encryption
{
	class CryptFilters : PdfObject
	{
		public CryptFilters()
		{
			this.standardCryptFilter = base.RegisterDirectProperty<CryptFilter>(new PdfPropertyDescriptor("StdCF"));
		}

		public CryptFilter StandardCryptFilter
		{
			get
			{
				return this.standardCryptFilter.GetValue();
			}
			set
			{
				this.standardCryptFilter.SetValue(value);
			}
		}

		readonly DirectProperty<CryptFilter> standardCryptFilter;
	}
}
