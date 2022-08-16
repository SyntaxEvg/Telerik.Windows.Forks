using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.PdfConverters;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Encryption
{
	class CryptFilterOld : PdfObjectOld
	{
		public CryptFilterOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.cryptFilterMethod = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "CFM"
			}, new PdfNameOld(contentManager, "None"), Converters.PdfNameConverter);
		}

		public PdfNameOld CryptFilterMethod
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

		readonly InstantLoadProperty<PdfNameOld> cryptFilterMethod;
	}
}
