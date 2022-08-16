using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Encryption
{
	class CryptFiltersOld : PdfObjectOld
	{
		public CryptFiltersOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.standardCryptFilter = base.CreateLoadOnDemandProperty<CryptFilterOld>(new PdfPropertyDescriptor
			{
				Name = "StdCF"
			}, Converters.PdfDictionaryToPdfObjectConverter);
		}

		public CryptFilterOld StandardCryptFilter
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

		readonly LoadOnDemandProperty<CryptFilterOld> standardCryptFilter;
	}
}
