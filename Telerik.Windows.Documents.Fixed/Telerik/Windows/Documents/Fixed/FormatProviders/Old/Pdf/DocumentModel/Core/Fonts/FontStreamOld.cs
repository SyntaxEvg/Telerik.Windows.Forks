using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.Fonts
{
	class FontStreamOld : PdfStreamOld
	{
		public FontStreamOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.subtype = base.CreateInstantLoadProperty<PdfNameOld>(new PdfPropertyDescriptor
			{
				Name = "Subtype"
			});
		}

		public PdfNameOld Subtype
		{
			get
			{
				return this.subtype.GetValue();
			}
			set
			{
				this.subtype.SetValue(value);
			}
		}

		readonly InstantLoadProperty<PdfNameOld> subtype;
	}
}
