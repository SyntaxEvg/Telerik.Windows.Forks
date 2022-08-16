using System;
using Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.PdfReader;
using Telerik.Windows.Documents.Fixed.FormatProviders.Pdf.Model.Types;

namespace Telerik.Windows.Documents.Fixed.FormatProviders.Old.Pdf.DocumentModel.Data
{
	class PdfStringOld : PdfSimpleTypeOld<byte[]>
	{
		public PdfStringOld(PdfContentManager contentManager)
			: base(contentManager)
		{
		}

		public PdfStringOld(PdfContentManager contentManager, byte[] value)
			: base(contentManager)
		{
			base.Value = value;
		}

		public override string ToString()
		{
			return PdfString.GetString(base.Value);
		}

		public string ToUnicodeString()
		{
			return PdfString.GetUtfEncodedTextString(base.Value);
		}

		public void Increment()
		{
			if (base.Value == null)
			{
				return;
			}
			byte b = 1;
			for (int i = base.Value.Length - 1; i >= 0; i--)
			{
				int num = (int)(base.Value[i] + b);
				b = (byte)(num / 256);
				base.Value[i] = (byte)(num % 256);
				if (b == 0)
				{
					return;
				}
			}
		}
	}
}
