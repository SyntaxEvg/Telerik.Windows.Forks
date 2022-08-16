using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.Utils;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Core.ColorSpaces
{
	class ICCProfileOld : PdfStreamOld
	{
		public ICCProfileOld(PdfContentManager contentManager)
			: base(contentManager)
		{
			this.n = base.CreateInstantLoadProperty<PdfIntOld>(new PdfPropertyDescriptor
			{
				Name = "N",
				IsRequired = true
			});
		}

		public ColorSpaceOld Alternate
		{
			get
			{
				if (this.alternate == null)
				{
					this.alternate = this.GetAlternateColorSpace();
				}
				return this.alternate;
			}
		}

		public PdfIntOld N
		{
			get
			{
				return this.n.GetValue();
			}
			set
			{
				this.n.SetValue(value);
			}
		}

		ColorSpaceOld GetAlternateColorSpace()
		{
			switch (this.N.Value)
			{
			case 1:
				return new DeviceGrayOld(base.ContentManager);
			case 3:
				return new DeviceRgbOld(base.ContentManager);
			case 4:
				return new DeviceCmykOld(base.ContentManager);
			}
			throw new NotSupportedException();
		}

		readonly InstantLoadProperty<PdfIntOld> n;

		ColorSpaceOld alternate;
	}
}
